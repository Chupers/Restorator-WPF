using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Restorator
{
    class RepositoryClass : Repository
    {
        public RepositoryClass()
        {
            connect = new SqlConnection(connectionString);
            connect.OpenAsync();
        }
       public SqlConnection connect = null;
      public  string connectionString = @"Data Source=.\CHUPRIS; Initial Catalog=RESTORATOR; User Id=Admin; Password=P@$$word";
        public  Stack<Restoran> LoadRest()
        {
            Stack<Restoran> restorans = new Stack<Restoran>();
            connect = new SqlConnection(connectionString);
             connect.Open();
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * FROM Rest";
                SqlCommand command = new SqlCommand(sql, connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string Filename = reader.GetString(1);
                    byte[] data = (byte[])reader.GetValue(2);
                    string name_Rest = reader.GetString(3);
                    string Type = reader.GetString(4);
                    string Work_time = reader.GetString(5);
                    string Sity = reader.GetString(6);
                    string Street = reader.GetString(7);
                    string Price = reader.GetString(8);
                    string Disk = reader.GetString(9);
                    string Cord = reader.GetString(10);
                    Restoran restoran = new Restoran(Filename, data, name_Rest, Type, Work_time, Sity, Street, Price, Disk, Cord);
                    restorans.Push(restoran);
                };
                
            }
            return restorans;
        }

        public async void PushRest(Restoran restoran)
        {
            string sql = string.Format("Insert Into Rest" +
   "(FileNama,ImageData,Name,Type,WorkTime,Sity,Street,Price,[Disk],Cord) Values(@FileNama,@ImageData,@Name,@Type,@WorkTime,@Sity,@Street,@Price,@Disk,@Cord)");
            using (SqlCommand cmd = new SqlCommand(sql, this.connect))
            {
               
                // Добавить параметры
                cmd.Parameters.AddWithValue("@FileNama", restoran.Filename);
                cmd.Parameters.AddWithValue("@ImageData", restoran.imagedata);
                cmd.Parameters.AddWithValue("@Name", restoran.Name);
                cmd.Parameters.AddWithValue("@Type", restoran.Type);
                cmd.Parameters.AddWithValue("@WorkTime", restoran.WorkTime);
                cmd.Parameters.AddWithValue("@Sity", restoran.Sity);
                cmd.Parameters.AddWithValue("@Street", restoran.Street);
                cmd.Parameters.AddWithValue("@Price", restoran.Price);
                cmd.Parameters.AddWithValue("@Disk", restoran.Disk);
                cmd.Parameters.AddWithValue("@Cord", restoran.Cord);
                await cmd.ExecuteNonQueryAsync();
            }
        }

        public Stack<Restoran> SearchRest(string Restoran)
        {
            Stack<Restoran> restorans = new Stack<Restoran>();
            string procedure = "Search_rest";
            string connectionString = @"Data Source=.\CHUPRIS; Initial Catalog=RESTORATOR; User Id=Admin; Password=P@$$word";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(procedure, connection);
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = System.Data.CommandType.StoredProcedure;
                // параметр для ввода имени
                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@name",
                    Value = Restoran
                };
                // добавляем параметр
                command.Parameters.Add(nameParam);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    string Filename = reader.GetString(1);
                    byte[] data = (byte[])reader.GetValue(2);
                    string name_Rest = reader.GetString(3);
                    string Type = reader.GetString(4);
                    string Work_time = reader.GetString(5);
                    string Sity = reader.GetString(6);
                    string Street = reader.GetString(7);
                    string Price = reader.GetString(8);
                    string Disk = reader.GetString(9);
                    string Cord = reader.GetString(10);
                    Restoran restoran = new Restoran(Filename, data, name_Rest, Type, Work_time, Sity, Street, Price, Disk, Cord);
                    restorans.Push(restoran);
                };
                return restorans;
            }
        }

        public async void DeleteRest(string Name)
        {
            string procedure = "deletRest";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(procedure, connection);
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = System.Data.CommandType.StoredProcedure;
                // параметр для ввода имени
                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@name",
                    Value = Name
                };
                // добавляем параметр
                command.Parameters.Add(nameParam);


                var result = command.ExecuteScalar();
            }
        }

        public async void DeleteComment( string Comment,string Rest)
        {
            string procedure = "deletComment";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(procedure, connection);
               
                command.CommandType = System.Data.CommandType.StoredProcedure;
               
               
                SqlParameter nameParam2 = new SqlParameter
                {
                    ParameterName = "@Comment",
                    Value = Comment
                };
                SqlParameter nameParam3 = new SqlParameter
                {
                    ParameterName = "@Rest",
                    Value = Rest
                };
              
                
                command.Parameters.Add(nameParam2);
                command.Parameters.Add(nameParam3);
             

                var result = command.ExecuteNonQueryAsync();
               
            }
        }
    }
}
