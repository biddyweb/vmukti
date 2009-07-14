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
using VMuktiAudio.VistaService.Business;

namespace VMuktiAudio.VistaService
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        System.Diagnostics.EventLog el = null;
        object objNetP2PRTCVista = null;
        VMuktiService.NetPeerClient npcRTCVistaClient = null;
        INetTcpRTCVistaServiceChannel ClientNetP2PRTCVistaChannel = null;
        RTCClient RClient = null;
        VMuktiService.NetPeerServer npsVistaAudio = null;

        public Window1()
        {
            InitializeComponent();
            try
            {
                el = new System.Diagnostics.EventLog("Application", Environment.MachineName, "Audio_Vista");
                this.Closing += new System.ComponentModel.CancelEventHandler(Window1_Closing);

                npsVistaAudio = new VMuktiService.NetPeerServer("net.tcp://localhost:6060/NetP2PRTCVista");
                npsVistaAudio.AddEndPoint("net.tcp://localhost:6060/NetP2PRTCVista");
                npsVistaAudio.OpenServer();

                npcRTCVistaClient = new VMuktiService.NetPeerClient();
                objNetP2PRTCVista = new clsNetTcpRTCVistaService();
                ((clsNetTcpRTCVistaService)objNetP2PRTCVista).entsvcJoin += new clsNetTcpRTCVistaService.DelsvcJoin(Window1_entsvcJoin);
                ((clsNetTcpRTCVistaService)objNetP2PRTCVista).entsvcCreateRTCClient += new clsNetTcpRTCVistaService.DelsvcCreateRTCClient(Window1_entsvcCreateRTCClient);
                ((clsNetTcpRTCVistaService)objNetP2PRTCVista).entsvcRegisterSIPPhone += new clsNetTcpRTCVistaService.DelsvcRegisterSIPPhone(Window1_entsvcRegisterSIPPhone);
                ((clsNetTcpRTCVistaService)objNetP2PRTCVista).entsvcAnswer += new clsNetTcpRTCVistaService.DelsvcAnswer(Window1_entsvcAnswer);
                ((clsNetTcpRTCVistaService)objNetP2PRTCVista).entsvcDial += new clsNetTcpRTCVistaService.DelsvcDial(Window1_entsvcDial);
                ((clsNetTcpRTCVistaService)objNetP2PRTCVista).entsvcHangup += new clsNetTcpRTCVistaService.DelsvcHangup(Window1_entsvcHangup);
                ((clsNetTcpRTCVistaService)objNetP2PRTCVista).entsvcHold += new clsNetTcpRTCVistaService.DelsvcHold(Window1_entsvcHold);
                ((clsNetTcpRTCVistaService)objNetP2PRTCVista).entsvcSendDTMF += new clsNetTcpRTCVistaService.DelsvcSendDTMF(Window1_entsvcSendDTMF);
                ((clsNetTcpRTCVistaService)objNetP2PRTCVista).entsvcTransfer += new clsNetTcpRTCVistaService.DelsvcTransfer(Window1_entsvcTransfer);
                ((clsNetTcpRTCVistaService)objNetP2PRTCVista).entsvcRTCEvent += new clsNetTcpRTCVistaService.DelsvcRTCEvent(Window1_entsvcRTCEvent);
                ((clsNetTcpRTCVistaService)objNetP2PRTCVista).entsvcUnJoin += new clsNetTcpRTCVistaService.DelsvcUnJoin(Window1_entsvcUnJoin);
                ClientNetP2PRTCVistaChannel = (INetTcpRTCVistaServiceChannel)npcRTCVistaClient.OpenClient<INetTcpRTCVistaServiceChannel>("net.tcp://localhost:6060/NetP2PRTCVista", "NetP2PRTCVistaMesh", ref objNetP2PRTCVista);
                ClientNetP2PRTCVistaChannel.svcJoin();

            }
            catch (Exception ex)
            {
                el.WriteEntry("Window1:" + ex.Message);
                el.Close();
            }
        }

        #region RTCVista WCF Events
        void Window1_entsvcJoin()
        {
        }

        void Window1_entsvcCreateRTCClient()
        {
        }

        void Window1_entsvcRegisterSIPPhone(string SIPUserName, string SIPPassword, string SIPServer)
        {
            RClient = new RTCClient(SIPUserName, SIPPassword, SIPServer);
            RClient.entstatus += new RTCClient.delStatus(RClient_entstatus);
        }

        void Window1_entsvcAnswer()
        {
            RClient.Anser();
        }

        void Window1_entsvcDial(string PhoneNo, int Channel)
        {
            RClient.Dial(PhoneNo, Channel);
        }

        void Window1_entsvcHangup(int Channel)
        {
            RClient.HangUp(Channel);
        }

        void Window1_entsvcHold(int Channel, string HoldContent)
        {
            RClient.Hold(Channel, HoldContent);
        }

        void Window1_entsvcSendDTMF(string DTMF, int Channel)
        {
            RClient.SendDTMF(DTMF, Channel);
        }

        void Window1_entsvcTransfer(string PhoneNo, int Channel)
        {
            RClient.Transfer(PhoneNo, Channel);
        }

        void Window1_entsvcRTCEvent(int ChannelId, string RTCEventName)
        {

        }

        void Window1_entsvcUnJoin()
        {
        }

        void RClient_entstatus(int ChannelId, string status)
        {
            try
            {
                switch (status)
                {
                    case "InPorgress":
                        break;

                    case "Connected":
                        ClientNetP2PRTCVistaChannel.svcRTCEvent(ChannelId, status);
                        break;

                    case "Disconnected":
                        ClientNetP2PRTCVistaChannel.svcRTCEvent(ChannelId, status);
                        break;

                    case "Incoming":
                        ClientNetP2PRTCVistaChannel.svcRTCEvent(ChannelId, status);
                        break;

                    case "Hold":
                        ClientNetP2PRTCVistaChannel.svcRTCEvent(ChannelId, status);
                        break;
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        void Window1_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                npsVistaAudio.CloseServer();
                npcRTCVistaClient.CloseClient<INetTcpRTCVistaServiceChannel>();
                ClientNetP2PRTCVistaChannel = null;
                objNetP2PRTCVista = null;
            }
            catch (Exception ex)
            {
                el.WriteEntry("Window1_Closing: " + ex.Message);
                el.Close();
            }
        }

    }
}
