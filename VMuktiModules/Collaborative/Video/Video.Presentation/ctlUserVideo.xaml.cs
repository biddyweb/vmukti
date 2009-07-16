
/* VMukti 1.0 -- An Open Source Unified Communications Engine
*
* Copyright (C) 2008 - 2009, VMukti Solutions Pvt. Ltd.
*
* Hardik Sanghvi <hardik@vmukti.com>
*
* See http://www.vmukti.com for more information about
* the VMukti project. Please do not directly contact
* any of the maintainers of this project for assistance;
* the project provides a web site, forums and mailing lists
* for your use.

* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.

* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU General Public License for more details.

* You should have received a copy of the GNU General Public License
* along with this program. If not, see <http://www.gnu.org/licenses/>

*/
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
using System.Text.RegularExpressions;
using DirectX.Capture;
using VMuktiService;
using System.Runtime.InteropServices;
using System.IO;
using Video.Business.Service.BasicHttp;
using Video.Business.Service.NetP2P;
using System.Windows.Threading;
using VMuktiAPI;
using Microsoft.Win32;
using System.ServiceModel;
using System.ComponentModel;


namespace Video.Presentation
{
    public partial class ctlUserVideo : UserControl
    {
        

        #region Constants for Mouse
 
        const int INPUT_MOUSE = 0;
        const int MOUSE_LEFTDOWN = 2;
        const int MOUSE_LEFTUP = 4;
        const int MOUSEEVENTF_RIGHTDOWN = 8;
        const int MOUSEEVENTF_RIGHT_UP = 16;

        public struct INPUT
        {
            public uint type;
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

        
        #endregion

        [DllImport("User32.dll", SetLastError = true)]
        public static extern int SendInput(int nInputs, ref INPUT pInputs, int cbsize);

        
        public DirectX.Capture.Capture capture = null;
        private Filters filters = null;

        NetPeerClient npcDirectXVideo = null;
        object objDirectXVideo = new ClsNetP2PUserVideo();
        INetP2PUserVideoChannel netp2pDirectXVideoChannel;

        BasicHttpClient bhcDirectXVideo = null;
        IHttpUserVideo httpDirectXVideoChannel;

        public delegate void DelSendImage(byte[] strmImage);
        public DelSendImage m_DelSendImage;

        public delegate void DelStampMe(byte[] strImage);
        public DelStampMe delStampMe;

        System.Threading.Thread startClient;

        public string MyUserName = "";
        public string MyVidType = "";
        public string MyURI = "";
        private int MyMeetingID = 0;
        private int MyID = 0;
        double OldHeight = 0.0;
        double OldWidth = 0.0;

        bool flg = false;
        public bool FullScreenMe;
        public bool FullScreenOther;
        public bool StampMe;
        public bool StampOther;
        public List<string> lstVideoInput = null;

        DispatcherTimer disptGetImages = new DispatcherTimer();
        DispatcherTimer disptgetVideoWind = new DispatcherTimer();  // timer for getting video window
        
        public delegate void DelRemoveUser(string UName);
        public event DelRemoveUser EntRemoveUser;

        public delegate void DelSetImage();
        public DelSetImage objSetImage;

        public delegate void DelAsyncSend(MemoryStream myStream);
        public DelAsyncSend objAsyncSend;

        public delegate void DelAsyncReceive(List<object> lstData);
        public DelAsyncReceive objAsyncReceive;

        string FSUname;

        TestFullScreen Tfs = null;
        ShowVideoInputs objShowVideo = null;

        BackgroundWorker SendImage_Worker = new BackgroundWorker();
        BackgroundWorker ReceiveImage_Worker = new BackgroundWorker();
        BackgroundWorker SetImage_Worker = new BackgroundWorker();

        public ctlUserVideo(string UName, string vidType, string videoURI)
        {
            try
            {
                InitializeComponent();
                VMuktiAPI.VMuktiHelper.RegisterEvent("SignOut").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(ctlUserVideo_VMuktiEvent);

                objAsyncSend = new DelAsyncSend(AsyncSendImage);
                objAsyncReceive = new DelAsyncReceive(AsyncRecImage);
                objSetImage = new DelSetImage(SetImage);

                SendImage_Worker.DoWork += new DoWorkEventHandler(SendImage_Worker_DoWork);
                ReceiveImage_Worker.DoWork += new DoWorkEventHandler(ReceiveImage_Worker_DoWork);
                SetImage_Worker.DoWork += new DoWorkEventHandler(SetImage_Worker_DoWork);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlUserVideo", "ctlUserVideo.xaml.cs");             
            }

            try
            {
                MyUserName = UName;
                lblUName.Content = UName;
                MyVidType = vidType;
                MyURI = videoURI;
                
                this.Tag = UName;

                disptGetImages.Interval = TimeSpan.FromSeconds(1);
                disptGetImages.Tick += new EventHandler(disptGetImages_Tick);

                disptgetVideoWind.Interval = TimeSpan.FromMilliseconds(500);
                disptgetVideoWind.Tick += new EventHandler(disptgetVideoWind_Tick);

                this.Loaded += new RoutedEventHandler(UserVideo_Loaded);
                this.Unloaded += new RoutedEventHandler(UserVideo_Unloaded);

                Application.Current.Exit += new ExitEventHandler(Current_Exit);

                if (MyVidType == "Me")
                {
                    cbMenu.Items.Add("Full Screen");
                    cbMenu.Items.Add("Cancel");
                    cbMenu.Items.Add("Video Configuration");
                }
                else
                {
                    cbMenu.Items.Add("Full Screen");
                    cbMenu.Items.Add("Cancel");
                }

                cbMenu.DropDownOpened += new EventHandler(cbMenu_DropDownOpened);
                cbMenu.DropDownClosed += new EventHandler(cbMenu_DropDownClosed);  

                if (videoURI != "")
                {
                    startClient = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(RegClient));
                    startClient.Start(videoURI);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlUserVideo", "ctlUserVideo.xaml.cs");             
            }
        }

        void cbMenu_DropDownOpened(object sender, EventArgs e)
        {
            try
            {
                cbMenu.SelectedItem = null;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "cbMenu_DropDownOpenedk", "ctlUserVideo.xaml.cs");
            }
        }

        void cbMenu_DropDownClosed(object sender, EventArgs e)
        {
            try
            {
                if (cbMenu.SelectedItem != null && cbMenu.SelectedItem.ToString() == "Full Screen")
                {
                    try
                    {
                        if (FullScreenMe || FullScreenOther)
                        {
                            MessageBox.Show("Video is all ready running in Full Screen");
                        }
                        else
                        {
                            if (MyVidType == "Me")
                            {
                                FullScreenMe = true;
                            }
                            else
                            {
                                FullScreenOther = true;
                            }

                            FSUname = lblUName.Content.ToString();

                            Tfs = new TestFullScreen();
                            Tfs.Title = lblUName.Content.ToString() + " Video In Full Screen Mode";
                            Tfs.Show();

                            Tfs.Closed += new EventHandler(Tfs_Closed);
                        }
                    }
                    catch (Exception ex)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "cbMenu_DropDownClosed", "ctlUserVideo.xaml.cs");
                    }
                }
                else if (cbMenu.SelectedItem != null && cbMenu.SelectedItem.ToString() == "Cancel")
                {
                    try
                    {
                        MessageBoxResult result = MessageBox.Show("Do You Really Want To Remove " + lblUName.Content.ToString() + "'s Video", "Remove Video", MessageBoxButton.YesNo);

                        if (result == MessageBoxResult.Yes)
                        {
                            if (EntRemoveUser != null)
                            {
                                EntRemoveUser(lblUName.Content.ToString());
                            }
                            netp2pDirectXVideoChannel.Close();
                        }
                        if (Tfs != null)
                        {
                            Tfs.Close();
                        }
                    }
                    catch (Exception ex)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "cbMenu_DropDownClosed", "ctlUserVideo.xaml.cs");
                    }
                }
                else if (cbMenu.SelectedItem != null && cbMenu.SelectedItem.ToString() == "Video Configuration")
                {
                    try
                    {
                        filters = new Filters();

                        if (filters.VideoInputDevices.Count > 1)
                        {
                            lstVideoInput = new List<string>();
                            lstVideoInput = filters.VideoInputDevices.GetVideoInputDevices();

                            objShowVideo = new ShowVideoInputs(lstVideoInput);
                            objShowVideo.EntSelectedDevice += new ShowVideoInputs.delSelectedDevice(objShowVideo_EntSelectedDevice);
                            objShowVideo.Show();
                        }
                        else
                        {
                            InitCapture("Low", 0);
                        }
                    }
                    catch (Exception ex)
                    {
                        if (string.Compare(ex.Message, "No devices of the category") == 0)
                        {
                            MessageBox.Show("Sorry You Dont Have Any Video Device Attached To Tour System", "VMukti Says: Video Conference");
                        }
                        else
                        {
                            VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "cbMenu_DropDownClosed", "ctlUserVideo.xaml.cs");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "cbMenu_DropDownClosed", "ctlUserVideo.xaml.cs");
            }
        }
        
        void Tfs_Closed(object sender, EventArgs e)
        {
            try
            {
                FullScreenMe = false;
                FullScreenOther = false;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Tfs_Closed", "ctlUserVideo.xaml.cs");             
            }
        }

        void disptgetVideoWind_Tick(object sender, EventArgs e)
        {
            try
            {

                ((System.Windows.Threading.DispatcherTimer)sender).Stop();

                System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(StartThread));
                t.IsBackground = true;
                t.Priority = System.Threading.ThreadPriority.BelowNormal;
                t.Start();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "disptgetVideoWind_Tick", "ctlUserVideo.xaml.cs");             
            }
        }

        void StartThread()
        {
            try
            {
                if (!SetImage_Worker.IsBusy)
                {
                    SetImage_Worker.RunWorkerAsync();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "StartThread", "ctlUserVideo.xaml.cs");             
            }
        }

        void SetImage()
        {
            try
            {
                capture.setVideo();
                disptgetVideoWind.Start();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SetImage", "ctlUserVideo.xaml.cs");
            }
        }

        void ctlUserVideo_VMuktiEvent(object sender, VMuktiEventArgs e)
        {
            try
            {
                ClosePod();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlUserVideo_VMuktiEvent", "ctlUserVideo.xaml.cs");             
            }
        }

        #region current control functions

        void UserVideo_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                
                if (capture == null & flg == false)
                {
                    if (MyVidType == "Me")
                    {

                        filters = new Filters();

                        cnvUserVideo.Visibility = Visibility.Visible;
                        m_DelSendImage = new DelSendImage(SetMyImage);

                        try
                        {
                            if (filters.VideoInputDevices.Count > 1)
                            {

                                lstVideoInput = new List<string>();
                                lstVideoInput = filters.VideoInputDevices.GetVideoInputDevices();

                                objShowVideo = new ShowVideoInputs(lstVideoInput);
                                objShowVideo.EntSelectedDevice += new ShowVideoInputs.delSelectedDevice(objShowVideo_EntSelectedDevice);
                                objShowVideo.Show();
                            }
                            else if (filters.VideoInputDevices.Count == 0)
                            {
                                
                            }
                            else
                            {
                                InitCapture("Low", 0);
                            }
                        }
                        catch (Exception ex)
                        {
                            VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UserVideo_Loaded", "ctlUserVideo.xaml.cs");             
                        }
                    }
                    else
                    {
                        picUserVideo.Visibility = Visibility.Visible;

                        m_DelSendImage = new DelSendImage(SetOtherImage);

                    }
                }
                else if (flg)
                {
                    double[] winXY = new double[2];
                    winXY = capture.PreviewWidowTag();

                    if (winXY[0] != 0 & winXY[1] != 0)
                    {
                        if (MyVidType == "Me")
                        {
                            flg = false;
                            capture.showVideo();
                        }
                    }
                    else
                    {
                        capture.hideVideo();
                        flg = true;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UserVideo_Loaded", "ctlUserVideo.xaml.cs");             
            }
	      }
			   
        //public double[] PreivewWindowLoc()
        //{
        //    try
        //    {
        //        //ClsException.WriteToLogFile("PreivewWindowLoc called");
        //        double[] winXY = new double[2];
        //        winXY = capture.PreviewWidowTag();

        //        return winXY;
        //    }
        //    catch (Exception ex)
        //    {
        //        //MessageBox.Show("PreivewWindowLoc"+ex.Message);
        //        return null;
        //    }
        //}

        //public void ShowVideo()
        //{
        //    capture.showVideo();
        //}

        //public void HideVideo()
        //{
        //    capture.hideVideo();
        //}
        

        void Current_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                if (capture != null)
                {
                    capture.Stop();
                    capture.Dispose();
                }
                if (netp2pDirectXVideoChannel != null)
                {
                    netp2pDirectXVideoChannel = null;
                }
                if (httpDirectXVideoChannel != null)
                {
                    httpDirectXVideoChannel = null;
                }
                VMuktiAPI.VMuktiHelper.UnRegisterEvent("SignOut");
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Current_Exit", "ctlUserVideo.xaml.cs");             
            }
        }
       
        void UserVideo_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (MyVidType == "Me")
                {
                    if (flg == false)
                    {
                        capture.hideVideo();
                        flg = true;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UserVideo_Unloaded", "ctlUserVideo.xaml.cs");             
            }
        }
        
        public void ClosePod()
        {
            try
            {
                if (capture != null)
                {
                    capture.Stop();
                    capture.Dispose();
                    
                }
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P)
                {
                    if (netp2pDirectXVideoChannel != null )
                    {
                        netp2pDirectXVideoChannel.svcUnJoin(VMuktiInfo.CurrentPeer.DisplayName);
						
                    }
                }
                else
                {
                    if (httpDirectXVideoChannel != null)
                    {
                        httpDirectXVideoChannel.svcUnJoin(VMuktiInfo.CurrentPeer.DisplayName);
                    }
                }
                if (disptGetImages != null)
                {
                    disptGetImages = null;
                }
                if (netp2pDirectXVideoChannel != null)
                {
                    netp2pDirectXVideoChannel.Close();
                    netp2pDirectXVideoChannel.Dispose();
                    netp2pDirectXVideoChannel = null;
                }

                Dispose();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ClosePod", "ctlUserVideo.xaml.cs");             
            }
        }

        public void CloseVideo()
        {
            try
            {
                if (capture != null)
                {
                    capture.Stop();
                    capture.Dispose();
                }
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P)
                {
                    netp2pDirectXVideoChannel.svcUnJoin(VMuktiInfo.CurrentPeer.DisplayName);
                }
                else
                {
                    httpDirectXVideoChannel.svcUnJoin(VMuktiInfo.CurrentPeer.DisplayName);
                    disptGetImages.Stop();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CloseVideo", "ctlUserVideo.xaml.cs");             
            }
        }

        public void RegOnURI(string UName, string URI)
        {
            try
            {
                if (MyURI != URI)
                {
                    MyUserName = UName;
                    startClient = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(RegClient));
                    startClient.Start(URI);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RegOnURI", "ctlUserVideo.xaml.cs");             
            }
        }

        #endregion

        void RegClient(object videoURI)
        {
            lock (this)
            {
                try
                {
                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P)
                    {
                        ((ClsNetP2PUserVideo)objDirectXVideo).EntsvcJoin += new ClsNetP2PUserVideo.delsvcJoin(ctlUserVideo_EntsvcJoin);
                        ((ClsNetP2PUserVideo)objDirectXVideo).EntsvcSendStream += new ClsNetP2PUserVideo.delsvcSendStream(ctlUserVideo_EntsvcSendStream);
                        ((ClsNetP2PUserVideo)objDirectXVideo).EntsvcUnJoin += new ClsNetP2PUserVideo.delsvcUnJoin(ctlUserVideo_EntsvcUnJoin);

                        npcDirectXVideo = new NetPeerClient();
                        netp2pDirectXVideoChannel = (INetP2PUserVideoChannel)npcDirectXVideo.OpenClient<INetP2PUserVideoChannel>(videoURI.ToString(), videoURI.ToString().Split(':')[2].Split('/')[2], ref objDirectXVideo);
                        netp2pDirectXVideoChannel.svcJoin(MyUserName);
                    }
                    else
                    {
                        int count = 0;
                        while (count < 90)
                        {
                            try
                            {
                                bhcDirectXVideo = new BasicHttpClient();
                                httpDirectXVideoChannel = (IHttpUserVideo)bhcDirectXVideo.OpenClient<IHttpUserVideo>(videoURI.ToString());
                                httpDirectXVideoChannel.svcJoin(MyUserName);

                                count = 90;

                                if (MyVidType != "Me")
                                {
                                    disptGetImages.Start();
                                }
                            }
                            catch
                            {
                                System.Threading.Thread.Sleep(TimeSpan.FromSeconds(1));
                                count++;
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RegClient", "ctlUserVideo.xaml.cs");             
                }
            }
        }

        #region Camera functions
       
        void objShowVideo_EntSelectedDevice(int selectedID)
        {
            try
            {
                InitCapture("Low", selectedID);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objShowVideo_EntSelectedDevice", "ctlUserVideo.xaml.cs");             
            }
        }

        void InitCapture(string Quality, int videoIndex)
        {
            try
            {
                if (capture != null)
                {
                    capture.Stop();
                    capture.Dispose();
                }

                capture = new DirectX.Capture.Capture(filters.VideoInputDevices[videoIndex], null);
                
                #region make these lines comment for using professional cam
                if (Quality == "High")
                {
                    capture.FrameSize = new System.Drawing.Size(capture.VideoCaps.MaxFrameSize.Width, capture.VideoCaps.MaxFrameSize.Height);
                }
                else if (Quality == "Low")
                {
                    capture.FrameSize = new System.Drawing.Size(160, 120);
                }
                capture.FrameRate = 30;
                #endregion

                double dblPicWidth = 0.0, dblPicHeight = 0.0;
                
                this.Dispatcher.BeginInvoke(DispatcherPriority.Background, new DispatcherOperationCallback(delegate(Object state)
                {
                    dblPicWidth = picUserVideo.ActualWidth;
                    dblPicHeight = picUserVideo.ActualHeight;
                    return null;
                }), null);

                Point pt = picUserVideo.TranslatePoint(new Point(), Application.Current.MainWindow);
                picUserVideo.Tag = pt;

                capture.PreviewWindow = cnvUserVideo;
                
                disptgetVideoWind.Start();   //timer of getting video window is start

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.SuperNode || VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithNetP2P)
                {
                    capture.FrameEvent2 += new DirectX.Capture.Capture.HeFrame(capture_FrameEventNetP2P);
                }
                else
                {
                    capture.FrameEvent2 += new DirectX.Capture.Capture.HeFrame(capture_FrameEventHttp);
                }
                capture.GrapImg();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "InitCapture", "ctlUserVideo.xaml.cs");             
            }
        }

        void capture_FrameEventNetP2P(System.Drawing.Bitmap BM)
        {
            MemoryStream mms = new MemoryStream();
            try
            {
                BM.Save(mms, System.Drawing.Imaging.ImageFormat.Jpeg);
                mms.Position = 0;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "capture_FrameEventNetP2P", "ctlUserVideo.xaml.cs");

            }

            try
            {
                if (FullScreenMe)
                {
                    if (m_DelSendImage != null)
                    {
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Background, m_DelSendImage, mms.ToArray());
                        mms.Position = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "capture_FrameEventNetP2P", "ctlUserVideo.xaml.cs");
            }

            try
            {
                //if (netp2pDirectXVideoChannel != null)
                //{
                //    netp2pDirectXVideoChannel.svcSendStream(VMuktiInfo.CurrentPeer.DisplayName, mms.ToArray());
                //    mms.Position = 0;
                //}
                if (!SendImage_Worker.IsBusy)
                {
                    SendImage_Worker.RunWorkerAsync(mms);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "capture_FrameEventNetP2P", "ctlUserVideo.xaml.cs");
            }
        }
        
        //This function will set the full screen of Me's video
        void SetMyImage(byte[] strmImage)
        {
            try
            {
                if (FullScreenMe)
                {
                    BitmapImage bmi = new BitmapImage();
                    bmi.BeginInit();
                    bmi.StreamSource = new MemoryStream(strmImage);
                    bmi.EndInit();
                    Tfs.FSTpicUserVideo.Source = bmi;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SetMzImage", "ctlUserVideo.xaml.cs");             
            }
        }

        public double[] PreivewWindowLoc()
        {
            try
            {
                double[] winXY = new double[2];
                winXY = capture.PreviewWidowTag();

                return winXY;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "PreviewWindowLoc", "ctlUserVideo.xaml.cs");             
                return null;
            }
        }

        public void ShowVideo()
        {
            try
            {
                capture.showVideo();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ShowVideo", "ctlUserVideo.xaml.cs");             
            }
        }

        public void HideVideo()
        {
            try
            {
                capture.hideVideo();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "HideVideo", "ctlUserVideo.xaml.cs");             
            }
        }

        #endregion

        #region http functions

        void disptGetImages_Tick(object sender, EventArgs e)
        {
            try
            {
                List<Video.Business.Service.DataContracts.clsImages> tempImages = new List<Video.Business.Service.DataContracts.clsImages>();
                tempImages = httpDirectXVideoChannel.svcReceiveStream(VMuktiInfo.CurrentPeer.DisplayName);

                   for (int i = 0; i < tempImages.Count; i++)
                {
                    BitmapImage bmi = new BitmapImage();
                    bmi.BeginInit();
                    bmi.StreamSource = new MemoryStream(tempImages[i].byteArrayImage);
                    bmi.EndInit();
                    picUserVideo.Source = bmi;
                }
                
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "disptGetImages_Tick", "ctlUserVideo.xaml.cs");             
            }
        }

        void capture_FrameEventHttp(System.Drawing.Bitmap BM)
        {
            MemoryStream mms = new MemoryStream();
            try
            {
                BM.Save(mms, System.Drawing.Imaging.ImageFormat.Jpeg);
                mms.Position = 0;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "capture_FrameEventHttp", "ctlUserVideo.xaml.cs");             
            }
            try
            {
                if (FullScreenMe)
                {
                    if (m_DelSendImage != null)
                    {
                        this.Dispatcher.BeginInvoke(DispatcherPriority.Background, m_DelSendImage, mms.ToArray());
                        mms.Position = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "capute_FrameEventHttp", "ctlUserVideo.xaml.cs");             

            }

            try
            {
                if (httpDirectXVideoChannel != null)
                {
                    httpDirectXVideoChannel.svcSendStream(VMuktiInfo.CurrentPeer.DisplayName, mms.ToArray());
                    mms.Position = 0;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "capture_FrameEventHttp", "ctlUserVideo.xaml.cs");             
            }
        }

        #endregion

        #region Netp2p Events

        void ctlUserVideo_EntsvcJoin(string uname)
        {
            try
            {
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlUserVideo_EntsvcJoin", "ctlUserVideo.xaml.cs");             
            }

        }

        void ctlUserVideo_EntsvcSendStream(string uname, byte[] byteArrayImage)
        {
            try
            {
                if (MyVidType != "Me")
                {

                    List<object> lstData = new List<object>();
                    lstData.Add(byteArrayImage);

                    if (!ReceiveImage_Worker.IsBusy)
                    {
                        ReceiveImage_Worker.RunWorkerAsync(lstData);
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlUserVideo_EntsvcSendStream", "ctlUserVideo.xaml.cs");
            }
        }

        void ctlUserVideo_EntsvcUnJoin(string uname)
        {
            try
            {
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlUserVideo_EntsvcUnJoin", "ctlUserVideo.xaml.cs");             
            }

        }

        

        #endregion 

        #region netp2p functions

        void SetOtherImage(byte[] strmImage)
        {
            try
            {
                //this condition will check for the fullscreen option.
                if (FullScreenOther)
                {
                    if (FSUname == MyUserName)
                    {
                        BitmapImage bmi = new BitmapImage();
                        bmi.BeginInit();
                        bmi.StreamSource = new MemoryStream(strmImage);
                        bmi.EndInit();
                        Tfs.FSTpicUserVideo.Source = bmi;
                    }
                }
                else
                {
                    BitmapImage bmi = new BitmapImage();
                    bmi.BeginInit();
                    bmi.StreamSource = new MemoryStream(strmImage);
                    bmi.EndInit();
                    picUserVideo.Source = bmi;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SetOtherImage", "ctlUserVideo.xaml.cs");
            }
        }

        #endregion

        #region SendImage Worker

        void SendImage_Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                AsyncCallback cbRec = new AsyncCallback(SendImageCallback);
                objAsyncSend.BeginInvoke((MemoryStream)e.Argument, cbRec, null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SendImage_Worker_DoWork", "ctlUserVideo.xaml.cs");
            }
        }

        

        void SendImageCallback(IAsyncResult ar)
        {
            try
            {
                objAsyncSend.EndInvoke(ar);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SendImageCallback", "ctlUserVideo.xaml.cs");
            }
        }

        void AsyncSendImage(MemoryStream myStream)
        {
            try
            {
                if (netp2pDirectXVideoChannel != null)
                {
                    netp2pDirectXVideoChannel.svcSendStream(VMuktiInfo.CurrentPeer.DisplayName, myStream.ToArray());
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "AsyncSendImage", "ctlUserVideo.xaml.cs");
            }
        }


        #endregion

        #region ReceiveImage Worker

        void ReceiveImage_Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                AsyncCallback cbReceive = new AsyncCallback(ReceiveImageCallback);
                objAsyncReceive.BeginInvoke((List<object>)e.Argument, cbReceive, null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ReceiveImage_Worker_DoWork", "ctlUserVideo.xaml.cs");
            }
        }

        void ReceiveImageCallback(IAsyncResult ar)
        {
            try
            {
                objAsyncReceive.EndInvoke(ar);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ReceiveImageCallback", "ctlUserVideo.xaml.cs");
            }
        }

        void AsyncRecImage(List<object> lstData)
        {
            try
            {
                if (lstData != null && lstData[0] != null)
                {
                    this.Dispatcher.BeginInvoke(DispatcherPriority.Background, m_DelSendImage, (byte[])lstData[0]);
                }

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "AsyncRecImage", "ctlUserVideo.xaml.cs");
            }
        }


        #endregion

        #region SetImage Worker

        void SetImage_Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, objSetImage);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SetImage_Worker_DoWork", "ctlUserVideo.xaml.cs");
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            
			try
			{		
                
				if (netp2pDirectXVideoChannel != null)
				{
                    netp2pDirectXVideoChannel.Close();
                    netp2pDirectXVideoChannel.Dispose();
                    netp2pDirectXVideoChannel = null;
				}
				if (httpDirectXVideoChannel != null)
				{
					httpDirectXVideoChannel = null;
				}
                GC.Collect();
                GC.WaitForPendingFinalizers();
                
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "Dispose", "ctlUserVideo.xaml.cs");             
			}
        }

        ~ctlUserVideo()
        {
			try
			{
                if (netp2pDirectXVideoChannel != null)
				{
                    netp2pDirectXVideoChannel.Close();
                    netp2pDirectXVideoChannel.Dispose();
                    netp2pDirectXVideoChannel = null;
				}
                if (httpDirectXVideoChannel != null)
				{
                    httpDirectXVideoChannel = null;
				}
                
                GC.SuppressFinalize(this);
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "~ctlUserVideo", "ctlUserVideo.xaml.cs");             
			}
        }

        #endregion
        
    }
}
