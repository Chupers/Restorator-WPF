using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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
using System.Drawing;
using System.Configuration;


namespace Restorator
{
   
    public partial class UserControlRest : UserControl
    {
        public UserControlRest()
        {
            InitializeComponent();
        }
        //public SqlConnection connect = null;
        private void Wrap_Loaded(object sender, RoutedEventArgs e)
        {
            LoadData();
        }
        private void  LoadData()
        {
            string time = DateTime.Now.ToShortTimeString();
            RepositoryClass repositoryClass = new RepositoryClass();
            Stack<Restoran> restorans = repositoryClass.LoadRest();
            foreach(Restoran restoran in restorans) {
                        MemoryStream ms = new MemoryStream(restoran.imagedata);
                        System.Drawing.Image newImage = System.Drawing.Image.FromStream(ms);
                        UserControl1 userControl1 = new UserControl1();
                string[] vs = time.Split(':');
                string[] worktime = restoran.WorkTime.Split('-');
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
                        userControl1.Button_time.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(21, 212, 79));
                        userControl1.Button_time.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(21, 212, 79));
                    }
                }
                else
                {
                    userControl1.Button_time.Foreground = new SolidColorBrush(System.Windows.Media.Color.FromRgb(237, 17, 17));
                    userControl1.Button_time.BorderBrush = new SolidColorBrush(System.Windows.Media.Color.FromRgb(237, 17, 17));
                    userControl1.Clock_name.Text = "Close";
                }
                BitmapImage bi = new BitmapImage();
                        bi.BeginInit();
                        MemoryStream ms2 = new MemoryStream();
                        newImage.Save(ms2, System.Drawing.Imaging.ImageFormat.Bmp);
                        ms2.Seek(0, SeekOrigin.Begin);
                        bi.StreamSource = ms2;
                        bi.EndInit();
                        userControl1.im.Source = bi;
                        userControl1.Name_Of_Rest.Content = restoran.Name;
                        Wrap.Children.Add(userControl1);
                    }
            }
        }
    }
