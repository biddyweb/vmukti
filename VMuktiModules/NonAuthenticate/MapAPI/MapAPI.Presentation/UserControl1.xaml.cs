using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;

namespace MapAPI.Presentation
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    /// 
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }
    public partial class UserControl1 : UserControl
    {
        public UserControl1(ModulePermissions[] MyPermissions)
        {
            try
            {
                InitializeComponent();
                this.Loaded += new RoutedEventHandler(UserControl1_Loaded);
                if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "threemap.html"))
                {
                    string mapString = AppDomain.CurrentDomain.BaseDirectory + "threemap.html";
                    frmMap.Source = new Uri(mapString);
                }
                else
                {
                    MessageBox.Show("File can not be Found.");
                }
            }
            catch (Exception exp)
            {
            }
            //System.Reflection.Assembly ass=System.Reflection.Assembly.GetAssembly(typeof(UserControl1));
            //frame1.Source = new Uri(, UriKind.RelativeOrAbsolute);
        }

        void UserControl1_Loaded(object sender, RoutedEventArgs e)
        {
            if(this.Parent != null)
            {
                this.Width = ((Grid)(this.Parent)).ActualWidth;
                ((Grid)(this.Parent)).SizeChanged += new SizeChangedEventHandler(UserControl1_SizeChanged);
            }

            
        }

        void UserControl1_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Height > 0)
            {
                this.Width = ((Grid)(this.Parent)).ActualWidth;
              
            }
        }
    }
}
