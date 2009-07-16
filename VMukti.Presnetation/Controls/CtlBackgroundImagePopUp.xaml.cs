using System;
using System.Collections.Generic;
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
using Microsoft.Win32;

namespace VMukti.Presentation.Controls
{
    /// <summary>
    /// Interaction logic for CtlBackgroundImagePopUp.xaml
    /// </summary>
    public partial class CtlBackgroundImagePopUp : UserControl
    {
        public delegate void delChangeBackground(string objstring);
        public event delChangeBackground entChangeBackground;
        //public static StringBuilder sb1;
        public CtlBackgroundImagePopUp()
        {
            try
            {
                InitializeComponent();
                btnBackgroundSetting.Click += new RoutedEventHandler(btnBackgroundSetting_Click);
                btnbg1.Click += new RoutedEventHandler(btnbg1_Click);
                btnbg2.Click += new RoutedEventHandler(btnbg2_Click);
              
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlBackGroundImagePopUP()", "Controls\\CtlBackgroundImagePopUp.xaml.cs");
            }

        }

        

        void btnbg2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                entChangeBackground(((string)(btnbg2.Tag)));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnbg2_Click()", "Controls\\CtlBackgroundImagePopUp.xaml.cs");
            }
        }

        void btnbg1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (entChangeBackground != null)
                {
                    entChangeBackground(((string)(btnbg1.Tag)));
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnbg1_Click()", "Controls\\CtlBackgroundImagePopUp.xaml.cs");
            }
        }

        void btnBackgroundSetting_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (entChangeBackground != null)
                {
                    entChangeBackground(null);
                }
                
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnBackGroundSetting_Click()", "Controls\\CtlBackgroundImagePopUp.xaml.cs");
            }

        }

        //public static StringBuilder ()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("User Is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
        //    sb.AppendLine();
        //    sb.Append("Peer Type is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString());
        //    sb.AppendLine();
        //    sb.Append("User's SuperNode is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
        //    sb.AppendLine();
        //    sb.Append("User's Machine Ip Address : " + VMuktiAPI.GetIPAddress.ClsGetIP4Address.GetIP4Address());
        //    sb.AppendLine();
        //    sb.AppendLine("----------------------------------------------------------------------------------------");
        //    return sb;
        //}
    }
}
