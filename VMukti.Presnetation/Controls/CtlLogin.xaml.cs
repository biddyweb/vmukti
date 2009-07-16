/* VMukti 2.0 -- An Open Source Video Communications Suite
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
using System.IO;
using System.Net;
using System.Windows;
using System.Windows.Controls; 
using System.Windows.Interop;
using Microsoft.Win32;
using VMukti.Business;
using VMukti.Business.WCFServices.BootStrapServices.DataContracts;
using VMukti.Business.WCFServices.SuperNodeServices.BasicHttp;
using VMukti.Business.WCFServices.SuperNodeServices.DataContract;
using VMuktiAPI;
using System.Windows.Input;
using System.Windows.Data;
using System.Text;

namespace VMukti.Presentation.Controls
{
	public partial class CtlLogin : System.Windows.Controls.UserControl, IDisposable
	{
        public static StringBuilder sb1 = new StringBuilder();
       
		private clsRTCAuthClient objRTCAuthClient = null;

		public delegate void DelAutherized();
		public event DelAutherized EntAutherized = null;
        
		clsSuperNodeDataContract objSuperNodeDataContract = null;
        public string strPassword;

		TextBox txtEmail = new TextBox();
		System.Windows.Threading.DispatcherTimer dispTimer4DomainLoading = new System.Windows.Threading.DispatcherTimer(System.Windows.Threading.DispatcherPriority.Normal);

        public int UserActivityId, uid, ActivityId;    //Added by : Alpa

		public CtlLogin()
		{
			try
			{
				this.InitializeComponent();

				this.Loaded += new RoutedEventHandler(CtlLogin_Loaded);

				dispTimer4DomainLoading.Tick += new EventHandler(dispTimer4DomainLoading_Tick);
				dispTimer4DomainLoading.Interval = TimeSpan.FromSeconds(1);
				txtEmail.KeyDown += new KeyEventHandler(txtEmail_KeyDown);
                VMuktiAPI.VMuktiHelper.RegisterEvent("SignOut").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(CtlLogin_VMuktiEvent);
               

			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Ctllogin", "Controls\\CtlLogin.xaml.cs");
			}
		}
         
        void CtlLogin_VMuktiEvent(object sender, VMuktiEventArgs e)
        {
            try
            {
                VMukti.Business.ClsUser.AddNewRecord(VMuktiAPI.VMuktiInfo.CurrentPeer.ID);
                VMuktiAPI.VMuktiInfo.CurrentPeer.ID = int.MinValue;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlLogin_VMuktiEvent()", "Controls\\CtlLogin.xaml.cs");
            }
        }


		void CtlLogin_Loaded(object sender, RoutedEventArgs e)
		{
            try
            {
                if (System.Deployment.Application.ApplicationDeployment.CurrentDeployment.ActivationUri.AbsoluteUri.ToString().Contains("?"))
                {
                    string[] str = (System.Deployment.Application.ApplicationDeployment.CurrentDeployment.ActivationUri.AbsoluteUri.ToString()).Split('?')[1].Split('&');
                    txtUserNameID.Text = DeCodeString(str[0].Split('=')[1].ToString());
                    pwdPasssword.Password = DeCodeString(str[1].Split('=')[1].ToString());

                    if (str.Length > 2)
                    {
                        txtEmail.Text = DeCodeString(str[2].Split('=')[1].ToString());

                        //if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                        //{
                        //    Business.clsDataBaseChannel.OpenDataBaseClient();
                        //}


                        //if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == PeerType.NodeWithHttp)
                        //{
                        //    VMukti.Business.clsDataBaseChannel.OpenDataBaseClient();
                        //}
                        btnOK_Click(null, null);
                    }
                    else
                    {
                        btnLogIn_Click(null, null);
                    }
                }
                #region Cookies Management

                //string strGetCookies = App.GetCookie(BrowserInteropHelper.Source);
                //if (strGetCookies != null && strGetCookies != "Login")
                //{
                //    if (strGetCookies.Split(',').Length > 1)
                //    {
                //        txtUserNameID.Text = strGetCookies.Split(',')[0];
                //        pwdPasssword.Password = strGetCookies.Split(',')[1];
                //        btnLogIn_Click(null, null);
                //    }                   
                //}

                #endregion
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlLogin_Loaded()", "Controls\\CtlLogin.xaml.cs");
            }
		}

		void dispTimer4DomainLoading_Tick(object sender, EventArgs e)
		{
			Authenticate();
		}

		private void txtUserNameID_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			try
			{
				if (e.Key.Equals(System.Windows.Input.Key.Enter))
				{
					pwdPasssword.Focus();
				}
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "txtUserNameID_KeyDown()", "Controls\\CtlLogin.xaml.cs");
			}
		}

		private void pwdPasssword_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
		{
			try
			{
				if (e.Key.Equals(System.Windows.Input.Key.Enter))
				{
					btnLogIn_Click(new object(), new RoutedEventArgs());
				}
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "pwdPassword_KeyDown()", "Controls\\CtlLogin.xaml.cs");
			}
		}

		private void chkRememberMe_Checked(object sender, System.Windows.RoutedEventArgs e)
		{
			
		}

		private void chkSigninAuto_Checked(object sender, System.Windows.RoutedEventArgs e)
		{
            try
            {
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "chkSigninAuto_checked()", "Controls\\CtlLogin.xaml.cs");
            }
		}

        private void btnForgetPassClick(object sender, System.Windows.RoutedEventArgs e)
        {

        }

		private void btnLogIn_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
                lblValidate.Content =string.Empty;
				txtUserNameID.IsEnabled = false;
				pwdPasssword.IsEnabled = false;
				btnForgetPass.IsEnabled = false;
				btnLogIn.IsEnabled = false;
				btnSignUp.IsEnabled = false;

				//if (chkRememberMe.IsChecked == true)
				//{
				//    RegistryKey MyReg = Registry.CurrentUser.CreateSubKey("Vmukti\\userInfo");
				//    MyReg.SetValue("username", txtUserNameID.Text);
				//    MyReg.SetValue("password", pwdPasssword.Password);
				//    MyReg.SetValue("ischeckedpassword", "true");
				//    MyReg.SetValue("ischeckedAutomatic", (bool)chkSigninAuto.IsChecked);
				//}
				dispTimer4DomainLoading.Start();
				// Authenticate();
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnLogin_Click()", "Controls\\CtlLogin.xaml.cs");
			}
		}

		private void Authenticate()
		{
			lock (this)
			{
				try
				{
                    //StreamReader sReader = new StreamReader(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\DomainStatus.txt");
                    //string strStatus = sReader.ReadToEnd();
                    //sReader.Close();

                    FileStream fs = new FileStream(AppDomain.CurrentDomain.BaseDirectory.ToString() + "\\DomainStatus.txt", FileMode.OpenOrCreate, FileAccess.Read, FileShare.ReadWrite);
                    StreamReader sReader = new StreamReader(fs);
					string strStatus = sReader.ReadToEnd();
					sReader.Close();
                    fs.Close();


					bool isSuperNode = false;
                    strPassword = encodestring(pwdPasssword.Password);
                    if (strStatus == "Initializing")
                    {
                        dispTimer4DomainLoading.Stop();
                        lblValidate.Content = "Checking....";

                        if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P)
                        {
                            isSuperNode = false;
                        }
                        else if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode)
                        {
                            isSuperNode = true;
                        }
                        if (VMuktiAPI.VMuktiInfo.MainConnectionString == string.Empty)
                        {
                            //ClsException.WriteToLogFile("calling bs join method");

                            CallBootStrapHTTPJoin();

                            //ClsException.WriteToLogFile("called bs join");
                        }

                        if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrAuthType == AuthType.SQLAuthentication)
                        {
                            {
                                //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                                //sb.AppendLine("During Login");
                                //sb.AppendLine("User is about to authorised");
                                //sb.AppendLine("Username:" + txtUserNameID.Text);
                                //sb.AppendLine("PassWord:" + strPassword);
                                //sb.AppendLine(sb1.ToString());
                                //ClsLogging.WriteToTresslog(sb);
                                bool AuthResult = false;
                                VMukti.Business.ClsUser CurrenUser = null;

                                //ClsException.WriteToLogFile("getting the username and pass from main db");

                                CurrenUser = VMukti.Business.ClsUser.GetByUNamePass(txtUserNameID.Text, strPassword, ref AuthResult);
                                if (CurrenUser == null)
                                {
                                    if (AuthResult)
                                    {
                                        //System.Text.StringBuilder sb2 = new System.Text.StringBuilder();
                                        //sb2.AppendLine("During Login");
                                        //sb2.AppendLine("user has enter worng password");
                                        //sb2.AppendLine("Username:" + txtUserNameID.Text);
                                        //sb2.AppendLine("PassWord:" + strPassword);
                                        //sb2.AppendLine(sb1.ToString());
                                        //ClsLogging.WriteToTresslog(sb2);
                                        lblValidate.Content = "Invalid UserName, Password!!";
                                        txtUserNameID.IsEnabled = true;
                                        pwdPasssword.IsEnabled = true;
                                        pwdPasssword.Focus();
                                    }
                                    else
                                    {
                                        //System.Text.StringBuilder sb3 = new System.Text.StringBuilder();
                                        //sb3.AppendLine("During Login");
                                        //sb3.AppendLine("user has enter wrong username");
                                        //sb3.AppendLine("Username:" + txtUserNameID.Text);
                                        //sb3.AppendLine("PassWord:" + strPassword);
                                        //sb3.AppendLine(sb1.ToString());
                                        //ClsLogging.WriteToTresslog(sb3);
                                        lblValidate.Content = "Invalid UserName, Password!!";
                                        txtUserNameID.IsEnabled = true;
                                        pwdPasssword.IsEnabled = true;
                                        txtUserNameID.Focus();
                                    }
                                    btnLogIn.IsEnabled = true;
                                    btnSignUp.IsEnabled = true;
                                    return;
                                }
                                else
                                {
                                    //System.Text.StringBuilder sb4 = new System.Text.StringBuilder();
                                    //sb4.AppendLine("During Login");
                                    //sb4.AppendLine("User login Successfully");
                                    //sb4.AppendLine("Username:" + txtUserNameID.Text);
                                    //sb4.AppendLine("PassWord:" + strPassword);
                                    //sb4.AppendLine(sb1.ToString());
                                    //ClsLogging.WriteToTresslog(sb4);
                                    VMuktiAPI.VMuktiInfo.CurrentPeer.ID = CurrenUser.ID;
                                    VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName = CurrenUser.DisplayName;
                                    VMuktiAPI.VMuktiInfo.CurrentPeer.RoleID = CurrenUser.RoleID;
                                    VMuktiAPI.VMuktiInfo.CurrentPeer.FName = CurrenUser.FName;
                                    VMuktiAPI.VMuktiInfo.CurrentPeer.LName = CurrenUser.LName;
                                    VMuktiAPI.VMuktiInfo.CurrentPeer.EMail = CurrenUser.EMail;
                                    VMuktiAPI.VMuktiInfo.CurrentPeer.PassWord = CurrenUser.PassWord;
                                    VMuktiAPI.VMuktiInfo.CurrentPeer.IsActive = CurrenUser.IsActive;
                                    VMuktiAPI.VMuktiInfo.CurrentPeer.Status = "Online";
                                    VMuktiAPI.VMuktiInfo.CurrentPeer.RoleName = CurrenUser.RoleName;
                                    VMuktiAPI.VMuktiInfo.CurrentPeer.CampaignID = CurrenUser.CampaignID;
                                    VMuktiAPI.VMuktiInfo.CurrentPeer.CampaignName = CurrenUser.CampaignName;
                                    VMuktiAPI.VMuktiInfo.CurrentPeer.ScriptID = CurrenUser.ScriptID;

                                    #region ALPA

                                    VMukti.Business.ClsUser.InsertRecord(VMuktiAPI.VMuktiInfo.CurrentPeer.ID);

                                    #endregion

                                }
                            }
                            try
                            {
                                //ClsException.WriteToLogFile("calling bs authorized user ");
                                App.chHttpBootStrapService.svcHttpBSAuthorizedUser(txtUserNameID.Text.Trim(), VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP, isSuperNode);
                                //ClsException.WriteToLogFile("called bs auth user");
                            }
                            catch (System.ServiceModel.EndpointNotFoundException ex)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Authenticate()", "Controls\\CtlLogin.xaml.cs");                                
                                VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                            }
                            catch (System.ServiceModel.CommunicationException exp)
                            {
                                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "Authenticate()--1", "Controls\\CtlLogin.xaml.cs");                                
                                VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                            }
                            if (VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP != "")
                            {
                                try
                                {
                                    //System.Text.StringBuilder sb = new System.Text.StringBuilder();
                                    //sb.AppendLine("Login:");
                                    //sb.AppendLine("Opening Supernode Client");
                                    //sb.AppendLine("SuperNodeIP:" + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                                    //sb.AppendLine(sb1.ToString());
                                    //ClsLogging.WriteToTresslog(sb);

                                    //ClsException.WriteToLogFile("opening http sp client");

                                    App.chHttpSuperNodeService = (IHttpSuperNodeService)App.objHttpSuperNode.OpenClient<IHttpSuperNodeService>("http://" + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP + ":80/HttpSuperNode");
                                    App.chHttpSuperNodeService.svcJoin(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString());

                                    //ClsException.WriteToLogFile("opened http sp client");
                                                                        

                                    //System.Text.StringBuilder sb5 = new System.Text.StringBuilder();
                                    //sb5.AppendLine("Login:");
                                    //sb5.AppendLine("supernode client open successfully.");
                                    //sb5.AppendLine("SuperNodeIP:" + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
                                    //sb5.AppendLine(sb1.ToString());
                                    //ClsLogging.WriteToTresslog(sb5);
                                }
                                catch (System.ServiceModel.EndpointNotFoundException ex)
                                {
                                    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Authenticate()--2", "Controls\\CtlLogin.xaml.cs");                                    
                                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                                }
                                catch (System.ServiceModel.CommunicationException exp)
                                {
                                    VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "Authenticate()--4", "Controls\\CtlLogin.xaml.cs");                                   
                                }
                            }

                            if (EntAutherized != null)
                            {
                                lblValidate.Content = "";
                                this.Visibility = Visibility.Collapsed;
                                EntAutherized();
                                VMuktiAPI.VMuktiHelper.CallEvent("SucessfulLogin", this, new VMuktiAPI.VMuktiEventArgs(new object[] { txtUserNameID.Text }));
                            }
                        }
                    }
				}
				catch (Exception ex)
				{
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Authenticate()--5", "Controls\\CtlLogin.xaml.cs");                    
					txtUserNameID.IsEnabled = true;
					pwdPasssword.IsEnabled = true;
					btnLogIn.IsEnabled = true;
					btnSignUp.IsEnabled = true;
					lblValidate.Content = "Can not connect to bootstrap server.";
                    VMuktiAPI.VMuktiHelper.CallEvent("BandwidthUsage", null, null);
				}
			}
		}        

		private void btnSignUp_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			try
			{
                lblValidate.Content = string.Empty;
				btnLogIn.Visibility = Visibility.Collapsed;
				btnSignUp.Visibility = Visibility.Collapsed;

				Label lblemail = new Label();
				lblemail.Content = "E-mail Address";
				Canvas.SetTop(lblemail, 121);
				Canvas.SetLeft(lblemail, 5);
				lblemail.Name = "lbemail";
				lblemail.Width = 90;
				lblemail.Height = 25;
                lblemail.Foreground = System.Windows.Media.Brushes.White;
				cnvMain.Children.Add(lblemail);


				Canvas.SetTop(txtEmail, 146);
				Canvas.SetLeft(txtEmail, 5);
				txtEmail.Name = "txtEmail";
				txtEmail.Width = 134;
				txtEmail.Height = 25;
				cnvMain.Children.Add(txtEmail);

				Button btnOK = new Button();
				Canvas.SetTop(btnOK, 180);
				Canvas.SetLeft(btnOK, 2);
				btnOK.Content = "OK";
                btnOK.Foreground = System.Windows.Media.Brushes.White;
				btnOK.Name = "btnok";
				btnOK.Height = 25;
				btnOK.Width = 68;
                ControlTemplate objstyle = (ControlTemplate)(Application.Current.Resources["GlassButton"]);
                btnOK.Template = objstyle;
				cnvMain.Children.Add(btnOK);


				Button btnCancel = new Button();
				Canvas.SetTop(btnCancel, 180);
				Canvas.SetLeft(btnCancel, 74);
				btnCancel.Content = "CANCEL";
                btnCancel.Foreground = System.Windows.Media.Brushes.White;
				btnCancel.Name = "btncan";
				btnCancel.Height = 25;
				btnCancel.Width = 68;
                btnCancel.Template = objstyle;
				cnvMain.Children.Add(btnCancel);

				btnOK.Click += new RoutedEventHandler(btnOK_Click);
				btnCancel.Click += new RoutedEventHandler(btnCancel_Click);
            
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSignUp_click()", "Controls\\CtlLogin.xaml.cs");
			}
		}

		void btnOK_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				if (txtUserNameID.Text != "" && pwdPasssword.Password != "" && txtEmail.Text != "")
				{
                    lblValidate.Content = string.Empty;
                    string emailStr = txtEmail.Text;
                    int indexAt = emailStr.IndexOf('@');
                    strPassword = encodestring(pwdPasssword.Password);
                    string substremail = emailStr.Substring(indexAt + 1);
                    int indexDot = substremail.IndexOf('.');
                    indexDot = indexDot + indexAt + 1;
                    int lengthEmail = emailStr.Length;
                    if (indexAt >= 1 && indexDot >= 3 && (indexDot - indexAt) >= 2 && lengthEmail >= 5)
                    {
                        
                        if (strPassword.Length < 6)
                        {
                            lblValidate.Content = "6 Characters for Password";
                        }
                        else if (!validate_text())
                        {
                            lblValidate.Content = "Only Characters(a-z) allow";
                        }
                        else
                        {
                            ClsUser objUser = new ClsUser();
                            
                            objUser.ID = -1;
                            objUser.DisplayName = txtUserNameID.Text.Trim();
                            objUser.RoleID = 2;
                            objUser.FName = txtUserNameID.Text.Trim();
                            objUser.LName = "";
                            objUser.EMail = txtEmail.Text;
                            objUser.PassWord = strPassword;
                            objUser.IsActive = true;
                            objUser.CreatedBy = 1;
                            objUser.ModifiedBy = 1;

                            objUser.RatePerHour = 0;
                            objUser.OverTimeRate = 0;
                            objUser.DoubleRatePerHour = 0;
                            objUser.DoubleOverTimeRate = 0;

                            lblValidate.Content = "Registering....";
                            while (VMuktiAPI.VMuktiInfo.MainConnectionString == string.Empty)
                            {
                                lblValidate.Content = "Try After Some Time.";
                            }
                           int retID = objUser.Save();

                           if (retID == 0)
                           {
                               lblValidate.Content = "Already Registered.";
                               VMuktiAPI.VMuktiHelper.CallEvent("BandwidthUsage", null, null);
                           }
                           else
                           {
                               lblValidate.Content = "User Created Successfully.";
                               txtUserNameID.Text = "";
                               pwdPasssword.Password = "";
                               strPassword = "";
                               txtEmail.Text = "";
                               for (int i = 0; i < cnvMain.Children.Count; i++)
                               {
                                   if (((UIElement)cnvMain.Children[i]).GetType().ToString() == "System.Windows.Controls.Label")
                                   {
                                       if (((Label)cnvMain.Children[i]).Name == "lbemail")
                                       {
                                           cnvMain.Children.RemoveAt(i);
                                       }
                                   }

                                   if (((UIElement)cnvMain.Children[i]).GetType().ToString() == "System.Windows.Controls.TextBox")
                                   {
                                       if (((TextBox)cnvMain.Children[i]).Name == "txtEmail")
                                       {
                                           cnvMain.Children.RemoveAt(i);
                                       }
                                   }

                                   if (((UIElement)cnvMain.Children[i]).GetType().ToString() == "System.Windows.Controls.Button")
                                   {
                                       if (((Button)cnvMain.Children[i]).Name == "btnok")
                                       {
                                           cnvMain.Children.RemoveAt(i);
                                       }
                                   }

                                   if (((UIElement)cnvMain.Children[i]).GetType().ToString() == "System.Windows.Controls.Button")
                                   {
                                       if (((Button)cnvMain.Children[i]).Name == "btncan")
                                       {
                                           cnvMain.Children.RemoveAt(i);
                                       }
                                   }
                               }

                               btnSignUp.Visibility = Visibility.Visible;
                               btnLogIn.Visibility = Visibility.Visible;

                               #region ALPA

                               VMukti.Business.ClsUser.AddRecord();

                               #endregion
                           }
                           
                        }
                    }
                    else
                    {
                        lblValidate.Content = "Please Enter a valid Email ID";
                    }

               
				}
				else
				{
					lblValidate.Content = "Feilds cant left blank";
				}
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "BtnOK_Click()", "Controls\\CtlLogin.xaml.cs");
			}
		}

		void btnCancel_Click(object sender, RoutedEventArgs e)
		{
            try
            {
                for (int i = 0; i < cnvMain.Children.Count; i++)
                {
                    if (((UIElement)cnvMain.Children[i]).GetType().ToString() == "System.Windows.Controls.Label")
                    {
                        if (((Label)cnvMain.Children[i]).Name == "lbemail")
                        {
                            cnvMain.Children.RemoveAt(i);
                        }
                    }

                    if (((UIElement)cnvMain.Children[i]).GetType().ToString() == "System.Windows.Controls.TextBox")
                    {
                        if (((TextBox)cnvMain.Children[i]).Name == "txtEmail")
                        {
                            cnvMain.Children.RemoveAt(i);
                        }
                    }

                    if (((UIElement)cnvMain.Children[i]).GetType().ToString() == "System.Windows.Controls.Button")
                    {
                        if (((Button)cnvMain.Children[i]).Name == "btnok")
                        {
                            cnvMain.Children.RemoveAt(i);
                        }
                    }

                    if (((UIElement)cnvMain.Children[i]).GetType().ToString() == "System.Windows.Controls.Button")
                    {
                        if (((Button)cnvMain.Children[i]).Name == "btncan")
                        {
                            cnvMain.Children.RemoveAt(i);
                        }
                    }
                }

                btnSignUp.Visibility = Visibility.Visible;
                btnLogIn.Visibility = Visibility.Visible;
                lblValidate.Content = string.Empty;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnCancel_click()", "Controls\\CtlLogin.xaml.cs");
            }
		}

		void txtEmail_KeyDown(object sender, KeyEventArgs e)
		{
            try
            {
                if (e.Key == Key.Enter)
                {
                    btnOK_Click(null, null);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "txtEmail_KeyDown()", "Controls\\CtlLogin.xaml.cs");
            }
		}

        public bool validate_text()
        {
            try
            {
                Boolean option = true;
                string temp = "dummy" + txtUserNameID.Text + "dummy";
                char[] delimiter1 = { ';', ':', '~', '`', '!', '$', '#', '%', '^', '&', '*', '(', ')', '-', '+', '=', '|', '?', '{', '}', '[', ']', '<', '"', '>', ',', '@' };
                string[] stest1 = null;
                stest1 = temp.Split(delimiter1);
                if (stest1.Length > 1)
                {
                    option = false;
                    
                }
                char[] len = txtUserNameID.Text.Substring(0, 1).ToCharArray();
                
                if (!(((int)len[0] >= (int)'a' && (int)len[0] <= (int)'z') || ((int)len[0] >= (int)'A' && (int)len[0] <= (int)'Z')))
                {
                    option = false;
                   
                }
                string chk = txtUserNameID.Text.TrimEnd();
                if (!(txtUserNameID.Text.Length == chk.Length))
                {
                    option = false;
                    
                }
                
                return option;
            }
            catch (Exception ex)
            {
                MessageBox.Show("validate_text(): " + ex.Message);
                return true;

            }

        }

        string encodestring(string str)
        {
            try
            {
                int j = 0;

                Byte[] encodedBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(str);
                Byte[] ans = new Byte[encodedBytes.Length];
                foreach (Byte b in encodedBytes)
                {

                    int i = Convert.ToInt32(b);

                    if ((i >= 65 && i <= 90) || (i >= 97 && i <= 122))
                    {
                        i += 4;
                        if ((i > 90 && i <= 97) || (i > 122 && i <= 129))
                            i = i - 26;

                    }
                    else if (i >= 48 && i <= 57)
                    {
                        i += 7;
                        if (i > 57)
                            i = i - 10;
                    }
                    else
                    {
                        if (i == 61)
                            i = 44;
                        else if (i == 44)
                            i = 61;
                    }

                    ans[j++] = Convert.ToByte(i);
                }
                return System.Text.ASCIIEncoding.ASCII.GetString(ans);
            }
            catch 
            {
                return null;
            }
        }

		static string DeCodeString(string str)
		{
			try
			{
                int j = 0;

                Byte[] encodedBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(str);
                Byte[] ans = new Byte[encodedBytes.Length];
                foreach (Byte b in encodedBytes)
                {

                    int i = Convert.ToInt32(b);

                    if ((i >= 65 && i <= 90) || (i >= 97 && i <= 122))
                    {
                        i -= 4;
                        if ((i < 65 && i >= 61) || (i < 97 && i >= 93))
                            i = i + 26;

                    }
                    else if (i >= 48 && i <= 57)
                    {
                        i -= 7;
                        if (i < 48)
                            i = i + 10;
                    }

                    else
                    {
                        if (i == 61)
                            i = 44;
                        else if (i == 44)
                            i = 61;
                    }

                    ans[j++] = Convert.ToByte(i);
                }
                return System.Text.ASCIIEncoding.ASCII.GetString(ans);
			}
			catch (Exception ex)
			{
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "DeCodeString()", "Controls\\CtlLogin.xaml.cs");				
				return null;
			}
			
		}

        void CallBootStrapHTTPJoin()
        {
            try
            {
                try
                {
                    //ClsException.WriteToLogFile("calling http bs join");
                    clsBootStrapInfo objBootStrapInfo = App.chHttpBootStrapService.svcHttpBSJoin("", null);
                    VMuktiAPI.VMuktiInfo.MainConnectionString = objBootStrapInfo.ConnectionString;

                    switch (objBootStrapInfo.AuthType)
                    {
                        case "SIPAuthentication":
                            VMuktiAPI.VMuktiInfo.CurrentPeer.CurrAuthType = AuthType.SIPAuthentication;
                            break;

                        case "SQLAuthentication":
                            VMuktiAPI.VMuktiInfo.CurrentPeer.CurrAuthType = AuthType.SQLAuthentication;
                            break;

                        default:
                            VMuktiAPI.VMuktiInfo.CurrentPeer.CurrAuthType = AuthType.NotDecided;
                            break;

                    }

                    bool isSuperNode = false;
                    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithNetP2P)
                    {
                        isSuperNode = false;
                    }
                    else if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.BootStrap || VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.SuperNode)
                    {
                        isSuperNode = true;
                    }

                    //ClsException.WriteToLogFile("calling http bs svcHttpBsGetSuperNodeIP");
                    clsSuperNodeDataContract objSuperNodeDataContract = App.chHttpBootStrapService.svcHttpBsGetSuperNodeIP(VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName, VMuktiAPI.VMuktiInfo.CurrentPeer.CurrentMachineIP, isSuperNode);
                    VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP = objSuperNodeDataContract.SuperNodeIP;
                }
                catch (System.ServiceModel.EndpointNotFoundException ex)
                {
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CallBootStrapHttpJoin()", "Controls\\CtlLogin.xaml.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                }
                catch (System.ServiceModel.CommunicationException exp)
                {
                    VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "CallBootStrapHttpJoin()", "Controls\\CtlLogin.xaml.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                }

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CallBootStrapHttpJoin()--1", "Controls\\CtlLogin.xaml.cs");
            }

        }

		#region Registry functions

		void RememberUser()
		{
            try
            {
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RememberUser()", "Controls\\CtlLogin.xaml.cs");
            }
			
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
            try
            {
               
                if (objRTCAuthClient != null)
                {
                    objRTCAuthClient = null;
                }

                if (EntAutherized != null)
                {
                    EntAutherized = null;
                }

                if (objSuperNodeDataContract != null)
                {
                    objSuperNodeDataContract = null;
                }
                if (txtEmail != null)
                {
                    txtEmail = null;
                }
                if (dispTimer4DomainLoading != null)
                {
                    dispTimer4DomainLoading = null;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose()", "Controls\\CtlLogin.xaml.cs");
            }
		}

		#endregion

		~CtlLogin()
		{
			Dispose();
		}       
	}
}