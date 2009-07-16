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

namespace WebSearch.Presentation
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
    public partial class CtlWebSearch : UserControl
    {
        public CtlWebSearch(ModulePermissions[] MyPermissions)
        {
            InitializeComponent();
            frmSearch.Source = new Uri(AppDomain.CurrentDomain.BaseDirectory + "googleSearchTabed.html");
            this.Loaded += new RoutedEventHandler(CtlWebSearch_Loaded);
        }

        void CtlWebSearch_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                this.Width = ((Grid)(this.Parent)).ActualWidth-10;
                ((Grid)(this.Parent)).SizeChanged += new SizeChangedEventHandler(CtlWebSearch_SizeChanged);
            }
            catch (Exception ex)
            {
            }
        }

        void CtlWebSearch_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                this.Width = ((Grid)(this.Parent)).ActualWidth-10;
            }
            catch (Exception ex)
            {
            }
        }
    }
}
