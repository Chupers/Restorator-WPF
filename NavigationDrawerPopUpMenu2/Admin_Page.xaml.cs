using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для Admin_Page.xaml
    /// </summary>
    public partial class Admin_Page : UserControl
    {
        public Admin_Page()
        {
            InitializeComponent();
        }
        public bool flag = false;
        public string File_image;
        private void Get_image_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDialog = new Microsoft.Win32.OpenFileDialog();
            Nullable<bool> result = openFileDialog.ShowDialog();
            File_image = openFileDialog.FileName;
            if (File_image.Length>5)
                flag = true;
            if (flag == true)
            {
                Get_image.Background = new SolidColorBrush(Color.FromRgb(0, 255, 127));
                Get_image.BorderBrush = new SolidColorBrush(Color.FromRgb(0, 255, 127));
                Get_image.Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }   
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (flag)
                SaveFileToDatabase();      
        }
       public string SelectItems;
        private void SelectCombo_Selected(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedItem = (ComboBoxItem)comboBox.SelectedItem;
            SelectItems = selectedItem.Name.ToString();
        }
        private  void SaveFileToDatabase()
        {
            try
            {
                string filename = File_image;
                // получаем короткое имя файла для сохранения в бд
                string shortFileName = filename.Substring(filename.LastIndexOf('\\') + 1);                                     
                byte[] imageData;
                using (System.IO.FileStream fs = new System.IO.FileStream(filename, FileMode.Open))
                {
                    imageData = new byte[fs.Length];
                    fs.Read(imageData, 0, imageData.Length);
                }
                Dickript.SelectAll();
                string diskript = Dickript.Selection.Text;
                Restoran restoran = new Restoran(filename, imageData, Name.Text, SelectItems, Work_time.Text, Sity.Text, Street.Text, StartPrice.Text, diskript, Cord.Text);
                RepositoryClass repository = new RepositoryClass();
                repository.PushRest(restoran);
                Get_image.Background = new SolidColorBrush(Color.FromRgb(33, 150, 243));
                Get_image.BorderBrush = new SolidColorBrush(Color.FromRgb(33, 150, 243));
                Get_image.Foreground = new SolidColorBrush(Color.FromRgb(255, 255, 255));
            }
            catch 
            {
                MessageBox.Show("Error");
            }
        }
    }
}
