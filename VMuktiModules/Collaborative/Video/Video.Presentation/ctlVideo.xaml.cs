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
using Video.Business.Service.NetP2P;
using Video.Business.Service.BasicHttp;
using VMuktiService;
using System.Threading;
using VMuktiAPI;
using System.Windows.Threading;
using Video.Business.Service.DataContracts;
using System.ServiceModel;

namespace Video.Presentation
{
    public enum MeetingRoles
    {
        Host = 1,
        Participant = 2
    }

    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3 
    }

    public partial class ctlVideo : UserControl
    {
        
        #region for Video variable Declaration

        object objNetP2PMainVideo = new ClsNetP2PMainVideo();
        INetP2PMainVideoChannel channelNetP2P;
        IHttpMainVideo channelHttp;

        IHttpSNodeVideo channelSNodeHttp;

        DispatcherTimer dispTimer = new DispatcherTimer(DispatcherPriority.Normal);

        //object objClsHttpMainVideo = new ClsHttpMainVideo();
        Thread thostMainVideo;
        Thread thostSNodeVideo;

        public delegate void DelGetUserList(List<object> objData);
        public DelGetUserList objGetUserList;

        public delegate void DelRemoveUser(List<object> objData);
        public DelRemoveUser objRemoveUser;

        public delegate void DelClose(List<object> objData);
        public DelClose objClose;

        int tempcounter;
        public string strUri;
        public string strLocalCameraUri;

        MeetingRoles _MyMeetingRole;

        public delegate void DelSignOutMessage(List<object> lstMsg);
        public DelSignOutMessage objDelSignOutMsg;

        ctlUserVideo objUserVideoMe = null;

        #endregion

        #region constructor

        public ctlVideo(VMuktiAPI.PeerType PeerType, string URI, ModulePermissions[] MyPermissions, string MyMeetingRole)
        {
            try
            {
                InitializeComponent();
                
                VMuktiAPI.VMuktiHelper.RegisterEvent("SignOut").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(ctlVideo_VMuktiEvent);
            }
            catch (Exception ex) 
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlVideo", "ctlVideo.xaml.cs");             
            }

            try
            {
                this.Loaded += new RoutedEventHandler(ctlVideo_Loaded);
                this.Unloaded += new RoutedEventHandler(ctlVideo_Unloaded);
                Application.Current.Exit += new ExitEventHandler(Current_Exit);

                if (MyMeetingRole == "Host")
                {
                    _MyMeetingRole = MeetingRoles.Host;
                }
                else
                {
                    _MyMeetingRole = MeetingRoles.Participant;
                }

                objGetUserList = new DelGetUserList(objDelGetUserList);
                objRemoveUser = new DelRemoveUser(objDelRemoveUser);
                objClose = new DelClose(objDelClose);

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.NodeWithHttp)
                {

                    thostSNodeVideo = new Thread(new ParameterizedThreadStart(hostSNodeVideoService));

                    List<object> lstMainVideoParams = new List<object>();
                    lstMainVideoParams.Add(PeerType);
                    lstMainVideoParams.Add(URI);
                    lstMainVideoParams.Add(MyPermissions);
                    lstMainVideoParams.Add(_MyMeetingRole);

                    List<object> lstSNodeVideoParams = new List<object>();
                    lstSNodeVideoParams.Add(PeerType);
                    lstSNodeVideoParams.Add("http://" + VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/NetP2PVideoHoster");
                    lstSNodeVideoParams.Add(MyPermissions);
                    lstSNodeVideoParams.Add(_MyMeetingRole);

                    lstSNodeVideoParams.Add(lstMainVideoParams);

                    thostSNodeVideo.Start(lstSNodeVideoParams);
                }
                else
                {
                    cnvHttp.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlVideo", "ctlVideo.xaml.cs");             
            }
        }

        #endregion

        #region UI Events

        void ctlVideo_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (e == null)
                {
                    (((cnvVideos.Parent as ScrollViewer).Parent as Grid).Parent as UserControl).Width = ((((cnvVideos.Parent as ScrollViewer).Parent as Grid).Parent as UserControl).Parent as Grid).Width;
                    cnvVideos.Width = ((((cnvVideos.Parent as ScrollViewer).Parent as Grid).Parent as UserControl).Parent as Grid).Width;
                }
                else
                {
                    (((cnvVideos.Parent as ScrollViewer).Parent as Grid).Parent as UserControl).Width = e.NewSize.Width;
                    cnvVideos.Width = e.NewSize.Width;

                    if (e.NewSize.Height > 250)
                    {
                        (((cnvVideos.Parent as ScrollViewer).Parent as Grid).Parent as UserControl).Height = e.NewSize.Height;
                        cnvVideos.Height = e.NewSize.Height;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlVideo_SizeChanged", "ctlVideo.xaml.cs");             
                
            }
        }

        void myViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            try
            {
                double[] xy = new double[2];
                xy = objUserVideoMe.PreivewWindowLoc();

                if (Math.Round(myViewer.HorizontalOffset, 0) > 20)
                {
                    objUserVideoMe.HideVideo();
                }

                if (Math.Round(myViewer.HorizontalOffset, 0) == 0)
                {
                    objUserVideoMe.ShowVideo();
                }

                if (Math.Round(myViewer.VerticalOffset, 0) > 20)
                {
                    objUserVideoMe.HideVideo();
                }

                if (Math.Round(myViewer.VerticalOffset, 0) == 0)
                {
                    objUserVideoMe.ShowVideo();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "myViewer_ScrollChanged", "ctlVideo.xaml.cs");             
            }
        }

        #endregion

        #region Host Services

        void hostSNodeVideoService(object lstParams)
        {
            try
            {
                List<object> lstTempObj = (List<object>)lstParams;
                strUri = lstTempObj[1].ToString();
                string strNetP2PURI = "";

                BasicHttpClient httpClient = new BasicHttpClient();
                channelSNodeHttp = (IHttpSNodeVideo)httpClient.OpenClient<IHttpSNodeVideo>(strUri);

                channelSNodeHttp.svcJoin(VMuktiInfo.CurrentPeer.DisplayName);
                strNetP2PURI = channelSNodeHttp.svcStartVideoServer(VMuktiInfo.CurrentPeer.DisplayName, lstTempObj[0].ToString());

                List<object> lstData = new List<object>();
                lstData.Add(VMuktiInfo.CurrentPeer.DisplayName);
                lstData.Add(strNetP2PURI);
                lstData.Add("First");
                lstData.Add(lstTempObj[lstTempObj.Count - 1]);

                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objGetUserList, lstData);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "hostSNodeVideoService", "ctlVideo.xaml.cs");             
            }
        }

        void hostMainVideoService(object lstParams)
        {
            try
            {
                List<object> lstTempObj = (List<object>)lstParams;
                strUri = lstTempObj[1].ToString();

                if ((VMuktiAPI.PeerType)lstTempObj[0] == PeerType.BootStrap || (VMuktiAPI.PeerType)lstTempObj[0] == PeerType.SuperNode || (VMuktiAPI.PeerType)lstTempObj[0] == PeerType.NodeWithNetP2P)
                {
                    NetPeerClient npcNetP2P = new NetPeerClient();
                    ((ClsNetP2PMainVideo)objNetP2PMainVideo).EntsvcJoin += new ClsNetP2PMainVideo.delsvcJoin(ctlVideo_EntsvcJoin);
                    ((ClsNetP2PMainVideo)objNetP2PMainVideo).EntsvcGetUserList += new ClsNetP2PMainVideo.delsvcGetUserList(ctlVideo_EntsvcGetUserList);
                    ((ClsNetP2PMainVideo)objNetP2PMainVideo).EntsvcSetUserList += new ClsNetP2PMainVideo.delsvcSetUserList(ctlVideo_EntsvcSetUserList);
                    ((ClsNetP2PMainVideo)objNetP2PMainVideo).EntsvcClose += new ClsNetP2PMainVideo.delsvcClose(ctlVideo_EntsvcClose);
                    ((ClsNetP2PMainVideo)objNetP2PMainVideo).EntsvcUnJoin += new ClsNetP2PMainVideo.delsvcUnJoin(ctlVideo_EntsvcUnJoin);
                    channelNetP2P = (INetP2PMainVideoChannel)npcNetP2P.OpenClient<INetP2PMainVideoChannel>(strUri, strUri.ToString().Split(':')[2].Split('/')[2], ref objNetP2PMainVideo);

                    while (tempcounter < 20)
                    {
                        try
                        {
                            channelNetP2P.svcJoin(VMuktiInfo.CurrentPeer.DisplayName);
                            tempcounter = 20;
                            channelNetP2P.svcGetUserList(VMuktiInfo.CurrentPeer.DisplayName, strLocalCameraUri);
                        }
                        catch
                        {
                            tempcounter++;
                            System.Threading.Thread.Sleep(1000);
                        }
                    }
                }
                else
                {
                    //BasicHttpClient httpClient = new BasicHttpClient();
                    //channelHttp = (IHttpMainVideo)httpClient.OpenClient<IHttpMainVideo>(strUri);

                    //while (tempcounter < 20)
                    //{
                    //    try
                    //    {
                    //        channelHttp.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    //        tempcounter = 20;
                    //    }
                    //    catch
                    //    {
                    //        tempcounter++;
                    //        System.Threading.Thread.Sleep(1000);
                    //    }
                    //}
                    //dispTimer.Interval = TimeSpan.FromSeconds(5);
                    //dispTimer.Tick += new EventHandler(dispTimer_Tick);
                    //dispTimer.Start();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "hostMainVideoService", "ctlVideo.xaml.cs");             
                
            }

        }

        #endregion

        #region NetP2P Events

        void ctlVideo_EntsvcJoin(string UName)
        {
            
        }

        void ctlVideo_EntsvcGetUserList(string UName, string videoURI)
        {
            try
            {
                if (UName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    List<object> lstData = new List<object>();
                    lstData.Add(UName);
                    lstData.Add(videoURI);

                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objGetUserList, lstData);

                    channelNetP2P.svcSetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, strLocalCameraUri);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlVideo_EntsvcGetUserList", "ctlVideo.xaml.cs");             
            }
        }

        void ctlVideo_EntsvcSetUserList(string UName, string videoURI)
        {
            try
            {
                if (UName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    List<object> lstData = new List<object>();
                    lstData.Add(UName);
                    lstData.Add(videoURI);

                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objGetUserList, lstData);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlVideo_EntsvcSetUserList", "ctlVideo.xaml.cs");             
            }
        }

        void ctlVideo_EntsvcClose(string UName)
        {
            try
            {
                if (UName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {

                    List<object> lstdata = new List<object>();

                    Dispatcher.BeginInvoke(DispatcherPriority.Normal, objClose, lstdata);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlVideo_EntsvcClose", "ctlVideo.xaml.cs");             
            }
        }

        void ctlVideo_EntsvcUnJoin(string UName)
        {
            try
            {
                if (UName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    List<object> lstData = new List<object>();
                    lstData.Add(UName);

                    this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objRemoveUser, lstData);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlVideo_EntsvcUnJoin", "ctlVideo.xaml.cs");             
            }
        }

        #endregion

        #region Other evetns and delegates

        void objDelGetUserList(List<object> lstData)
        {
            try
            {
                int i = 0;
                for (i = 0; i < cnvVideos.Children.Count; i++)
                {
                    if (((ctlUserVideo)cnvVideos.Children[i]).lblUName.Content.ToString() == lstData[0].ToString())
                    {
                        break;
                    }
                }
                if (i > 0 && i == cnvVideos.Children.Count)
                {
                    //ctlUserVideo objUserVideo = new ctlUserVideo(lstData[0].ToString(), "Other", lstData[1].ToString());
					ctlUserVideo objUserVideoOther = new ctlUserVideo(lstData[0].ToString(), "Other", lstData[1].ToString());
                    objUserVideoOther .Margin = new Thickness(5.0);
                    objUserVideoOther .Padding = new Thickness(5.0);
                    objUserVideoOther .MinHeight = 200.0;
                    objUserVideoOther .MinWidth = 200.0;
                    objUserVideoOther .MaxHeight = 200.0;
                    objUserVideoOther .MaxWidth = 200.0;

                    objUserVideoOther .SetValue(Canvas.TopProperty, 0.0);
                    objUserVideoOther .SetValue(Canvas.LeftProperty, (cnvVideos.Children.Count * 200.0) + 5.0);

                    

                    cnvVideos.Children.Add(objUserVideoOther);
                    objUserVideoOther.EntRemoveUser += new ctlUserVideo.DelRemoveUser(objUserVideoOther_EntRemoveUser);

                    if (i == 1)
                    {
                        myViewer.ScrollChanged += new ScrollChangedEventHandler(myViewer_ScrollChanged);
                    }
                }
                else if (i == 0)
                {
                    strLocalCameraUri = lstData[1].ToString();
                    
                    objUserVideoMe = new ctlUserVideo(lstData[0].ToString(), "Me", lstData[1].ToString());
                    objUserVideoMe.Margin = new Thickness(5.0);
                    objUserVideoMe.Padding = new Thickness(5.0);
                    objUserVideoMe.MinHeight = 200.0;
                    objUserVideoMe.MinWidth = 200.0;
                    objUserVideoMe.MaxHeight = 200.0;
                    objUserVideoMe.MaxWidth = 200.0;

                    objUserVideoMe.SetValue(Canvas.TopProperty, 0.0);
                    objUserVideoMe.SetValue(Canvas.LeftProperty, (cnvVideos.Children.Count * 200.0) + 0.0);

                    cnvVideos.Width += 200.0;

                    cnvVideos.Children.Add(objUserVideoMe);
                    objUserVideoMe.EntRemoveUser += new ctlUserVideo.DelRemoveUser(objUserVideoMe_EntRemoveUser);
                }

                
                if (lstData.Count == 4 && lstData[2].ToString() == "First")
                {
                    thostMainVideo = new Thread(new ParameterizedThreadStart(hostMainVideoService));
                    thostMainVideo.Start(lstData[3]);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objDelGetUserList", "ctlVideo.xaml.cs");             
            }
        }

        void objUserVideoOther_EntRemoveUser(string UName)
        {
            try
            {
                //ClsException.WriteToLogFile("objUserVideoOther_EntRemoveUser is called with uname=" + UName);
                List<object> lstData = new List<object>();
                lstData.Add(UName);

                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objRemoveUser, lstData);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objUserVideoOther_EntRemoveUser", "ctlVideo.xaml.cs");             
                //MessageBox.Show("objUserVideoOther_EntRemoveUser"+ex.Message);
            }
        }

        void objUserVideoMe_EntRemoveUser(string UName)
        {
            try
            {
                //ClsException.WriteToLogFile("objUserVideoMe_EntRemoveUser is called with uname=" + UName);
                List<object> lstData = new List<object>();
                lstData.Add(UName);

                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objRemoveUser, lstData);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objUserVideoMe_EntRemoveUser", "ctlVideo.xaml.cs");             
                //MessageBox.Show("objUserVideoMe_EntRemoveUser"+ex.Message);
            }

        }

        void objDelRemoveUser(List<object> lstData)
        {
            try
            {
                for (int i = 0; i < cnvVideos.Children.Count; i++)
                {
                    if (((ctlUserVideo)cnvVideos.Children[i]).lblUName.Content.ToString() == lstData[0].ToString())
                    {
                        cnvVideos.Children.RemoveAt(i);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objDelRemoveUser", "ctlVideo.xaml.cs");             
            }
        }

        void objDelClose(List<object> objData)
        {
            try
            {
                ClosePod();

                cnvVideos.Visibility = Visibility.Collapsed;
                cnvCloseVideo.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "objDelClose", "ctlVideo.xaml.cs");
            }
        }

        void dispTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                List<ClsMessage> myMessages = channelHttp.svcGetMessage(VMuktiInfo.CurrentPeer.DisplayName);
                if (myMessages != null)
                {
                    for (int i = 0; i < myMessages.Count; i++)
                    {
                        if (myMessages[i].strFrom != VMuktiInfo.CurrentPeer.DisplayName)
                        {
                            if (myMessages[i].strMsg == "EntsvcGetUserList")
                            {
                                List<object> lstData = new List<object>();
                                lstData.Add(myMessages[i].strFrom);
                                lstData.Add(myMessages[i].strUri);

                                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objGetUserList, lstData);

                                channelHttp.svcSetUserList(VMuktiInfo.CurrentPeer.DisplayName, ((ctlUserVideo)cnvVideos.Children[0]).MyURI);
                            }
                            else if (myMessages[i].strMsg == "EntsvcSetUserList")
                            {
                                List<object> lstData = new List<object>();
                                lstData.Add(myMessages[i].strFrom);
                                lstData.Add(myMessages[i].strUri);

                                this.Dispatcher.BeginInvoke(DispatcherPriority.Normal, objGetUserList, lstData);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "dispTimer_Tick", "ctlVideo.xaml.cs");             
            }
        }

        void ctlVideo_Loaded(object sender, RoutedEventArgs e)
        {
            try 
            {
                this.Width = ((Grid)this.Parent).ActualWidth;
                ((Grid)this.Parent).SizeChanged += new SizeChangedEventHandler(ctlVideo_SizeChanged);
                ctlVideo_SizeChanged(null, null);

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlVideo_Loaded", "ctlVideo.xaml.cs");             
            }
            //if (_MyMeetingRole != MeetingRoles.Host)
            //{
            //    this.Visibility = Visibility.Collapsed;
            //}
            //else
            //{
            //    this.Visibility = Visibility.Visible;
            //}
        }

        void ctlVideo_Unloaded(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    if (dispTimer != null)
            //    {
            //        dispTimer.Stop();
            //    }
            //    if (channelHttp != null)
            //    {
            //        channelHttp = null;
            //    }
            //    if (channelNetP2P != null && channelNetP2P.State == CommunicationState.Opened)
            //    {
            //        channelNetP2P.svcUnJoin(VMuktiInfo.CurrentPeer.DisplayName);
            //        channelNetP2P.Dispose();
            //        channelNetP2P = null;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Collaborative--:--Video.Presentation--:--ctlVideo.xaml.cs--:--ctlVideo_Unloaded()");
            //    ClsException.LogError(ex);
            //    ClsException.WriteToErrorLogFile(ex);
            //}
        }

        void Current_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                if (dispTimer != null)
                {
                    dispTimer.Stop();
                }
                if (channelHttp != null)
                {
                    channelHttp = null;
                }
                if (channelNetP2P != null && channelNetP2P.State == CommunicationState.Opened)
                {
                    channelNetP2P.svcUnJoin(VMuktiInfo.CurrentPeer.DisplayName);
                    channelNetP2P.Dispose();
                    channelNetP2P = null;
                }
                cnvVideos.Children.Clear();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Current_Exit", "ctlVideo.xaml.cs");             
            }
        }

        #endregion

        #region Sing Out Event and closePOD

        void ctlVideo_VMuktiEvent(object sender, VMuktiEventArgs e)
        {
            ClosePod();
        }

        public void ClosePod()
        {
            //call unjoin method
            try
            {

                if (_MyMeetingRole == MeetingRoles.Host)
                {
                    if (channelNetP2P != null)
                    {
                        channelNetP2P.svcClose(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    }
                }

                foreach (ctlUserVideo ctl1 in cnvVideos.Children)
                {
                    ctl1.ClosePod();
                }
                if (dispTimer != null)
                {
                    dispTimer.Stop();
                }
                if (channelHttp != null)
                {
                    channelHttp = null;
                }
                if (channelNetP2P != null && channelNetP2P.State == CommunicationState.Opened)
                {
                    channelNetP2P.svcUnJoin(VMuktiInfo.CurrentPeer.DisplayName);
                }

                if (channelNetP2P != null)
                {
                    channelNetP2P.Close();
                    channelNetP2P.Dispose();
                    channelNetP2P = null;
                }

                cnvVideos.Children.Clear();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ClosePod", "ctlVideo.xaml.cs");
            }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            //MessageBox.Show("Dispose of chat called");
			try
			{		
                
				if (channelNetP2P != null)
				{
                    channelNetP2P.Close();
                    channelNetP2P.Dispose();
                    channelNetP2P = null;
				}
				if (channelHttp != null)
				{
					channelHttp = null;
				}
                GC.Collect();
                GC.WaitForPendingFinalizers();
                //GC.SuppressFinalize(this);
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "Dispose", "ctlVideo.xaml.cs");             
			}
        }

        ~ctlVideo()
        {
            //MessageBox.Show("destructor of chat called");
			try
			{

                if (channelNetP2P != null)
				{
                    channelNetP2P = null;
				}
                if (channelHttp != null)
				{
                    channelHttp = null;
				}
                
                GC.SuppressFinalize(this);
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "~ctlVideo", "ctlVideo.xaml.cs");             
			}
        }

        #endregion

    }
}
