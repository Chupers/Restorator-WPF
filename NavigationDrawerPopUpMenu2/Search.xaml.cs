using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Globalization;
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
    public partial class Search : UserControl
    {
        public Search()
        {
            InitializeComponent();
            string time = DateTime.Now.ToShortTimeString();
        }
        bool flag = true;
        private void Find_button_Click(object sender, RoutedEventArgs e)
        {
            Result_panel.Children.Clear();
            Find();
        }
        private void Time_Work(string time,string DBtime)
        {
            string[] vs = time.Split(':');
            string[] worktime = DBtime.Split('-');
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
                   
                }
            }
            else
            {
                flag = false;
            }
        }
        public string time;
       public string SelectItems;
        public void Find()
        {
           
            RepositoryClass repositoryClass = new RepositoryClass();
            Stack<Restoran> restorans = repositoryClass.SearchRest(Text_find.Text);
            foreach (Restoran restoran in restorans)
            {
                MemoryStream ms = new MemoryStream(restoran.imagedata);
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
                userControl1.Name_Of_Rest.Content = restoran.Name;
                time = DateTime.Now.ToShortTimeString();
                string DBTIME = restoran.WorkTime;
                if (CheckTime.IsChecked == true)
                    Time_Work(time, DBTIME);
                if (flag)
                {

                    if (SelectItems == null)
                        Result_panel.Children.Add(userControl1);
                    else
                    {
                        if (restoran.Type== SelectItems)
                            Result_panel.Children.Add(userControl1);
                    }
                }
                flag = true;
            }
        }
                
            
        

        private void SelectCombo_Selected(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            SelectItems = selectedItem.Name.ToString();
        }
    }
}
