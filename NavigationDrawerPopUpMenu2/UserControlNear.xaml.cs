using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsPresentation;

namespace Restorator
{

    public partial class UserControlNear : UserControl
    {
        
            public bool flag = true;

        public UserControlNear()
        {
            InitializeComponent();
             
        }
        public void LoadMap(double x,double y)
        {
            GMap.NET.GMaps.Instance.Mode = GMap.NET.AccessMode.ServerAndCache;
            // choose your provider here
            mapView.MapProvider = GMapProviders.GoogleMap;
            mapView.Position = new PointLatLng(x,y);
            mapView.MinZoom = 12;
            mapView.MaxZoom = 17;
            GMapMarker gMapMarker;
            RepositoryClass repository = new RepositoryClass();
            Stack<Restoran> restorans = repository.LoadRest();
            foreach (Restoran r in restorans)
            {
                string time = DateTime.Now.ToShortTimeString();
                UserControl1 userControl1 = new UserControl1();
               
                string[] vs = time.Split(':');
                string[] worktime = r.WorkTime.Split('-');
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
                       userControl1.Button_time.Foreground = new SolidColorBrush(Color.FromRgb(21, 212, 79));
                        userControl1.Button_time.BorderBrush = new SolidColorBrush(Color.FromRgb(21, 212, 79));
                    }
                }
                else
                {
                    userControl1.Button_time.Foreground = new SolidColorBrush(Color.FromRgb(237, 17, 17));
                    userControl1.Button_time.BorderBrush = new SolidColorBrush(Color.FromRgb(237, 17, 17));
                    userControl1.Clock_name.Text = "Close";
                }
                MemoryStream ms = new MemoryStream(r.imagedata);
                System.Drawing.Image newImage = System.Drawing.Image.FromStream(ms);
                
                BitmapImage bi = new BitmapImage();
                bi.BeginInit();
                MemoryStream ms2 = new MemoryStream();
                newImage.Save(ms2, System.Drawing.Imaging.ImageFormat.Bmp);
                ms2.Seek(0, SeekOrigin.Begin);
                bi.StreamSource = ms2;
                bi.EndInit();
                userControl1.im.Source = bi;
                userControl1.Name_Of_Rest.Content = r.Name;
                userControl1.MainGrid.Width = 250;
                userControl1.MainGrid.Height = 250;
                Thickness thickness = new Thickness(0, 0, 0, 0);
                userControl1.MainGrid.Margin = thickness;
                string[] cord = r.Cord.Split(',');
                CultureInfo et = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
                double Lang = double.Parse(cord[0]);
                Thread.CurrentThread.CurrentCulture = et;
                CultureInfo et2 = Thread.CurrentThread.CurrentCulture;
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
                double Lant = double.Parse(cord[1]);
                Thread.CurrentThread.CurrentCulture = et;
                gMapMarker = new GMapMarker(new PointLatLng(Lang, Lant));
                marker marker = new marker();
                marker.Stack.Children.Add(userControl1);
                marker.NameRest.Text = r.Name;
                gMapMarker.Shape = marker;
                mapView.Markers.Add(gMapMarker);
            }
            // whole world zoom
            mapView.Zoom = 17;
            if (flag)
            {
                // lets the map use the mousewheel to zoom
                mapView.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
                // lets the user drag the map
                mapView.CanDragMap = true;
                // lets the user drag the map with the left mouse button
                mapView.DragButton = MouseButton.Left;
            }
        }
    }
}
      
