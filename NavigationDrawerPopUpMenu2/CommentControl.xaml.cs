using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для CommentControl.xaml
    /// </summary>
    public partial class CommentControl : UserControl
    {
        public CommentControl()
        {
            InitializeComponent();
        }
        public void LoadComment()
        {
            if (User.UserName == Username.Content.ToString() || User.UserType == "Admin")
            {
                DeleteGrid.IsEnabled = true;
                DeleteGrid.Visibility = Visibility.Visible;
            }
        }
        public string NameRest;
        private void DeleteGrid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            RepositoryClass repositoryClass = new RepositoryClass();
            NotificationWindow notificationWindow = new NotificationWindow();
            if (notificationWindow.ShowDialog() == true)
                repositoryClass.DeleteComment(comment.Text, NameRest);
        }
    }
}
