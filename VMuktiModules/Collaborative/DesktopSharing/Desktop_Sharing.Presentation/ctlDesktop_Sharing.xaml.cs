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
using Desktop_Sharing.Business.Service.NetP2P;
using VMuktiAPI;
using VMuktiService;
using System.Windows.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Drawing;
using System.Collections;
using System.ServiceModel;
using Desktop_Sharing.Business.Service.BasicHttp;
using Desktop_Sharing.Business.Service.DataContracts;
using Desktop_Sharing.Business.Service.MessageContract;
using System.ComponentModel;
  
namespace Desktop_Sharing.Presentation
{
    /// <summary>
    /// Interaction logic for ctlDesktop_Sharing.xaml
    /// </summary>
    /// 

    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }
    public partial class ctlDesktop_Sharing : UserControl
    {
        [DllImport("User32.dll", SetLastError = true)]
        public static extern int SendInput(int nInputs, ref INPUT pInputs, int cbsize);

        [DllImport("user32.dll")]
        public static extern IntPtr SetActiveWindow(IntPtr hWnd);

        #region for Desktop variable Declaration

        [DllImport("gdi32.dll", EntryPoint = "CreateDCA")]
        private static extern int CreateDC(string lpDriverName, string lpDeviceName, string lpOutput, string lpInitData);

        [DllImport("GDI32.dll")]
        private static extern int CreateCompatibleDC(int hDC);

        [DllImport("GDI32.dll")]
        private static extern int CreateCompatibleBitmap(int hDC, int nWidth, int nHeight);

        [DllImport("gdi32.dll", EntryPoint = "GetDeviceCaps")]
        private static extern int GetDeviceCaps(int hdc, int nIndex);

        [DllImport("GDI32.dll")]
        private static extern int SelectObject(int hDC, int hObject);

        [DllImport("GDI32.dll")]
        private static extern int BitBlt(int srchDC, int srcX, int srcY, int srcW, int srcH, int desthDC, int destX, int destY, int op);

        [DllImport("GDI32.dll")]
        private static extern int DeleteDC(int hDC);

        [DllImport("GDI32.dll")]
        private static extern int DeleteObject(int hObj);

        const int SRCCOPY = 13369376;
        private System.Drawing.Bitmap oBackground;
        private int FW, FH;
        public System.Threading.Thread t1;
        #endregion
        #region Constants for Mouse
        const int INPUT_MOUSE = 0;
        const int MOUSE_LEFTDOWN = 2;
        const int MOUSE_LEFTUP = 4;
        const int MOUSEEVENTF_RIGHTDOWN = 8;
        const int MOUSEEVENTF_RIGHT_UP = 16;
        public struct INPUT
        {   public uint type;
            public MOUSEINPUT MouseInput;
        };
        public struct MOUSEINPUT
        {
            public int MouseX;
            public int MouseY;
            public int MouseData;
            public int MouseFlag;
            public int Time;
            public IntPtr MouseExtraInfo;
        };

        INPUT myMouseInput = new INPUT();
        string[] strX;
        string[] strY;
        #endregion
        #region Keyboard
        [DllImport("user32.dll")]
        public static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, uint dwExtraInfo);

        [DllImport("User32.dll")]
        public static extern int FindWindow(string lpClassName, string lpWindowName);

        [DllImport("User32.dll")]
        public static extern int SendMessage(int hWnd, uint Msg, int wParam, int lParam);

        const byte KEYEVENTF_KEYUP = 2;
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_CLOSE = 0xF060;
        int value;
        #region Alphabates
        const byte VK_Space = 32;
        const byte VK_Enter = 13;
        const byte VK_BackSpace = 8;
        const byte VK_Tab = 9;
        const byte VK_Esc = 27;

        const byte VK_A = 65;
        const byte VK_B = 66;
        const byte VK_C = 67;
        const byte VK_D = 68;
        const byte VK_E = 69;
        const byte VK_F = 70;
        const byte VK_G = 71;
        const byte VK_H = 72;
        const byte VK_I = 73;
        const byte VK_J = 74;
        const byte VK_K = 75;
        const byte VK_L = 76;
        const byte VK_M = 77;
        const byte VK_N = 78;
        const byte VK_O = 79;
        const byte VK_P = 80;
        const byte VK_Q = 81;
        const byte VK_R = 82;
        const byte VK_S = 83;
        const byte VK_T = 84;
        const byte VK_U = 85;
        const byte VK_V = 86;
        const byte VK_W = 87;
        const byte VK_X = 88;
        const byte VK_Y = 89;
        const byte VK_Z = 90;

        const byte VK_D0 = 48;
        const byte VK_D1 = 49;
        const byte VK_D2 = 50;
        const byte VK_D3 = 51;
        const byte VK_D4 = 52;
        const byte VK_D5 = 53;
        const byte VK_D6 = 54;
        const byte VK_D7 = 55;
        const byte VK_D8 = 56;
        const byte VK_D9 = 57;
        #endregion

        #region NumberPad

        const byte VK_Multiple = 106;
        const byte VK_Pluse = 107;
        const byte VK_Minus = 109;
        const byte VK_Dot = 110;
        const byte VK_Divide = 111;

        const byte VK_Num0 = 96;
        const byte VK_Num1 = 97;
        const byte VK_Num2 = 98;
        const byte VK_Num3 = 99;
        const byte VK_Num4 = 100;
        const byte VK_Num5 = 101;
        const byte VK_Num6 = 102;
        const byte VK_Num7 = 103;
        const byte VK_Num8 = 104;
        const byte VK_Num9 = 105;
        #endregion

        #region Function Keys
        const byte VK_F1 = 112;
        const byte VK_F2 = 113;
        const byte VK_F3 = 114;
        const byte VK_F4 = 115;
        const byte VK_F5 = 116;
        const byte VK_F6 = 117;
        const byte VK_F7 = 118;
        const byte VK_F8 = 119;
        const byte VK_F9 = 120;
        const byte VK_F10 = 121;
        const byte VK_F11 = 122;
        const byte VK_F12 = 123;
        #endregion

        #region Other Keys
        const byte VK_SemiColon = 186;
        const byte VK_Equals = 187;
        const byte VK_COMMA = 188;
        const byte VK_OemMinus = 189;
        const byte VK_OemPeriod = 190;
        const byte VK_OemQuestion = 191;
        const byte VK_OemTidle = 192;
        const byte VK_OpenSquareBracket = 219;
        const byte VK_OemPipe = 220;
        const byte VK_CloseSquareBracket = 221;
        const byte VK_SingleCote = 222;
        #endregion

        #region other Useful Keys
        const byte VK_Left_Arrow = 37;
        const byte VK_Up_Arrow = 38;
        const byte VK_Right_Arrow = 39;
        const byte VK_Down_Arrow = 40;
        const byte VK_Insert = 45;
        const byte VK_Delete = 46;
        const byte VK_End = 35;
        const byte VK_Home = 36;
        const byte VK_Page_Up = 33;
        const byte VK_Page_Down = 34;
        const byte VK_Scroll_Lock = 145;
        const byte VK_Num_Lock = 144;
        const byte VK_Caps_Lock = 20;
        #endregion

        #region SystemKeys
        const byte VK_Ctrl = 17;
        const byte VK_Shift = 16;
        const byte VK_Alter = 18;
        const byte VK_LWin = 91;
        const byte VK_RWin = 92;
        const byte VK_Apps = 93;
        #endregion


        bool boolshift;
        bool boolCtrl;
        bool boolAlt;
        bool boolWindows;

        System.Windows.Threading.DispatcherTimer KeyBoardTimer = new DispatcherTimer();

        #endregion

        object objNetTcpDesktop;
        INetTcpDesktopChannel channelNetTcp;
        object objHttpDesktop;
        IHttpDesktop channelHttp;
        ctlUser_Desktop objUser_Desktop;

        public delegate void DelGetUserList(List<string> objData);
        public DelGetUserList objGetUserList;

        public delegate void DelSendMessage(List<object> lstData);
        public DelSendMessage objDelSendMsg;

        public delegate void DelRemoveUser(List<object> objData);
        public DelRemoveUser objRemoveUser;

        public delegate void DelGetMessage(clsGetMessage objGetMsg1);
        public DelGetMessage objDelGetMsg;

        public delegate void DelDisAllowView(List<object> lstData);
        public DelDisAllowView objDisAllowView;

        public delegate void DelStopControl();
        public DelStopControl objStopControl;

        public delegate void DelSendBlock(List<object> lstData);
        public DelSendBlock objDelSendBlock;

        Hashtable hashID_Name;

        bool DoWork;

       // System.Threading.Thread ThrHostDesktop = null;

        int temp;
        int tempcounter;
        public string strUri;
        bool ImgSize;
        string strSelectedUser;
        string strFullScreen;
        bool blControl;
        bool blFullScreen;
        int Shared;

        clsMessageContract objContractMouse;     
        
        #region dhaval
        FullWindow objFullWindow = new FullWindow();
        #endregion dhaval
        FullScreen objFullScreen = new FullScreen();
        List<string> lstName = new List<string>();
        List<string> lstDisAllowControl = new List<string>();       
        List<string> lstDisAllowView = new List<string>();       

        DispatcherTimer dispTimer = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Normal);
        private BackgroundWorker m_asyncSendImage = new BackgroundWorker();
        BackgroundWorker bgHostService;
        System.Threading.Thread thGlobalVariable;

        public ctlDesktop_Sharing(PeerType PeerType, string uri, ModulePermissions[] MyPermissions, string Role)
        {
            try
            {
                InitializeComponent();
                thGlobalVariable = new System.Threading.Thread(new System.Threading.ThreadStart(GlobalVariable));
                thGlobalVariable.Start();

                bgHostService = new BackgroundWorker();

                VMuktiAPI.VMuktiHelper.RegisterEvent("SignOut").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(ctlDesktop_Sharing_VMuktiEvent);
                
                //objGetUserList = new DelGetUserList(objDelGetUserList);
                //objDelSendMsg = new DelSendMessage(delSendMessage);
                //objRemoveUser = new DelRemoveUser(delRemoveUser);
                //objDelGetMsg = new DelGetMessage(delGetMessage);
                //objDisAllowView = new DelDisAllowView(delDisAllowView);
                //objStopControl = new DelStopControl(delStopControl);                
                
                myMouseInput.type = INPUT_MOUSE;
                KeyBoardTimer.Interval = new TimeSpan(0, 0, 0, 0, 0);
                KeyBoardTimer.Tick += new EventHandler(KeyBoardTimer_Tick);
                objFullWindow.txtInp.PreviewKeyDown += new KeyEventHandler(txtInp_PreviewKeyDown);             

                btnView.Tag = 0;
                btnControl.Tag = 0;

                this.Loaded += new RoutedEventHandler(ctlDesktop_Sharing_Loaded);
                btnView.Click += new RoutedEventHandler(btnView_Click);
                btnControl.Click += new RoutedEventHandler(btnControl_Click);

               // ThrHostDesktop = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(HostDesktopService));

                bgHostService.DoWork += new DoWorkEventHandler(bgHostService_DoWork);
                List<object> lstParams = new List<object>();
                lstParams.Add(PeerType);
                lstParams.Add(uri);
                lstParams.Add(MyPermissions);
               // ThrHostDesktop.Start(lstParams);
                bgHostService.RunWorkerAsync(lstParams);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlDesktop_Sharing", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        #region Global Variable Initialize

        void GlobalVariable()
        {
            try
            {
                objNetTcpDesktop = new clsNetTcpDesktop();
                objHttpDesktop = new clsHttpDesktop();
                hashID_Name = new Hashtable();

                DoWork = true;

                objGetUserList = new DelGetUserList(objDelGetUserList);
                objDelSendMsg = new DelSendMessage(delSendMessage);
                objRemoveUser = new DelRemoveUser(delRemoveUser);
                objDelGetMsg = new DelGetMessage(delGetMessage);
                objDisAllowView = new DelDisAllowView(delDisAllowView);
                objStopControl = new DelStopControl(delStopControl);                
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GlobalVariable", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        #endregion


        #region BWHostService

        void bgHostService_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                List<object> lstTempObj = (List<object>)e.Argument;
                strUri = lstTempObj[1].ToString();

                if ((VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.NodeWithNetP2P || (VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.BootStrap || (VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.SuperNode)
                {
                    NetPeerClient npcDesktop = new NetPeerClient();
                    ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcJoin += new clsNetTcpDesktop.delsvcJoin(ctlDesktop_Sharing_EntsvcJoin);
                    ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcSendMessage += new clsNetTcpDesktop.delsvcSendMessage(ctlDesktop_Sharing_EntsvcSendMessage);
                    ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcGetUserList += new clsNetTcpDesktop.delsvcGetUserList(ctlDesktop_Sharing_EntsvcGetUserList);
                    ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcSetUserList += new clsNetTcpDesktop.delsvcSetUserList(ctlDesktop_Sharing_EntsvcSetUserList);
                    ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcSelectedDesktop += new clsNetTcpDesktop.delsvcSelectedDesktop(ctlDesktop_Sharing_EntsvcSelectedDesktop);
                    ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcStopControl += new clsNetTcpDesktop.delsvcStopControl(ctlDesktop_Sharing_EntsvcStopControl);
                    ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcSendXY += new clsNetTcpDesktop.delsvcSendXY(ctlDesktop_Sharing_EntsvcSendXY);
                    ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcBtnDown += new clsNetTcpDesktop.delsvcBtnDown(ctlDesktop_Sharing_EntsvcBtnDown);
                    ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcBtnUp += new clsNetTcpDesktop.delsvcBtnUp(ctlDesktop_Sharing_EntsvcBtnUp);
                    ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcSendKey += new clsNetTcpDesktop.delsvcSendKey(ctlDesktop_Sharing_EntsvcSendKey);
                    ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcAllowView += new clsNetTcpDesktop.delsvcAllowView(ctlDesktop_Sharing_EntsvcAllowView);
                    ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcAllowControl += new clsNetTcpDesktop.delsvcAllowControl(ctlDesktop_Sharing_EntsvcAllowControl);
                    ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcUnJoin += new clsNetTcpDesktop.delsvcUnJoin(ctlDesktop_Sharing_EntsvcUnJoin);

                    channelNetTcp = (INetTcpDesktopChannel)npcDesktop.OpenClient<INetTcpDesktopChannel>(strUri, strUri.ToString().Split(':')[2].Split('/')[2], ref objNetTcpDesktop);

                    while (temp < 20)
                    {
                        try
                        {
                            #region MsgContract
                            clsMessageContract objMsgContract = new clsMessageContract();
                            objMsgContract.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                            objMsgContract.stremImage = new MemoryStream();
                            #endregion MsgContract
                            channelNetTcp.svcJoin(objMsgContract);
                            temp = 20;
                            #region MsgContract
                            clsMessageContract objContractGetUserList = new clsMessageContract();
                            objContractGetUserList.stremImage = new MemoryStream();
                            objContractGetUserList.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                            objContractGetUserList.strType = "Get";
                            channelNetTcp.svcGetUserList(objContractGetUserList);
                            #endregion MsgContract
                            fncStartSendingImage();
                        }
                        catch
                        {
                            temp++;
                            System.Threading.Thread.Sleep(1000);
                        }
                    }
                }
                else
                {
                    BasicHttpClient bhcDesktop = new BasicHttpClient();
                    bhcDesktop.NewBasicHttpBinding().TransferMode = TransferMode.Streamed;
                    channelHttp = (IHttpDesktop)bhcDesktop.OpenClient<IHttpDesktop>(strUri);
                    while (tempcounter < 20)
                    {
                        try
                        {
                            #region MsgContract
                            clsMessageContract objMsgContract = new clsMessageContract();
                            objMsgContract.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                            objMsgContract.stremImage = new MemoryStream();
                            #endregion MsgContract
                            channelHttp.svcJoin(objMsgContract);
                            tempcounter = 20;
                            #region MsgContract
                            clsMessageContract objContractGetUserList = new clsMessageContract();
                            objContractGetUserList.stremImage = new MemoryStream();
                            objContractGetUserList.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                            objContractGetUserList.strType = "Get";
                            #endregion MsgContract
                            channelHttp.svcGetUserList(objContractGetUserList);
                            fncStartSendingImage();
                        }
                        catch
                        {
                            tempcounter++;
                            System.Threading.Thread.Sleep(1000);
                        }
                    }

                    dispTimer.Interval = TimeSpan.FromSeconds(2);
                    dispTimer.Tick += new EventHandler(dispTimer_Tick);
                    dispTimer.Start();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "bgHostService_DoWork", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        #endregion

        void ctlDesktop_Sharing_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                ((Grid)this.Parent).SizeChanged += new SizeChangedEventHandler(ctlDesktop_Sharing_SizeChanged);
                ctlDesktop_Sharing_SizeChanged(null, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlDesktop_Sharing_Loaded", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        void ctlDesktop_Sharing_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (e == null)
                {
                    ((UserControl)((Grid)((ScrollViewer)cnvDesktops.Parent).Parent).Parent).Width = ((Grid)((UserControl)((Grid)((ScrollViewer)cnvDesktops.Parent).Parent).Parent).Parent).Width;
                }
                else
                {
                    ((UserControl)((Grid)((ScrollViewer)cnvDesktops.Parent).Parent).Parent).Width = e.NewSize.Width;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlDesktop_Sharing_SizeChanged", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        #region Hosting Service for Desktop Sharing & Controling

        public void HostDesktopService(object lstParams)
        {
            try
            {
                List<object> lstTempObj = (List<object>)lstParams;
                strUri = lstTempObj[1].ToString();

                if ((VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.NodeWithNetP2P || (VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.BootStrap || (VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.SuperNode)
                {
                    NetPeerClient npcDesktop = new NetPeerClient();
                    ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcJoin += new clsNetTcpDesktop.delsvcJoin(ctlDesktop_Sharing_EntsvcJoin);
                    ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcSendMessage += new clsNetTcpDesktop.delsvcSendMessage(ctlDesktop_Sharing_EntsvcSendMessage);
                    ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcGetUserList += new clsNetTcpDesktop.delsvcGetUserList(ctlDesktop_Sharing_EntsvcGetUserList);
                    ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcSetUserList += new clsNetTcpDesktop.delsvcSetUserList(ctlDesktop_Sharing_EntsvcSetUserList);
                    ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcSelectedDesktop += new clsNetTcpDesktop.delsvcSelectedDesktop(ctlDesktop_Sharing_EntsvcSelectedDesktop);
                    ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcStopControl += new clsNetTcpDesktop.delsvcStopControl(ctlDesktop_Sharing_EntsvcStopControl);
                    ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcSendXY += new clsNetTcpDesktop.delsvcSendXY(ctlDesktop_Sharing_EntsvcSendXY);
                    ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcBtnDown += new clsNetTcpDesktop.delsvcBtnDown(ctlDesktop_Sharing_EntsvcBtnDown);
                    ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcBtnUp += new clsNetTcpDesktop.delsvcBtnUp(ctlDesktop_Sharing_EntsvcBtnUp);
                    ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcSendKey += new clsNetTcpDesktop.delsvcSendKey(ctlDesktop_Sharing_EntsvcSendKey);
                    ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcAllowView += new clsNetTcpDesktop.delsvcAllowView(ctlDesktop_Sharing_EntsvcAllowView);
                    ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcAllowControl += new clsNetTcpDesktop.delsvcAllowControl(ctlDesktop_Sharing_EntsvcAllowControl);
                    ((clsNetTcpDesktop)objNetTcpDesktop).EntsvcUnJoin += new clsNetTcpDesktop.delsvcUnJoin(ctlDesktop_Sharing_EntsvcUnJoin);

                    channelNetTcp = (INetTcpDesktopChannel)npcDesktop.OpenClient<INetTcpDesktopChannel>(strUri, strUri.ToString().Split(':')[2].Split('/')[2], ref objNetTcpDesktop);

                    while (temp < 20)
                    {
                        try
                        {
                            #region MsgContract
                            clsMessageContract objMsgContract = new clsMessageContract();
                            objMsgContract.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                            objMsgContract.stremImage = new MemoryStream();
                            #endregion MsgContract
                            channelNetTcp.svcJoin(objMsgContract);
                            temp = 20;
                            #region MsgContract
                            clsMessageContract objContractGetUserList = new clsMessageContract();
                            objContractGetUserList.stremImage = new MemoryStream();
                            objContractGetUserList.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                            objContractGetUserList.strType = "Get";
                            channelNetTcp.svcGetUserList(objContractGetUserList);
                            #endregion MsgContract
                            fncStartSendingImage();
                        }
                        catch
                        {
                            temp++;
                            System.Threading.Thread.Sleep(1000);
                        }
                    }
                }
                else
                {
                    BasicHttpClient bhcDesktop = new BasicHttpClient();
                    bhcDesktop.NewBasicHttpBinding().TransferMode = TransferMode.Streamed;
                    channelHttp = (IHttpDesktop)bhcDesktop.OpenClient<IHttpDesktop>(strUri);
                    while (tempcounter < 20)
                    {
                        try
                        {
                             #region MsgContract
                            clsMessageContract objMsgContract = new clsMessageContract();
                            objMsgContract.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                            objMsgContract.stremImage = new MemoryStream();
                            #endregion MsgContract
                            channelHttp.svcJoin(objMsgContract);
                            tempcounter = 20;
                            #region MsgContract
                            clsMessageContract objContractGetUserList = new clsMessageContract();
                            objContractGetUserList.stremImage = new MemoryStream();
                            objContractGetUserList.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                            objContractGetUserList.strType = "Get";
                            #endregion MsgContract                           
                            channelHttp.svcGetUserList(objContractGetUserList);
                            fncStartSendingImage();
                        }
                        catch
                        {
                            tempcounter++;
                            System.Threading.Thread.Sleep(1000);
                        }
                    }

                    dispTimer.Interval = TimeSpan.FromSeconds(2);
                    dispTimer.Tick += new EventHandler(dispTimer_Tick);
                    dispTimer.Start();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "HostDesktopService", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        #endregion

        #region Sending PrintScreen @ Regular Intervals
        protected void CaptureScreen()
        {
            try
            {
                int hSDC;
                int hMDC;
                int hBMP;
                int hBMPOld;
                int r;
                hSDC = CreateDC("DISPLAY", "", "", "");
                hMDC = CreateCompatibleDC(hSDC);
                FW = GetDeviceCaps(hSDC, 8);
                FH = GetDeviceCaps(hSDC, 10);
                hBMP = CreateCompatibleBitmap(hSDC, FW, FH);
                hBMPOld = SelectObject(hMDC, hBMP);
                r = BitBlt(hMDC, 0, 0, FW, FH, hSDC, 0, 0, 13369376);
                hBMP = SelectObject(hMDC, hBMPOld);
                r = DeleteDC(hSDC);
                r = DeleteDC(hMDC);

                oBackground = System.Drawing.Image.FromHbitmap(new IntPtr(hBMP));
                DeleteObject(hBMP);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CaptureScreen", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        #endregion
        
        #region Delegates , Functions and Cross Events

        void fncStartSendingImage()
        {
            try
            {
                m_asyncSendImage.WorkerReportsProgress = true;
                m_asyncSendImage.WorkerSupportsCancellation = true;
                m_asyncSendImage.RunWorkerCompleted+=new RunWorkerCompletedEventHandler(m_asyncSendImage_RunWorkerCompleted);
                m_asyncSendImage.ProgressChanged+=new ProgressChangedEventHandler(m_asyncSendImage_ProgressChanged);
                m_asyncSendImage.DoWork+=new DoWorkEventHandler(m_asyncSendImage_DoWork);
                
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncStartSendingImage", "ctlDesktop_Sharing.xaml.cs");
            }
        }
        #region send Image Worker
        void m_asyncSendImage_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
        }
        void m_asyncSendImage_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }
        void m_asyncSendImage_DoWork(object sender, DoWorkEventArgs e)
        {
            while (true)
            {
                try
                {
                    if (DoWork)
                    {
                        CaptureScreen();
                        BitmapImage bmp = new BitmapImage();
                        MemoryStream mms = new MemoryStream();
                        MemoryStream scrStream = new MemoryStream();
                        System.Drawing.Image othumb;
                        if (ImgSize == false)
                        {
                            othumb = new Bitmap(124, 124, oBackground.PixelFormat);
                        }
                        else
                        {
                            othumb = new Bitmap(1024, 768, oBackground.PixelFormat);
                        }
                        Graphics oGraphic = Graphics.FromImage(othumb);
                        oGraphic.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.Default;
                        oGraphic.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

                        System.Drawing.Rectangle orect;
                        if (ImgSize == false)
                        {
                            orect = new System.Drawing.Rectangle(0, 0, 124, 124);
                        }
                        else
                        {
                            orect = new System.Drawing.Rectangle(0, 0, 1024, 768);
                        }
                        oGraphic.DrawImage(oBackground, orect);

                        othumb.Save(mms, System.Drawing.Imaging.ImageFormat.Jpeg);
                        oBackground.Dispose();                       
                       
                        byte[] myBytes = mms.ToArray();       //Image

                        if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
                        {

                            scrStream.Write(myBytes, 0, myBytes.Length);      //Inserting Image to MemoryStream                          
                            scrStream.Position = 0;
                            #region MsgContract
                            clsMessageContract objContractStream = new clsMessageContract();
                            objContractStream.stremImage = scrStream;
                            objContractStream.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                            #endregion MsgContract
                            if (channelNetTcp != null)
                            {
                                channelNetTcp.svcSendMessage(objContractStream);
                            }
                        }
                        else
                        {
                            scrStream.Write(myBytes, 0, myBytes.Length);      //Inserting Image to MemoryStream  
                            scrStream.Position = 0;
                            #region MsgContract
                            clsMessageContract objContractStream = new clsMessageContract();
                            objContractStream.stremImage = scrStream;
                            objContractStream.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                            #endregion MsgContract
                            if (channelHttp != null)
                            {
                                channelHttp.svcSendMessage(objContractStream);
                            }
                        }
                    }

                    System.Threading.Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    if (ex.Message != "Thread was being aborted.")
                    {
                        VMuktiHelper.ExceptionHandler(ex, "m_asyncSendImage_DoWork", "ctlDesktop_Sharing.xaml.cs");
                    }
                }
            }        
        }
        #endregion send Image Worker
        
        void objDelGetUserList(List<string> lstData)
        {
            try
            {
                int i = 0;
                i = cnvDesktops.Children.Count;

                if (i > 0)
                {
                    for (int j = 0; j < lstData.Count; j++)
                    {
                        bool flag = true;
                        for (int k = 0; k < cnvDesktops.Children.Count; k++)
                        {
                            if (((ctlUser_Desktop)cnvDesktops.Children[k]).lblUName.Content.ToString() == lstData[j].ToString())
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            objUser_Desktop = new ctlUser_Desktop(lstData[j].ToString());
                            cnvDesktops.Children.Add(objUser_Desktop);
                            objUser_Desktop.EntSelected += new ctlUser_Desktop.delSelected(objUser_Desktop_EntSelected);
                            objUser_Desktop.EntRemoveUser+=new ctlUser_Desktop.DelRemoveUser(objUser_Desktop_EntRemoveUser);
                            objUser_Desktop.EntFullScreen+=new ctlUser_Desktop.DelFullScreen(objUser_Desktop_EntFullScreen);
                        }
                    }
                }
                else if(i==0)
                {
                    objUser_Desktop = new ctlUser_Desktop(lstData[0].ToString());
                    cnvDesktops.Children.Add(objUser_Desktop);
                    objUser_Desktop.EntSelected +=new ctlUser_Desktop.delSelected(objUser_Desktop_EntSelected);
                    objUser_Desktop.EntRemoveUser += new ctlUser_Desktop.DelRemoveUser(objUser_Desktop_EntRemoveUser);
                    objUser_Desktop.EntFullScreen += new ctlUser_Desktop.DelFullScreen(objUser_Desktop_EntFullScreen);
                    
                    if (m_asyncSendImage.IsBusy)
                    {
                        m_asyncSendImage.CancelAsync();
                    }
                    else
                    {
                        m_asyncSendImage.RunWorkerAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "objDelGetUserList", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        void objUser_Desktop_EntFullScreen(string uName)
        {
            try
            {
                if (!blFullScreen)
                {
                    strFullScreen = uName;
                    objFullWindow.Show();                   
                    blFullScreen = true;
                }
                else
                {
                    MessageBox.Show("One Desktop is running in Full Screen Mode");
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "objUser_Desktop_EntFullScreen", "ctlDesktop_Sharing.xaml.cs");

            }
        }

        void objFullScreen_Closed(object sender, EventArgs e)
        {
            try
            {
                blFullScreen = false;
                strFullScreen = string.Empty;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "objFullScreen_Closed", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        void objUser_Desktop_EntRemoveUser(string UName)
        {
            try
            {
                List<object> lstData = new List<object>();
                lstData.Add(UName);
                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objRemoveUser, lstData);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "objUser_Desktop_EntRemoveUser", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        void objUser_Desktop_EntSelected(string uName)
        {
            try
            {
                bool flg = true;
                
                for (int i = 0; i < lstDisAllowView.Count; i++)
                {
                    if (lstDisAllowView[i] == uName)
                    {
                        flg = false;
                        break;
                    }
                }
                
                for (int i = 0; i < lstDisAllowControl.Count; i++)
                {
                    if (lstDisAllowControl[i] == uName)
                    {
                        flg = false;
                        MessageBox.Show(uName + " is not Allowing You To Control his/her Desktop", "Desktop Sharing Says: ");
                        break;
                    }
                }
                if (flg)  
                {
                    strSelectedUser = uName;
                    myViewer.Visibility = Visibility.Collapsed;
                    cnvDesktops.Visibility = Visibility.Collapsed;
                    lblUser_Desktop.Visibility = Visibility.Visible;
                    lblUser_Desktop.Content = (uName + "'s  Desktop").ToUpper();
                    #region dhaval
                    objFullWindow.EntFSMouseMove+=new FullWindow.delFSMouseMove(objFullWindow_EntFSMouseMove);
                    objFullWindow.EntFSLeftDown+=new FullWindow.delFSLeftDown(objFullWindow_EntFSLeftDown);
                    objFullWindow.EntFSLeftUp+=new FullWindow.delFSLeftUp(objFullWindow_EntFSLeftUp);
                    objFullWindow.EntFSRightDown+=new FullWindow.delFSRightDown(objFullWindow_EntFSRightDown);
                    objFullWindow.EntFSRightUp+=new FullWindow.delFSRightUp(objFullWindow_EntFSRightUp);
                    #endregion dhaval                    
                    blControl = true;
                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
                    {
                        #region MsgContract
                        clsMessageContract objSelectedUser = new clsMessageContract();
                        objSelectedUser.stremImage=new MemoryStream();
                        objSelectedUser.strFrom=strSelectedUser;
                        #endregion MsgContract
                        channelNetTcp.svcSelectedDesktop(objSelectedUser);
                    }
                    else
                    {
                        #region MsgContract
                        clsMessageContract objSelectedUser = new clsMessageContract();
                        objSelectedUser.stremImage = new MemoryStream();
                        objSelectedUser.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                        objSelectedUser.strTo = uName;
                        #endregion MsgContract
                        channelHttp.svcSelectedDesktop(objSelectedUser);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "objUser_Desktop_EntSelected", "ctlDesktop_Sharing.xaml.cs");
            }
        }
        void delSendMessage(List<object> lstData)
        {
            try
            {
                if (!blControl)
                {
                    for (int i = 0; i < cnvDesktops.Children.Count; i++)
                    {
                        if (((ctlUser_Desktop)cnvDesktops.Children[i]).lblUName.Content.ToString() == lstData[0].ToString())
                        {
                            if (lstData[0].ToString() == strFullScreen)
                            {
                                BitmapImage bmiFull = new BitmapImage();
                                bmiFull.BeginInit();
                                bmiFull.StreamSource = new MemoryStream((byte[])lstData[1]);
                                bmiFull.EndInit();
                                #region dhaval                               
                                objFullWindow.imgFullScreen.Source = bmiFull;
                                #endregion dhaval                               
                            }
                            else
                            {
                                BitmapImage bmi = new BitmapImage();
                                bmi.BeginInit();
                                bmi.StreamSource = new MemoryStream((byte[])lstData[1]);
                                bmi.EndInit();
                                ((ctlUser_Desktop)cnvDesktops.Children[i]).picUserVideo.Source = bmi;
                            }
                        }
                    }
                }
                else
                {
                    if (lstData[0].ToString() == strSelectedUser)
                    {
                        BitmapImage bmi = new BitmapImage();
                        bmi.BeginInit();
                        bmi.StreamSource = new MemoryStream((byte[])lstData[1]);
                        bmi.EndInit();
                        #region dhaval
                        this.Visibility = Visibility.Collapsed;
                        objFullWindow.imgFullScreen.Source = bmi;                        
                        objFullWindow.Show();
                        if (Shared == 0)
                        {
                            btnControl_Click(null, null);
                            Shared = 1;
                        }                        
                        #endregion dhaval
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "delSendMessage", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        void delRemoveUser(List<object> lstData)
        {
            try
            {
                for (int i = 0; i < cnvDesktops.Children.Count; i++)
                {
                    if (((ctlUser_Desktop)cnvDesktops.Children[i]).lblUName.Content.ToString() == lstData[0].ToString())
                    {
                        cnvDesktops.Children.RemoveAt(i);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "delRemoveUser", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        void delDisAllowView(List<object> lstData)
        {
            try
            {
                for (int i = 0; i < cnvDesktops.Children.Count; i++)
                {
                    if (((ctlUser_Desktop)cnvDesktops.Children[i]).lblUName.Content.ToString() == lstData[0].ToString())
                    {
                        MessageBox.Show(lstData[0].ToString() + "does not allow you to see his Desktop");
                        btnStop_Click(null, null);
                        BitmapImage bmi = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "/Modules/DesktopSharing/Control/no_signal.JPG", UriKind.RelativeOrAbsolute));
                        ((ctlUser_Desktop)cnvDesktops.Children[i]).picUserVideo.Source = bmi;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "delDisAllowView", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        void delStopControl()
        {
            try
            {
                btnStop_Click(null, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "delStopControl", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        void ctlDesktop_Sharing_VMuktiEvent(object sender, VMuktiEventArgs e)
        {
            try
            {
                ClosePod();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlDesktop_Sharing_VMuktiEvent", "ctlDesktop_Sharing.xaml.cs");
            }
        }
        
        public void ClosePod()
        {
            try
            {
                if (dispTimer != null)
                {
                    dispTimer.Stop();
                }
                if (channelNetTcp != null && channelNetTcp.State == CommunicationState.Opened)
                {
                    #region MsgContract
                    clsMessageContract objContract = new clsMessageContract();
                    objContract.stremImage = new MemoryStream();
                    objContract.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                    #endregion MsgContract
                    channelNetTcp.svcUnJoin(objContract);
                    
                    
                }
                else
                {
                    #region MsgContract
                    clsMessageContract objContract = new clsMessageContract();
                    objContract.stremImage = new MemoryStream();
                    objContract.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                    #endregion MsgContract
                    channelHttp.svcUnJoin(objContract);                    
                    channelHttp = null;
                }

                if (channelNetTcp != null)
                {
                    channelNetTcp.Close();
                    channelNetTcp.Dispose();
                    channelNetTcp = null;
                }

                cnvDesktops.Children.Clear();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ClosePod", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        string fncStreamToString(Stream streamInput)
        {
            try
            {
                byte[] byteArry = fncStreamToByteArry(streamInput);
                //convert byte[] to string
                char[] buffer = new char[byteArry.Length];
                
                for (int j = 0; j < byteArry.Length; j++)
                {
                    buffer[j] = (char)byteArry[j];
                }
                return new string(buffer);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncStreamToString", "ctlDesktop_Sharing.xaml.cs");
                return null;
            }
        }
        Stream fncStringToStream(string strInput)
        {
            try
            {
                int length = strInput.Length;
                byte[] resultBytes = new byte[length];
                
                for (int i = 0; i < length; i++)
                {
                    resultBytes[i] = (byte)strInput[i];
                }

                Stream mmsConvert = new MemoryStream(resultBytes);
					 mmsConvert.Position = 0;
                return mmsConvert;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncStringToStream", "ctlDesktop_Sharing.xaml.cs");
                return null;
            }
        }
        byte[] fncStreamToByteArry(Stream streamInput)
        {
            try
            {
                List<byte> myBytes = new List<byte>();
                int num;
                while ((num = streamInput.ReadByte()) != -1)
                {
                    myBytes.Add((byte)num);
                }
                return myBytes.ToArray();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "fncStreamToByteArry", "ctlDesktop_Sharing.xaml.cs");
                return null;
            }
        }
        void delGetMessage(clsGetMessage objGetMsg)
        {
            try
            {
               
                string str = objGetMsg.id.ToString();
                switch(str)
                {
                    case "1":
                        
                        #region GetUserList Messege
                        // GetUserList Messege
                        try
                        {
                           
                            string uName = objGetMsg.strFrom;

                            if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName != uName)
                            {

                                bool flag = true;
                                for (int j = 0; j < lstName.Count; j++)
                                {
                                    if (lstName[j] == uName)
                                    {
                                        flag = false;
                                        break;
                                    }
                                }
                                if (flag)
                                {
                                    lstName.Add(uName);
                                }
                                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objGetUserList, lstName);

                                if (channelHttp != null)
                                {
                                    #region MsgContract
                                    clsMessageContract objContarctSetUserlist = new clsMessageContract();
                                    objContarctSetUserlist.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                                    objContarctSetUserlist.strType = "Set";
                                    objContarctSetUserlist.stremImage = new MemoryStream();
                                    #endregion MsgContract
                                    channelHttp.svcSetUserList(objContarctSetUserlist);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            VMuktiHelper.ExceptionHandler(ex, "delGetMessage", "ctlDesktop_Sharing.xaml.cs");
                        }
                        break;
                        #endregion

                    case "2":
                        
                        #region SetUserList Messege
                        // SetUserList Messege

                        try
                        {
                          
                            string uNameSet = objGetMsg.strFrom;

                            if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName != uNameSet)
                            {
                                bool flagSet = true;
                                for (int i = 0; i < lstName.Count; i++)
                                {
                                    if (lstName[i] == uNameSet)
                                    {
                                        flagSet = false;
                                        break;
                                    }
                                }
                                if (flagSet)
                                {
                                    lstName.Add(uNameSet);
                                }

                                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objGetUserList, lstName);
                            }
                        }
                        catch (Exception ex)
                        {
                            VMuktiHelper.ExceptionHandler(ex, "delGetMessage", "ctlDesktop_Sharing.xaml.cs");
                        }
                        break;
                        #endregion
                    
                    case "3":
                    
                        #region Image Messege
                        // Image Messege

                        try
                        {
                            byte[] myBytes = fncStreamToByteArry(objGetMsg.stremImage);
                            MemoryStream mmsTemp = new MemoryStream();
                            mmsTemp.Write(myBytes, 0, myBytes.Length);
                            mmsTemp.Position = 0;
                            string uNameImg = objGetMsg.strFrom;
                            byte[] byteData = mmsTemp.ToArray();

                            if (uNameImg != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                            {
                                List<object> lstData = new List<object>();
                                lstData.Add(uNameImg);
                                lstData.Add(byteData);

                                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objDelSendMsg, lstData);
                            }
                        }
                        catch (Exception ex)
                        {
                            VMuktiHelper.ExceptionHandler(ex, "delGetMessage", "ctlDesktop_Sharing.xaml.cs");
                        }
                        
                        break;
                        #endregion

                    case "4":
                        
                        #region DesktopSelected Messege

                        // DesktopSelected Messege

                        try
                        {

                            string uNameSel = objGetMsg.strFrom;

                            if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == uNameSel)
                            {
                                ImgSize = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            VMuktiHelper.ExceptionHandler(ex, "delGetMessage", "ctlDesktop_Sharing.xaml.cs");
                        }

                        break;
                        #endregion

                    case "5":
                        
                        #region StopControl Messege
                        //StopControl Messege

                        try
                        {

                            string uNameStop = objGetMsg.strFrom;

                            if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == uNameStop)
                            {
                                ImgSize = false;
                            }
                        }
                        catch (Exception ex)
                        {
                            VMuktiHelper.ExceptionHandler(ex, "delGetMessage", "ctlDesktop_Sharing.xaml.cs");
                        }
                        
                        break;
                        #endregion
                    
                    case "6":
                    
                        #region Buttonup Messege
                        //Buttonup Messege
                        try
                        {
                            
                            string mouseButton = objGetMsg.mouseButton.ToString();                            
                            string ToBtnUp = objGetMsg.strTo;

                            if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == ToBtnUp)
                            {
                                if (mouseButton == "1")
                                {
                                    myMouseInput.MouseInput.MouseFlag = MOUSE_LEFTUP;
                                    SendInput(1, ref myMouseInput, Marshal.SizeOf(myMouseInput));
                                }
                                else if (mouseButton == "2")
                                {
                                    myMouseInput.MouseInput.MouseFlag = MOUSEEVENTF_RIGHT_UP;
                                    SendInput(1, ref myMouseInput, Marshal.SizeOf(myMouseInput));
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            VMuktiHelper.ExceptionHandler(ex, "delGetMessage", "ctlDesktop_Sharing.xaml.cs");
                        }
                        break;
                        #endregion

                    case "7":
                        
                        #region Buttondown Messege
                        //Buttondown Messege
                        try
                        {
                            string mouseButtonDown = objGetMsg.mouseButton.ToString();                          
                            string ToBtnDown = objGetMsg.strTo;

                            if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == ToBtnDown)
                            {
                                if (mouseButtonDown == "1")
                                {
                                    myMouseInput.MouseInput.MouseFlag = MOUSE_LEFTDOWN;
                                    SendInput(1, ref myMouseInput, Marshal.SizeOf(myMouseInput));
                                }
                                else if (mouseButtonDown == "2")
                                {
                                    myMouseInput.MouseInput.MouseFlag = MOUSEEVENTF_RIGHTDOWN;
                                    SendInput(1, ref myMouseInput, Marshal.SizeOf(myMouseInput));
                                }
                            }

                        }
                        catch (Exception ex)
                        {
                            VMuktiHelper.ExceptionHandler(ex, "delGetMessage", "ctlDesktop_Sharing.xaml.cs");
                        }
                        break;
                        #endregion

                    case "8":
                        
                        #region SendXY Messege
                        //SendXY Messege
                        try
                        {
                            string To = objGetMsg.strTo;
                            if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == To)
                            {
                                System.Windows.Forms.Cursor.Position = new System.Drawing.Point(Convert.ToInt32(objGetMsg.x), Convert.ToInt32(objGetMsg.y));
                            }
                        }
                        catch (Exception ex)
                        {
                            VMuktiHelper.ExceptionHandler(ex, "delGetMessage", "ctlDesktop_Sharing.xaml.cs");
                        }
                        break;
                        #endregion
                    
                    case "9":
                    
                        #region SendKey Messege
                        //SendKey Messege
                        try
                        {
                           
                            string ToKey = objGetMsg.strTo;

                            if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == ToKey)
                            {
                                whichbuttonpushed(objGetMsg.key);
                            }
                        }
                        catch (Exception ex)
                        {
                            VMuktiHelper.ExceptionHandler(ex, "delGetMessage", "ctlDesktop_Sharing.xaml.cs");
                        }
                        
                        break;
                        #endregion

                    case "10":
                        
                        #region View Messege
                        //View Messege
                        try
                        {
                            string ViewTag = objGetMsg.ViewTag.ToString();

                            string ToView = objGetMsg.strFrom;

                            if (ToView != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                            {
                                if (ViewTag == "0")
                                {
                                    List<object> lstData = new List<object>();
                                    lstData.Add(ToView);
                                    
                                    lstDisAllowView.Add(ToView);
                                    
                                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objDisAllowView, lstData);
                                }                                
                                else
                                {
                                    for (int j = 0; j < lstDisAllowView.Count; j++)
                                    {
                                        if (lstDisAllowView[j] == ToView)
                                        {
                                            lstDisAllowView.RemoveAt(j);
                                            break;
                                        }
                                    }
                                }
                               
                            }
                        }
                        catch (Exception ex)
                        {
                            VMuktiHelper.ExceptionHandler(ex, "delGetMessage", "ctlDesktop_Sharing.xaml.cs");
                        }
                        break;
                        #endregion

                    case "11":
                        
                        #region Control Messege
                        //Control Messege
                        try
                        {
                            string ControlTag = objGetMsg.ControlTag.ToString();                  
                           
                            string ToControl = objGetMsg.strFrom;

                            if (ToControl != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                            {
                                bool blControl = false;
                                if (ControlTag == "0")
                                {
                                    blControl = false;
                                }
                                else if (ControlTag == "1")
                                {
                                    blControl = true;
                                }

                                if (!blControl)
                                {
                                    bool flagControl = true;

                                    for (int i = 0; i < lstDisAllowControl.Count; i++)
                                    {
                                        if (lstDisAllowControl[i] == ToControl)
                                        {
                                            flagControl = false;
                                            break;
                                        }
                                    }

                                    if (flagControl)
                                    {
                                        lstDisAllowControl.Add(ToControl);
                                    }

                                    if (strSelectedUser == ToControl)
                                    {
                                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objStopControl);
                                    }
                                }

                                else if (blControl)
                                {
                                    for (int j = 0; j < lstDisAllowControl.Count; j++)
                                    {
                                        if (lstDisAllowControl[j] == ToControl)
                                        {
                                            lstDisAllowControl.RemoveAt(j);
                                            break;
                                        }
                                    }
                                }

                            }
                        }
                        catch (Exception ex)
                        {
                            VMuktiHelper.ExceptionHandler(ex, "delGetMessage", "ctlDesktop_Sharing.xaml.cs");
                        }

                        break;
                        #endregion
                    
                    case "12":
                    
                        #region Unjoin Messege
                        string uNameUnJoin = objGetMsg.strFrom;
                        if (uNameUnJoin != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                        {
                            List<object> lstData = new List<object>();
                            lstData.Add(uNameUnJoin);

                            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objRemoveUser, lstData);
                        }
                        break;
                        #endregion
                        
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "delGetMessage", "ctlDesktop_Sharing.xaml.cs");
            }
        }
        #endregion
        
        #region Desktop Control Events
        #region dhaval
        void objFullWindow_EntFSRightUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                int mouseVal = 2;
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
                {
                    #region MsgContract
                    clsMessageContract objContract = new clsMessageContract();
                    objContract.mouseButton = mouseVal;
                    objContract.stremImage = new MemoryStream();
                    objContract.strTo = strSelectedUser;
                    #endregion MsgContract
                    channelNetTcp.svcBtnUp(objContract);
                }
                else
                {
                    #region MsgContract
                    clsMessageContract objContract = new clsMessageContract();
                    objContract.mouseButton = mouseVal;
                    objContract.stremImage = new MemoryStream();
                    objContract.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                    objContract.strTo = strSelectedUser;
                    #endregion MsgContract
                    channelHttp.svcBtnUp(objContract);
                }
                objFullWindow.txtInp.Focus();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "imgUser_Desktop_PreviewMouseRightButtonUp", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        void objFullWindow_EntFSRightDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                int mouseVal = 2;
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
                {
                    #region MsgContract
                    clsMessageContract objContract = new clsMessageContract();
                    objContract.mouseButton = mouseVal;
                    objContract.stremImage = new MemoryStream();
                    objContract.strTo = strSelectedUser;
                    #endregion MsgContract
                
                    channelNetTcp.svcBtnDown(objContract);
                }
                else
                {
                    #region sending last mouse position messege
                    channelHttp.svcSendXY(objContractMouse);
                    #endregion
                    #region MsgContract
                    clsMessageContract objContract = new clsMessageContract();
                    objContract.mouseButton = mouseVal;
                    objContract.stremImage = new MemoryStream();
                    objContract.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                    objContract.strTo = strSelectedUser;
                    #endregion MsgContract
                    channelHttp.svcBtnDown(objContract);

                }
                objFullWindow.txtInp.Focus();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "imgUser_Desktop_PreviewMouseRightButtonDown", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        void objFullWindow_EntFSLeftUp(object sender, MouseButtonEventArgs e)
        {
            try
            {
                int mouseVal = 1;

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
                {
                    #region MsgContract
                    clsMessageContract objContract = new clsMessageContract();
                    objContract.mouseButton = mouseVal;
                    objContract.stremImage = new MemoryStream();
                    objContract.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                    objContract.strTo = strSelectedUser;
                    #endregion MsgContract
                    channelNetTcp.svcBtnUp(objContract);
                }
                else
                {
                    #region MsgContract
                    clsMessageContract objContract = new clsMessageContract();
                    objContract.mouseButton = mouseVal;
                    objContract.stremImage = new MemoryStream();
                    objContract.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                    objContract.strTo = strSelectedUser;
                    #endregion MsgContract
                    channelHttp.svcBtnUp(objContract);
                }
                double x = (e.GetPosition(objFullWindow.imgFullScreen).X) - 20;
                double y = (e.GetPosition(objFullWindow.imgFullScreen).Y) - 20;
                objFullWindow.txtInp.Margin= new Thickness(x, y, 0, 0);
                objFullWindow.txtInp.Focus();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "imgUser_Desktop_PreviewMouseLeftButtonUp", "ctlDesktop_Sharing.xaml.cs");
            }
        }
        void objFullWindow_EntFSLeftDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                int mouseVal = 1;
                
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
                {
                    #region MsgContract
                    clsMessageContract objContract = new clsMessageContract();
                    objContract.mouseButton = mouseVal;
                    objContract.stremImage = new MemoryStream();
                    objContract.strTo = strSelectedUser;
                    #endregion MsgContract
                    channelNetTcp.svcBtnDown(objContract);
                }
                else
                {
                    #region sending last mouse position messege
                    channelHttp.svcSendXY(objContractMouse);
                    #endregion
                    #region MsgContract
                    clsMessageContract objContract = new clsMessageContract();
                    objContract.mouseButton = mouseVal;
                    objContract.stremImage = new MemoryStream();
                    objContract.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                    objContract.strTo = strSelectedUser;
                    #endregion MsgContract
                    channelHttp.svcBtnDown(objContract);                

                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "imgUser_Desktop_PreviewMouseLeftButtonDown", "ctlDesktop_Sharing.xaml.cs");
            }
        }
        void objFullWindow_EntFSMouseMove(object sender, MouseEventArgs e)
        {
            try
            {

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
                {
                    #region MsgContract
                    clsMessageContract objContract = new clsMessageContract();
                    objContract.stremImage = new MemoryStream();
                    objContract.strTo = strSelectedUser;
                    objContract.x = e.GetPosition(objFullWindow.imgFullScreen).X;
                    objContract.y = e.GetPosition(objFullWindow.imgFullScreen).Y;                   
                    #endregion MsgContract
                    channelNetTcp.svcSendXY(objContract);
                    if (objContract.y >= 0.0 && objContract.y <= 2.5)
                    {
                        objFullScreen.btnStop.Click+=new RoutedEventHandler(btnStop_Click);
                        objFullScreen.lblName.Content = strSelectedUser + "'s Desktop";
                        objFullScreen.Left = 412;
                        objFullScreen.Top = 0;
                        objFullScreen.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        if (objFullScreen.Visibility == Visibility.Visible)
                        {
                            objFullScreen.Visibility = Visibility.Hidden;
                        }
                    }

                }
                else
                {
                    #region MsgContract
                    objContractMouse = new clsMessageContract();
                    objContractMouse.stremImage = new MemoryStream();
                    objContractMouse.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                    objContractMouse.strTo = strSelectedUser;
                    objContractMouse.x = e.GetPosition(objFullWindow.imgFullScreen).X;
                    objContractMouse.y = e.GetPosition(objFullWindow.imgFullScreen).Y;
                    #endregion MsgContract                   
                    if (objContractMouse.y >= 0.0 && objContractMouse.y <= 2.5)
                    {
                        objFullScreen.btnStop.Click += new RoutedEventHandler(btnStop_Click);
                        objFullScreen.lblName.Content = strSelectedUser + "'s Desktop";
                        objFullScreen.Left = 412;
                        objFullScreen.Top = 0;
                        objFullScreen.Visibility = Visibility.Visible;
                    }
                    else
                    {
                        if (objFullScreen.Visibility == Visibility.Visible)
                        {
                            objFullScreen.Visibility = Visibility.Hidden;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "imgUser_Desktop_PreviewMouseMove", "ctlDesktop_Sharing.xaml.cs");
            }
        }
        #endregion dhaval

        void btnStop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Shared == 1)
                {
                    btnControl_Click(null, null);
                    Shared = 0;
                }
                objFullWindow.Visibility = Visibility.Hidden;
                objFullScreen.Visibility = Visibility.Hidden;                
                lblUser_Desktop.Visibility = Visibility.Collapsed;                
                this.Visibility = Visibility.Visible;
                myViewer.Visibility = Visibility.Visible;
                cnvDesktops.Visibility = Visibility.Visible;

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
                {
                    #region MsgContract
                    clsMessageContract objContractSelectedUser = new clsMessageContract();
                    objContractSelectedUser.stremImage = new MemoryStream();
                    objContractSelectedUser.strFrom = strSelectedUser;
                    #endregion MsgContract
                    channelNetTcp.svcStopControl(objContractSelectedUser);
                }
                else
                {
                    #region MsgContract
                    clsMessageContract objContractSelectedUser = new clsMessageContract();
                    objContractSelectedUser.stremImage = new MemoryStream();
                    objContractSelectedUser.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                    objContractSelectedUser.strTo = strSelectedUser;
                    #endregion MsgContract
                    channelHttp.svcStopControl(objContractSelectedUser);
                }
                strSelectedUser = string.Empty;
                objFullWindow.EntFSMouseMove -= new FullWindow.delFSMouseMove(objFullWindow_EntFSMouseMove);
                objFullWindow.EntFSLeftDown -= new FullWindow.delFSLeftDown(objFullWindow_EntFSLeftDown);
                objFullWindow.EntFSLeftUp -= new FullWindow.delFSLeftUp(objFullWindow_EntFSLeftUp);
                objFullWindow.EntFSRightDown -= new FullWindow.delFSRightDown(objFullWindow_EntFSRightDown);
                objFullWindow.EntFSRightUp -= new FullWindow.delFSRightUp(objFullWindow_EntFSRightUp);
                blControl = false;

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnStop_Click", "ctlDesktop_Sharing.xaml.cs");
            }
        }      
        void btnClose_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                lblUser_Desktop.Visibility = Visibility.Collapsed;
                myViewer.Visibility = Visibility.Visible;
                cnvDesktops.Visibility = Visibility.Visible;

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
                {
                    #region MsgContract
                    clsMessageContract objContractSelectedUser = new clsMessageContract();
                    objContractSelectedUser.stremImage = new MemoryStream();
                    objContractSelectedUser.strFrom = strSelectedUser;
                    #endregion MsgContract
                    channelNetTcp.svcStopControl(objContractSelectedUser);
                }
                else
                {
                    #region MsgContract
                    clsMessageContract objContractSelectedUser = new clsMessageContract();
                    objContractSelectedUser.stremImage = new MemoryStream();
                    objContractSelectedUser.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                    objContractSelectedUser.strTo = strSelectedUser;
                    #endregion MsgContract
                    channelHttp.svcStopControl(objContractSelectedUser);
                }
                strSelectedUser = string.Empty;
                blControl = false;

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnClose_Click", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        void KeyBoardTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                if (value == 90) //F1
            {
                int FW = ctlDesktop_Sharing.FindWindow("HH Parent", "Microsoft Internet Explorer");

                int SM = ctlDesktop_Sharing.SendMessage(FW, ctlDesktop_Sharing.WM_SYSCOMMAND, ctlDesktop_Sharing.SC_CLOSE, 0);
                objFullWindow.txtInp.Focus();
            }
            else if (value == 92) //F3
            {
                whichbuttonpushed(92);
                objFullWindow.txtInp.Focus();
            }

            else if (value == 93) //F4
            {
                whichbuttonpushed(93);
                whichbuttonpushed(99);
                objFullWindow.txtInp.Focus();
            }

            else if (value == 95) //F6
            {
                whichbuttonpushed(95);
                whichbuttonpushed(99);
                objFullWindow.txtInp.Focus();
            }

            else if (value == 99) //F10
            {
                whichbuttonpushed(99);
                objFullWindow.txtInp.Focus();
            }
            else if (value == 100) //F11
            {
                whichbuttonpushed(100);
                whichbuttonpushed(99);
            }
            else if (value == 156)
            {
                keybd_event(VK_Alter, 0, 0, 0);
                keybd_event(VK_Alter, 0, KEYEVENTF_KEYUP, 0);
            }
            else if (value == 72)
            {
                keybd_event(VK_Apps, 0, 0, 0);
                keybd_event(VK_Apps, 0, KEYEVENTF_KEYUP, 0);
            }
            else if (value == 70 || value == 71)
            {
                keybd_event(VK_LWin, 0, 0, 0);
                keybd_event(VK_LWin, 0, KEYEVENTF_KEYUP, 0);
            }
            KeyBoardTimer.Stop();
            value = 0;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "KeyBoardTimer_Tick", "ctlDesktop_Sharing.xaml.cs");
            }
        }
        public void txtInp_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                value = Convert.ToInt16(e.Key.GetHashCode().ToString());
                
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
                {
                    #region MsgContract
                    clsMessageContract objContract = new clsMessageContract();
                    objContract.key = value;
                    objContract.stremImage = new MemoryStream();
                    objContract.strTo = strSelectedUser;
                    #endregion MsgContract
                    channelNetTcp.svcSendKey(objContract);
                }
                else
                {
                    #region MsgContract
                    clsMessageContract objContract = new clsMessageContract();
                    objContract.key = value;
                    objContract.stremImage = new MemoryStream();
                    objContract.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                    objContract.strTo = strSelectedUser;
                    #endregion MsgContract
                    channelHttp.svcSendKey(objContract);
                }
                objFullWindow.txtInp.Text = "";
                KeyBoardTimer.Start();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "txtInput_PreviewKeyDown", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        public void whichbuttonpushed(int num)
        {
            try
            {
                if (num == 116 || num == 117)
                {
                    boolshift = true;
                }
                else if (num == 118 || num == 119)
                {
                    boolCtrl = true;
                }
                else if (num == 156)
                {
                    boolAlt = true;
                }
                else if (num == 70)
                {
                    boolWindows = true;
                }

                else
                {
                    if (boolshift)
                    {
                        keybd_event(VK_Shift, 0, 0, 0);
                    }
                    else if (boolCtrl)
                    {
                        keybd_event(VK_Ctrl, 0, 0, 0);
                    }
                    else if (boolAlt)
                    {
                        keybd_event(VK_Alter, 0, 0, 0);
                    }
                    else if (boolWindows)
                    {
                        keybd_event(VK_LWin, 0, 0, 0);
                    }

                    switch (num)
                    {
                        #region For Caps Alphabates //Space 18,Enter 6,BackSpace 2,Tab 3,Esc 13
                        case 18:
                            keybd_event(VK_Space, 0, 0, 0);
                            keybd_event(VK_Space, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 6:
                            keybd_event(VK_Enter, 0, 0, 0);
                            keybd_event(VK_Enter, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 2:
                            keybd_event(VK_BackSpace, 0, 0, 0);
                            keybd_event(VK_BackSpace, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 3:
                            keybd_event(VK_Tab, 0, 0, 0);
                            keybd_event(VK_Tab, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 13:
                            keybd_event(VK_Esc, 0, 0, 0);
                            keybd_event(VK_Esc, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        #endregion

                        #region Digits
                        case 34:
                            keybd_event(VK_D0, 0, 0, 0);
                            keybd_event(VK_D0, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 35:
                            keybd_event(VK_D1, 0, 0, 0);
                            keybd_event(VK_D1, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 36:
                            keybd_event(VK_D2, 0, 0, 0);
                            keybd_event(VK_D2, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 37:
                            keybd_event(VK_D3, 0, 0, 0);
                            keybd_event(VK_D3, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 38:
                            keybd_event(VK_D4, 0, 0, 0);
                            keybd_event(VK_D4, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 39:
                            keybd_event(VK_D5, 0, 0, 0);
                            keybd_event(VK_D5, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 40:
                            keybd_event(VK_D6, 0, 0, 0);
                            keybd_event(VK_D6, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 41:
                            keybd_event(VK_D7, 0, 0, 0);
                            keybd_event(VK_D7, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 42:
                            keybd_event(VK_D8, 0, 0, 0);
                            keybd_event(VK_D8, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 43:
                            keybd_event(VK_D9, 0, 0, 0);
                            keybd_event(VK_D9, 0, KEYEVENTF_KEYUP, 0);
                            break;
                        #endregion

                        #region Alphabates for Small  //44 to 69
                        case 44:
                            keybd_event(VK_A, 0, 0, 0);
                            keybd_event(VK_A, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 45:
                            keybd_event(VK_B, 0, 0, 0);
                            keybd_event(VK_B, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 46:
                            keybd_event(VK_C, 0, 0, 0);
                            keybd_event(VK_C, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 47:
                            keybd_event(VK_D, 0, 0, 0);
                            keybd_event(VK_D, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 48:
                            keybd_event(VK_E, 0, 0, 0);
                            keybd_event(VK_E, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 49:
                            keybd_event(VK_F, 0, 0, 0);
                            keybd_event(VK_F, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 50:
                            keybd_event(VK_G, 0, 0, 0);
                            keybd_event(VK_G, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 51:
                            keybd_event(VK_H, 0, 0, 0);
                            keybd_event(VK_H, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 52:
                            keybd_event(VK_I, 0, 0, 0);
                            keybd_event(VK_I, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 53:
                            keybd_event(VK_J, 0, 0, 0);
                            keybd_event(VK_J, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 54:
                            keybd_event(VK_K, 0, 0, 0);
                            keybd_event(VK_K, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 55:
                            keybd_event(VK_L, 0, 0, 0);
                            keybd_event(VK_L, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 56:
                            keybd_event(VK_M, 0, 0, 0);
                            keybd_event(VK_M, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 57:
                            keybd_event(VK_N, 0, 0, 0);
                            keybd_event(VK_N, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 58:
                            keybd_event(VK_O, 0, 0, 0);
                            keybd_event(VK_O, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 59:
                            keybd_event(VK_P, 0, 0, 0);
                            keybd_event(VK_P, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 60:
                            keybd_event(VK_Q, 0, 0, 0);
                            keybd_event(VK_Q, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 61:
                            keybd_event(VK_R, 0, 0, 0);
                            keybd_event(VK_R, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 62:
                            keybd_event(VK_S, 0, 0, 0);
                            keybd_event(VK_S, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 63:
                            keybd_event(VK_T, 0, 0, 0);
                            keybd_event(VK_T, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 64:
                            keybd_event(VK_U, 0, 0, 0);
                            keybd_event(VK_U, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 65:
                            keybd_event(VK_V, 0, 0, 0);
                            keybd_event(VK_V, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 66:
                            keybd_event(VK_W, 0, 0, 0);
                            keybd_event(VK_W, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 67:
                            keybd_event(VK_X, 0, 0, 0);
                            keybd_event(VK_X, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 68:
                            keybd_event(VK_Y, 0, 0, 0);
                            keybd_event(VK_Y, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 69:
                            keybd_event(VK_Z, 0, 0, 0);
                            keybd_event(VK_Z, 0, KEYEVENTF_KEYUP, 0);
                            break;
                        #endregion

                        #region NumbarPad

                        case 84:
                            keybd_event(VK_Multiple, 0, 0, 0);
                            keybd_event(VK_Multiple, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 85:
                            keybd_event(VK_Pluse, 0, 0, 0);
                            keybd_event(VK_Pluse, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 87:
                            keybd_event(VK_Minus, 0, 0, 0);
                            keybd_event(VK_Minus, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 88:
                            keybd_event(VK_Dot, 0, 0, 0);
                            keybd_event(VK_Dot, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 89:
                            keybd_event(VK_Divide, 0, 0, 0);
                            keybd_event(VK_Divide, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 74:
                            keybd_event(VK_Num0, 0, 0, 0);
                            keybd_event(VK_Num0, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 75:
                            keybd_event(VK_Num1, 0, 0, 0);
                            keybd_event(VK_Num1, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 76:
                            keybd_event(VK_Num2, 0, 0, 0);
                            keybd_event(VK_Num2, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 77:
                            keybd_event(VK_Num3, 0, 0, 0);
                            keybd_event(VK_Num3, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 78:
                            keybd_event(VK_Num4, 0, 0, 0);
                            keybd_event(VK_Num4, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 79:
                            keybd_event(VK_Num5, 0, 0, 0);
                            keybd_event(VK_Num5, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 80:
                            keybd_event(VK_Num6, 0, 0, 0);
                            keybd_event(VK_Num6, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 81:
                            keybd_event(VK_Num7, 0, 0, 0);
                            keybd_event(VK_Num7, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 82:
                            keybd_event(VK_Num8, 0, 0, 0);
                            keybd_event(VK_Num8, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 83:
                            keybd_event(VK_Num9, 0, 0, 0);
                            keybd_event(VK_Num9, 0, KEYEVENTF_KEYUP, 0);
                            break;
                        #endregion

                        #region Function Keys  //90 to 102
                        case 90:
                            keybd_event(VK_F1, 0, 0, 0);
                            keybd_event(VK_F1, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 91:
                            keybd_event(VK_F2, 0, 0, 0);
                            keybd_event(VK_F2, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 92:
                            keybd_event(VK_F3, 0, 0, 0);
                            keybd_event(VK_F3, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 93:
                            keybd_event(VK_F4, 0, 0, 0);
                            keybd_event(VK_F4, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 94:
                            keybd_event(VK_F5, 0, 0, 0);
                            keybd_event(VK_F5, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 95:
                            keybd_event(VK_F6, 0, 0, 0);
                            keybd_event(VK_F6, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 96:
                            keybd_event(VK_F7, 0, 0, 0);
                            keybd_event(VK_F7, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 97:
                            keybd_event(VK_F8, 0, 0, 0);
                            keybd_event(VK_F8, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 98:
                            keybd_event(VK_F9, 0, 0, 0);
                            keybd_event(VK_F9, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 99:
                            keybd_event(VK_F10, 0, 0, 0);
                            keybd_event(VK_F10, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 100:
                            keybd_event(VK_F11, 0, 0, 0);
                            keybd_event(VK_F11, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 101:
                            keybd_event(VK_F12, 0, 0, 0);
                            keybd_event(VK_F12, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        #endregion

                        #region Other
                        case 140:
                            keybd_event(VK_SemiColon, 0, 0, 0);
                            keybd_event(VK_SemiColon, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 141:
                            keybd_event(VK_Equals, 0, 0, 0);
                            keybd_event(VK_Equals, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 142:
                            keybd_event(VK_COMMA, 0, 0, 0);
                            keybd_event(VK_COMMA, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 143:
                            keybd_event(VK_OemMinus, 0, 0, 0);
                            keybd_event(VK_OemMinus, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 144:
                            keybd_event(VK_OemPeriod, 0, 0, 0);
                            keybd_event(VK_OemPeriod, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 145:
                            keybd_event(VK_OemQuestion, 0, 0, 0);
                            keybd_event(VK_OemQuestion, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 146:
                            keybd_event(VK_OemTidle, 0, 0, 0);
                            keybd_event(VK_OemTidle, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 149:
                            keybd_event(VK_OpenSquareBracket, 0, 0, 0);
                            keybd_event(VK_OpenSquareBracket, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 150:
                            keybd_event(VK_OemPipe, 0, 0, 0);
                            keybd_event(VK_OemPipe, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 151:
                            keybd_event(VK_CloseSquareBracket, 0, 0, 0);
                            keybd_event(VK_CloseSquareBracket, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 152:
                            keybd_event(VK_SingleCote, 0, 0, 0);
                            keybd_event(VK_SingleCote, 0, KEYEVENTF_KEYUP, 0);
                            break;


                        #endregion

                        #region Some Other Useful Key
                        case 23:
                            keybd_event(VK_Left_Arrow, 0, 0, 0);
                            keybd_event(VK_Left_Arrow, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 24:
                            keybd_event(VK_Up_Arrow, 0, 0, 0);
                            keybd_event(VK_Up_Arrow, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 25:
                            keybd_event(VK_Right_Arrow, 0, 0, 0);
                            keybd_event(VK_Right_Arrow, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 26:
                            keybd_event(VK_Down_Arrow, 0, 0, 0);
                            keybd_event(VK_Down_Arrow, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 31:
                            keybd_event(VK_Insert, 0, 0, 0);
                            keybd_event(VK_Insert, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 32:
                            keybd_event(VK_Delete, 0, 0, 0);
                            keybd_event(VK_Delete, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 21:
                            keybd_event(VK_End, 0, 0, 0);
                            keybd_event(VK_End, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 22:
                            keybd_event(VK_Home, 0, 0, 0);
                            keybd_event(VK_Home, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 19:
                            keybd_event(VK_Page_Up, 0, 0, 0);
                            keybd_event(VK_Page_Up, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 20:
                            keybd_event(VK_Page_Down, 0, 0, 0);
                            keybd_event(VK_Page_Down, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 115:
                            keybd_event(VK_Scroll_Lock, 0, 0, 0);
                            keybd_event(VK_Scroll_Lock, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 114:
                            keybd_event(VK_Num_Lock, 0, 0, 0);
                            keybd_event(VK_Num_Lock, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 8:
                            keybd_event(VK_Caps_Lock, 0, 0, 0);
                            keybd_event(VK_Caps_Lock, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 71:
                            keybd_event(VK_RWin, 0, 0, 0);
                            keybd_event(VK_RWin, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        case 72:
                            keybd_event(VK_Apps, 0, 0, 0);
                            keybd_event(VK_Apps, 0, KEYEVENTF_KEYUP, 0);
                            break;

                        #endregion

                    }
                    if (boolshift)
                    {
                        boolshift = false;
                        keybd_event(VK_Shift, 0, KEYEVENTF_KEYUP, 0);
                    }
                    else if (boolCtrl)
                    {
                        boolCtrl = false;
                        keybd_event(VK_Ctrl, 0, KEYEVENTF_KEYUP, 0);
                    }
                    else if (boolAlt)
                    {
                        boolAlt = false;
                        keybd_event(VK_Alter, 0, KEYEVENTF_KEYUP, 0);
                    }
                    else if (boolWindows)
                    {
                        boolWindows = false;
                        keybd_event(VK_LWin, 0, KEYEVENTF_KEYUP, 0);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "whichbuttonpushed", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        void btnView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnView.Tag.ToString() == "0")
                {
                    btnView.Tag = 1;
                    btnView.Background = System.Windows.Media.Brushes.Red;
                    btnView.Content = "Allow View";
                    btnControl.IsEnabled = false;
                    DoWork = false;                    

                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
                    {
                        #region MsgContract
                        clsMessageContract objContarct = new clsMessageContract();
                        objContarct.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                        objContarct.stremImage = new MemoryStream();
                        #endregion MsgContract
                        channelNetTcp.svcAllowView(objContarct);
                    }
                    else
                    {

                        #region MsgContract
                        clsMessageContract objContarct = new clsMessageContract();
                        objContarct.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                        objContarct.stremImage = new MemoryStream();
                        #endregion MsgContract
                        channelHttp.svcAllowView(objContarct);
                    }
                }
                else if (btnView.Tag.ToString() == "1")
                {
                    btnView.Tag = 0;
                    btnView.Background = System.Windows.Media.Brushes.Green;
                    btnView.Content = "Stop View";
                    DoWork = true;                    
                    btnControl.IsEnabled = true;

                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
                    {
                        #region MsgContract
                        clsMessageContract objContarct = new clsMessageContract();
                        objContarct.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                        objContarct.stremImage = new MemoryStream();
                        objContarct.blView = true;
                        #endregion MsgContract
                        channelNetTcp.svcAllowView(objContarct);
                    }
                    else
                    {
                        #region MsgContract
                        clsMessageContract objContarct = new clsMessageContract();
                        objContarct.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                        objContarct.stremImage = new MemoryStream();
                        objContarct.blView = true;
                        #endregion MsgContract
                        channelHttp.svcAllowView(objContarct);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnView_Click", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        void btnControl_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnControl.Tag.ToString() == "0")
                {
                    btnControl.Tag = 1;
                    btnControl.Background = System.Windows.Media.Brushes.Red;
                    btnControl.Content = "Allow Control";

                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
                    {
                        #region MsgContract
                        clsMessageContract objContarct = new clsMessageContract();
                        objContarct.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                        objContarct.stremImage = new MemoryStream();
                        #endregion MsgContract
                        channelNetTcp.svcAllowControl(objContarct);
                    }
                    else
                    {
                        #region MsgContract
                        clsMessageContract objContarct = new clsMessageContract();
                        objContarct.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                        objContarct.stremImage = new MemoryStream();
                        #endregion MsgContract
                        channelHttp.svcAllowControl(objContarct);
                    }
                }
                else if (btnControl.Tag.ToString() == "1")
                {
                    btnControl.Tag = 0;
                    btnControl.Background = System.Windows.Media.Brushes.Green;
                    btnControl.Content = "Stop Control";

                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
                    {
                        #region MsgContract
                        clsMessageContract objContarct = new clsMessageContract();
                        objContarct.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                        objContarct.stremImage = new MemoryStream();
                        objContarct.blControl = true;
                        #endregion MsgContract
                        channelNetTcp.svcAllowControl(objContarct);
                    }
                    else
                    {
                        #region MsgContract
                        clsMessageContract objContarct = new clsMessageContract();
                        objContarct.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                        objContarct.stremImage = new MemoryStream();
                        objContarct.blControl = true;
                        #endregion MsgContract
                        channelHttp.svcAllowControl(objContarct);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnControl_Click", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        #endregion
        
        #region NetTCP Events

        void ctlDesktop_Sharing_EntsvcJoin(clsMessageContract mcJoin)
        {
            try {
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlDesktop_Sharing_EntsvcJoin", "ctlDesktop_Sharing.xaml.cs");
            }
        }
        
        void ctlDesktop_Sharing_EntsvcGetUserList(clsMessageContract objMsgContract)
        {
            try
            {
                string strName = objMsgContract.strFrom;
                ClsException.WriteToLogFile("ctlDesktop_Sharing_EntsvcGetUserList:-  " + strName);
              
                if (!string.IsNullOrEmpty(strName) && strName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName )
                {
                    try
                    {
                        bool flag = true;
                        
                        for (int i = 0; i < lstName.Count; i++)
                        {
                            if (lstName[i] == strName)
                            {
                                flag = false;
                                break;
                            }
                        }

                        if (flag)
                        {
                            lstName.Add(strName);
                        }
                    }
                    catch (Exception ex)
                    {
                        VMuktiHelper.ExceptionHandler(ex, "ctlDesktop_Sharing_EntsvcGetUserList", "ctlDesktop_Sharing.xaml.cs");
                    }

                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objGetUserList, lstName);
                    if (channelNetTcp != null)
                    {
                        Stream mmsName = fncStringToStream(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                        mmsName.Position = 0;
                        #region MsgContract
                        clsMessageContract objContarctSetUserlist = new clsMessageContract();
                        objContarctSetUserlist.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                        objContarctSetUserlist.strType = "Set";
                        objContarctSetUserlist.stremImage = new MemoryStream();
                        #endregion MsgContract
                        channelNetTcp.svcSetUserList(objContarctSetUserlist);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlDesktop_Sharing_EntsvcGetUserList", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        void ctlDesktop_Sharing_EntsvcSetUserList(clsMessageContract objMsgContract1)
        {
            try
            {
                string uName = objMsgContract1.strFrom;
                if (uName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    try
                    {
                        bool flag = true;
                        for (int i = 0; i < lstName.Count; i++)
                        {
                            if (lstName[i] == uName)
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            lstName.Add(uName);
                        }
                    }
                    catch
                    {

                    }

                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objGetUserList, lstName);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlDesktop_Sharing_EntsvcSetUserList", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        void ctlDesktop_Sharing_EntsvcSelectedDesktop(clsMessageContract objContractSelected)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == objContractSelected.strFrom)
                {
                    ImgSize = true;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlDesktop_Sharing_EntsvcSelectedDesktop", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        void ctlDesktop_Sharing_EntsvcStopControl(clsMessageContract objContract)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == objContract.strFrom)
                {
                    ImgSize = false;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlDesktop_Sharing_EntsvcStopControl", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        void ctlDesktop_Sharing_EntsvcSendMessage(clsMessageContract objContractStream)
        {
            try
            {
                byte[] tempArry = fncStreamToByteArry(objContractStream.stremImage);             
                
                MemoryStream mmsTemp = new MemoryStream();
                mmsTemp.Write(tempArry, 0, tempArry.Length);
                mmsTemp.Position = 0;

                byte[] byteData = mmsTemp.ToArray();
                string uName = objContractStream.strFrom; 


                if (uName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    List<object> lstData = new List<object>();
                    lstData.Add(uName);
                    lstData.Add(byteData);
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objDelSendMsg, lstData);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlDesktop_Sharing_EntsvcSendMessage", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        void ctlDesktop_Sharing_EntsvcBtnDown(clsMessageContract objContract)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == objContract.strTo)
                {
                    if (objContract.mouseButton.ToString() == "1")
                    {
                        myMouseInput.MouseInput.MouseFlag = MOUSE_LEFTDOWN;
                        SendInput(1, ref myMouseInput, Marshal.SizeOf(myMouseInput));
                    }
                    else if (objContract.mouseButton.ToString() == "2")
                    {
                        myMouseInput.MouseInput.MouseFlag = MOUSEEVENTF_RIGHTDOWN;
                        SendInput(1, ref myMouseInput, Marshal.SizeOf(myMouseInput));
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlDesktop_Sharing_EntsvcBtnDown", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        void ctlDesktop_Sharing_EntsvcBtnUp(clsMessageContract objContract)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == objContract.strTo)
                {
                    if (objContract.mouseButton.ToString() == "1")
                    {
                        myMouseInput.MouseInput.MouseFlag = MOUSE_LEFTUP;
                        SendInput(1, ref myMouseInput, Marshal.SizeOf(myMouseInput));
                    }
                    else if (objContract.mouseButton.ToString() == "2")
                    {
                        myMouseInput.MouseInput.MouseFlag = MOUSEEVENTF_RIGHT_UP;
                        SendInput(1, ref myMouseInput, Marshal.SizeOf(myMouseInput));
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlDesktop_Sharing_EntsvcBtnUp", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        void ctlDesktop_Sharing_EntsvcSendXY(clsMessageContract objContract)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == objContract.strTo)
                {
                    
                    strX = objContract.x.ToString().Split('.');
                    strY = objContract.y.ToString().Split('.');
                    System.Windows.Forms.Cursor.Position = new System.Drawing.Point(int.Parse(strX[0]), int.Parse(strY[0]));
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlDesktop_Sharing_EntsvcSendXY", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        void ctlDesktop_Sharing_EntsvcSendKey(clsMessageContract objContract)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == objContract.strTo)
                {
                    whichbuttonpushed(objContract.key);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlDesktop_Sharing_EntsvcSendKey", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        void ctlDesktop_Sharing_EntsvcAllowView(clsMessageContract objContract)
        {
            try
            {
                if (objContract.strFrom != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    if (!objContract.blView)
                    {
                        List<object> lstData = new List<object>();
                        lstData.Add(objContract.strFrom);                       
                        lstDisAllowView.Add(objContract.strFrom);                        
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objDisAllowView, lstData);
                    }                    
                    else
                    {
                        for (int j = 0; j < lstDisAllowView.Count; j++)
                        {
                            if (lstDisAllowView[j] == objContract.strFrom)
                            {
                                lstDisAllowView.RemoveAt(j);
                                break;
                            }
                        }
                    }
                   
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlDesktop_Sharing_EntsvcAllowView", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        void ctlDesktop_Sharing_EntsvcAllowControl(clsMessageContract objContract)
        {
            try
            {
                if (objContract.strFrom != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    if (!objContract.blControl)
                    {
                        bool flag = true;

                        for (int i = 0; i < lstDisAllowControl.Count; i++)
                        {
                            if (lstDisAllowControl[i] == objContract.strFrom)
                            {
                                flag = false;
                                break;
                            }
                        }

                        if (flag)
                        {
                            lstDisAllowControl.Add(objContract.strFrom);
                        }

                        if (strSelectedUser == objContract.strFrom)
                        {

                            this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objStopControl);
                            
                        }
                    }
                    else if (objContract.blControl)
                    {
                        for (int j = 0; j < lstDisAllowControl.Count; j++)
                        {
                            if (lstDisAllowControl[j] == objContract.strFrom)
                            {
                                lstDisAllowControl.RemoveAt(j);
                                break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlDesktop_Sharing_EntsvcAllowControl", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        void ctlDesktop_Sharing_EntsvcUnJoin(clsMessageContract objContract)
        {
            try
            {
                m_asyncSendImage.Dispose();
                if (objContract.strFrom != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    List<object> lstData = new List<object>();
                    lstData.Add(objContract.strFrom);

                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objRemoveUser, lstData);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlDesktop_Sharing_EntsvcUnJoin", "ctlDesktop_Sharing.xaml.cs");
            }
        }

        #endregion

        #region Http Timer
        
        void dispTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                #region MsgContract
                clsMessageContract objMsgContract = new clsMessageContract();
                objMsgContract.strFrom = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
                objMsgContract.stremImage = new MemoryStream();
                #endregion MsgContract
              //  Stream objStream = new MemoryStream();
                clsGetMessage obj = new clsGetMessage();
                obj = channelHttp.svcGetMessages(objMsgContract);                
                Stream tempBytes = null;
                if (!string.IsNullOrEmpty(obj.strFrom))
                {
                     tempBytes = fncStringToStream(obj.strFrom);
                }
                else
                {
                     tempBytes = fncStringToStream(obj.strTo);
                }

                if (tempBytes.Length>0)
                {
                    Dispatcher.BeginInvoke(DispatcherPriority.Normal, objDelGetMsg, obj);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "dispTimer_Tick", "ctlDesktop_Sharing.xaml.cs");
            }
        }
        #endregion
    }
}
