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
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Window
    {
        public LoginPage()
        {
            InitializeComponent();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            SignUp signUp = new SignUp();
            signUp.Show();
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Login().GetAwaiter();
        }
        private async Task Login()
        {
            string procedure = "login";
            string connectionString = @"Data Source=.\CHUPRIS; Initial Catalog=RESTORATOR; User Id=Admin; Password=P@$$word";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(procedure, connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter nameParam = new SqlParameter
                {
                    ParameterName = "@name",
                    Value = UserName.Text
                };     
                command.Parameters.Add(nameParam);
                SqlParameter PassParam = new SqlParameter
                {
                    ParameterName = "@pass",
                    Value = Passwords.Password.GetHashCode()
                };
                command.Parameters.Add(PassParam);
                var result = command.ExecuteScalar();
                if (result == null)
                {
                    MessageBox.Show("Неверное имя пользователя или пароль");
                }
                else
                {
                    SqlDataReader reader = await command.ExecuteReaderAsync();
                    while (reader.Read())
                    {
                        User.UserType = reader.GetString(3);
                    }
                    User.UserName = UserName.Text;
                    User.LOGIN = true;
                    MainWindow window = (MainWindow)Application.Current.MainWindow;
                    window.Username.Text = UserName.Text;
                    if(User.UserType =="Admin")
                    {
                        window.Admin_button.Visibility = Visibility.Visible;
                        window.Admin_button.IsEnabled = true;
                    }
                    else
                    {
                        window.Admin_button.Visibility = Visibility.Hidden;
                        window.Admin_button.IsEnabled = false ;
                    }
                    this.Close();
                }
            }
        }
    }
}
