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
using System.Windows.Navigation;
using System.Windows.Shapes;
using VMuktiAPI;

namespace AutoProgressivePhone.Presentation
{
    /// <summary>
    /// Interaction logic for BtnSoftphone.xaml
    /// </summary>
    public partial class BtnAutoSoftphone : UserControl
    {
        UserControl abc = null;
        ctlDialer dialer = null;
        public static int Entered = 0;
        public BtnAutoSoftphone(PeerType objLocalPeerType, string uri, ModulePermissions[] MyPermissions, string Role)
        {
            try
            {
                VMuktiAPI.VMuktiHelper.RegisterEvent("SignOut").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(BtnAutoSoftphone_VMuktiEvent);
                this.abc = new UserControl();
                InitializeComponent();
                dialer = new ctlDialer(objLocalPeerType, uri, MyPermissions, Role);
                this.abc = dialer;
                this.abc.Background = Brushes.SteelBlue;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "BtnAutoSoftphone()", "BtnSoftphone.xaml.cs");
            }
        }

        void BtnAutoSoftphone_VMuktiEvent(object sender, VMuktiEventArgs e)
        {
            ClosePod();
        }    

        private void btnSoftPhone_MouseEnter(object sender, MouseEventArgs e)
        {
            try
            {
                if (Entered == 0)
                {
                    Point x = base.PointToScreen(Mouse.GetPosition(this));                    
                    new PopUp().ShowPopUp(this.abc, x);
                    Entered = 1;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSoftPhone_MouseEnter()", "BtnSoftphone.xaml.cs");
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
