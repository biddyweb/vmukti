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

namespace Dialer_AutoProgressive.Presentation
{
    /// <summary>
    /// Interaction logic for btnDialerPopUp.xaml
    /// </summary>
    public partial class BtnAutoDialerPopUp : UserControl
    {
        public static int Entered = 0;
        UserControl Dialer = null;
        MyDialer dialer = null;
        public BtnAutoDialerPopUp(ModulePermissions[] MyPermissions)
        {
            try
            {
                this.Dialer = new UserControl();
                InitializeComponent();
                VMuktiAPI.VMuktiHelper.RegisterEvent("SignOut").VMuktiEvent += new VMuktiAPI.VMuktiEvents.VMuktiEventHandler(BtnAutoDialerPopUp_VMuktiEvent);
                dialer = new MyDialer(MyPermissions);
                this.Dialer.Content = dialer;
                this.Dialer.Background = Brushes.SteelBlue;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "BtnAutoDialerPopUp()", "btnDialerPopUp.xaml.cs");
            }
        }
        void BtnAutoDialerPopUp_VMuktiEvent(object sender, VMuktiAPI.VMuktiEventArgs e)
        {
            try
            {
                ClosePod();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "BtnAutoDialerPopUp_VMuktiEvent()", "btnDialerPopUp.xaml.cs");
            }
        }

       
        private void btnDialer_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (Entered == 0)
                {
                    Point x = base.PointToScreen(Mouse.GetPosition(this));                  
                    new Dialer_PopUp().ShowPopUp(this.Dialer, x);
                    Entered = 1;
                }
            }
            catch(Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnDialer_MouseEnter()", "btnDialerPopUp.xaml.cs");
            }
        }
        public void ClosePod()
        {
            try
            {
                if (dialer != null)
                {
                    dialer.ClosePod();
                    dialer = null;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "PopUpDispo_MouseLeave()", "BtnDispositionRender.xaml.cs");
            }
        }
       

    }
}
