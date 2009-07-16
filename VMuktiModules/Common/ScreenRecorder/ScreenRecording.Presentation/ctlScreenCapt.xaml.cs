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
using System.Diagnostics;
using VMuktiAPI;
using VMuktiService;
using System.ServiceModel;
using ScreenRecording.Business;
//with change.....

namespace ScreenRecording.Presentation
{
    /// <summary>
    /// Interaction logic for ctlScreenCapt.xaml
    /// </summary>
    /// 
    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }
    
    public partial class ctlScreenCapt : UserControl
    {
        //int Count;
        Process prc = null;

        public ctlScreenCapt(ModulePermissions[] MyPermissions)
        {
           
            InitializeComponent();
            VMuktiAPI.VMuktiHelper.RegisterEvent("SignOut").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(ctlScreenCapt_VMuktiEvent);
           
            try
            {
                string strCurrentPeerType = VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString();

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P)
                {
                    
                    prc = new Process();
                    prc.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "\\VideoExe\\Video.exe";
                    ClsScreenRecordingClient.OpenScreenRecordingClient();
                    int PortNo = ClsScreenRecordingClient.chHTTPScreenRecordingChannel.StreamSuperNode();
                    prc.StartInfo.Arguments =VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName + "\\" + strCurrentPeerType + "\\" + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP + "\\" + PortNo;
                    prc.Start();
                    
                    System.Threading.Thread.Sleep(8000);

                    ClsScreenRecordingClient.chHTTPScreenRecordingChannel.ReStream(PortNo,VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    
                }
                else if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap)
                {
                    
                    prc = new Process();
                    prc.StartInfo.FileName = AppDomain.CurrentDomain.BaseDirectory + "\\VideoExe\\Video.exe";
                    prc.StartInfo.Arguments = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName + "\\" + strCurrentPeerType + "\\" + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP;
                    prc.Start();
                }
            }
           
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "ctlScreenCapt()--:--ctlScreenCapt.cs--:--" + exp.Message + " :--:--");
                ClsException.LogError(exp);
                ClsException.WriteToErrorLogFile(exp);
                
            }
        }

        void ctlScreenCapt_VMuktiEvent(object sender, VMuktiEventArgs e)
        {
            ClosePod();
        }
        public void ClosePod()
        {
            ClsScreenRecordingClient.chHTTPScreenRecordingChannel.StopRecording(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);                        
            prc.Kill();
        }
    }
}
