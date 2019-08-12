using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Data.SqlClient;
using System.IO;
using System;
using System.Windows.Media;
using System.Globalization;
using System.Threading;
using System.Collections.Generic;
namespace Restorator
{
    /// <summary>
    /// Логика взаимодействия для UserControl1.xaml
    /// </summary>
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();

        }
        public void Time_Work(string time, string time_of_work)
        {
            string[] vs = time.Split(':');
            string[] worktime = time_of_work.Split('-');
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
        public  Like like;
        public SqlConnection connect = null;
        private void Grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Find();
        }
        private string ChoiceIconf(string types)
        {
           
            switch (types)
            {
                case "Ресторан":
                    return "image_exempl/010-wine.png";           
                case "Кафе":
                    return "image_exempl/050-cafe.png";
                case"Бар":
                    return "image_exempl/028-wine-1.png";
                case "Пиццерия":
                    return "image_exempl/016-pizza.png";
                case "ФастФуд":
                    return "image_exempl/009-burger.png";
                case "Кофейня":
                    return "image_exempl/012-coffee-1.png";
                default:
                    return "image_exempl/017-cutlery-1.png";

            }
        }
        public void Find()
        {

            RepositoryClass repositoryClass = new RepositoryClass();
            Stack<Restoran> restorans = repositoryClass.SearchRest(Name_Of_Rest.Content.ToString());
            foreach (Restoran restoran in restorans)
            {
                PagesofRest pagesofRest = new PagesofRest();

                pagesofRest.Images.Source = this.im.Source;
                pagesofRest.Images.HorizontalAlignment = HorizontalAlignment.Left;
                pagesofRest.Rest_name.Content = restoran.Name;
                pagesofRest.Location.Text = restoran.Street;
                pagesofRest.Discription_blok.Text = restoran.Disk;
                string type = restoran.Type;
                pagesofRest.Types.Source = new BitmapImage(new Uri(ChoiceIconf(type), UriKind.Relative));
                pagesofRest.Price.Text = restoran.Price;
                pagesofRest.time_of_work.Text = restoran.WorkTime;
                string time = DateTime.Now.ToShortTimeString();
                Time_Work(time, pagesofRest.time_of_work.Text);
                string Cordinate = restoran.Cord;
                string[] Cordinatesplit = Cordinate.Split(',');
                CultureInfo et = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
                double x = double.Parse(Cordinatesplit[0]);
                Thread.CurrentThread.CurrentCulture = et;
                CultureInfo et2 = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
                double y = double.Parse(Cordinatesplit[1]);
                Thread.CurrentThread.CurrentCulture = et;
                pagesofRest.Second_name.Text = Name_Of_Rest.Content.ToString();
                pagesofRest.Loadmap(x, y);
                MainWindow window = (MainWindow)Application.Current.MainWindow;

                window.GridMain.Children.Clear();
                window.GridMain.Children.Add(pagesofRest);


            }
        }
       

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!User.LOGIN)
            {
                LoginPage login = new LoginPage();
                login.Show();
            }
            if (User.LOGIN)
            {
                if (Convert.ToInt32(like.CheckLike(Name_Of_Rest.Content.ToString())) < 1)
                {
                    like.TakeLike(Name_Of_Rest.Content.ToString()).GetAwaiter();
                    LikeButton.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
                  
                }
                else
                {
                    LikeButton.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
                    like.DeleteLike(Name_Of_Rest.Content.ToString());
                    
                }
                CountLike.Text = like.CountLike(Name_Of_Rest.Content.ToString());
            }
            
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            
            like = new Like();
            CountLike.Text = like.CountLike(Name_Of_Rest.Content.ToString()).ToString();
            if (Convert.ToInt32(like.CheckLike(Name_Of_Rest.Content.ToString())) >= 1)
            {
                LikeButton.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }
            if (User.LOGIN && User.UserType == "Admin")
            {
                DeleteGrid.IsEnabled = true;
                DeleteGrid.Visibility = Visibility.Visible;
            }
        }

        private void Grid_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            RepositoryClass repository = new RepositoryClass();

            NotificationWindow notificationWindow = new NotificationWindow();
            if (notificationWindow.ShowDialog() == true)
                repository.DeleteRest(Name_Of_Rest.Content.ToString());


        }
    }
}
