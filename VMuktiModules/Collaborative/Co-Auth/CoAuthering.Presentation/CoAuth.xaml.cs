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
using System.IO;
using System.Net;
using System.ServiceModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using CoAuthering.Business;
using CoAuthering.Business.BasicHTTP;
using CoAuthering.Business.DataContracts;
using CoAuthering.Business.NetP2P;
using VMuktiAPI;
using VMuktiService;
using System.ComponentModel;
      

namespace CoAuthering.Presentation
{

	#region Size Setter
   
    
	public class CoAuthRichTextBoxTop : IValueConverter
	{
        

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			try
			{
				if (value == null)
				{
					return 0.0;
				}
				return double.Parse(value.ToString()) - 30.0;
			}
			catch (Exception exp)
			{
                VMuktiHelper.ExceptionHandler(exp, "Convert", "CoAuth.xaml.cs");
				return null;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			try
			{
				return null;
			}
			catch (Exception exp)
			{
                VMuktiHelper.ExceptionHandler(exp, "ConvertBack", "CoAuth.xaml.cs");
				return null;
			}
		}
	}

	public class CutRichTextBoxHeight : IValueConverter
	{
      
		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			try
			{
				if (value == null)
				{
					return 0.0;
				}
				return (double.Parse(value.ToString()) * 0.80) - 10.0;
			}
			catch (Exception exp)
			{
                VMuktiHelper.ExceptionHandler(exp, "Convert", "CoAuth.xaml.cs");
				return null;
			}
		}

     

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			try
			{
				return null;
			}
			catch (Exception exp)
			{
                VMuktiHelper.ExceptionHandler(exp, "ConvertBack", "CoAuth.xaml.cs");
				return null;
			}
		}
	}

	public class CutRichTextBoxWidth : IValueConverter
	{
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			try
			{
				if (value == null)
				{
					return 0.0;
				}
				return double.Parse(value.ToString()) - 10.0;
			}
			catch (Exception exp)
			{
                VMuktiHelper.ExceptionHandler(exp, "Convert", "CoAuth.xaml.cs");

				return null;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			try
			{
				return null;
			}
			catch (Exception exp)
			{
                VMuktiHelper.ExceptionHandler(exp, "ConvertBack", "CoAuth.xaml.cs");

				return null;
			}
		}

       
	}

	public class CutRichTextBoxLeft : IValueConverter
	{
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			try
			{
				if (value == null)
				{
					return 0.0;
				}
				return (double.Parse(value.ToString()) / 2.0) - 250.0;
			}
			catch (Exception exp)
			{
                VMuktiHelper.ExceptionHandler(exp, "Convert", "CoAuth.xaml.cs");

				return null;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			try
			{
				return null;
			}
			catch (Exception exp)
			{
                VMuktiHelper.ExceptionHandler(exp, "ConvertBack", "CoAuth.xaml.cs");

				return null;
			}
		}

       
	}

	public class CutRichTextBoxTop : IValueConverter
	{
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			try
			{
				if (value == null)
				{
					return 0.0;
				}
				return (double.Parse(value.ToString()) / 2.0) - 150.0;
			}
			catch (Exception exp)
			{
                VMuktiHelper.ExceptionHandler(exp, "Convert", "CoAuth.xaml.cs");
				return null;
			}
		}

       

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			try
			{
				return null;
			}
			catch (Exception exp)
			{
                VMuktiHelper.ExceptionHandler(exp, "ConvertBack", "CoAuth.xaml.cs");

				return null;
			}
		}
	}

	public class OkButtonLeft : IValueConverter
	{
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			try
			{
				if (value == null)
				{
					return 0.0;
				}
				return (double.Parse(value.ToString()) / 2.0) - 50.0;
			}
			catch (Exception exp)
			{
                VMuktiHelper.ExceptionHandler(exp, "Convert", "CoAuth.xaml.cs");

				return null;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			try
			{
				return null;
			}
			catch (Exception exp)
			{
                VMuktiHelper.ExceptionHandler(exp, "ConvertBack", "CoAuth.xaml.cs");
				return null;
			}
		}
	}

	public class OkButtonTop : IValueConverter
	{
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			try
			{
				if (value == null)
				{
					return 0.0;
				}
				return (double.Parse(value.ToString()) / 2.0) + 150.0 + 5.0;
			}
			catch (Exception exp)
			{
                VMuktiHelper.ExceptionHandler(exp, "Convert", "CoAuth.xaml.cs");
				return null;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			try
			{
				return null;
			}
			catch (Exception exp)
			{
                VMuktiHelper.ExceptionHandler(exp, "ConvertBack", "CoAuth.xaml.cs");

				return null;
			}
		}

       
	}

	#endregion

	public enum ModulePermissions
	{
		Add = 0,
		Edit = 1,
		Delete = 2,
		View = 3
	}

	public partial class CoAuth : System.Windows.Controls.UserControl, IDisposable
	{
       
        /// <summary>
        ///  dispTmr timer is used for synchronize the data that is written in the co-auth module....using this timer it will synchronize
        ///  after every 5 seconds this timer will fire and check any update is there or not....if any update is there then this timer will fire HTTP
        ///  or P2P function.
        /// </summary>        
		System.Windows.Threading.DispatcherTimer dispTmr = new System.Windows.Threading.DispatcherTimer();
        bool typeFlg, flgWriting;
        int saveDoc, saveCDoc; 
        
        /// <summary>
        /// Thread  "tHostChat" is used for initializing the clients channel.
        /// </summary>
        //System.Threading.Thread tHostChat;

		RichTextBox rtbTemp = new RichTextBox();
		TextRange trTemp;

		
		//ModulePermissions[] modPer;		
		public string strUri;
		int temp ;
		int tempcounter;


        /// <summary>
        /// MyCompedData is used to store the data which is coming in small blocks.
        /// </summary>
        byte[] MyCompedData;
        int pointer;


        /// <summary>
        /// dispTimer4HTTP timer will use when any node has type HTTP come.using this timer is used for receiving messages from other participants.
        /// </summary>
        System.Windows.Threading.DispatcherTimer dispTimer4HTTP = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Normal);
		
        /// <summary>
        /// lstParticipants is used for storing participants which are online in the current co-auth modules.
        /// </summary>
        List<string> lstParticipants;

        /// <summary>
        /// delegate  "DelSetTextrange" is used when other participants are sending their bunch of data. in this delegate function those data will be retrived
        /// </summary>
        /// <param name="byteLength"></param>
		delegate void DelSetTextrange(int byteLength);
		DelSetTextrange delSetTextRange;

        /// <summary>
        /// delegate "DelSetDataInRBox" is used when whole data is retrived from other participants and feeding into the RichTextBox.
        /// </summary>
		delegate void DelSetDataInRBox();
		DelSetDataInRBox delSetDataInRBox;
     

        /// <summary>
        /// deletate "DelDisplayName" is used when any new participant is coming into the module.
        /// </summary>
        /// <param name="lstUserName"></param>
        public delegate void DelDisplayName(string lstUserName);
        public DelDisplayName objDelDisplayName;

        /// <summary>
        /// delegate "DelTimerTick" is used when timer "dispTmr" tick is fired and any change is there in the richtextbox content.
        /// </summary>
		public delegate void DelTimerTick();
		DelTimerTick objdelTimerTick;

        /// <summary>
        /// delegate "DelHTTPTimeTick" is used when "dispTimer4HTTP" tick is fired(Only in case of NodeWithHTTP).
        /// </summary>
		public delegate void DelHTTPTimeTick();
		DelHTTPTimeTick objdelHTTPTimerTick;

        /// <summary>
        /// delegate "DelRemoveParticipant" is used when any participants is leaving from current co-auth.
        /// </summary>
        /// <param name="lstUserName"></param>
		public delegate void DelRemoveParticipant(string lstUserName);
		DelRemoveParticipant objDelRemoveParticipant ;

        /// <summary>
        /// DelAsyncGetMessage is used for receiving Messages asynchroniously (In case of NodeWithHTTP).
        /// </summary>
        /// <param name="myMessages"></param>
        public delegate void DelAsyncGetMessage(List<clsCoAuthDataMember> myMessages);
        public DelAsyncGetMessage objDelAsyncGetMessage;


      
        /// <summary>
        /// This region is for defining the HTTP and NetP2P Channels which are used during the co-auth
        /// </summary>
        #region HTTP & NetP2P Channel Declaration
        
        NetPeerClient ncActiveCallInfo;
        object objNetP2PCoAuthService;
		public INetP2PICoAuthServiceChannel chNetP2PCoAuthServiceChannel;
		public IHttpCoAuthServiceChannel chHTTPCoAuthServiceChannel;

        #endregion

        // StringBuilder sb1 = CreateTressInfo();

        
        /// <summary>
        /// This is Main constructor of Co-Auth Module when user drag n drop co-auth module or drag any buddy on co-auth module 
        /// at that time this constructor is called.        
        /// </summary>
        /// <param name="PeerType"></param>
        /// <param name="uri"></param>
        /// <param name="MyPermissions"></param>
        /// <param name="Role">If user has draged the module then it is Host.......and if somebody has drag buddy on it at that time it is Guest.</param>
        /// 
        System.Threading.Thread thGlobalVariable;
        BackgroundWorker bgHostService;

        public CoAuth(VMuktiAPI.PeerType PeerType, string uri, ModulePermissions[] MyPermissions, string Role)
		{
			try
			{
				InitializeComponent();

                thGlobalVariable = new System.Threading.Thread(new System.Threading.ThreadStart(GloablVariable));
                thGlobalVariable.Start();

                bgHostService = new BackgroundWorker();
                bgHostService.DoWork += new DoWorkEventHandler(bgHostService_DoWork);

                //delSetTextRange = new DelSetTextrange(setTextRange);
                //delSetDataInRBox = new DelSetDataInRBox(setDataInRBox);
                //objDelDisplayName = new DelDisplayName(DisplayName);

                //objdelTimerTick = new DelTimerTick(RefreshTimerTick);
                //objdelHTTPTimerTick = new DelHTTPTimeTick(HTTPTimerTick);
				
                //objDelRemoveParticipant = new DelRemoveParticipant(RemoveParticipant);

                //objDelAsyncGetMessage = new DelAsyncGetMessage(delAsyncGetMessage);

				
				string strIP = GetIP4Address();
				UserInfo.CreMachName = strIP;
				UserInfo.MyRole = Role;
				
				VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName = VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName;
				//tHostChat = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(hostCoAuthServices));
				List<object> lstParams = new List<object>();
				lstParams.Add(PeerType);
				lstParams.Add(uri);
				lstParams.Add(MyPermissions);

                bgHostService.RunWorkerAsync(lstParams);

				//tHostChat.Start(lstParams);
				trTemp = new TextRange(rtbTemp.Document.ContentStart, rtbTemp.Document.ContentEnd);
				rtbCoAuth.TextChanged += new TextChangedEventHandler(rtbCoAuth_TextChanged);
				btnOk.Click += new RoutedEventHandler(btnOk_Click);

				rtbCoAuth.Document.MinPageWidth = 1024;

				dispTmr.Tick += new EventHandler(dispTmr_Tick);
				dispTmr.Interval = new TimeSpan(0, 0, 0, 0, 1000);
				dispTmr.Start();

				Application.Current.Exit += new ExitEventHandler(Current_Exit);
				VMuktiAPI.VMuktiHelper.RegisterEvent("SignOut").VMuktiEvent += new VMuktiAPI.VMuktiEvents.VMuktiEventHandler(CoAuth_SignOut_VMuktiEvent);

               
               this.Loaded += new RoutedEventHandler(CoAuth_Loaded);
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "CoAuth", "CoAuth.xaml.cs");

			}
		}
		   
        #region Initialize Global Variable

        void GloablVariable()
        {
            try
            {
                
                lstParticipants = new List<string>();

                objNetP2PCoAuthService = new NetP2PCoAuthService();

                delSetTextRange = new DelSetTextrange(setTextRange);
                delSetDataInRBox = new DelSetDataInRBox(setDataInRBox);
                objDelDisplayName = new DelDisplayName(DisplayName);

                objdelTimerTick = new DelTimerTick(RefreshTimerTick);
                objdelHTTPTimerTick = new DelHTTPTimeTick(HTTPTimerTick);

                objDelRemoveParticipant = new DelRemoveParticipant(RemoveParticipant);

                objDelAsyncGetMessage = new DelAsyncGetMessage(delAsyncGetMessage);

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GloablVariable", "CoAuth.xaml.cs");
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
                    ((NetP2PCoAuthService)objNetP2PCoAuthService).EntSvcJoin += new NetP2PCoAuthService.DelSvcJoin(CoAuth_EntSvcJoin);
                    ((NetP2PCoAuthService)objNetP2PCoAuthService).EntsvcSaveDoc += new NetP2PCoAuthService.DelsvcSaveDoc(CoAuth_EntsvcSaveDoc);
                    ((NetP2PCoAuthService)objNetP2PCoAuthService).EntsvcSetCompBytes += new NetP2PCoAuthService.DelsvcSetCompBytes(CoAuth_EntsvcSetCompBytes);
                    ((NetP2PCoAuthService)objNetP2PCoAuthService).EntsvcSetLength += new NetP2PCoAuthService.DelsvcSetLength(CoAuth_EntsvcSetLength);
                    ((NetP2PCoAuthService)objNetP2PCoAuthService).EntsvcUnJoin += new NetP2PCoAuthService.DelsvcUnJoin(CoAuth_EntsvcUnJoin);
                    ((NetP2PCoAuthService)objNetP2PCoAuthService).EntsvcReplySetLength += new NetP2PCoAuthService.DelsvcReplySetLength(CoAuth_EntsvcReplySetLength);
                    ((NetP2PCoAuthService)objNetP2PCoAuthService).EntsvcGetUserList += new NetP2PCoAuthService.DelsvcGetUserList(CoAuth_EntsvcGetUserList);
                    ((NetP2PCoAuthService)objNetP2PCoAuthService).EntsvcSetUserList += new NetP2PCoAuthService.DelsvcSetUserList(CoAuth_EntsvcSetUserList);
                    ((NetP2PCoAuthService)objNetP2PCoAuthService).EntsvcSignOutCoAuth += new NetP2PCoAuthService.delsvcSignOutCoAuth(CoAuth_EntsvcSetUserList_EntsvcSignOutCoAuth);
                    if (chNetP2PCoAuthServiceChannel != null && chNetP2PCoAuthServiceChannel.State == CommunicationState.Opened)
                    {
                        chNetP2PCoAuthServiceChannel.Close();
                        chNetP2PCoAuthServiceChannel = null;
                    }
                    ncActiveCallInfo = new VMuktiService.NetPeerClient();
                    chNetP2PCoAuthServiceChannel = (INetP2PICoAuthServiceChannel)ncActiveCallInfo.OpenClient<INetP2PICoAuthServiceChannel>(strUri, strUri.ToString().Split(':')[2].Split('/')[2], ref objNetP2PCoAuthService);
                    while (temp < 20)
                    {
                        try
                        {
                            chNetP2PCoAuthServiceChannel.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                            temp = 20;
                            chNetP2PCoAuthServiceChannel.svcGetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
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
                    BasicHttpClient bhcChat = new BasicHttpClient();
                    if (chHTTPCoAuthServiceChannel != null && chHTTPCoAuthServiceChannel.State == CommunicationState.Opened)
                    {
                        chHTTPCoAuthServiceChannel.Close();
                        chHTTPCoAuthServiceChannel = null;
                    }
                    chHTTPCoAuthServiceChannel = (IHttpCoAuthServiceChannel)bhcChat.OpenClient<IHttpCoAuthServiceChannel>(strUri);

                    while (tempcounter < 20)
                    {
                        try
                        {
                            chHTTPCoAuthServiceChannel.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                            tempcounter = 20;
                            chHTTPCoAuthServiceChannel.svcGetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                        }
                        catch
                        {
                            tempcounter++;
                            System.Threading.Thread.Sleep(1000);
                        }
                    }

                    dispTimer4HTTP.Interval = TimeSpan.FromSeconds(2);
                    dispTimer4HTTP.Tick += new EventHandler(dispTimer4HTTP_Tick);
                    dispTimer4HTTP.Start();
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "bgHostService_DoWork", "CoAuth.xaml.cs");
            }
        }

        #endregion

#region Sizechanged

        void CoAuth_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.Parent != null)
                {
                    ((Grid)(this.Parent)).SizeChanged += new SizeChangedEventHandler(CoAuth_SizeChanged);
                    this.Width = ((Grid)(this.Parent)).ActualWidth;
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "CoAuth_Loaded", "CoAuth.xaml.cs");

            }
        }

        void CoAuth_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            try
            {
                if (e.NewSize.Height > 0)
                {
                    this.Width = ((Grid)(this.Parent)).ActualWidth;
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "CoAuth_SizeChanged", "CoAuth.xaml.cs");

            }
        }

        #endregion

        /// <summary>
        /// Fire when application is exits.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		void Current_Exit(object sender, ExitEventArgs e)
		{
            // ** as per chat
			try
			{
				ClosePod();
				if (dispTimer4HTTP != null)
				{
					dispTimer4HTTP.Stop();
				}
				if (chHTTPCoAuthServiceChannel != null)
				{
					chHTTPCoAuthServiceChannel = null;
				}
				if (chNetP2PCoAuthServiceChannel != null && chNetP2PCoAuthServiceChannel.State == CommunicationState.Opened)
				{
					chNetP2PCoAuthServiceChannel = null;
				}				
				VMuktiAPI.VMuktiHelper.UnRegisterEvent("SignOut");
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "Current_Exit", "CoAuth.xaml.cs");

			}
		}

		
        /// <summary>
        /// "CoAuth_SignOut_VMuktiEvent" event is fired from PGHome when user click on SignOut Event and or user closed the pod.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void CoAuth_SignOut_VMuktiEvent(object sender, VMuktiAPI.VMuktiEventArgs e)
		{
            // ** as per chat
            
            ClosePod();
		}

		public void ClosePod()
		{
            // ** as per chat
			try
			{
				//call unjoin method
                         

				if (chNetP2PCoAuthServiceChannel != null)
				{
					chNetP2PCoAuthServiceChannel.svcSignOutCoAuth(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName,lstParticipants);
					chNetP2PCoAuthServiceChannel.svcUnJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
				}
				else if (chHTTPCoAuthServiceChannel != null)
				{
					chHTTPCoAuthServiceChannel.svcSignOutCoAuth(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName,lstParticipants);
					chHTTPCoAuthServiceChannel.svcUnJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
				}

				if (chNetP2PCoAuthServiceChannel != null)
				{
                    chNetP2PCoAuthServiceChannel.Close();
                    chNetP2PCoAuthServiceChannel.Dispose();
                    chNetP2PCoAuthServiceChannel = null;
				}
				if (chHTTPCoAuthServiceChannel != null)
				{
					chHTTPCoAuthServiceChannel = null;
				}
				if (dispTimer4HTTP != null)
				{
					dispTimer4HTTP.Stop();
				}

				if (dispTmr != null)
				{
					dispTmr.Stop();
				}
			}
			catch (Exception exp)
			{

                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ClosePod", "CoAuth.xaml.cs");

			}
		}

		static string GetIP4Address()
		{
			string IP4Address = String.Empty;
			try
			{
				foreach (IPAddress IPA in Dns.GetHostAddresses(System.Net.Dns.GetHostName()))
				{
					if (IPA.AddressFamily.ToString() == "InterNetwork")
					{
						IP4Address = IPA.ToString();
						break;
					}
				}
				
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetIP4Address", "CoAuth.xaml.cs");

			}
            return IP4Address;
		}

       
        /// <summary>
        /// When any new participant comes it will be added to the participants list in this method.
        /// </summary>
        /// <param name="lstUserName"></param>
        public void DisplayName(string lstUserName)
        {
            // ** as per chat
            try
            {

                bool flag = true;
                for (int i = 0; i < lstParticipants.Count; i++)
                {
                    if (lstParticipants[i] == lstUserName)
                    {
                        flag = false;
                        break;
                    }
                }
                if (flag)
                {
                    lstParticipants.Add(lstUserName);
                    lblParticipants.Content += " " + lstUserName + " ";
                                                         

                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DisplayName", "CoAuth.xaml.cs");

            }
        }
		
        /// <summary>
        /// This function is used to Remove participants from the list when he/she has left the module.
        /// </summary>
        /// <param name="lstUserName"></param>
        public void RemoveParticipant(string lstUserName)
		{
            // lstUserName has left the Co-Auth           
                     
            
			try
			{
				if (lstUserName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
				{
					foreach(string sParticipant in lstParticipants)
					{
						if(sParticipant == lstUserName)
						{
							lstParticipants.Remove(lstUserName);
							lblParticipants.Content = lblParticipants.Content.ToString().Replace(" " + lstUserName + " ", "");
                            break;
						}
					}
				}
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "RemoveParticipant", "CoAuth.xaml.cs");

			}
		}
	
		
        /// <summary>
        /// This function is used for opening the clients of HTTP or NetP2P channels.
        /// </summary>
        /// <param name="lstParams"></param>
        void hostCoAuthServices(object lstParams)
		{
            // ** as per chat
			List<object> lstTempObj = (List<object>)lstParams;
			strUri = lstTempObj[1].ToString();
			try
			{
				if ((VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.NodeWithNetP2P || (VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.BootStrap || (VMuktiAPI.PeerType)lstTempObj[0] == VMuktiAPI.PeerType.SuperNode)
				{
					((NetP2PCoAuthService)objNetP2PCoAuthService).EntSvcJoin += new NetP2PCoAuthService.DelSvcJoin(CoAuth_EntSvcJoin);
					((NetP2PCoAuthService)objNetP2PCoAuthService).EntsvcSaveDoc += new NetP2PCoAuthService.DelsvcSaveDoc(CoAuth_EntsvcSaveDoc);
					((NetP2PCoAuthService)objNetP2PCoAuthService).EntsvcSetCompBytes += new NetP2PCoAuthService.DelsvcSetCompBytes(CoAuth_EntsvcSetCompBytes);
					((NetP2PCoAuthService)objNetP2PCoAuthService).EntsvcSetLength += new NetP2PCoAuthService.DelsvcSetLength(CoAuth_EntsvcSetLength);
					((NetP2PCoAuthService)objNetP2PCoAuthService).EntsvcUnJoin += new NetP2PCoAuthService.DelsvcUnJoin(CoAuth_EntsvcUnJoin);
					((NetP2PCoAuthService)objNetP2PCoAuthService).EntsvcReplySetLength += new NetP2PCoAuthService.DelsvcReplySetLength(CoAuth_EntsvcReplySetLength);
					((NetP2PCoAuthService)objNetP2PCoAuthService).EntsvcGetUserList += new NetP2PCoAuthService.DelsvcGetUserList(CoAuth_EntsvcGetUserList);
					((NetP2PCoAuthService)objNetP2PCoAuthService).EntsvcSetUserList += new NetP2PCoAuthService.DelsvcSetUserList(CoAuth_EntsvcSetUserList);
					((NetP2PCoAuthService)objNetP2PCoAuthService).EntsvcSignOutCoAuth += new NetP2PCoAuthService.delsvcSignOutCoAuth(CoAuth_EntsvcSetUserList_EntsvcSignOutCoAuth);
					if (chNetP2PCoAuthServiceChannel != null && chNetP2PCoAuthServiceChannel.State == CommunicationState.Opened)
					{
						chNetP2PCoAuthServiceChannel.Close();
						chNetP2PCoAuthServiceChannel = null;
					}
					ncActiveCallInfo = new VMuktiService.NetPeerClient();
					chNetP2PCoAuthServiceChannel = (INetP2PICoAuthServiceChannel)ncActiveCallInfo.OpenClient<INetP2PICoAuthServiceChannel>(strUri, strUri.ToString().Split(':')[2].Split('/')[2], ref objNetP2PCoAuthService);
					while (temp < 20)
					{
						try
						{
							chNetP2PCoAuthServiceChannel.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
							temp = 20;
							chNetP2PCoAuthServiceChannel.svcGetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
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
					BasicHttpClient bhcChat = new BasicHttpClient();
					if (chHTTPCoAuthServiceChannel != null && chHTTPCoAuthServiceChannel.State == CommunicationState.Opened)
					{
						chHTTPCoAuthServiceChannel.Close();
						chHTTPCoAuthServiceChannel = null;
					}
					chHTTPCoAuthServiceChannel = (IHttpCoAuthServiceChannel)bhcChat.OpenClient<IHttpCoAuthServiceChannel>(strUri);

					while (tempcounter < 20)
					{
						try
						{
							chHTTPCoAuthServiceChannel.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
							tempcounter = 20;
							chHTTPCoAuthServiceChannel.svcGetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
						}
						catch
						{
							tempcounter++;
							System.Threading.Thread.Sleep(1000);
						}
					}
                   
                   
					dispTimer4HTTP.Interval = TimeSpan.FromSeconds(2);
					dispTimer4HTTP.Tick += new EventHandler(dispTimer4HTTP_Tick);
					dispTimer4HTTP.Start();
				}
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "hostCoAuthServices", "CoAuth.xaml.cs");
			}
		}

        /// <summary>
        /// This funciton will only used when Node type is NodeWithHTTP. It will fire in 2 seconds and if any messages stored in dummy clinet 
        /// it will retrive in this method.This function will start one thread which is going to retrive the incoming messages.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		void dispTimer4HTTP_Tick(object sender, EventArgs e)
		{
			dispTimer4HTTP.Stop();

            System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(StartThread));
            t.IsBackground = true;
            t.Priority = System.Threading.ThreadPriority.Normal;
            t.Start(); 
			
		}

        /// <summary>
        /// This function will call the HTTP WCF service asnchroniously to retrive the incoming message.
        /// </summary>
        void StartThread()
        {
            try
            {
                chHTTPCoAuthServiceChannel.BeginsvcGetChangedContext(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, UserInfo.MyRole,OnCompletion,null);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "StartThread", "CoAuth.xaml.cs");
            }
        }

        /// <summary>
        /// In this function will retrive the asnchronious data for HTTP Node.
        /// </summary>
        /// <param name="result"></param>
        void OnCompletion(IAsyncResult result)
        {
            try
            {
                List<clsCoAuthDataMember> objMsgs = chHTTPCoAuthServiceChannel.EndsvcGetChangedContext(result);
                result.AsyncWaitHandle.Close();
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelAsyncGetMessage, objMsgs);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "OnCompletion", "CoAuth.xaml.cs");

            }
        }


        /// <summary>
        /// this function will use for processing the retrived data from "OnCompletion".
        /// </summary>
        /// <param name="myMessages"></param>
        void delAsyncGetMessage(List<clsCoAuthDataMember> myMessages)
        {
            try
            {
                if (myMessages != null && myMessages.Count > 0)
                {
                    dispTmr.Stop();
                    foreach (clsCoAuthDataMember clsMember in myMessages)
                    {
                        if (clsMember.strMsgType == "GetUserList")
                        {
                            try
                            {
                                if (clsMember.strSender != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                {
                                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName,clsMember.strSender);

                                    if (chNetP2PCoAuthServiceChannel != null)
                                    {
                                        chNetP2PCoAuthServiceChannel.svcSetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                                    }

                                    else
                                    {
                                        chHTTPCoAuthServiceChannel.svcSetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                                    }
                                }
                            }
                            catch (Exception exp)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "delAsyncGetMessage", "CoAuth.xaml.cs");
                            }
                        }
                        else if (clsMember.strMsgType == "SetUserList")
                        {
                            if (clsMember.strSender != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                            {
                                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, clsMember.strSender);                                
                            }
                        }

                        else if (clsMember.strMsgType == "CoAuthData")
                        {
                            try
                            {
                                // ** Receiving CoAuthData 
                                
                                
                                // sender clsMember.strSender
                                // receiver VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName
                                // Number of bytes received pointer
                                // Node type
                                if (clsMember.strSender != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                {
                                    if (MyCompedData == null)
                                    {
                                        
                                        MyCompedData = new byte[clsMember.intPointer];
                                    }
                                    for (int i = 0; i < clsMember.myData.Length; i++)
                                    {
                                        MyCompedData[pointer] = clsMember.myData[i];
                                        pointer += 1;
                                    }
                                }
                            }
                            catch (Exception exp)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "delAsyncGetMessage", "CoAuth.xaml.cs");

                            }
                        }
                        else if (clsMember.strMsgType == "LoadCoAuthData")
                        {
                            //}
                            System.Text.StringBuilder sb = new StringBuilder();
                            try
                            {
                                if (clsMember.strSender != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                {
                                    // ** Saving CoAuthData 
                                    // sender clsMember.strSender
                                    // receiver VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName
                                    // Node type
                                    // Number of bytes received pointer                                 
                                                                     
                                    saveCDoc = -1;
                                    saveDoc = 0;
                                    TextRange tr = new TextRange(rtbCoAuth.Document.ContentStart, rtbCoAuth.Document.ContentEnd);
                                    if (typeFlg == true)
                                    {
                                        typeFlg = false;

                                        TextRange trNotInc = new TextRange(rtbCutText.Document.ContentStart, rtbCutText.Document.ContentEnd);
                                        trTemp.Load(new MemoryStream(Compressor.Decompress(MyCompedData)), DataFormats.Rtf);

                                        byte[] mainString = System.Text.ASCIIEncoding.ASCII.GetBytes(trTemp.Text);
                                        byte[] myString = System.Text.ASCIIEncoding.ASCII.GetBytes(tr.Text);
                                        List<byte> notIncludedString = new List<byte>();

                                        int mnStrPointer = 0;
                                        int myStrPointer = 0;
                                        try
                                        {
                                            while (mnStrPointer < mainString.Length)
                                            {
                                                if (mnStrPointer < myString.Length)
                                                {
                                                    if (mainString[mnStrPointer] != myString[myStrPointer])
                                                    {
                                                        while (myString[myStrPointer] != 13)
                                                        {
                                                            notIncludedString.Add(myString[myStrPointer]);
                                                            myStrPointer++;
                                                        }
                                                        while (mainString[mnStrPointer] != 13)
                                                        {
                                                            mnStrPointer++;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        myStrPointer++;
                                                        mnStrPointer++;
                                                    }
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                        }
                                        catch (Exception exp)
                                        {
                                            VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "delAsyncGetMessage", "CoAuth.xaml.cs");
                                        }

                                        if (mnStrPointer < myString.Length)
                                        {
                                            for (int j = mnStrPointer; j < myString.Length; j++)
                                            {
                                                notIncludedString.Add(myString[j]);
                                            }
                                        }

                                        trNotInc.Text = System.Text.ASCIIEncoding.ASCII.GetString(notIncludedString.ToArray());

                                        if (trNotInc.Text.Replace("\r", "").Replace("\n", "").Length != 0)
                                        {
                                            cnvCoAuth.Opacity = 0.5;
                                            cnvCoAuth.Background = System.Windows.Media.Brushes.AliceBlue;
                                            cnvCoAuth.IsEnabled = false;

                                            InkTrans.Visibility = Visibility.Visible;
                                            lblCaption.Visibility = Visibility.Visible;
                                            rtbCutText.Visibility = Visibility.Visible;
                                            btnOk.Visibility = Visibility.Visible;
                                            rtbCutText.Focus();
                                        }
                                    }

                                    rtbCoAuth.TextChanged -= new TextChangedEventHandler(rtbCoAuth_TextChanged);
                                    tr.Load(new MemoryStream(Compressor.Decompress(MyCompedData)), DataFormats.Rtf);
                                    rtbCoAuth.TextChanged += new TextChangedEventHandler(rtbCoAuth_TextChanged);
                                }
                            }
                            catch (Exception exp)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "delAsyncGetMessage", "CoAuth.xaml.cs");

                            }
                            finally
                            { 
                                MyCompedData = null;
                                flgWriting = false;
                                pointer = 0;
                                if (UserInfo.MyRole == "Host")
                                {
                                    MyUsers.MyCompedData = null;
                                    MyUsers.flgWriting = false;
                                    MyUsers.pointer = 0;
                                }
                            }
                        }

                        else if (clsMember.strMsgType == "SignOut")
                        {
                            if (clsMember.strSender != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                            {
                                // ** as per chat                                                            
                                
                                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelRemoveParticipant, clsMember.strSender);
                            }
                        }

                        else if (clsMember.strMsgType == "SetLength")
                        {
                            if (clsMember.isLengthSet)
                            {
                                TextRange tr = new TextRange(rtbCoAuth.Document.ContentStart, rtbCoAuth.Document.ContentEnd);
                                MemoryStream myStr = new MemoryStream();
                                tr.Save(myStr, DataFormats.Rtf);

                                byte[] myOrgData = myStr.ToArray();
                                byte[] myCompData = Compressor.Compress(myOrgData);

                                byte[] arr = new byte[5000];
                                byte[] Larr;
                                int i = 0;

                                if (myCompData.Length > 5000)
                                {
                                    for (i = 0; i < myCompData.Length / 5000; i++)
                                    {
                                        for (int j = 0; j < 5000; j++)
                                        {
                                            arr[j] = myCompData[(i * 5000) + j];
                                        }
                                        // ** Sending CoAuthData 
                                        StringBuilder sb = new StringBuilder();
                                        
                                        for (int te = 0; te < lstParticipants.Count; te++)
                                        {
                                            sb.AppendLine(lstParticipants[te]);
                                        }
                                                                            
                                        // sender VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName
                                        // Receiver lstparticipants
                                        // Node type
                                        chHTTPCoAuthServiceChannel.svcSetCompBytes(myCompData.Length, arr, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstParticipants);
                                    }

                                    if (i * 5000 != myCompData.Length)
                                    {
                                        Larr = new byte[(myCompData.Length) - (i * 5000)];
                                        for (int j = 0; j < (myCompData.Length) - (i * 5000); j++)
                                        {
                                            Larr[j] = myCompData[(i * 5000) + j];
                                        }
                                        // ** Sending CoAuthData 
                                        StringBuilder sb2 = new StringBuilder();
                                        
                                        for (int te1 = 0; te1 < lstParticipants.Count; te1++)
                                        {
                                            sb2.AppendLine(lstParticipants[te1]);
                                        }
                                                                              
                                        // sender VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName
                                        // Receiver lstparticipants
                                        // Node type
                                        chHTTPCoAuthServiceChannel.svcSetCompBytes(myCompData.Length, Larr, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstParticipants);
                                    }
                                }
                                else
                                {
                                    Larr = new byte[myCompData.Length];
                                    for (int j = 0; j < myCompData.Length; j++)
                                    {
                                        Larr[j] = myCompData[j];
                                    }
                                    // ** Sending CoAuthData 
                                    StringBuilder sb22 = new StringBuilder();
                                    
                                    for (int te12 = 0; te12 < lstParticipants.Count; te12++)
                                    {
                                        sb22.AppendLine(lstParticipants[te12]);
                                    }
                                                                     
                                    // sender VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName
                                    // Receiver lstparticipants
                                    // Node types
                                    chHTTPCoAuthServiceChannel.svcSetCompBytes(myCompData.Length, Larr, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstParticipants);
                                }
                                // ** Saving CoAuthData 
                                StringBuilder sb23 = new StringBuilder();
                               
                                for (int te13 = 0; te13 < lstParticipants.Count; te13++)
                                {
                                    sb23.AppendLine(lstParticipants[te13]);
                                }
                                                            
                                // sender VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName
                                // Receiver lstparticipants
                                // Node types
                                chHTTPCoAuthServiceChannel.svcSaveDoc(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstParticipants);
                            }
                        }

                        else if (clsMember.strMsgType == "SetWritingFlag")
                        {
                            if (MyUsers.flgWriting == false)
                            {
                                // ** Setting Writing flag for user 
                                // sender clsMember.strSender
                                // bytelength
                                // Node types
                                // true
                                
                                                              
                                MyUsers.flgWriting = true;
                                MyUsers.pointer = 0;
                                MyUsers.MyCompedData = new byte[clsMember.byteLength];
                                chHTTPCoAuthServiceChannel.svcReplySetLength(clsMember.byteLength, true, clsMember.strSender);

                            }
                            else
                            {
                                // ** Setting Writing flag for user 
                                // sender clsMember.strSender
                                // bytelength                            
                                // Node types                             
                                
                                chHTTPCoAuthServiceChannel.svcReplySetLength(clsMember.byteLength, false, clsMember.strSender);
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "delAsyncGetMessage", "CoAuth.xaml.cs");

            }
            finally
            {
                dispTimer4HTTP.Start();
                dispTmr.Start();
            }
        }

        


      
		
        /// <summary>
        /// This timer tick will fire in every 5 second......for synchronize the data with other participants. this function will start one new
        /// thread.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void dispTmr_Tick(object sender, EventArgs e)
		{
            try
            {
               
                System.Threading.Thread t = new System.Threading.Thread(new System.Threading.ThreadStart(StartRefreshTimerThread));
                t.IsBackground = true;
                t.Priority = System.Threading.ThreadPriority.Normal;
                t.Start();
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "dispTmr_Tick", "CoAuth.xaml.cs");

            }
			
		}
        
        /// <summary>
        /// This function will start work on synchronize the data when called from "dispTmr_Tick".
        /// </summary>
        void StartRefreshTimerThread()
        {
            try
            {
                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objdelTimerTick);               
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "StartRefreshTimerThread", "CoAuth.xaml.cs");

            }

        }

       
        /// <summary>
        /// This function will fire when thread "StartRefreshTimerThread" calles it.
        /// </summary>
		void RefreshTimerTick()
		{
			try
			{
				if (typeFlg == true)
				{
					if (saveDoc == 2)
					{
                        dispTmr.Stop();
						SaveDoc();
						typeFlg = false;
						saveCDoc = -1;
						saveDoc = 0;
					}
					else
					{
						saveDoc++;
					}

					if (saveCDoc == 10)
					{
                        dispTmr.Stop();
						SaveDoc();
						saveCDoc = 0;
						saveDoc = 0;
					}
					else
					{
						saveCDoc++;
					}
				}
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "RefreshTimerTick", "CoAuth.xaml.cs");

			}
		}


        /// <summary>
        /// This Function will fire when calling thread is executed "StartRefreshTimerThread". This fucntion will execute and go to server if any update is
        /// there  in the richtextbox....if any update is there then this will go to "HOST" and does the processing.
        /// </summary>
        void SaveDoc()
        {
            //lock (this)            
            {
                try
                {

                    TextRange tr = new TextRange(rtbCoAuth.Document.ContentStart, rtbCoAuth.Document.ContentEnd);
                    MemoryStream myStr = new MemoryStream();
                    tr.Save(myStr, DataFormats.Rtf);

                    if (tr.Text.Length > 0)
                    {
                        byte[] myOrgData = myStr.ToArray();
                        byte[] myCompData = Compressor.Compress(myOrgData);

                        if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap)
                        {

                            if (chNetP2PCoAuthServiceChannel != null)
                            {
                                // ** Setting Writing flag for user                                                             
                                
                                // sender VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName                                    
                                // Node types    
                                // Myrole UserInfo.MyRole
                                chNetP2PCoAuthServiceChannel.svcSetLength(myCompData.Length, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, UserInfo.MyRole);
                            }
                        }
                        else
                        {
                            
                            byte[] arr = new byte[5000];
                            byte[] Larr;
                            int i = 0;

                           
                            bool isLengthSet = false;
                            if (UserInfo.MyRole == "Host")
                            {

                                if (MyUsers.flgWriting == false)
                                {
                                    dispTimer4HTTP.Stop();
                                    MyUsers.flgWriting = true;
                                    MyUsers.pointer = 0;
                                    MyUsers.MyCompedData = new byte[myCompData.Length];
                                    isLengthSet = true;
                                }
                            }
                            else
                            {
                                // ** Setting Writing flag for user 

                               
                                 // sender VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName                                    
                                // Node types    
                                // Myrole UserInfo.MyRole
                                chHTTPCoAuthServiceChannel.svcSetLength(myCompData.Length, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, UserInfo.MyRole);
                            }
                            if (isLengthSet)
                            {
                                if (myCompData.Length > 5000)
                                {
                                    for (i = 0; i < myCompData.Length / 5000; i++)
                                    {
                                        for (int j = 0; j < 5000; j++)
                                        {
                                            arr[j] = myCompData[(i * 5000) + j];
                                        }
                                        // ** Sending CoAuth Data

                                        StringBuilder sb3 = new StringBuilder();
                                       
                                        for (int d = 0; d < lstParticipants.Count; d++)
                                        {
                                            sb3.AppendLine(lstParticipants[d]);
                                        }
                                       
                                        
                                        // sender VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName
                                        // Receiver lstParticipants
                                        // Node types 
                                        chHTTPCoAuthServiceChannel.svcSetCompBytes(myCompData.Length, arr, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstParticipants);
                                    }

                                    if (i * 5000 != myCompData.Length)
                                    {
                                        Larr = new byte[(myCompData.Length) - (i * 5000)];
                                        for (int j = 0; j < (myCompData.Length) - (i * 5000); j++)
                                        {
                                            Larr[j] = myCompData[(i * 5000) + j];
                                        }
                                        // ** Sending CoAuth Data
                                        StringBuilder sb31 = new StringBuilder();
                                        
                                        for (int d = 0; d < lstParticipants.Count; d++)
                                        {
                                            sb31.AppendLine(lstParticipants[d]);
                                        }
                                                                               
                                        // sender VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName
                                        // Receiver lstParticipants
                                        // Node types 

                                        chHTTPCoAuthServiceChannel.svcSetCompBytes(myCompData.Length, Larr, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstParticipants);
                                    }
                                }
                                else
                                {
                                    Larr = new byte[myCompData.Length];
                                    for (int j = 0; j < myCompData.Length; j++)
                                    {
                                        Larr[j] = myCompData[j];
                                    }
                                    // ** Sending CoAuth Data
                                    StringBuilder sb32 = new StringBuilder();
                                    
                                    for (int d1 = 0; d1 < lstParticipants.Count; d1++)
                                    {
                                        sb32.AppendLine(lstParticipants[d1]);
                                    }
                                                                       
                                    // sender VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName
                                    // Receiver lstParticipants
                                    // Node types 
                                    chHTTPCoAuthServiceChannel.svcSetCompBytes(myCompData.Length, Larr, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstParticipants);
                                }
                                // ** Saving CoAuth Data
                                // sender VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName
                                // Receiver lstParticipants
                                StringBuilder sb33 = new StringBuilder();
                                
                                for (int d2 = 0; d2 < lstParticipants.Count; d2++)
                                {
                                    sb33.AppendLine(lstParticipants[d2]);
                                }
                                                              
                                // Node types 
                                chHTTPCoAuthServiceChannel.svcSaveDoc(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstParticipants);
                                if (UserInfo.MyRole == "Host")
                                {
                                    MyUsers.MyCompedData = null;
                                    MyUsers.flgWriting = false;
                                    MyUsers.pointer = 0;
                                }
                                MyCompedData = null;
                                flgWriting = false;
                                pointer = 0;
                               
                            }
                        }

                    }
                }

                catch (Exception exp)
                {
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "SaveDoc", "CoAuth.xaml.cs");
                }
                finally
                {
                    dispTmr.Start();
                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                    {
                        dispTimer4HTTP.Start();
                    }
                }
            }
        }


        /// <summary>
        /// Click on this button to close the "Discarded text Windows".
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
		void btnOk_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				cnvCoAuth.Opacity = 1;
				cnvCoAuth.Background = System.Windows.Media.Brushes.Transparent;
				cnvCoAuth.IsEnabled = true;
				InkTrans.Visibility = Visibility.Collapsed;
				lblCaption.Visibility = Visibility.Collapsed;
				rtbCutText.Visibility = Visibility.Collapsed;
				btnOk.Visibility = Visibility.Collapsed;
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "btnOk_Click", "CoAuth.xaml.cs");
			}
		}

		void rtbCoAuth_TextChanged(object sender, TextChangedEventArgs e)
		{
			try
			{
				typeFlg = true;
				saveDoc = 0;
				isFileEdited = true;
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "rtbCoAuth_TextChanged", "CoAuth.xaml.cs");

			}

		}

       



        /// <summary>
        /// Application Command Region is used for "Save/Open/CreateNew" stuffs in the Richtextbox.
        /// </summary>
		#region Application Command
		string strOpenFilePath = string.Empty;
		bool isFileEdited;
		private void CommandBinding_CanExecute_SaveAs(object sender, CanExecuteRoutedEventArgs e)
		{
			try
			{
				e.CanExecute = true;
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "CommandBinding_CanExecute_SaveAs", "CoAuth.xaml.cs");

			}
		}
		private void CommandBinding_Executed_SaveAs(object sender, ExecutedRoutedEventArgs e)
		{
			try
			{
				
					Microsoft.Win32.SaveFileDialog dlgSaveFile = new Microsoft.Win32.SaveFileDialog();
					dlgSaveFile.Filter = "Text Files (*.rtf)|*.rtf";
					dlgSaveFile.FilterIndex = 1;
					dlgSaveFile.RestoreDirectory = true;

					if (dlgSaveFile.ShowDialog() == true)
					{
						
						// Code to write the stream goes here.
						
						SaveFile(dlgSaveFile.FileName, rtbCoAuth);
						strOpenFilePath = dlgSaveFile.FileName;
						//  }
					}

                

                //if (strOpenFilePath.Length == 0)
                //{
					
                //    Microsoft.Win32.SaveFileDialog dlgSaveFile = new Microsoft.Win32.SaveFileDialog();
                //    dlgSaveFile.Filter = "Text Files (*.rtf)|*.rtf";
                //    dlgSaveFile.FilterIndex = 1;
                //    dlgSaveFile.RestoreDirectory = true;

                //    if (dlgSaveFile.ShowDialog() == true)
                //    {
						
                //        // Code to write the stream goes here.
						
                //        SaveFile(dlgSaveFile.FileName, rtbCoAuth);
                //        strOpenFilePath = dlgSaveFile.FileName;
                //        //  }
                //    }
                //}
                //else
                //{
					
                //    SaveFile(strOpenFilePath, rtbCoAuth);
                //}
                //isFileEdited = false;
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "CommandBinding_Executed_SaveAs", "CoAuth.xaml.cs");

			}
		}
		private void CommandBinding_CanExecute_Open(object sender, CanExecuteRoutedEventArgs e)
		{
			try
			{
				e.CanExecute = true;
				
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "CommandBinding_CanExecute_Open", "CoAuth.xaml.cs");
			}
		}

		private void CommandBinding_Executed_Open(object sender, ExecutedRoutedEventArgs e)
		{
			try
			{

				TextRange trange = new TextRange(rtbCoAuth.Document.ContentStart, rtbCoAuth.Document.ContentEnd);

				if (isFileEdited)
				{
					
					if (strOpenFilePath.Length == 0)
					{
						System.Windows.Forms.DialogResult dlr;
						dlr = System.Windows.Forms.MessageBox.Show("Do you want to save the file", "Save", System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question);

						if (dlr == System.Windows.Forms.DialogResult.Yes)
						{
							System.Windows.Forms.SaveFileDialog dlgSaveFile = new System.Windows.Forms.SaveFileDialog();
							dlgSaveFile.Filter = "Text Files (*.rtf)|*.rtf";
							dlgSaveFile.FilterIndex = 1;
							dlgSaveFile.RestoreDirectory = true;

							if (dlgSaveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
							{
								
								{
									// Code to write the stream goes here.
									
									SaveFile(dlgSaveFile.FileName, rtbCoAuth);
								}
								trange.Text = string.Empty;
							}
						}
						else if (dlr == System.Windows.Forms.DialogResult.No)
						{
						}
						else if (dlr == System.Windows.Forms.DialogResult.Cancel)
						{
							return;
						}
					}
					else
					{
						
						SaveFile(strOpenFilePath, rtbCoAuth);
						
					}
				}

				System.Windows.Forms.OpenFileDialog dlgFileOpen = new System.Windows.Forms.OpenFileDialog();
				
				dlgFileOpen.InitialDirectory = "c:\\";
				
				dlgFileOpen.Filter = "Text Files (*.rtf)|*.rtf";
				dlgFileOpen.FilterIndex = 1;
				dlgFileOpen.RestoreDirectory = true;

				if (dlgFileOpen.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
					
					byte[] btr = File.ReadAllBytes(dlgFileOpen.FileName);
                    lblFileName.Content = dlgFileOpen.FileName.Substring(dlgFileOpen.FileName.LastIndexOf('\\') + 1);
					Stream s = new MemoryStream(btr);
					trange.Load(s, System.Windows.DataFormats.Rtf);

					
					strOpenFilePath = dlgFileOpen.FileName;
					strOpenFilePath = dlgFileOpen.FileName;
				}
				else
				{
					if (strOpenFilePath.Length != 0)
					{
                        
					}
				}
				isFileEdited = false;
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "CommandBinding_Executed_Open", "CoAuth.xaml.cs");

			}
		}

		private void CommandBinding_Executed_New(object sender, ExecutedRoutedEventArgs e)
		{
			try
			{
				TextRange tr = new TextRange(rtbCoAuth.Document.ContentStart, rtbCoAuth.Document.ContentEnd);
				

				if (isFileEdited)
				{
					

					System.Windows.Forms.DialogResult dlr;
					dlr = System.Windows.Forms.MessageBox.Show("Do you want to save the file", "Save", System.Windows.Forms.MessageBoxButtons.YesNoCancel, System.Windows.Forms.MessageBoxIcon.Question);

					if (dlr == System.Windows.Forms.DialogResult.Yes)
					{
                        if (strOpenFilePath.Length == 0)
                        {

                            Microsoft.Win32.SaveFileDialog dlgSaveFile = new Microsoft.Win32.SaveFileDialog();
						dlgSaveFile.Filter = "Text Files (*.rtf)|*.rtf";
						dlgSaveFile.FilterIndex = 1;
						dlgSaveFile.RestoreDirectory = true;

                            if (dlgSaveFile.ShowDialog() == true)
						{
							
                                // Code to write the stream goes here.
								
								SaveFile(dlgSaveFile.FileName, rtbCoAuth);
							}
                        }
                        else
                        {
                            SaveFile(strOpenFilePath, rtbCoAuth);
                        }

							tr.Text = string.Empty;
                        lblFileName.Content = "New Document";
							isFileEdited = false;

                        //System.Windows.Forms.SaveFileDialog dlgSaveFile = new System.Windows.Forms.SaveFileDialog();
                        //dlgSaveFile.Filter = "Text Files (*.rtf)|*.rtf";
                        //dlgSaveFile.FilterIndex = 1;
                        //dlgSaveFile.RestoreDirectory = true;

                        //if (dlgSaveFile.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                        //{
							
                        //    {
								
                        //        SaveFile(dlgSaveFile.FileName, rtbCoAuth);
                        //    }
                        //    tr.Text = string.Empty;
                        //    isFileEdited = false;
                        //}

					}
					else if (dlr == System.Windows.Forms.DialogResult.No)
					{
						tr.Text = "";
                        lblFileName.Content = "New Document";
						strOpenFilePath = string.Empty;
						isFileEdited = false;
					}
					else if (dlr == System.Windows.Forms.DialogResult.Cancel)
					{
						return;
					}
				}
				else
				{
					tr.Text = string.Empty;
                    lblFileName.Content = "New Document";
					isFileEdited = false;
					strOpenFilePath = string.Empty;
				}
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "CommandBinding_Executed_New", "CoAuth.xaml.cs");

			}
		}
		private void CommandBinding_CanExecute_New(object sender, CanExecuteRoutedEventArgs e)
		{
			try
			{
				e.CanExecute = true;
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "CommandBinding_CanExecute_New", "CoAuth.xaml.cs");

			}
		}

       
		static void SaveFile(string filename, System.Windows.Controls.RichTextBox richTextBox)
		{
			try
			{
				if (string.IsNullOrEmpty(filename))
				{
					throw new Exception("File Name is not provided");
				}
				using (FileStream stream = File.OpenWrite(filename))
				{
					TextRange documentTextRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
					string dataFormat = System.Windows.DataFormats.Rtf;
					documentTextRange.Save(stream, dataFormat);
				}
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "SaveFile", "CoAuth.xaml.cs");

			}
		}

        //static void SaveFile(string filename, System.Windows.Controls.RichTextBox richTextBox)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(filename))
        //        {
        //            throw new Exception("File Name is not provided");
        //        }
        //        using (FileStream stream = File.OpenWrite(filename))
        //        {
        //            TextRange documentTextRange = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
        //            string dataFormat = System.Windows.DataFormats.Rtf;
        //            documentTextRange.Save(stream, dataFormat);
        //        }
        //    }
        //    catch (Exception exp)
        //    {
        //        VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "SaveFile", "CoAuth.xaml.cs");

        //    }
        //}

        private void CommandBinding_CanExecute_Save(object sender, CanExecuteRoutedEventArgs e)
        {
            try
            {
                e.CanExecute = true;
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "CommandBinding_CanExecute_Save", "CoAuth.xaml.cs");

            }
        }

        private void CommandBinding_Executed_Save(object sender, ExecutedRoutedEventArgs e)
        {
            try
            {
                if (strOpenFilePath.Length == 0)
                {

                    Microsoft.Win32.SaveFileDialog dlgSaveFile = new Microsoft.Win32.SaveFileDialog();
                    dlgSaveFile.Filter = "Text Files (*.rtf)|*.rtf";
                    dlgSaveFile.FilterIndex = 1;
                    dlgSaveFile.RestoreDirectory = true;

                    if (dlgSaveFile.ShowDialog() == true)
                    {

                        // Code to write the stream goes here.

                        SaveFile(dlgSaveFile.FileName, rtbCoAuth);
                        strOpenFilePath = dlgSaveFile.FileName;
                    }
                }
                else
                {
                    SaveFile(strOpenFilePath, rtbCoAuth);
                }
                isFileEdited = false;
            }
            catch (Exception ex)
            {
			}
		}
		#endregion

      
		#region NetP2P Function Implementation
		
       
		// Server Side Implementation
        /// <summary>
        /// This function will used for Send richtextbox data to other participants in the small bunch of 5000 bytes 
        /// </summary>
        /// <param name="byteLength"></param>
		void setTextRange(int byteLength)
		{
            try
            {
                dispTmr.Stop();
                TextRange tr = new TextRange(rtbCoAuth.Document.ContentStart, rtbCoAuth.Document.ContentEnd);
                MemoryStream myMemoryStream = new MemoryStream();
                tr.Save(myMemoryStream, DataFormats.Rtf);

                byte[] myOrgData = myMemoryStream.ToArray();
                byte[] myCompData = Compressor.Compress(myOrgData);

                byteLength = myCompData.Length;

                byte[] arr = new byte[5000];
                byte[] Larr;
                int i = 0;
                
                if (myCompData.Length > 5000)
                {
                    for (i = 0; i < myCompData.Length / 5000; i++)
                    {
                        for (int j = 0; j < 5000; j++)
                        {
                            arr[j] = myCompData[(i * 5000) + j];
                        }
                        // ** Sending CoAuthData 
                        StringBuilder sb3 = new StringBuilder();
                        
                        for (int d = 0; d < lstParticipants.Count; d++)
                        {
                            sb3.AppendLine(lstParticipants[d]);
                        }
                                             
                        // sender VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName
                        // lstParticipants
                        chNetP2PCoAuthServiceChannel.svcSetCompBytes(byteLength, arr, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstParticipants);
                    }

                    if (i * 5000 != myCompData.Length)
                    {
                        Larr = new byte[(myCompData.Length) - (i * 5000)];
                        for (int j = 0; j < (myCompData.Length) - (i * 5000); j++)
                        {
                            Larr[j] = myCompData[(i * 5000) + j];
                        }
                        // ** Sending CoAuthData 
                        StringBuilder sb31 = new StringBuilder();
                       
                        for (int d1 = 0; d1 < lstParticipants.Count; d1++)
                        {
                            sb31.AppendLine(lstParticipants[d1]);
                        }
                                               
                        // sender VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName
                        // lstParticipants
                        chNetP2PCoAuthServiceChannel.svcSetCompBytes(byteLength, Larr, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstParticipants);
                    }
                }
                else
                {
                    Larr = new byte[myCompData.Length];
                    for (int j = 0; j < myCompData.Length; j++)
                    {
                        Larr[j] = myCompData[j];
                    }
                    // ** Sending CoAuthData 
                    StringBuilder sb32 = new StringBuilder();
                    
                    for (int d2 = 0; d2 < lstParticipants.Count; d2++)
                    {
                        sb32.AppendLine(lstParticipants[d2]);
                    }
                                      
                    // sender VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName
                    // lstParticipants
                    chNetP2PCoAuthServiceChannel.svcSetCompBytes(byteLength, Larr, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstParticipants);
                }
                // ** Sending Last bunch CoAuthData 
                StringBuilder sb34 = new StringBuilder();
                
                for (int d = 0; d < lstParticipants.Count; d++)
                {
                    sb34.AppendLine(lstParticipants[d]);
                }
               
             
                // sender VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName
                // lstParticipants
                chNetP2PCoAuthServiceChannel.svcSaveDoc(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstParticipants);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "setTextRange", "CoAuth.xaml.cs");
            }
            finally
            {
                dispTmr.Start();
                MyCompedData = null;
                flgWriting = false;
                pointer = 0;
                if (UserInfo.MyRole == "Host")
                {
                    MyUsers.MyCompedData = null;
                    MyUsers.flgWriting = false;
                    MyUsers.pointer = 0;
                }
            }

		}


        /// <summary>
        /// This function will used to set the Incoming data to the RichTextBox and if any change is there in the richtext box then it will pop up 
        /// Discarded PopUp Message Windows.
        /// </summary>
        void setDataInRBox()
        {
            try
            {
                // ** Saving COAuth Data that is received             
                               
                saveCDoc = -1;
                saveDoc = 0;
                TextRange tr = new TextRange(rtbCoAuth.Document.ContentStart, rtbCoAuth.Document.ContentEnd);
                if (typeFlg == true)
                {
                    typeFlg = false;

                    TextRange trNotInc = new TextRange(rtbCutText.Document.ContentStart, rtbCutText.Document.ContentEnd);
                    trTemp.Load(new MemoryStream(Compressor.Decompress(MyCompedData)), DataFormats.Rtf);

                    byte[] mainString = System.Text.ASCIIEncoding.ASCII.GetBytes(trTemp.Text);
                    byte[] myString = System.Text.ASCIIEncoding.ASCII.GetBytes(tr.Text);
                    List<byte> notIncludedString = new List<byte>();

                    int mnStrPointer = 0;
                    int myStrPointer = 0;
                    try
                    {
                        while (mnStrPointer < mainString.Length)
                        {
                            if (mnStrPointer < myString.Length)
                            {
                                if (mainString[mnStrPointer] != myString[myStrPointer])
                                {
                                    while (myString[myStrPointer] != 13)
                                    {
                                        notIncludedString.Add(myString[myStrPointer]);
                                        myStrPointer++;
                                    }
                                    while (mainString[mnStrPointer] != 13)
                                    {
                                        mnStrPointer++;
                                    }
                                }
                                else
                                {
                                    myStrPointer++;
                                    mnStrPointer++;
                                }
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    catch (Exception exp)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "setDataInRBox", "CoAuth.xaml.cs");

                    }

                    if (mnStrPointer < myString.Length)
                    {
                        for (int j = mnStrPointer; j < myString.Length; j++)
                        {
                            notIncludedString.Add(myString[j]);
                        }
                    }

                    trNotInc.Text = System.Text.ASCIIEncoding.ASCII.GetString(notIncludedString.ToArray());

                    if (trNotInc.Text.Replace("\r", "").Replace("\n", "").Length != 0)
                    {
                        cnvCoAuth.Opacity = 0.5;
                        cnvCoAuth.Background = System.Windows.Media.Brushes.AliceBlue;
                        cnvCoAuth.IsEnabled = false;
                        InkTrans.Visibility = Visibility.Visible;
                        lblCaption.Visibility = Visibility.Visible;
                        rtbCutText.Visibility = Visibility.Visible;
                        btnOk.Visibility = Visibility.Visible;
                        rtbCutText.Focus();
                    }
                }
                rtbCoAuth.TextChanged -= new TextChangedEventHandler(rtbCoAuth_TextChanged);
                tr.Load(new MemoryStream(Compressor.Decompress(MyCompedData)), DataFormats.Rtf);
                rtbCoAuth.TextChanged += new TextChangedEventHandler(rtbCoAuth_TextChanged);
                
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "setDataInRBox", "CoAuth.xaml.cs");

            }

            finally
            {
                MyCompedData = null;
                flgWriting = false;
                pointer = 0;
                if (UserInfo.MyRole == "Host")
                {
                    MyUsers.MyCompedData = null;
                    MyUsers.flgWriting = false;
                    MyUsers.pointer = 0;
                }
            }

        }

        /// <summary>
        /// This function will execute on the "Host" side. if writing flag is "False" then only users can write the data.........if writing flag is "True"
        /// then user is not allowed to write its data.
        /// </summary>
        /// <param name="byteLength"></param>
        /// <param name="uName"></param>
        /// <param name="strRole"></param>
        void CoAuth_EntsvcSetLength(int byteLength, string uName, string strRole)
		{
			try
			{
				if (UserInfo.MyRole == "Host")
				{
					if (MyUsers.flgWriting == false)
					{
						MyUsers.flgWriting = true;
						MyUsers.pointer = 0;
						MyUsers.MyCompedData = new byte[byteLength];
						chNetP2PCoAuthServiceChannel.svcReplySetLength(byteLength, true, uName);
					}
					else
					{
						chNetP2PCoAuthServiceChannel.svcReplySetLength(byteLength, false, uName);
					}
				}

			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "CoAuth_EntsvcSetLength", "CoAuth.xaml.cs");
			}
		}

        /// <summary>
        /// This function will execute on the side of user who has requested to write data on "Host". if parameter "isLenghtSet" is true then user can write the data
        /// otherwise he/she has to wait for next tick.
        /// </summary>
        /// <param name="byteLength"></param>
        /// <param name="isLenghtSet"></param>
        /// <param name="uName"></param>
		void CoAuth_EntsvcReplySetLength(int byteLength, bool isLenghtSet, string uName)
		{
			try
			{
				if (isLenghtSet && uName == VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
				{
					this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, delSetTextRange, byteLength);
				}
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "CoAuth_EntsvcReplySetLength", "CoAuth.xaml.cs");

			}

		}

        /// <summary>
        /// this NetP2P function will be used to retrive the data sended by the co-auth data to other participants.
        /// </summary>
        /// <param name="byteLength"></param>
        /// <param name="myDoc"></param>
        /// <param name="uName"></param>
        /// <param name="strReceivers"></param>
		void CoAuth_EntsvcSetCompBytes(int byteLength, byte[] myDoc, string uName, List<string> strReceivers)
		{
			try
			{
                if (UserInfo.MyRole == "Host")
                {
                    if (MyUsers.MyCompedData == null)
                    {

                        MyUsers.MyCompedData = new byte[byteLength];
                    }

                    for (int i = 0; i < myDoc.Length; i++)
                    {
                        MyUsers.MyCompedData[MyUsers.pointer] = myDoc[i];
                        MyUsers.pointer++;
                    }
                  
                }                
				if (uName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
				{
                    // ** Receiving CoAuthData 
                                                            
                    // sender Uname
                    // receiver VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName
                    // Number of bytes received pointer
					if (flgWriting == false)
					{
						flgWriting = true;
						pointer = 0;
						MyCompedData = new byte[byteLength];
					}
					for (int i = 0; i < myDoc.Length; i++)
					{
						MyCompedData[pointer] = myDoc[i];
						pointer++;
					}
				}
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "CoAuth_EntsvcSetCompBytes", "CoAuth.xaml.cs");
			}
		}


        /// <summary>
        /// This NetP2P function will fire when the last bunch of the data will come.
        /// </summary>
        /// <param name="uName"></param>
        /// <param name="to"></param>
		void CoAuth_EntsvcSaveDoc(string uName, List<string> to)
		{
			try
			{
				if (uName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
				{

					this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, delSetDataInRBox);
               
                }
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "CoAuth_EntsvcSaveDoc", "CoAuth.xaml.cs");

			}

		}

        /// <summary>
        /// This is the first function after Opeining the NetP2P channels.
        /// </summary>
        /// <param name="uName"></param>
		void CoAuth_EntSvcJoin(string uName)
		{
			try
			{
				if (UserInfo.MyRole == "Host")
				{
					// Implement on Serverside
					try
					{
						MyUsers.Users.Add(new CoAuthUser(uName));
					}
					catch
					{
						MessageBox.Show("Error In Svc Join");
					}
				}
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "CoAuth_EntSvcJoin", "CoAuth.xaml.cs");

			}
		}

        /// <summary>
        /// this function will fire when user is about to leave the co-auth module.
        /// </summary>
        /// <param name="uName"></param>
		void CoAuth_EntsvcUnJoin(string uName)
		{
			try
			{
				if (UserInfo.MyRole == "Host")
				{
					for (int i = 0; i < MyUsers.Users.Count; i++)
					{
						if (MyUsers.Users[i].UserName == uName)
						{
							try
							{
								MyUsers.Users.RemoveAt(i);
							}
							catch
							{
							}
							break;
						}
					}
				}
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "CoAuth_EntsvcUnJoin", "CoAuth.xaml.cs");

			}
		}

        /// <summary>
        /// This function will use to set the newly added Participants into the Participants list.
        /// </summary>
        /// <param name="uName"></param>
        void CoAuth_EntsvcSetUserList(string uName)
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CoAuth_EntsvcSetUserList", "CoAuth.xaml.cs");

			}
		}

        /// This function will use to set the newly added Participants into the Participants list.
        /// </summary>
        /// <param name="uName"></param>
        void CoAuth_EntsvcGetUserList(string uName)
		{
            try
            {

                if (uName != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                {
                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, uName);
                    if (chNetP2PCoAuthServiceChannel != null)
                    {
                        chNetP2PCoAuthServiceChannel.svcSetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
                    }
                    else
                    {
                        chHTTPCoAuthServiceChannel.svcSetUserList(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);

                    }  
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CoAuth_EntsvcGetUserList", "CoAuth.xaml.cs");

            }

		}

        /// <summary>
        /// This function will fire when some user has left the coauth module.
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
		void CoAuth_EntsvcSetUserList_EntsvcSignOutCoAuth(string from, List<string> to)
		{
			try
			{
				if (from != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
				{					
					this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelRemoveParticipant,from);
				}
			}
			catch (Exception exp)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "CoAuth_EntsvcSetUserList_EntsvcSignOutCoAuth", "CoAuth.xaml.cs");
			}
		}

		#endregion


		#region IDisposable Members
		//private bool disposed;

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);

		}

        private void Dispose(bool disposing)
        {

            try
            {
                dispTmr = null;
                rtbTemp = null;
                trTemp = null;
                //modPer = null;
                //tHostChat = null;
                dispTimer4HTTP = null;
                lstParticipants = null;
                delSetTextRange = null;
                delSetDataInRBox = null;
                objDelDisplayName = null;
                objdelTimerTick = null;
                ncActiveCallInfo = null;
                objNetP2PCoAuthService = null;
                if (chNetP2PCoAuthServiceChannel != null)
                {
                    chNetP2PCoAuthServiceChannel.Close();
                    chNetP2PCoAuthServiceChannel.Dispose();
                    chNetP2PCoAuthServiceChannel = null;
                }
                chHTTPCoAuthServiceChannel = null;
                MyCompedData = null;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose", "CoAuth.xaml.cs");

            }
        }

	

		~CoAuth()
		{
			Dispose(false);
		}

		#endregion


        #region logging function

      
        #endregion


        #region COmmented code

        void HTTPTimerTick()
        {
            try
            {

                List<clsCoAuthDataMember> myMessages = new List<clsCoAuthDataMember>();
                if (chHTTPCoAuthServiceChannel != null && chHTTPCoAuthServiceChannel.State == CommunicationState.Opened)
                {
                    myMessages = chHTTPCoAuthServiceChannel.svcGetChangedContext(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, UserInfo.MyRole);

                    if (myMessages != null && myMessages.Count > 0)
                    {
                        dispTmr.Stop();
                        foreach (clsCoAuthDataMember clsMember in myMessages)
                        {
                            if (clsMember.strMsgType == "GetUserList")
                            {
                                try
                                {
                                    if (clsMember.strSender != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                    {
                                        if (clsMember.Participants.Count == 0)
                                        {
                                            List<string> lstUser = new List<string>();
                                            lstUser.Add(clsMember.strSender);
                                            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, lstUser, clsMember.strMsgType);
                                        }
                                        else if (clsMember.Participants.Count == 1)
                                        {
                                            if (clsMember.Participants[0] == VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                            {
                                                List<string> lstUser = new List<string>();
                                                lstUser.Add(clsMember.strSender);
                                                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, lstUser, clsMember.strMsgType);
                                            }
                                            else
                                            {
                                                this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, clsMember.Participants, clsMember.strMsgType);
                                            }
                                        }
                                        else
                                        {
                                            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, clsMember.Participants, clsMember.strMsgType);
                                        }
                                    }
                                }
                                catch (Exception exp)
                                {
                                    VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "HTTPTimerTick", "CoAuth.xaml.cs");

                                }
                            }
                            else if (clsMember.strMsgType == "SetUserList")
                            {
                                if (clsMember.strSender != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                {
                                    
                                    if (clsMember.Participants.Count == 0)
                                    {
                                        List<string> lstUser = new List<string>();
                                        lstUser.Add(clsMember.strSender);
                                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, lstUser, clsMember.strMsgType);
                                    }
                                    else if (clsMember.Participants.Count == 1)
                                    {
                                        if (clsMember.Participants[0] == VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                        {
                                            List<string> lstUser = new List<string>();
                                            lstUser.Add(clsMember.strSender);
                                            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, lstUser, clsMember.strMsgType);
                                        }
                                        else
                                        {
                                            this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, clsMember.Participants, clsMember.strMsgType);
                                        }
                                    }
                                    else
                                    {
                                        this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelDisplayName, clsMember.Participants, clsMember.strMsgType);
                                    }
                                }
                            }

                            else if (clsMember.strMsgType == "CoAuthData")
                            {
                                try
                                {
                                    // ** Receiving CoAuthData 
                                                                    
                                    // sender clsMember.strSender
                                    // receiver VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName
                                    // Number of bytes received pointer
                                    // Node type
                                    if (clsMember.strSender != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                    {
                                        if (MyCompedData == null)
                                        {
                                             MyCompedData = new byte[clsMember.intPointer];
                                        }
                                        for (int i = 0; i < clsMember.myData.Length; i++)
                                        {
                                            MyCompedData[pointer] = clsMember.myData[i];
                                            pointer += 1;
                                        }
                                    }
                                }
                                catch (Exception exp)
                                {
                                    VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "HTTPTimerTick", "CoAuth.xaml.cs");

                                }
                            }
                            else if (clsMember.strMsgType == "LoadCoAuthData")
                            {
                                //}
                                StringBuilder sb = new StringBuilder();
                                try
                                {
                                    if (clsMember.strSender != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                    {
                                        // ** Saving CoAuthData 
                                        // sender clsMember.strSender
                                        // receiver VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName
                                        // Node type
                                        // Number of bytes received pointer                                       
                                       
                                        saveCDoc = -1;
                                        saveDoc = 0;
                                        TextRange tr = new TextRange(rtbCoAuth.Document.ContentStart, rtbCoAuth.Document.ContentEnd);
                                        if (typeFlg == true)
                                        {
                                            typeFlg = false;

                                            TextRange trNotInc = new TextRange(rtbCutText.Document.ContentStart, rtbCutText.Document.ContentEnd);
                                            trTemp.Load(new MemoryStream(Compressor.Decompress(MyCompedData)), DataFormats.Rtf);

                                            byte[] mainString = System.Text.ASCIIEncoding.ASCII.GetBytes(trTemp.Text);
                                            byte[] myString = System.Text.ASCIIEncoding.ASCII.GetBytes(tr.Text);
                                            List<byte> notIncludedString = new List<byte>();

                                            int mnStrPointer = 0;
                                            int myStrPointer = 0;
                                            try
                                            {
                                                while (mnStrPointer < mainString.Length)
                                                {
                                                    if (mnStrPointer < myString.Length)
                                                    {
                                                        if (mainString[mnStrPointer] != myString[myStrPointer])
                                                        {
                                                            while (myString[myStrPointer] != 13)
                                                            {
                                                                notIncludedString.Add(myString[myStrPointer]);
                                                                myStrPointer++;
                                                            }
                                                            while (mainString[mnStrPointer] != 13)
                                                            {
                                                                mnStrPointer++;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            myStrPointer++;
                                                            mnStrPointer++;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        break;
                                                    }
                                                }
                                            }
                                            catch (Exception exp)
                                            {
                                                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "HTTPTimerTick", "CoAuth.xaml.cs");

                                            }

                                            if (mnStrPointer < myString.Length)
                                            {
                                                for (int j = mnStrPointer; j < myString.Length; j++)
                                                {
                                                    notIncludedString.Add(myString[j]);
                                                }
                                            }

                                            trNotInc.Text = System.Text.ASCIIEncoding.ASCII.GetString(notIncludedString.ToArray());

                                            if (trNotInc.Text.Replace("\r", "").Replace("\n", "").Length != 0)
                                            {
                                                cnvCoAuth.Opacity = 0.5;
                                                cnvCoAuth.Background = System.Windows.Media.Brushes.AliceBlue;
                                                cnvCoAuth.IsEnabled = false;

                                                InkTrans.Visibility = Visibility.Visible;
                                                lblCaption.Visibility = Visibility.Visible;
                                                rtbCutText.Visibility = Visibility.Visible;
                                                btnOk.Visibility = Visibility.Visible;
                                                rtbCutText.Focus();
                                            }
                                        }

                                        rtbCoAuth.TextChanged -= new TextChangedEventHandler(rtbCoAuth_TextChanged);
                                        tr.Load(new MemoryStream(Compressor.Decompress(MyCompedData)), DataFormats.Rtf);
                                        rtbCoAuth.TextChanged += new TextChangedEventHandler(rtbCoAuth_TextChanged);
                                    }
                                }
                                catch (Exception exp)
                                {
                                    VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "HTTPTimerTick", "CoAuth.xaml.cs");

                                }
                                finally
                                {
                                    MyCompedData = null;
                                    flgWriting = false;
                                    pointer = 0;
                                    if (UserInfo.MyRole == "Host")
                                    {
                                        MyUsers.MyCompedData = null;
                                        MyUsers.flgWriting = false;
                                        MyUsers.pointer = 0;
                                    }
                                }
                            }

                            else if (clsMember.strMsgType == "SignOut")
                            {
                                if (clsMember.strSender != VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                                {
                                    // ** as per chat

                                    this.Dispatcher.BeginInvoke(System.Windows.Threading.DispatcherPriority.Normal, objDelRemoveParticipant, clsMember.strSender);
                                }
                            }

                            else if (clsMember.strMsgType == "SetLength")
                            {
                                if (clsMember.isLengthSet)
                                {
                                    TextRange tr = new TextRange(rtbCoAuth.Document.ContentStart, rtbCoAuth.Document.ContentEnd);
                                    MemoryStream myStr = new MemoryStream();
                                    tr.Save(myStr, DataFormats.Rtf);

                                    byte[] myOrgData = myStr.ToArray();
                                    byte[] myCompData = Compressor.Compress(myOrgData);

                                    byte[] arr = new byte[5000];
                                    byte[] Larr;
                                    int i = 0;

                                    if (myCompData.Length > 5000)
                                    {
                                        for (i = 0; i < myCompData.Length / 5000; i++)
                                        {
                                            for (int j = 0; j < 5000; j++)
                                            {
                                                arr[j] = myCompData[(i * 5000) + j];
                                            }
                                            // ** Sending CoAuthData 
                                            StringBuilder sb = new StringBuilder();
                                          
                                            for (int te = 0; te < lstParticipants.Count; te++)
                                            {
                                                sb.AppendLine(lstParticipants[te]);
                                            }
                                           
                                            
                                            // sender VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName
                                            // Receiver lstparticipants
                                            // Node type
                                            chHTTPCoAuthServiceChannel.svcSetCompBytes(myCompData.Length, arr, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstParticipants);
                                        }

                                        if (i * 5000 != myCompData.Length)
                                        {
                                            Larr = new byte[(myCompData.Length) - (i * 5000)];
                                            for (int j = 0; j < (myCompData.Length) - (i * 5000); j++)
                                            {
                                                Larr[j] = myCompData[(i * 5000) + j];
                                            }
                                            // ** Sending CoAuthData 
                                            StringBuilder sb2 = new StringBuilder();
                                            
                                            for (int te1 = 0; te1 < lstParticipants.Count; te1++)
                                            {
                                                sb2.AppendLine(lstParticipants[te1]);
                                            }
                                            
                                           
                                            // sender VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName
                                            // Receiver lstparticipants
                                            // Node type
                                            chHTTPCoAuthServiceChannel.svcSetCompBytes(myCompData.Length, Larr, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstParticipants);
                                        }
                                    }
                                    else
                                    {
                                        Larr = new byte[myCompData.Length];
                                        for (int j = 0; j < myCompData.Length; j++)
                                        {
                                            Larr[j] = myCompData[j];
                                        }
                                        // ** Sending CoAuthData 
                                        StringBuilder sb22 = new StringBuilder();
                                       
                                        for (int te12 = 0; te12 < lstParticipants.Count; te12++)
                                        {
                                            sb22.AppendLine(lstParticipants[te12]);
                                        }
                                       
                                        
                                        // sender VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName
                                        // Receiver lstparticipants
                                        // Node types
                                        chHTTPCoAuthServiceChannel.svcSetCompBytes(myCompData.Length, Larr, VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstParticipants);
                                    }
                                    // ** Saving CoAuthData 
                                    StringBuilder sb23 = new StringBuilder();
                                   
                                    for (int te13 = 0; te13 < lstParticipants.Count; te13++)
                                    {
                                        sb23.AppendLine(lstParticipants[te13]);
                                    }
                                    
                                   
                                    // sender VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName
                                    // Receiver lstparticipants
                                    // Node types
                                    chHTTPCoAuthServiceChannel.svcSaveDoc(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, lstParticipants);
                                }
                            }

                            else if (clsMember.strMsgType == "SetWritingFlag")
                            {
                                if (MyUsers.flgWriting == false)
                                {
                                    // ** Setting Writing flag for user 
                                    // sender clsMember.strSender
                                    // bytelength
                                    // Node types
                                    // true
                                   
                                    
                                    MyUsers.flgWriting = true;
                                    MyUsers.pointer = 0;
                                    MyUsers.MyCompedData = new byte[clsMember.byteLength];
                                    chHTTPCoAuthServiceChannel.svcReplySetLength(clsMember.byteLength, true, clsMember.strSender);

                                }
                                else
                                {
                                    // ** Setting Writing flag for user 
                                    // sender clsMember.strSender
                                    // bytelength

                                   
                                   
                                    // Node types
                                    
                                    
                                    chHTTPCoAuthServiceChannel.svcReplySetLength(clsMember.byteLength, false, clsMember.strSender);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "HTTPTimerTick", "CoAuth.xaml.cs");

            }
            finally
            {
                dispTimer4HTTP.Start();
                dispTmr.Start();
            }
        }
        #endregion
    }
}