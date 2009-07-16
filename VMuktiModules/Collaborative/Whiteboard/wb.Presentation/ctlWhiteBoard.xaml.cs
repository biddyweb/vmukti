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
using System.Collections.ObjectModel;
//using System.Collections.Generic;
using Whiteboard.Business.Service.NetP2P;
using Whiteboard.Business.Service.BasicHttp;
using Whiteboard.Business.Service.DataContracts;
using System.Windows.Ink;
using System.Windows.Forms;
using System.Data;
using System.Drawing;
//using System.Text;
using System.ServiceModel;
using VMuktiAPI;
using VMuktiService;
//using System.Design;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.ComponentModel.Design.Serialization;


namespace wb.Presentation
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    /// 

    public enum ModulePermissions
    {
        Add = 0,
        Edit = 1,
        Delete = 2,
        View = 3
    }

    public partial class ctlWhiteBoard : System.Windows.Controls.UserControl,IDisposable
    {
        object objNetTcpWhiteboard;
        INetTcpWhiteboardChannel channelNetTcp;
        IHttpWhiteboard channelHttp;

        System.Windows.Threading.DispatcherTimer dispTimer = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Normal);

        object objHttpWhiteboard;

       // List<clsStrokes> lstMessage = new List<clsStrokes>();

        //System.Threading.Thread tHostWhiteboard = null;

        //public delegate void DelSendMessage(List<object> lstMsg);
        //public DelSendMessage objDelSendMsg;

        int temp;
        int tempcounter;
        //StringBuilder sb1 = CreateTressInfo();
        public delegate void UIDelegate(List<object> lstMsg);
        public UIDelegate uid;

        public delegate void thickDelegate(double thickness);
        public thickDelegate tkdel;

        public delegate void fontDelegate(double fontSize);
        public fontDelegate fndel;

        public delegate void textDelegate(object strl);
        public textDelegate txtdel;

        public delegate void strokeDelegate(string strokecol);
        public strokeDelegate stkdel;

        public delegate void colorDelegate(string color);
        public colorDelegate coldel;

        public delegate void clearDelegate();
        public clearDelegate cldel;

        public bool mDown;

        public bool mHeight;
        public bool mWidth;
        public int flag;
        
        WhiteBoard WBControl = new WhiteBoard();

        System.Windows.Controls.RichTextBox t;
        System.Windows.Media.Color genCol;
        System.Windows.Forms.ColorDialog c;
        DrawingAttributes ink;
        DrawingAttributes highlighter;
        System.Windows.Ink.StrokeCollection selection;

        double x1, y1, x2, y2;
        double lineThickNess;
        double sizeOfFont;
        string drawElement;
        bool flgMDown;
        bool flgDrawn;

        StylusPoint[] guidCol;
        StylusPoint[] guidCol1;
        Stroke guidStrok;

        //public delegate void DelShowPopUp(string msg, string from);
        //public event DelShowPopUp EntShowPopUp;

        //public string[] strGuestName = null;

        public delegate void DelDisplayName(string lstUserName);
        public DelDisplayName objDelDisplayName;

        public delegate void DelSignOutMessage(List<object> lstMsg);
        public DelSignOutMessage objDelSignOutMsg;

        public delegate void DelGetMessage();
        public DelGetMessage objDelGetMsg;

        public delegate void DelMouseUDraw();
        public DelMouseUDraw objDelMouseUDraw;

       // ModulePermissions[] modPer;

        public string strUri;

        string strRole;

        List<string> lstName;

        public delegate void DelAsyncGetStroke(List<clsStrokes> myMessages);
        public DelAsyncGetStroke objDelAsyncGetStroke;

        System.Threading.Thread thGlobalVariable;
        BackgroundWorker bgHostService;


        public ctlWhiteBoard(VMuktiAPI.PeerType PeerType, string uri, ModulePermissions[] MyPermissions,string Role)
        {
            try
            {
                InitializeComponent();

                thGlobalVariable = new System.Threading.Thread(new System.Threading.ThreadStart(GlobalVariable));
                thGlobalVariable.Start();

                bgHostService = new BackgroundWorker();

                this.Unloaded += new RoutedEventHandler(ctlWhiteBoard_Unloaded);
                
                //uid = new UIDelegate(DrawOnCanvas);
                //tkdel = new thickDelegate(StrokeThickness);
                //fndel = new fontDelegate(FontSize);
                //txtdel = new textDelegate(Ttool);
                //stkdel = new strokeDelegate(Strokes);
                //coldel = new colorDelegate(ColorSel);
                //cldel = new clearDelegate(ClearAll);

                tbWBTray.Visibility = Visibility.Visible;
                genCol = new System.Windows.Media.Color();
                genCol = System.Windows.Media.Color.FromRgb(0, 0, 0);
                inkCanvas1.DefaultDrawingAttributes.Color = genCol;

                for (int i = 1; i <= 5; i++)
                {
                    comboBox1.Items.Add(i.ToString());
                }

                //objDelDisplayName = new DelDisplayName(DisplayName);
                //objDelSignOutMsg = new DelSignOutMessage(delSignoutMessage);
                //objDelGetMsg = new DelGetMessage(delGetMessage);
                //objDelMouseUDraw = new DelMouseUDraw(delMouseUDraw);
                //objDelAsyncGetStroke = new DelAsyncGetStroke(delAsyncGetStroke);
                
                //modPer = MyPermissions;
                //objDelSendMsg = new DelSendMessage(delSendMessage);
                VMuktiAPI.VMuktiHelper.RegisterEvent("SignOut").VMuktiEvent += new VMuktiAPI.VMuktiEvents.VMuktiEventHandler(ctlWhiteBoard_VMuktiEvent);

                System.Windows.Application.Current.Exit += new ExitEventHandler(Current_Exit);

                bgHostService.DoWork += new DoWorkEventHandler(bgHostService_DoWork);

                //tHostWhiteboard = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(hostWhiteboardservice));


                List<object> lstParams = new List<object>();
                lstParams.Add(PeerType);
                lstParams.Add(uri);
                lstParams.Add(MyPermissions);
                //tHostWhiteboard.Start(lstParams);

                bgHostService.RunWorkerAsync(lstParams);

                strRole = Role;
                
                if (strRole != "Host")
                {
                    tbWBTray.Visibility = Visibility.Collapsed;
                }                
                this.Loaded += new RoutedEventHandler(ctlWhiteBoard_Loaded);
            }
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlWhiteBoard", "ctlWhiteBoard.xaml.cs");
			}
        }

        #region Global Variable Initialization

        void GlobalVariable()
        {
            try
            {
                objNetTcpWhiteboard = new clsNetTcpWhiteboard();
                objHttpWhiteboard = new clsHttpWhiteboard();
                mDown = false;
                mHeight = false;
                mWidth = false;
                flag = 1;
                flgMDown = false;
                flgDrawn = false;

                
                c = new ColorDialog();
                ink = new DrawingAttributes();

                x1 = 0;
                y1 = 0;
                x2 = 0;
                y2 = 0;
                lineThickNess = 1;
                sizeOfFont = 1;

                guidCol = new StylusPoint[5];
                guidCol1 = new StylusPoint[2];

                lstName = new List<string>();

                uid = new UIDelegate(DrawOnCanvas);
                tkdel = new thickDelegate(StrokeThickness);
                fndel = new fontDelegate(FontSize);
                txtdel = new textDelegate(Ttool);
                stkdel = new strokeDelegate(Strokes);
                coldel = new colorDelegate(ColorSel);
                cldel = new clearDelegate(ClearAll);

                objDelDisplayName = new DelDisplayName(DisplayName);
                objDelSignOutMsg = new DelSignOutMessage(delSignoutMessage);
                objDelGetMsg = new DelGetMessage(delGetMessage);
                objDelMouseUDraw = new DelMouseUDraw(delMouseUDraw);
                objDelAsyncGetStroke = new DelAsyncGetStroke(delAsyncGetStroke);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GlobalVariable", "ctlWhiteBoard.xaml.cs");
            }
        }

        #endregion

        #region BWHostService

        void bgHostService_DoWork(object sender, DoWorkEventArgs e)
        {
            List<object> lstTempObj = (List<object>)e.Argument;
            strUri = lstTempObj[1].ToString();

            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
                {
                    NetPeerClient npcWhiteboard = new NetPeerClient();
                    ((clsNetTcpWhiteboard)objNetTcpWhiteboard).EJoin += new clsNetTcpWhiteboard.UserJoin(Window1_EJoin);
                    ((clsNetTcpWhiteboard)objNetTcpWhiteboard).EClear += new clsNetTcpWhiteboard.ClearCnv(Window1_EClear);
                    ((clsNetTcpWhiteboard)objNetTcpWhiteboard).EColor += new clsNetTcpWhiteboard.ChangeColor(Window1_EColor);
                    ((clsNetTcpWhiteboard)objNetTcpWhiteboard).EEllipse += new clsNetTcpWhiteboard.DrawEllipse(Window1_EEllipse);
                    ((clsNetTcpWhiteboard)objNetTcpWhiteboard).ELine += new clsNetTcpWhiteboard.DrawLine(Window1_ELine);
                    ((clsNetTcpWhiteboard)objNetTcpWhiteboard).ERect += new clsNetTcpWhiteboard.DrawRect(Window1_ERect);
                    ((clsNetTcpWhiteboard)objNetTcpWhiteboard).EStamper += new clsNetTcpWhiteboard.DrawStamper(Window1_EStamper);
                    ((clsNetTcpWhiteboard)objNetTcpWhiteboard).EStrokes += new clsNetTcpWhiteboard.DrawStrokes(Window1_EStrokes);
                    ((clsNetTcpWhiteboard)objNetTcpWhiteboard).EText += new clsNetTcpWhiteboard.ChangeText(Window1_EText);
                    ((clsNetTcpWhiteboard)objNetTcpWhiteboard).EThickness += new clsNetTcpWhiteboard.ChangeThickness(Window1_EThickness);
                    ((clsNetTcpWhiteboard)objNetTcpWhiteboard).EFontSize += new clsNetTcpWhiteboard.ChangeFontSize(Window1_EFontSize);
                    ((clsNetTcpWhiteboard)objNetTcpWhiteboard).ETTool += new clsNetTcpWhiteboard.DrawTextTool(Window1_ETTool);
                    ((clsNetTcpWhiteboard)objNetTcpWhiteboard).EGetUserList += new clsNetTcpWhiteboard.GetUserList(ctlWhiteBoard_EGetUserList);
                    ((clsNetTcpWhiteboard)objNetTcpWhiteboard).ESetUserList += new clsNetTcpWhiteboard.SetUserList(ctlWhiteBoard_ESetUserList);
                    ((clsNetTcpWhiteboard)objNetTcpWhiteboard).ESignOutChat += new clsNetTcpWhiteboard.SignOutChat(ctlWhiteBoard_ESignOutChat);
                    ((clsNetTcpWhiteboard)objNetTcpWhiteboard).EUnjoin += new clsNetTcpWhiteboard.Unjoin(ctlWhiteBoard_EUnjoin);

                    channelNetTcp = (INetTcpWhiteboardChannel)npcWhiteboard.OpenClient<INetTcpWhiteboardChannel>(strUri, strUri.ToString().Split(':')[2].Split('/')[2], ref objNetTcpWhiteboard);

                    while (temp < 20)
                    {
                        try
                        {
                            channelNetTcp.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                            temp = 20;
                            channelNetTcp.svcGetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
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
                    BasicHttpClient bhcWhiteboard = new BasicHttpClient();
                    channelHttp = (IHttpWhiteboard)bhcWhiteboard.OpenClient<IHttpWhiteboard>(strUri);

                    while (tempcounter < 20)
                    {
                        try
                        {
                            channelHttp.svcWBJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                            tempcounter = 20;
                            channelHttp.svcGetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "bgHostService_DoWork()", "ctlWhiteBoard.xaml.cs");
            }

        }

        #endregion

        #region ressize

        void ctlWhiteBoard_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (((Grid)(this.Parent)) != null)
                {
                    this.Width = ((Grid)(this.Parent)).ActualWidth;
                    ((Grid)(this.Parent)).SizeChanged += new SizeChangedEventHandler(ctlWhiteBoard_SizeChanged);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlWhiteBoard_Loaded()", "ctlWhiteBoard.xaml.cs");

            }
        }

        void ctlWhiteBoard_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (e.NewSize.Width > 0)
                {
                    this.Width = ((Grid)(this.Parent)).ActualWidth;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlWhiteBoard_SizeChanged()", "ctlWhiteBoard.xaml.cs");

            }
        }

        #endregion

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
                if (channelNetTcp != null && channelNetTcp.State == CommunicationState.Opened)
                {
                    channelNetTcp = null;
                }
                VMuktiAPI.VMuktiHelper.UnRegisterEvent("SignOut");
            }			
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Current_Exit", "ctlWhiteBoard.xaml.cs");
			}
        }

		void ctlWhiteBoard_VMuktiEvent(object sender, VMuktiAPI.VMuktiEventArgs e)
		{
			try
			{
				ClosePod();
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlWhiteBoard_VmuktiEvent", "ctlWhiteBoard.xaml.cs");
			}
		}

        void ctlWhiteBoard_Unloaded(object sender, RoutedEventArgs e)
        {

        }

        public void hostWhiteboardservice(object lstParams)//same as chat
		{
			try
			{
				List<object> lstTempObj = (List<object>)lstParams;
				strUri = lstTempObj[1].ToString();
			
				try
				{
					if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
					{
						NetPeerClient npcWhiteboard = new NetPeerClient();
						((clsNetTcpWhiteboard)objNetTcpWhiteboard).EJoin += new clsNetTcpWhiteboard.UserJoin(Window1_EJoin);
						((clsNetTcpWhiteboard)objNetTcpWhiteboard).EClear += new clsNetTcpWhiteboard.ClearCnv(Window1_EClear);
						((clsNetTcpWhiteboard)objNetTcpWhiteboard).EColor += new clsNetTcpWhiteboard.ChangeColor(Window1_EColor);
						((clsNetTcpWhiteboard)objNetTcpWhiteboard).EEllipse += new clsNetTcpWhiteboard.DrawEllipse(Window1_EEllipse);
						((clsNetTcpWhiteboard)objNetTcpWhiteboard).ELine += new clsNetTcpWhiteboard.DrawLine(Window1_ELine);
						((clsNetTcpWhiteboard)objNetTcpWhiteboard).ERect += new clsNetTcpWhiteboard.DrawRect(Window1_ERect);
						((clsNetTcpWhiteboard)objNetTcpWhiteboard).EStamper += new clsNetTcpWhiteboard.DrawStamper(Window1_EStamper);
						((clsNetTcpWhiteboard)objNetTcpWhiteboard).EStrokes += new clsNetTcpWhiteboard.DrawStrokes(Window1_EStrokes);
						((clsNetTcpWhiteboard)objNetTcpWhiteboard).EText += new clsNetTcpWhiteboard.ChangeText(Window1_EText);
						((clsNetTcpWhiteboard)objNetTcpWhiteboard).EThickness += new clsNetTcpWhiteboard.ChangeThickness(Window1_EThickness);
                        ((clsNetTcpWhiteboard)objNetTcpWhiteboard).EFontSize += new clsNetTcpWhiteboard.ChangeFontSize(Window1_EFontSize);
						((clsNetTcpWhiteboard)objNetTcpWhiteboard).ETTool += new clsNetTcpWhiteboard.DrawTextTool(Window1_ETTool);
						((clsNetTcpWhiteboard)objNetTcpWhiteboard).EGetUserList += new clsNetTcpWhiteboard.GetUserList(ctlWhiteBoard_EGetUserList);
						((clsNetTcpWhiteboard)objNetTcpWhiteboard).ESetUserList += new clsNetTcpWhiteboard.SetUserList(ctlWhiteBoard_ESetUserList);
						((clsNetTcpWhiteboard)objNetTcpWhiteboard).ESignOutChat += new clsNetTcpWhiteboard.SignOutChat(ctlWhiteBoard_ESignOutChat);
                        ((clsNetTcpWhiteboard)objNetTcpWhiteboard).EUnjoin += new clsNetTcpWhiteboard.Unjoin(ctlWhiteBoard_EUnjoin);

						channelNetTcp = (INetTcpWhiteboardChannel)npcWhiteboard.OpenClient<INetTcpWhiteboardChannel>(strUri, strUri.ToString().Split(':')[2].Split('/')[2], ref objNetTcpWhiteboard);

						while (temp < 20)
						{
							try
							{
								channelNetTcp.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
								temp = 20;
								channelNetTcp.svcGetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
							}
							catch
							{
								temp++;
								System.Threading.Thread.Sleep(1000);
							}
						}
                        VMuktiAPI.ClsException.WriteToLogFile("strUri: " + strUri + " strUri.ToString().Split(':')[2].Split('/')[2]: " + strUri.ToString().Split(':')[2].Split('/')[2].ToString());
					}
					else
					{
						BasicHttpClient bhcWhiteboard = new BasicHttpClient();
						channelHttp = (IHttpWhiteboard)bhcWhiteboard.OpenClient<IHttpWhiteboard>(strUri);

						while (tempcounter < 20)
						{
							try
							{
								channelHttp.svcWBJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
								tempcounter = 20;
								channelHttp.svcGetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
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
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "hostWhiteboardservice()-1", "ctlWhiteBoard.xaml.cs");
				}
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "hostWhiteboardservice()", "ctlWhiteBoard.xaml.cs");
			}
        }

        void ctlWhiteBoard_EUnjoin(string uName)
        {
            
        } 

        void ctlWhiteBoard_ESignOutChat(string from, List<string> to)
		{
			try
			{
				if (from != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
				{
					List<object> lstStr = new List<object>();
					lstStr.Add(from);
					lstStr.Add(to);
					this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelSignOutMsg, lstStr);
				}
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlWhiteBoard_ESignOutChat()", "ctlWhiteBoard.xaml.cs");

			}

        }        
     
        void delSignoutMessage(List<object> lstStr)
		{
			try
			{
                lstName.Remove(lstStr[0].ToString());
                
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delSignoutMessage()", "ctlWhiteBoard.xaml.cs");

			}
        } //same as chat

        void dispTimer_Tick(object sender, EventArgs e)
        {
			try
			{
                ((System.Windows.Threading.DispatcherTimer)sender).Stop();
                System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(StartThread));
                t.IsBackground = true;
                t.Priority = System.Threading.ThreadPriority.Normal;
                t.Start();                
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "dispTimer_Tick()", "ctlWhiteBoard.xaml.cs");
			}
        }

        void StartThread()
        {
            try
            {
                //this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelGetMsg);
                if (channelHttp != null)
                {
                    channelHttp.BeginsvcGetStrokes(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "", VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, OnCompletion, null);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "StartThread()", "ctlWhiteBoard.xaml.cs");
            }
        }

        void OnCompletion(IAsyncResult result)
        {
            try
            {
                List<clsStrokes> objMsgs = channelHttp.EndsvcGetStrokes(result);
                result.AsyncWaitHandle.Close();
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelAsyncGetStroke, objMsgs);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "OnCompletion()", "ctlWhiteBoard.xaml.cs");
            }

        }

        void delAsyncGetStroke(List<clsStrokes> myMessages)
        {
            try
            {
                //List<clsStrokes> myMessages = channelHttp.svcGetStrokes(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "", VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                if (myMessages != null)
                {
                    for (int i = 0; i < myMessages.Count; i++)
                    {
                        switch (myMessages[i].strOpName)
                        {
                            #region Strokes
                            case "Strokes":
                                {
                                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, stkdel, myMessages[i].strStrokeCollection);
                                    break;
                                }
                            #endregion

                            #region TextTool

                            case "TextTool":
                                {
                                    drawElement = "Texttool";
                                    this.x1 = myMessages[i].dblX1;
                                    this.y1 = myMessages[i].dblY1;
                                    this.x2 = myMessages[i].dblX2;
                                    this.y2 = myMessages[i].dblY2;
                                    List<object> lstStr = new List<object>();
                                    lstStr.Add(myMessages[i].strFrom);
                                    lstStr.Add(myMessages[i].lstTo);
                                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, uid, lstStr);
                                    break;
                                }
                            #endregion

                            #region Rect

                            case "Rect":
                                {
                                    drawElement = "Rectangle";
                                    this.x1 = myMessages[i].dblX1;
                                    this.y1 = myMessages[i].dblY1;
                                    this.x2 = myMessages[i].dblX2;
                                    this.y2 = myMessages[i].dblY2;
                                    List<object> lstStr = new List<object>();
                                    lstStr.Add(myMessages[i].strFrom);
                                    lstStr.Add(myMessages[i].lstTo);
                                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, uid, lstStr);
                                    break;
                                }

                            #endregion

                            #region Ellipse

                            case "Ellipse":
                                {
                                    drawElement = "Ellipse";
                                    this.x1 = myMessages[i].dblX1;
                                    this.y1 = myMessages[i].dblY1;
                                    this.x2 = myMessages[i].dblX2;
                                    this.y2 = myMessages[i].dblY2;
                                    List<object> lstStr = new List<object>();
                                    lstStr.Add(myMessages[i].strFrom);
                                    lstStr.Add(myMessages[i].lstTo);
                                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, uid, lstStr);
                                    break;
                                }

                            #endregion

                            #region ChangeThickness

                            case "ChangeThickness":
                                {
                                    try
                                    {
                                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, tkdel, myMessages[i].dblFontSize);
                                    }
                                    catch (Exception exp)
                                    {
                                        exp.Data.Add("My Key", "Whiteboard--:--Whiteboard.xaml.cs--:--WhiteBoard_EThickness()--:--");
                                    }
                                    break;
                                }

                            #endregion

                            #region fontSize

                            case "ChangeFontSize":
                                {
                                    try
                                    {
                                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, fndel, myMessages[i].dblFontSize);
                                    }
                                    catch (Exception exp)
                                    {
                                        exp.Data.Add("My Key", "Whiteboard--:--Whiteboard.xaml.cs--:--WhiteBoard_EThickness()--:--");
                                    }
                                    break;
                                }

                            #endregion


                            #region Stamper

                            case "Stamper":
                                {
                                    drawElement = "stamper";
                                    this.x1 = myMessages[i].dblX1;
                                    this.y1 = myMessages[i].dblY1;
                                    List<object> lstStr = new List<object>();
                                    lstStr.Add(myMessages[i].strFrom);
                                    lstStr.Add(myMessages[i].lstTo);
                                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, uid, lstStr);
                                    break;
                                }

                            #endregion

                            #region Line

                            case "Line":
                                {
                                    drawElement = "Line";
                                    lineThickNess = myMessages[i].dblThickness;
                                    this.x1 = myMessages[i].dblX1;
                                    this.y1 = myMessages[i].dblY1;
                                    this.x2 = myMessages[i].dblX2;
                                    this.y2 = myMessages[i].dblY2;
                                    List<object> lstStr = new List<object>();
                                    lstStr.Add(myMessages[i].strFrom);
                                    lstStr.Add(myMessages[i].lstTo);
                                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, uid, lstStr);
                                    break;
                                }

                            #endregion

                            #region Color

                            case "Color":
                                {
                                    try
                                    {
                                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, coldel, myMessages[i].strColor);
                                    }
                                    catch (Exception exp)
                                    {
                                        exp.Data.Add("My Key", "Whiteboard--:--Whiteboard.xaml.cs--:--WhiteBoard_EColor()--:--");
                                    }
                                    break;
                                }

                            #endregion

                            #region Clear

                            case "Clear":
                                {
                                    try
                                    {
                                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, cldel);
                                    }
                                    catch (Exception exp)
                                    {
                                        exp.Data.Add("My Key", "Whiteboard--:--Whiteboard.xaml.cs--:--WhiteBoard_EClear()--:--");
                                    }
                                    break;
                                }

                            #endregion

                            #region ChangeText

                            case "ChangeText":
                                {
                                    try
                                    {
                                        List<string> strl = new List<string>();
                                        strl.Add(myMessages[i].strText);
                                        strl.Add(myMessages[i].intCNo.ToString());
                                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, txtdel, strl);
                                    }
                                    catch (Exception exp)
                                    {
                                        exp.Data.Add("My Key", "Whiteboard--:--Whiteboard.xaml.cs--:--WhiteBoard_EText()--:--");
                                    }
                                    break;
                                }

                            #endregion

                            #region SetUserList

                            case "SetUserList":
                                {
                                    if (myMessages[i].strFrom != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                    {
                                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, myMessages[i].strFrom);
                                    }
                                    break;
                                }
                            #endregion

                            #region GetUserList

                            case "GetUserList":
                                {
                                    try
                                    {
                                        if (myMessages[i].strFrom != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                        {
                                            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, myMessages[i].strFrom);
                                            if (channelNetTcp != null)
                                            {
                                                channelNetTcp.svcSetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                                            }

                                            else
                                            {
                                                channelHttp.svcSetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                                            }
                                        }
                                    }
                                    catch (Exception exp)
                                    {
                                        System.Windows.MessageBox.Show("dispTimer_Tick : GetUserList  " + exp.Message);
                                    }
                                    break;
                                }
                            #endregion

                            #region SignOut

                            case "SignOut":
                                {
                                    try
                                    {
                                        if (myMessages[i].strFrom != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                        {
                                            List<object> lstStr = new List<object>();
                                            lstStr.Add(myMessages[i].strFrom);
                                            lstStr.Add(myMessages[i].lstTo);
                                            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelSignOutMsg, lstStr);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delAsyncGetStroke()", "ctlWhiteBoard.xaml.cs");
                                    }
                                    break;
                                }

                            #endregion
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delAsyncGetStroke()", "ctlWhiteBoard.xaml.cs");
            }
            try
            {
                if (this.dispTimer != null)
                {
                    this.dispTimer.Start();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delAsyncGetStroke()", "ctlWhiteBoard.xaml.cs");
            }
        }

        public void DisplayName(string lstUserName)
        {
            try
            {

                bool flag = true;
                for (int i = 0; i < lstName.Count; i++)
                {
                    if (lstName[i] == lstUserName)
                    {
                        flag = false;
                        break;
                    }
                }

                if (flag)
                {
                    lstName.Add(lstUserName);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DisplayName()", "ctlWhiteBoard.xaml.cs");
            }
        }

        public void ClosePod()
		{
			try
			{
				if (channelNetTcp != null)
				{
					channelNetTcp.svcSignOutChat(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName);
				}
				else if (channelHttp != null)
				{
					channelHttp.svcSignOutChat(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName);
				}

				if (channelNetTcp != null)
				{
                    channelNetTcp.Close();
                    channelNetTcp.Dispose();
                    channelNetTcp = null;
				}
				if (channelHttp != null)
				{
					channelHttp = null;
				}
                if (dispTimer != null)
                {
                    dispTimer.Stop();
                    dispTimer = null;
                }
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ClosePod()", "ctlWhiteBoard.xaml.cs");
			}
        }

        void delGetMessage()
        {
            try
            {
                List<clsStrokes> myMessages = channelHttp.svcGetStrokes(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "", VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                if (myMessages != null)
                {
                    for (int i = 0; i < myMessages.Count; i++)
                    {
                        switch (myMessages[i].strOpName)
                        {
                            #region Strokes
                            case "Strokes":
                                {
                                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, stkdel, myMessages[i].strStrokeCollection);
                                    break;
                                }
                            #endregion

                            #region TextTool

                            case "TextTool":
                                {
                                    drawElement = "Texttool";
                                    this.x1 = myMessages[i].dblX1;
                                    this.y1 = myMessages[i].dblY1;
                                    this.x2 = myMessages[i].dblX2;
                                    this.y2 = myMessages[i].dblY2;
                                    List<object> lstStr = new List<object>();
                                    lstStr.Add(myMessages[i].strFrom);
                                    lstStr.Add(myMessages[i].lstTo);
                                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, uid, lstStr);
                                    break;
                                }
                            #endregion

                            #region Rect 

                            case "Rect":
                                {
                                    drawElement = "Rectangle";
                                    this.x1 = myMessages[i].dblX1;
                                    this.y1 = myMessages[i].dblY1;
                                    this.x2 = myMessages[i].dblX2;
                                    this.y2 = myMessages[i].dblY2;
                                    List<object> lstStr = new List<object>();
                                    lstStr.Add(myMessages[i].strFrom);
                                    lstStr.Add(myMessages[i].lstTo);
                                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, uid, lstStr);
                                    break;
                                }

                            #endregion

                            #region Ellipse

                            case "Ellipse":
                                {
                                    drawElement = "Ellipse";
                                    this.x1 = myMessages[i].dblX1;
                                    this.y1 = myMessages[i].dblY1;
                                    this.x2 = myMessages[i].dblX2;
                                    this.y2 = myMessages[i].dblY2;
                                    List<object> lstStr = new List<object>();
                                    lstStr.Add(myMessages[i].strFrom);
                                    lstStr.Add(myMessages[i].lstTo);
                                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, uid, lstStr);
                                    break;
                                }

                            #endregion

                            #region ChangeThickness

                            case "ChangeThickness":
                                {
                                    try
                                    {
                                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, tkdel, myMessages[i].dblFontSize);
                                    }
                                    catch (Exception exp)
                                    {
                                        exp.Data.Add("My Key", "Whiteboard--:--Whiteboard.xaml.cs--:--WhiteBoard_EThickness()--:--");
                                    }
                                    break;
                                }

                            #endregion

                            #region Stamper

                            case "Stamper":
                                {
                                    drawElement = "stamper";
                                    this.x1 = myMessages[i].dblX1;
                                    this.y1 = myMessages[i].dblY1;
                                    List<object> lstStr = new List<object>();
                                    lstStr.Add(myMessages[i].strFrom);
                                    lstStr.Add(myMessages[i].lstTo);
                                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, uid, lstStr);
                                    break;
                                }

                            #endregion

                            #region Line

                            case "Line":
                                {
                                    drawElement = "Line";
                                    this.x1 = myMessages[i].dblX1;
                                    this.y1 = myMessages[i].dblY1;
                                    this.x2 = myMessages[i].dblX2;
                                    this.y2 = myMessages[i].dblY2;
                                    List<object> lstStr = new List<object>();
                                    lstStr.Add(myMessages[i].strFrom);
                                    lstStr.Add(myMessages[i].lstTo);
                                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, uid, lstStr);
                                    break;
                                }

                            #endregion 

                            #region Color

                            case "Color":
                                {
                                    try
                                    {
                                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, coldel, myMessages[i].strColor);
                                    }
                                    catch (Exception exp)
                                    {
                                        exp.Data.Add("My Key", "Whiteboard--:--Whiteboard.xaml.cs--:--WhiteBoard_EColor()--:--");
                                    }
                                    break;
                                }

                            #endregion

                            #region Clear

                            case "Clear":
                                {
                                    try
                                    {
                                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, cldel);
                                    }
                                    catch (Exception exp)
                                    {
                                        exp.Data.Add("My Key", "Whiteboard--:--Whiteboard.xaml.cs--:--WhiteBoard_EClear()--:--");
                                    }
                                    break;
                                }

                            #endregion 

                            #region ChangeText

                            case "ChangeText":
                                {
                                    try
                                    {
                                        List<string> strl = new List<string>();
                                        strl.Add(myMessages[i].strText);
                                        strl.Add(myMessages[i].intCNo.ToString());
                                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, txtdel, strl);
                                    }
                                    catch (Exception exp)
                                    {
                                        exp.Data.Add("My Key", "Whiteboard--:--Whiteboard.xaml.cs--:--WhiteBoard_EText()--:--");
                                    }
                                    break;
                                }

                            #endregion 
                                
                            #region SetUserList

                            case "SetUserList":
                                {
                                    if (myMessages[i].strFrom != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                    {
                                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, myMessages[i].strFrom);
                                    }
                                    break;
                                }

#endregion

                            #region GetUserList

                            case "GetUserList":
                                {
                                    try
                                    {
                                        if (myMessages[i].strFrom != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                        {
                                            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, myMessages[i].strFrom);
                                            if (channelNetTcp != null)
                                            {
                                                channelNetTcp.svcSetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                                            }

                                            else
                                            {
                                                channelHttp.svcSetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                                            }
                                        }
                                    }
                                    catch (Exception exp)
                                    {
                                        System.Windows.MessageBox.Show("dispTimer_Tick : GetUserList  " + exp.Message);
                                    }
                                    break;
                                }
                            #endregion

                            #region SignOut

                            case "SignOut":
                                {
                                    try
                                    {
                                        if (myMessages[i].strFrom != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                        {
                                            List<object> lstStr = new List<object>();
                                            lstStr.Add(myMessages[i].strFrom);
                                            lstStr.Add(myMessages[i].lstTo);
                                            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelSignOutMsg, lstStr);
                                        }
                                    }
                                    catch (Exception exp)
                                    {
                                        VMuktiHelper.ExceptionHandler(exp, "delGetMessage2()", "ctlWhiteBoard.xaml.cs");
                                       
                                    }
                                    break;
                                }

                            #endregion
                        }
                    }
                }            
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delGetMessage()", "ctlWhiteBoard.xaml.cs");
            }
            try
            {
                if (this.dispTimer != null)
                {
                    this.dispTimer.Start();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delGetMessage()", "ctlWhiteBoard.xaml.cs");
            }
        }

        #region WCF Events

        void Window1_ETTool(string from, List<string> to, string strOpName, double x1, double y1, double x2, double y2)
        {
            if (from != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
            {
                drawElement = "Texttool";
                List<object> lstStr = new List<object>();
                lstStr.Add(from);
                lstStr.Add(to);
                this.x1 = x1;
                this.y1 = y1;
                this.x2 = x2;
                this.y2 = y2;
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, uid, lstStr);
            }
        }

        void Window1_EThickness(string from, List<string> to, string strOpName, double thickness)
        {
            if (from != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
            {
                try
                {
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, tkdel, thickness);
                }
                catch (Exception exp)
                {
                    exp.Data.Add("My Key", "Whiteboard--:--Whiteboard.xaml.cs--:--WhiteBoard_EThickness()--:--");
                }
            }
        }

        void Window1_EFontSize(string from, List<string> to, string strOpName, double fontSize)
        {
            if (from != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
            {
                try
                {
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, fndel, fontSize);
                }
                catch (Exception exp)
                {
                    exp.Data.Add("My Key", "Whiteboard--:--Whiteboard.xaml.cs--:--WhiteBoard_EThickness()--:--");
                }
            }
        }

        void Window1_EText(string from, List<string> to, string strOpName, string text, int chldNo)
        {
            if (from != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
            {
                try
                {
                    List<string> strl = new List<string>();
                    strl.Add(text);
                    strl.Add(chldNo.ToString());
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, txtdel, strl);
                }
                catch (Exception exp)
                {
                    exp.Data.Add("My Key", "Whiteboard--:--Whiteboard.xaml.cs--:--WhiteBoard_EText()--:--");
                }
            }
        }

        void Window1_EStrokes(string from, List<string> to, string strOpName, string strokecol)
        {
            try
            {
            if (from != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
            {
                try
                {
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, stkdel, strokecol);
                }
                catch (Exception exp)
                {
                    exp.Data.Add("My Key", "Whiteboard--:--Whiteboard.xaml.cs--:--WhiteBoard_EStrokes()--:--");
                }
            }
            }
                catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Window1_Estroke()", "ctlWhiteBoard.xaml.cs");
            }
            }
        
        void Window1_EStamper(string from, List<string> to, string strOpName, string strImg, double x1, double y1)
        {
            try
            {
            if (from != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
            {
                drawElement = "stamper";
                List<object> lstStr = new List<object>();
                lstStr.Add(from);
                lstStr.Add(to);
                this.x1 = x1;
                this.y1 = y1;
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, uid, lstStr);
            }
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Window1_EStamper()", "ctlWhiteBoard.xaml.cs");
            }
        }

        void Window1_ERect(string from, List<string> to, string strOpName, double x1, double y1, double x2, double y2)
        {
            try
            {
            if (from != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
            {
                drawElement = "Rectangle";
                List<object> lstStr = new List<object>();
                lstStr.Add(from);
                lstStr.Add(to);
                this.x1 = x1;
                this.y1 = y1;
                this.x2 = x2;
                this.y2 = y2;
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, uid, lstStr);
            }
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Window1_ERect()", "ctlWhiteBoard.xaml.cs");
            }
        }
        void Window1_ELine(string from, List<string> to, string strOpName, double x1, double y1, double x2, double y2,double thickness)
        {
            try
            {
                if (from != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    List<object> lstStr = new List<object>();
                    lstStr.Add(from);
                    lstStr.Add(to);
                    drawElement = "Line";
                    this.x1 = x1;
                    this.y1 = y1;
                    this.x2 = x2;
                    this.y2 = y2;
                    lineThickNess = thickness;
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, uid, lstStr);
                }
            }
            catch (Exception exp)
            {
                System.Windows.MessageBox.Show(exp.Message);
            }
        }

        void Window1_EEllipse(string from, List<string> to, string strOpName, double x1, double y1, double x2, double y2)
        {
            try
            {
            if (from != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
            {
                drawElement = "Ellipse";
                List<object> lstStr = new List<object>();
                lstStr.Add(from);
                lstStr.Add(to);
                this.x1 = x1;
                this.y1 = y1;
                this.x2 = x2;
                this.y2 = y2;
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, uid, lstStr);
            }
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Window1_EEllipse()", "ctlWhiteBoard.xaml.cs");
            }
        }
        void Window1_EColor(string from, List<string> to, string strOpName, string color)
        {
            try
            {
            if (from != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
            {
                try
                {
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, coldel, color);
                }
                catch (Exception exp)
                {
                    exp.Data.Add("My Key", "Whiteboard--:--Whiteboard.xaml.cs--:--WhiteBoard_EColor()--:--");
                }
            }
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Window1_EColor()", "ctlWhiteBoard.xaml.cs");
            }
        }
        void Window1_EClear(string from, List<string> to, string strOpName)
        {
            try
            {
            if (from != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
            {
                try
                {
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, cldel);
                }
                catch (Exception exp)
                {
                    exp.Data.Add("My Key", "Whiteboard--:--Whiteboard.xaml.cs--:--WhiteBoard_EClear()--:--");
                }
            }
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Window1_EClear()", "ctlWhiteBoard.xaml.cs");
            }
        }

        void Window1_EJoin(string uname)
        {

        }

        void ctlWhiteBoard_ESetUserList(string uName)
        {
            try
            {
            if (uName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, uName);
            }
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlWhiteBoard_ESetUserList()", "ctlWhiteBoard.xaml.cs");
            }
        }
        void ctlWhiteBoard_EGetUserList(string uName)
        {
            try
            {
                if (uName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, uName);
                    if (channelNetTcp != null)
                    {
                        channelNetTcp.svcSetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    }

                    if(channelHttp!=null)
                    {
                        channelHttp.svcSetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    }
                }
            }
            catch (Exception exp)
            {
                System.Windows.MessageBox.Show("ctlWhiteBoard_EGetUserList " + exp.Message);
            }
        }

        #endregion

        void inkCanvas1_StrokeErased(object sender, RoutedEventArgs e)
		{
			try
			{
				StrokeCollectionConverter aa = new StrokeCollectionConverter();

				if (channelHttp != null)
				{
					channelHttp.svcDrawStrokes(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Strokes", aa.ConvertToString(inkCanvas1.Strokes));
				}
				if (channelNetTcp != null)
				{
                    //string str = this.Name;
					channelNetTcp.svcDrawStrokes(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Strokes", aa.ConvertToString(inkCanvas1.Strokes));
				}
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "inkCanvas1_StrokeErased()", "ctlWhiteBoard.xaml.cs");
			}
        }

        void inkCanvas1_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
		{
			try
			{
				if (e.Key == Key.Delete)
				{
					StrokeCollectionConverter aa = new StrokeCollectionConverter();
					if (channelHttp != null)
					{
						channelHttp.svcDrawStrokes(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Strokes", aa.ConvertToString(inkCanvas1.Strokes));
					}
					if (channelNetTcp != null)
					{
						channelNetTcp.svcDrawStrokes(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Strokes", aa.ConvertToString(inkCanvas1.Strokes));
					}
				}
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "inkCanvas1_KeyUp()", "ctlWhiteBoard.xaml.cs");
                
			}
        }

        void inkCanvas1_SelectionChanged(object sender, EventArgs e)
        {
			try
			{
				StrokeCollectionConverter aa = new StrokeCollectionConverter();
				if (channelHttp != null)
				{
					channelHttp.svcDrawStrokes(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Strokes", aa.ConvertToString(inkCanvas1.Strokes));
				}
				if (channelNetTcp != null)
				{
                    string str = sender.GetType().ToString();
					channelNetTcp.svcDrawStrokes(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Strokes", aa.ConvertToString(inkCanvas1.Strokes));
				}
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "inkCanvas1_SelectionChanged()", "ctlWhiteBoard.xaml.cs");
			}
        }

        void inkCanvas1_SelectionMoved(object sender, EventArgs e)
        {
			try
			{

				StrokeCollectionConverter aa = new StrokeCollectionConverter();
				if (channelHttp != null)
				{
					channelHttp.svcDrawStrokes(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Strokes", aa.ConvertToString(inkCanvas1.Strokes));
				}
				if (channelNetTcp != null)
				{
                    //string str = sender.GetType().ToString();
					channelNetTcp.svcDrawStrokes(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Strokes", aa.ConvertToString(inkCanvas1.Strokes));
				}
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "inkCanvas1_SelectionMoved()", "ctlWhiteBoard.xaml.cs");

			}
        }

        void inkCanvas1_SelectionResizing(object sender, EventArgs e)
        {
                ReadOnlyCollection<UIElement> elements = inkCanvas1.GetSelectedElements();

                foreach (UIElement element in elements)
                {

                    // obtain actual location of element relative to InkCanvas

               System.Windows.Point locationInInkCanvas = element.TranslatePoint(new System.Windows.Point(0, 0), inkCanvas1);



                    // set the location via Left/Top properties

                    element.SetValue(InkCanvas.LeftProperty, locationInInkCanvas.X);

                    element.SetValue(InkCanvas.TopProperty, locationInInkCanvas.Y);



                    // un-set right/bottom properties

                    element.SetValue(InkCanvas.RightProperty, double.NaN);

                    element.SetValue(InkCanvas.BottomProperty, double.NaN);

                    // re-translate any render transform

                    Matrix matRender = element.RenderTransform.Value;

                    matRender.Translate(-matRender.OffsetX, -matRender.OffsetY);

                    element.RenderTransform = new MatrixTransform(matRender);



                    // set margins to zero

                    if (element is FrameworkElement)
                    {

                        ((FrameworkElement)element).Margin = new Thickness(0d);

                    }

                }
                //          inkCanvas1.GetSelectedElements()[0].SetValue(MarginProperty, new Thickness(x1, y1, x2, y2));
            }


        void inkCanvas1_SelectionResized(object sender, EventArgs e)
		{
			try
			{
               //double xx1=inkCanvas1.GetSelectionBounds().TopLeft.X;
               
               // double yy1 = inkCanvas1.GetSelectionBounds().TopLeft.Y;
               // double xx2 = inkCanvas1.GetSelectionBounds().BottomRight.X;
               // double yy2 = inkCanvas1.GetSelectionBounds().BottomRight.Y;
               // inkCanvas1.GetSelectedElements()[0].SetValue(MarginProperty, new Thickness(x1, y1, x2, y2));
               //// channelNetTcp.svcDrawTextTool(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "TextTool", x1, y1, x2, y2);

				//StrokeCollectionConverter aa = new StrokeCollectionConverter();
		/*		if (channelHttp != null)
				{
					channelHttp.svcDrawStrokes(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Strokes", aa.ConvertToString(inkCanvas1.Strokes));
				}
				if (channelNetTcp != null)
				{
                    string str = sender.GetType().ToString();
					channelNetTcp.svcDrawStrokes(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Strokes", aa.ConvertToString(inkCanvas1.Strokes));
				}*/
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "inkCanvas1_SelectionResized()", "ctlWhiteBoard.xaml.cs");
			}
        }

        void comboBox1_SelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			try
			{
				lineThickNess = double.Parse(comboBox1.SelectedItem.ToString());

				if (channelHttp != null)
				{
					channelHttp.svcChangeThickNess(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "ChangeThickness", lineThickNess);
				}
				if (channelNetTcp != null)
				{
					channelNetTcp.svcChangeThickNess(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "ChangeThickness", lineThickNess);
				}
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "comboBox1_SelectionChanged()", "ctlWhiteBoard.xaml.cs");

			}
        }

        void inkCanvas1_StrokeCollected(object sender, InkCanvasStrokeCollectedEventArgs e)
        {
			try
			{
				StrokeCollectionConverter aa = new StrokeCollectionConverter();
				if (channelHttp != null)
				{
					channelHttp.svcDrawStrokes(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Strokes", aa.ConvertToString(inkCanvas1.Strokes));
				}
				if (channelNetTcp != null)
				{
					channelNetTcp.svcDrawStrokes(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Strokes", aa.ConvertToString(inkCanvas1.Strokes));
				}
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "inkCanvas1_StrokeCollected()", "ctlWhiteBoard.xaml.cs");
			}
        }

        public void rect(object o, EventArgs e)
		{
			try
			{
				inkCanvas1.DefaultDrawingAttributes.IsHighlighter = false;
				inkCanvas1.EditingMode = InkCanvasEditingMode.None;
				drawElement = "Rectangle";
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "rect()", "ctlWhiteBoard.xaml.cs");

			}
        }

        public void ellipse(object sender, EventArgs e)
		{
			try
			{
				inkCanvas1.DefaultDrawingAttributes.IsHighlighter = false;
				inkCanvas1.EditingMode = InkCanvasEditingMode.None;
				drawElement = "Ellipse";
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ellipse()", "ctlWhiteBoard.xaml.cs");

			}
        }

        public void Line1(object sender, EventArgs e)
        {
            try
            {
            inkCanvas1.DefaultDrawingAttributes.IsHighlighter = false;
            inkCanvas1.EditingMode = InkCanvasEditingMode.None;
            drawElement = "Line";
		}
		catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Line1()", "ctlWhiteBoard.xaml.cs");

			}
        }

        public void select(object sender, EventArgs e)
        {
            try
            {
                inkCanvas1.EditingMode = InkCanvasEditingMode.Select;
                drawElement = "Select";
                selection = inkCanvas1.GetSelectedStrokes();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "select()", "ctlWhiteBoard.xaml.cs");

            }
        }

        public void erase(object o, EventArgs e)
		{
			try
			{
				drawElement = "Erase";
				inkCanvas1.EditingMode = InkCanvasEditingMode.EraseByPoint;

			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "erase()", "ctlWhiteBoard.xaml.cs");

			}
        }

		public void pen(object o, EventArgs e)
		{
			try
			{
				drawElement = "pen";

				inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
				inkCanvas1.DefaultDrawingAttributes.Color = genCol;

                //  inkCanvas1.DefaultStylusPointDescription=new StylusPointDescription(System.Windows.Input.StylusPointDescription)

				ink.IsHighlighter = false;
				ink.Color = genCol;
				inkCanvas1.DefaultDrawingAttributes = ink;

                //StrokeCollection sc = inkCanvas1.Strokes;
                //string s = sc[0].StylusPoints[0].ToString();
                //System.Windows.MessageBox.Show(s);


                //            StrokeCollection.InkSerializedFormat = "Rectangle";
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "pen()", "ctlWhiteBoard.xaml.cs");
			}
		}

        public void strokeerase(object o, EventArgs e)
        {
            try
            {
            drawElement = "strokeerase";
            inkCanvas1.EditingMode = InkCanvasEditingMode.EraseByStroke;
		}
		catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "strokeerase()", "ctlWhiteBoard.xaml.cs");

			}
        }

        public void clear(object o, EventArgs e)
		{
			try
			{
				inkCanvas1.Strokes.Clear();
				inkCanvas1.Children.Clear();

				if (channelHttp != null)
				{
					channelHttp.svcClearCnv(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Clear");
				}
				if (channelNetTcp != null)
				{
					channelNetTcp.svcClearCnv(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Clear");
				}
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "clear()", "ctlWhiteBoard.xaml.cs");

            }
        }

        public void color(object o, EventArgs e)
		{
			try
			{
				selection = inkCanvas1.GetSelectedStrokes();
				if (selection.Count > 0)
				{
					foreach (System.Windows.Ink.Stroke stroke in selection)
					{
						c.ShowDialog();
						byte r1 = c.Color.R;
						byte g1 = c.Color.G;
						byte b1 = c.Color.B;
						stroke.DrawingAttributes.Color = System.Windows.Media.Color.FromRgb(r1, g1, b1);
					}

					StrokeCollectionConverter aa = new StrokeCollectionConverter();
					if (channelHttp != null)
					{
						channelHttp.svcDrawStrokes(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Strokes", aa.ConvertToString(inkCanvas1.Strokes));
					}
					if (channelNetTcp != null)
					{
						channelNetTcp.svcDrawStrokes(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Strokes", aa.ConvertToString(inkCanvas1.Strokes));
					}
				}
				else
				{
					c.ShowDialog();
					byte a = c.Color.A;
					byte r = c.Color.R;
					byte g = c.Color.G;
					byte b = c.Color.B;

					genCol = System.Windows.Media.Color.FromArgb(a, r, g, b);
					System.Windows.Media.ColorConverter aa = new System.Windows.Media.ColorConverter();
					if (channelHttp != null)
					{
						channelHttp.svcChangeColor(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Color", aa.ConvertToString(genCol));
					}
					if (channelNetTcp != null)
					{
						channelNetTcp.svcChangeColor(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Color", aa.ConvertToString(genCol));
					}
				}
				inkCanvas1.DefaultDrawingAttributes.Color = genCol;
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "color()", "ctlWhiteBoard.xaml.cs");
			}
        }

        public void stamper(object o, EventArgs e)
		{
			try
			{
				inkCanvas1.EditingMode = InkCanvasEditingMode.None;
				drawElement = "stamper";
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "stamper()", "ctlWhiteBoard.xaml.cs");
			}
        }

        public void texttool(object o, EventArgs e)
        {
            try
            {
                inkCanvas1.EditingMode = InkCanvasEditingMode.None;
                drawElement = "Texttool";
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "texttool()", "ctlWhiteBoard.xaml.cs");
            }
        }

        public void marker(object o, EventArgs e)
        {
            try
            {
                drawElement = "marker";
                highlighter = new DrawingAttributes();

                //highlighter.StylusTipTransform = DrawingAttributeIds.StylusTipTransform.
                //highlighter.AddPropertyData(, (System.Guid) new RectangleStylusShape(2,2));// new Rectangle());
                highlighter.IsHighlighter = true;
                highlighter.Color = genCol;
                highlighter.IgnorePressure = true;
                highlighter.StylusTip = StylusTip.Rectangle;
                highlighter.Height = 30;
                highlighter.Width = 10;
                inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
                inkCanvas1.DefaultDrawingAttributes = highlighter;
            }
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "marker()", "ctlWhiteBoard.xaml.cs");
			}
        }

        public void mouseD(object o, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                if (strRole == "Host")
                {
                    flgMDown = true;
                    if (drawElement == "Rectangle" || drawElement == "Ellipse" || drawElement == "Line" || drawElement == "Texttool" || drawElement == "stamper")
                    {
                        x1 = e.GetPosition(inkCanvas1).X;
                        y1 = e.GetPosition(inkCanvas1).Y;
                    }
                    else if (drawElement == "marker")
                    {
                        inkCanvas1.DefaultDrawingAttributes = highlighter;
                    }
                    else if (drawElement == "pen")
                    {
                        inkCanvas1.DefaultDrawingAttributes.Color = genCol;
                        inkCanvas1.EditingMode = InkCanvasEditingMode.Ink;
                    }
                    else if (drawElement == "strokeerase")
                    {
                        inkCanvas1.EditingMode = InkCanvasEditingMode.EraseByStroke;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "mouseD()", "ctlWhiteBoard.xaml.cs");
            }
        }

        public void mouseU(object o, System.Windows.Input.MouseEventArgs e)
		{
            try
            {
                if (strRole == "Host")
                {
                    flgMDown = false;
                    if (flgDrawn == true)
                    {
                        inkCanvas1.Strokes.Remove(inkCanvas1.Strokes[inkCanvas1.Strokes.Count - 1]);
                        flgDrawn = false;
                    }
                    if (drawElement == "Rectangle" || drawElement == "Ellipse" || drawElement == "Line" || drawElement == "Texttool" || drawElement == "stamper")
                    {
                        x2 = e.GetPosition(inkCanvas1).X;
                        y2 = e.GetPosition(inkCanvas1).Y;
                    }
                    //MouseUDrawOnCanvas();

                    System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(MouseUDrawOnCanvas));
                    t.IsBackground = true;
                    t.Priority = System.Threading.ThreadPriority.Normal;
                    t.Start();

                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "mouseU()", "ctlWhiteBoard.xaml.cs");
            }
        }

        public void mouseM(object o, System.Windows.Input.MouseEventArgs e)
		{
			try
			{
				if (flgMDown == true && (drawElement == "Rectangle" || drawElement == "Texttool" || drawElement == "Ellipse"))
				{
					if (flgDrawn == true)
					{
						inkCanvas1.Strokes.Remove(inkCanvas1.Strokes[inkCanvas1.Strokes.Count - 1]);
					}
					guidCol[0].X = x1;
					guidCol[0].Y = y1;

					guidCol[1].X = e.GetPosition(inkCanvas1).X;
					guidCol[1].Y = y1;

					guidCol[2].X = e.GetPosition(inkCanvas1).X;
					guidCol[2].Y = e.GetPosition(inkCanvas1).Y;

					guidCol[3].X = x1;
					guidCol[3].Y = e.GetPosition(inkCanvas1).Y;

					guidCol[4].X = x1;
					guidCol[4].Y = y1;

					StylusPointCollection a = new StylusPointCollection((IEnumerable<StylusPoint>)guidCol);
					guidStrok = new Stroke(a);
					guidStrok.DrawingAttributes.Color = genCol;
					guidStrok.DrawingAttributes.StylusTip = StylusTip.Ellipse;
					inkCanvas1.Strokes.Add(guidStrok);
					flgDrawn = true;
				}


				else if (flgMDown == true && drawElement == "Line")
				{
					if (flgDrawn == true)
					{
						inkCanvas1.Strokes.Remove(inkCanvas1.Strokes[inkCanvas1.Strokes.Count - 1]);
					}
					guidCol1[0].X = x1;
					guidCol1[0].Y = y1;

					guidCol1[1].X = e.GetPosition(inkCanvas1).X;
					guidCol1[1].Y = e.GetPosition(inkCanvas1).Y;

					StylusPointCollection a = new StylusPointCollection((IEnumerable<StylusPoint>)guidCol1);
					guidStrok = new Stroke(a);
					guidStrok.DrawingAttributes.Color = genCol;
					inkCanvas1.Strokes.Add(guidStrok);
					flgDrawn = true;
				}
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "mouseM()", "ctlWhiteBoard.xaml.cs");
			}
        }        

        public void DrawOnCanvas(List<object> lstStr)//only drawelement parameter needs to be included for logging
        {
            try
            {
                if (drawElement == "Rectangle")
                {
                   inkCanvas1.EditingMode = InkCanvasEditingMode.None;
                    inkCanvas1.DefaultDrawingAttributes.IsHighlighter = false;
                    inkCanvas1.Children.Add(WBControl.DrawRectangle(x1, y1, x2, y2, genCol));
                    
                                   }
                else if (drawElement == "Ellipse")
                {
                    inkCanvas1.Children.Add(WBControl.DrawEllipse(x1, y1, x2, y2, genCol));
                   
                }
                else if (drawElement == "Line")
                {
                    inkCanvas1.Children.Add(WBControl.DrawLine(x1, y1, x2, y2, genCol, lineThickNess));
                }
                else if (drawElement == "Texttool")
                {
                    t = new System.Windows.Controls.RichTextBox();
                    t.Height = y2 - y1;
                    t.Width = x2 - x1;
                    t.LostFocus += new RoutedEventHandler(t_LostFocus);
                    t.Margin = new Thickness(x1, y1, 0, 0);
                    t.Tag = inkCanvas1.Children.Count;
                    inkCanvas1.Children.Add(t);
                    t.BorderThickness = new Thickness(0);
                   // t = null;
                    drawElement = "Texttool";                    
                
                    drawElement = "";

                }
                else if (drawElement == "stamper")
                {
                    Uri u = new Uri("Images\\1.gif", UriKind.RelativeOrAbsolute);
                    BitmapImage m = new BitmapImage(u);
                    System.Windows.Controls.Image img = new System.Windows.Controls.Image();
                    img.Height = 20;
                    img.Width = 20;
                    img.Source = m;
                    img.Margin = new Thickness(x1, y1, 0, 0);
                    inkCanvas1.Children.Add(img);
                    inkCanvas1.EditingMode = InkCanvasEditingMode.None;
                   
                }
                else if (drawElement == "Erase")
                {
                    StrokeCollectionConverter aa = new StrokeCollectionConverter();
                    if (channelHttp != null)
                    {
                        channelHttp.svcDrawStrokes(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Strokes", aa.ConvertToString(inkCanvas1.Strokes));
                    }
                    if (channelNetTcp != null)
                    {
                        channelNetTcp.svcDrawStrokes(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Strokes", aa.ConvertToString(inkCanvas1.Strokes));
                    }
                    
                }
            }
			catch (Exception ex)
           {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DrawOnCanvas()", "ctlWhiteBoard.xaml.cs");
			}
        }

        public void MouseUDrawOnCanvas()
        {
            try
            {

                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelMouseUDraw);

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "MouseUDrawOnCanvas()", "ctlWhiteBoard.xaml.cs");
            }

                
        }

        void delMouseUDraw()
        {
            try
            {
                if (drawElement == "Rectangle")
                {
                    inkCanvas1.EditingMode = InkCanvasEditingMode.None;
                    inkCanvas1.DefaultDrawingAttributes.IsHighlighter = false;
                    inkCanvas1.Children.Add(WBControl.DrawRectangle(x1, y1, x2, y2, genCol));
                    //object ob= inkCanvas1.Children[0].GetType();

                    if (channelHttp != null)
                    {
                        channelHttp.svcDrawRect(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Rect", x1, y1, x2, y2);
                    }
                    if (channelNetTcp != null)
                    {
                        this.ToString();
                        channelNetTcp.svcDrawRect(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Rect", x1, y1, x2, y2);
                    }
                }
                else if (drawElement == "Ellipse")
                {
                    inkCanvas1.Children.Add(WBControl.DrawEllipse(x1, y1, x2, y2, genCol));

                    if (channelHttp != null)
                    {
                        channelHttp.svcDrawEllipse(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Ellipse", x1, y1, x2, y2);
                    }
                    if (channelNetTcp != null)
                    {
                        channelNetTcp.svcDrawEllipse(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Ellipse", x1, y1, x2, y2);
                    }
                }
                else if (drawElement == "Line")
                {
                    inkCanvas1.Children.Add(WBControl.DrawLine(x1, y1, x2, y2, genCol, lineThickNess));

                    if (channelHttp != null)
                    {
                        channelHttp.svcDrawLine(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Line", x1, y1, x2, y2,lineThickNess);
                    }
                    if (channelNetTcp != null)
                    {
                        channelNetTcp.svcDrawLine(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Line", x1, y1, x2, y2,lineThickNess);
                    }
                }
                else if (drawElement == "Texttool")
                {
                    t = new System.Windows.Controls.RichTextBox();
                    t.Height = y2 - y1;
                    t.Width = x2 - x1;
                    t.LostFocus += new RoutedEventHandler(t_LostFocus);
                    t.Margin = new Thickness(x1, y1, 0, 0);
                    t.Tag = inkCanvas1.Children.Count;
                    inkCanvas1.Children.Add(t);
                    t.FontSize = 8 + 2*(int.Parse(comboBox1.SelectedItem.ToString()));
                    t.BorderThickness = new Thickness(0);
                    //t = null;
                    drawElement = "";

                    if (channelHttp != null)
                    {
                        channelHttp.svcDrawTextTool(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "TextTool", x1, y1, x2, y2);
                    }
                    if (channelNetTcp != null)
                    {
                        channelNetTcp.svcDrawTextTool(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "TextTool", x1, y1, x2, y2);
                    }
                }
                else if (drawElement == "stamper")
                {
                    Uri u = new Uri("Images\\1.gif", UriKind.RelativeOrAbsolute);
                    BitmapImage m = new BitmapImage(u);
                    System.Windows.Controls.Image i = new System.Windows.Controls.Image();
                    i.Height = 20;
                    i.Width = 20;
                    i.Source = m;
                    i.Margin = new Thickness(x1, y1, 0, 0);
                    inkCanvas1.Children.Add(i);
                    inkCanvas1.EditingMode = InkCanvasEditingMode.None;

                    if (channelHttp != null)
                    {
                        channelHttp.svcDrawStamper(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Stamper", "", x1, y1);
                    }
                    if (channelNetTcp != null)
                    {
                        channelNetTcp.svcDrawStamper(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Stamper", "", x1, y1);
                    }
                }
                else if (drawElement == "Erase")
                {
                    StrokeCollectionConverter aa = new StrokeCollectionConverter();
                    if (channelHttp != null)
                    {
                        channelHttp.svcDrawStrokes(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Strokes", aa.ConvertToString(inkCanvas1.Strokes));
                    }
                    if (channelNetTcp != null)
                    {
                       // string i = this.Name;
                        channelNetTcp.svcDrawStrokes(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "Strokes", aa.ConvertToString(inkCanvas1.Strokes));
                    }
                }
            }
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "delMouseUDraw()", "ctlWhiteBoard.xaml.cs");
			}
        }

        void t_LostFocus(object sender, RoutedEventArgs e)
		{
			try
			{
				System.Windows.Controls.RichTextBox t1 = (System.Windows.Controls.RichTextBox)sender;
				t1.Background = inkCanvas1.Background;

				((System.Windows.Controls.RichTextBox)sender).SelectAll();

				if (channelHttp != null)
				{
					channelHttp.svcChangeText(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "ChangeText", ((System.Windows.Controls.RichTextBox)sender).Selection.Text, int.Parse(((System.Windows.Controls.RichTextBox)sender).Tag.ToString()));

				}
				if (channelNetTcp != null)
				{
					channelNetTcp.svcChangeText(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "ChangeText", ((System.Windows.Controls.RichTextBox)sender).Selection.Text, int.Parse(((System.Windows.Controls.RichTextBox)sender).Tag.ToString()));
				}
				((System.Windows.Controls.RichTextBox)sender).AppendText("");
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "t_LostFocus()", "ctlWhiteBoard.xaml.cs");
			}
        }

        public void selectionchanged(object sender, SelectionChangedEventArgs se)
		{
			try
			{
                
				if (comboBox1.SelectedIndex >= 0)
				{
                   // StrokeThickness(double.Parse(comboBox1.SelectedItem.ToString()));
                    lineThickNess = double.Parse(comboBox1.SelectedItem.ToString());
                    if (channelHttp != null)
                    {
                        channelHttp.svcChangeThickNess(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "ChangeThickness", lineThickNess);
                    }
                    if (channelNetTcp != null)
                    {
                        channelNetTcp.svcChangeThickNess(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "ChangeThickness", lineThickNess);
                    }
					ink.Height = int.Parse(comboBox1.SelectedItem.ToString());
					ink.Width = int.Parse(comboBox1.SelectedItem.ToString());
                    if (t != null)
                    {
                        t.FontSize = (int)(8 + 2 * (int.Parse(comboBox1.SelectedItem.ToString())));
                        sizeOfFont = t.FontSize;
                        if (channelHttp != null)
                        {
                            channelHttp.svcChangeFontSize(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "ChangeFontSize", sizeOfFont);
                        }
                        if (channelNetTcp != null)
                        {
                            channelNetTcp.svcChangeFontSize(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstName, "ChangeFontSize", sizeOfFont);
                        }
                    }
				}
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "selectionchanged()", "ctlWhiteBoard.xaml.cs");

			}
        }

        public void StrokeThickness(double thickness)
        {
            try
            {

                this.lineThickNess = thickness;

            }
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "StrokeThickness()", "ctlWhiteBoard.xaml.cs");

			}
        }

        public void FontSize(double fontSize)
        {
            try
            {
                sizeOfFont = fontSize;
                t.FontSize = sizeOfFont;
                
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FontSize()", "ctlWhiteBoard.xaml.cs");
			}
        }

        public void Ttool(object strl)
        {
            try
            {

                ((System.Windows.Controls.RichTextBox)inkCanvas1.Children[int.Parse(((List<string>)strl)[1])]).SelectAll();
                ((System.Windows.Controls.RichTextBox)inkCanvas1.Children[int.Parse(((List<string>)strl)[1])]).Selection.Text = ((List<string>)strl)[0];
                ((System.Windows.Controls.RichTextBox)inkCanvas1.Children[int.Parse(((List<string>)strl)[1])]).Background = inkCanvas1.Background;
                ((System.Windows.Controls.RichTextBox)inkCanvas1.Children[int.Parse(((List<string>)strl)[1])]).IsReadOnly = true;

            }
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Ttool()", "ctlWhiteBoard.xaml.cs");

			}
        }

        public void Strokes(string strokecol)
        {
			try
			{
				inkCanvas1.Strokes.Clear();
				StrokeCollectionConverter aa = new StrokeCollectionConverter();
				inkCanvas1.Strokes.Add((StrokeCollection)aa.ConvertFromString(strokecol));
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Strokes()", "ctlWhiteBoard.xaml.cs");
			}
        }

        public void ColorSel(string color)
		{
			try
			{
				genCol = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString(color);
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ColorSel()", "ctlWhiteBoard.xaml.cs");

			}
        }

        public void ClearAll()
		{
			try
			{
				inkCanvas1.Strokes.Clear();
				inkCanvas1.Children.Clear();
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ClearAll()", "ctlWhiteBoard.xaml.cs");
			}
        }

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
            if (channelHttp != null)
            {
                channelHttp = null;
            }
            if (channelNetTcp != null)
            {
                channelNetTcp.Close();
                channelNetTcp.Dispose();
                channelNetTcp = null;
            }
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose()", "ctlWhiteBoard.xaml.cs");
            }
        }

        ~ctlWhiteBoard()
        {
            try
            {
                if (channelHttp != null)
                {
                    channelHttp = null;
                }
                if (channelNetTcp != null)
                {
                    channelNetTcp.Close();
                    channelNetTcp.Dispose();
                    channelNetTcp = null;
                }

            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "~ctlWhiteBoard", "ctlWhiteBoard.xaml.cs");
            }

        }

        #endregion


    }
}
