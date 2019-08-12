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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Restorator
{
    /// <summary>
    /// Логика взаимодействия для PagesofRest.xaml
    /// </summary>
    public partial class PagesofRest : UserControl
    {

        public PagesofRest()
        {
            InitializeComponent();
        }
        public void Loadmap(double x,double y)
        {
            UserControlNear userControlNear = new UserControlNear();
            userControlNear.IsEnabled = false;
            userControlNear.LoadMap(x, y);
            userControlNear.flag = false;
            Map.Children.Add(userControlNear);
        }
        string connectionString = @"Data Source=.\CHUPRIS; Initial Catalog=RESTORATOR; User Id=Admin; Password=P@$$word";
        public Like like;
       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!User.LOGIN)
            {
                LoginPage login = new LoginPage();
                login.Show();
            }
            if (User.LOGIN)
            {
                if (Convert.ToInt32(like.CheckLike(Rest_name.Content.ToString())) < 1)
                {
                    like.TakeLike(Rest_name.Content.ToString()).GetAwaiter();
                    LikeButton.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                }
                else
                {
                    LikeButton.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    like.DeleteLike(Rest_name.Content.ToString());
                }
                CountLike.Text = like.CountLike(Rest_name.Content.ToString());
            }
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            string time = DateTime.Now.ToShortTimeString();
            Time_Work(time);
            like = new Like();
            CountLike.Text = like.CountLike(Rest_name.Content.ToString()).ToString();
            if (Convert.ToInt32(like.CheckLike(Rest_name.Content.ToString())) >= 1)
            {
                LikeButton.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
            LoadComment().GetAwaiter();
        }
        private async Task LoadComment()
        {
            string procedure = "GetComment";
          
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
                    Value = Rest_name.Content
                };
            
                // добавляем параметр
                command.Parameters.Add(nameParam);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    
                    string username = reader.GetString(1);
                    string Comment = reader.GetString(3);
                    CommentControl commentControl = new CommentControl();
                    commentControl.comment.Text = Comment;
                    commentControl.Username.Content = username;
                    commentControl.NameRest = Rest_name.Content.ToString();
                    commentControl.LoadComment();
                    Comments.Children.Add(commentControl);
                }
            }
        }       
        public  void Time_Work(string time)
        {
            string[] vs = time.Split(':');
            string[] worktime = time_of_work.Text.Split('-');
            string[] startWork = worktime[0].Split(':');
            string[] Endwork = worktime[1].Split(':');
            if (Convert.ToInt32(Endwork[0]) < Convert.ToInt32(startWork[0]))
            { 
                Endwork[0] = (Convert.ToInt32(Endwork[0]) + 24).ToString();
            }
            if (Convert.ToInt32(startWork[0]) <= Convert.ToInt32(vs[0]) && Convert.ToInt32(Endwork[0]) >= Convert.ToInt32(vs[0]))
            {
                if (Convert.ToInt32(startWork[1]) <= Convert.ToInt32(vs[1]) && Convert.ToInt32(Endwork[1]) >= Convert.ToInt32(vs[1]))
                {
                    Button_time.Foreground = new SolidColorBrush(Color.FromRgb(21, 212, 79));
                    Button_time.BorderBrush = new SolidColorBrush(Color.FromRgb(21, 212, 79));
                }
            }
            else
            {
                Button_time.Foreground = new SolidColorBrush(Color.FromRgb(237, 17, 17));
                Button_time.BorderBrush = new SolidColorBrush(Color.FromRgb(237, 17, 17));
                Clock_name.Text = "Close";
            }
        }
        public UserControl usc = null;
        private void PackIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            MainWindow window = (MainWindow)Application.Current.MainWindow;
            window.GridMain.Children.Clear();
            UserControlRest restPage = new UserControlRest();
            window.GridMain.Children.Add(restPage);
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (User.LOGIN)
            {
                string procedure = "Comment";
               
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(procedure, connection);
                    // указываем, что команда представляет хранимую процедуру
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    // параметр для ввода имени
                    SqlParameter nameParam = new SqlParameter
                    {
                        ParameterName = "@User",
                        Value = User.UserName.ToString()
                    };
                    // добавляем параметр
                    command.Parameters.Add(nameParam);
                    SqlParameter nameParam2 = new SqlParameter
                    {
                        ParameterName = "@Rest",
                        Value = Rest_name.Content.ToString()
                    };
                    command.Parameters.Add(nameParam2);
                    Your_comment.SelectAll();
                    SqlParameter nameParam3 = new SqlParameter
                    {
                        ParameterName = "@Comment",
                        Value = Your_comment.Selection.Text
                    };
                    command.Parameters.Add(nameParam3);
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    var result = command.ExecuteNonQuery();
                }
            }
            else
            {
                LoginPage login = new LoginPage();
                login.Show();
            }
        }
    }
}

