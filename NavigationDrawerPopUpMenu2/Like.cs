using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Restorator
{
    public  class Like
    {
        public SqlConnection connect = null;
        public async Task TakeLike(string RestName)
        {
                string connectionString = @"Data Source=.\CHUPRIS; Initial Catalog=RESTORATOR; User Id=Admin; Password=P@$$word";
                connect = new SqlConnection(connectionString);
                connect.Open();
                string sql = string.Format("Insert Into Lukas" +
                   "(Name_of_Rest,Name_User) Values(@Name_of_Rest,@Name_User)");
                using (SqlCommand cmd = new SqlCommand(sql, this.connect))
                {
                    // Добавить параметры
                    cmd.Parameters.AddWithValue("@Name_of_Rest", RestName);
                    cmd.Parameters.AddWithValue("@Name_User", User.UserName);
                  
                    cmd.ExecuteNonQuery();
                }
                connect.Close();  
        }
        public  string CountLike(string RestName)
        {
            string cou = "0";
                string procedure = "Countlike";
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
                        Value = RestName
                    };
                    // добавляем параметр
                    command.Parameters.Add(nameParam);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        cou = reader.GetInt32(0).ToString();
                    }
                }
                return cou;
            }
        public string CheckLike(string RestName)
        {
            string cou = "0";
            string procedure = "Checklike";
            string connectionString = @"Data Source=.\CHUPRIS; Initial Catalog=RESTORATOR; User Id=Admin; Password=P@$$word";
            try {
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
                        Value = RestName
                    };

                    // добавляем параметр
                    command.Parameters.Add(nameParam);
                    SqlParameter nameParam2 = new SqlParameter
                    {
                        ParameterName = "@user",
                        Value = User.UserName
                    };
                    command.Parameters.Add(nameParam2);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        cou = reader.GetInt32(0).ToString();
                    }
                } 
            }
            catch
            {

            }
            return cou;
        }
        public void  DeleteLike(string RestName)
        {
            string cou = "0";
            string procedure = "deletlike";
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
                    Value = RestName
                };
                command.Parameters.Add(nameParam);
                SqlParameter nameParam2 = new SqlParameter
                {
                    ParameterName = "@user",
                    Value = User.UserName
                };
                command.Parameters.Add(nameParam2);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {

                    cou = reader.GetInt32(0).ToString();
                }
            }
        }
    }
}
