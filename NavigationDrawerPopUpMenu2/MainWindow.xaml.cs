using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Device.Location;
using System.IO;
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
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DragMove();
            LoadMain().GetAwaiter();

        }
        LoginPage login;
        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }
        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
        }
        private async Task LoadMain()
        {
            string procedure = "Main";
            string connectionString = @"Data Source=.\CHUPRIS; Initial Catalog=RESTORATOR; User Id=Admin; Password=P@$$word";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                SqlCommand command = new SqlCommand(procedure, connection);
                // указываем, что команда представляет хранимую процедуру
                command.CommandType = System.Data.CommandType.StoredProcedure;
                // параметр для ввода имени
                SqlDataReader reader = await command.ExecuteReaderAsync();
                while (reader.Read())
                {
                    MainPage pagesofRest = new MainPage();

                    byte[] data = (byte[])reader.GetValue(3);
                    MemoryStream ms = new MemoryStream(data);
                    System.Drawing.Image newImage = System.Drawing.Image.FromStream(ms);
                    BitmapImage bi = new BitmapImage();
                    bi.BeginInit();
                    MemoryStream ms2 = new MemoryStream();
                    newImage.Save(ms2, System.Drawing.Imaging.ImageFormat.Bmp);
                    ms2.Seek(0, SeekOrigin.Begin);
                    bi.StreamSource = ms2;
                    bi.EndInit();
                    pagesofRest.im.Source = bi;
                    pagesofRest.Second_name.Text = reader.GetString(1);
                    pagesofRest.Location.Text = reader.GetString(5);
                    pagesofRest.Price.Text = reader.GetString(4);
                    pagesofRest.time_of_work.Text = reader.GetString(2);
                    MainWindow window = (MainWindow)Application.Current.MainWindow;
                    window.GridMain.Children.Clear();
                    window.GridMain.Children.Add(pagesofRest);
                }
                
            }
        }
        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {   
            UserControl usc = null;
            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                case "ItemHome":
                    GridMain.Children.Clear();
                    LoadMain().GetAwaiter();

                    break;
                case "ItemCreate":
                    GridMain.Children.Clear();
                        usc = new UserControlRest();
                    GridMain.Children.Add(usc);
                    break;
                case "Nearme":
                    GridMain.Children.Clear();
                    UserControlNear near = new UserControlNear();
                    near.LoadMap(53.9084091, 27.5732671);
                    GridMain.Children.Add(near);
                    break;
                case "Find":
                    GridMain.Children.Clear();
                    usc = new Search();
                    GridMain.Children.Add(usc);
                    break;
                case "Account":
                    login = new LoginPage();
                    login.Show();
                    break;
                case "Exit":
                    if (User.LOGIN)
                    {
                        NotificationWindow notificationWindow = new NotificationWindow();
                        if (notificationWindow.ShowDialog() == true)
                        {
                            User.LOGIN = false;
                            User.UserName = "Unknow";
                            Username.Text = "Unknow";
                            this.Admin_button.Visibility = Visibility.Hidden;
                            this.Admin_button.IsEnabled = false;
                        }
                    }
                    break;
                default:
                    break;
            }
        }
        private void GridMenu_MouseLeave(object sender, MouseEventArgs e)
        {
        }
        private void CommandBinding_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            GridMain.Children.Clear();
            Admin_Page admin_ = new Admin_Page();
            GridMain.Children.Add(admin_);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}

