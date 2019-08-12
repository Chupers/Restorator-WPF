using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Restorator
{
    /// <summary>
    /// Логика взаимодействия для SignUp.xaml
    /// </summary>
    public partial class SignUp : Window
    {
      
       
        public SignUp()
        {
            InitializeComponent();
        }
          
       
        public SqlConnection connect;
        public bool flag = true;
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Username.Text.Length < 5 && Username.Text.Length > 20)
            {
                MessageBox.Show("Неверное имя пользователя (Минимальная длинна 5 Максимальная 20)");
                flag = false;
            }
            if (Pass1.Password != Pass2.Password)
            {
                flag = false;
                MessageBox.Show("Пароли не совпадают");
            }
            if (Pass1.Password.Length <5)
            {
                flag = false;
                MessageBox.Show("Пароль слишком легкий");
            }
            if (flag)
            {
                try
                {
                    string connectionString = @"Data Source=.\CHUPRIS; Initial Catalog=RESTORATOR; User Id=Admin; Password=P@$$word";
                    connect = new SqlConnection(connectionString);
                    connect.Open();
                    string sql = string.Format("Insert Into Users" +
                       "(Logins,Passwords) Values(@Logins,@Passwords)");
                    using (SqlCommand cmd = new SqlCommand(sql, this.connect))
                    {
                        // Добавить параметры
                        cmd.Parameters.AddWithValue("@Logins", Username.Text);
                        cmd.Parameters.AddWithValue("@Passwords", Pass1.Password.GetHashCode());
                        cmd.ExecuteNonQuery();
                    }
                    connect.Close();
                }
                catch
                {
                    MessageBox.Show("Такой пользователь уже существует");
                }
                    this.Close();
            }
        }
    }
}
