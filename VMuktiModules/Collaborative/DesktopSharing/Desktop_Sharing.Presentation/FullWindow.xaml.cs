using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using VMuktiAPI;
using VMuktiService;

namespace Desktop_Sharing.Presentation
{
    /// <summary>
    /// Interaction logic for FullWindow.xaml
    /// </summary>
    
    public partial class FullWindow : Window
    {
        public delegate void delFSMouseMove(object sender, MouseEventArgs e);
        public delegate void delFSLeftDown(object sender, MouseButtonEventArgs e);
        public delegate void delFSLeftUp(object sender, MouseButtonEventArgs e);
        public delegate void delFSRightDown(object sender, MouseButtonEventArgs e);
        public delegate void delFSRightUp(object sender, MouseButtonEventArgs e);
        public event delFSMouseMove EntFSMouseMove;
        public event delFSLeftDown EntFSLeftDown;
        public event delFSLeftUp EntFSLeftUp;
        public event delFSRightDown EntFSRightDown;
        public event delFSRightUp EntFSRightUp;
        FullScreen objFullScreen = new FullScreen();
        public FullWindow()
        {
            try
            {
                InitializeComponent();                
                imgFullScreen.PreviewMouseMove += new MouseEventHandler(imgFullScreen_PreviewMouseMove);
                imgFullScreen.PreviewMouseLeftButtonDown += new MouseButtonEventHandler(imgFullScreen_PreviewMouseLeftButtonDown);
                imgFullScreen.PreviewMouseLeftButtonUp += new MouseButtonEventHandler(imgFullScreen_PreviewMouseLeftButtonUp);
                imgFullScreen.PreviewMouseRightButtonDown += new MouseButtonEventHandler(imgFullScreen_PreviewMouseRightButtonDown);
                imgFullScreen.PreviewMouseRightButtonUp += new MouseButtonEventHandler(imgFullScreen_PreviewMouseRightButtonUp);              
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FullWindow()", "FullWindow.xaml.cs");
            }
        }
        void imgFullScreen_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                if (EntFSMouseMove != null)
                {
                    EntFSMouseMove(sender, e);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "imgFullScreen_PreviewMouseMove", "FullWindow.xaml.cs");
            }
        }
        void imgFullScreen_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (EntFSLeftDown != null)
                {
                    EntFSLeftDown(sender, e);
                }
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "imgFullScreen_PreviewMouseLeftButtonDown", "FullWindow.xaml.cs");
            }
        }
        void imgFullScreen_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (EntFSLeftUp != null)
                {
                    EntFSLeftUp(sender, e);
                }
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "imgFullScreen_PreviewMouseLeftButtonUp", "FullWindow.xaml.cs");
            }
        }
        void imgFullScreen_PreviewMouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (EntFSRightDown != null)
                {
                    EntFSRightDown(sender, e);
                }
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "imgFullScreen_PreviewMouseRightButtonDown", "FullWindow.xaml.cs");
            }
        }
        void imgFullScreen_PreviewMouseRightButtonUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (EntFSRightUp != null)
                {
                    EntFSRightUp(sender, e);
                }
            }
            catch(Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "imgFullScreen_PreviewMouseRightButtonUp", "FullWindow.xaml.cs");
            }
        }       
        
    }
}
