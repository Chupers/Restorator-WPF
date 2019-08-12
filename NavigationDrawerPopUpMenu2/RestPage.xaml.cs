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

namespace Restorator
{
    /// <summary>
    /// Логика взаимодействия для Search.xaml
    /// </summary>
    public partial class RestPage : UserControl
    {
        public RestPage ()
        {
            InitializeComponent();
        }

        public SqlConnection connect = null;
        private void Wrap_Loaded(object sender, RoutedEventArgs e)
        {
            RepositoryClass repository = new RepositoryClass();
            Stack<Restoran> restorans = repository.LoadRest();
            foreach (Restoran r in restorans)
            {
                MemoryStream ms = new MemoryStream(r.imagedata);
                System.Drawing.Image newImage = System.Drawing.Image.FromStream(ms);
                UserControl1 userControl1 = new UserControl1();
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                MemoryStream ms2 = new MemoryStream();
                newImage.Save(ms2, System.Drawing.Imaging.ImageFormat.Bmp);
                ms2.Seek(0, SeekOrigin.Begin);
                bi.StreamSource = ms2;
                bi.EndInit();
                userControl1.im.Source = bi;
                userControl1.Name_Of_Rest.Content = r.Name;
                Result_panel.Children.Add(userControl1);
            }

        }


    }
}
