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
using System.Collections;
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
using DispositionRender.Business;
using VMuktiAPI;

namespace DispositionRender.Presentation
{
    /// <summary>
    /// Interaction logic for CtlDispositionRender.xaml
    /// </summary>
    /// 

    //Set permission for module DispositionRender
    public enum ModulePermissions
    {
        View = 3
    }

    public partial class CtlDispositionRender : System.Windows.Controls.UserControl
    {
        //public static StringBuilder sb1;
        //public static StringBuilder CreateTressInfo()
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
        bool blApplicationExit = false;
        string sPhoneNumber = string.Empty;
        string sCurrentChannelID = string.Empty;
        string sCurrentDispostion = string.Empty;
        string sCallingType = string.Empty;
        ClsDispositionRenderCollection objDispColl = null;
        ModulePermissions[] _MyPermissions;

        public CtlDispositionRender(ModulePermissions[] MyPermissions)
        {
            try
            {
                InitializeComponent();

                _MyPermissions = MyPermissions;
                FncPermissionsReview();

                //VMuktiAPI.VMuktiInfo.ConnectionString = "Data Source=61.17.213.134\\SqlExpress;Initial Catalog=VMukti;User ID=sa;PassWord=mahavir;";
                VMuktiHelper.RegisterEvent("SetCampaignID").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(CtlDispositionRender_VMuktiEventCampaignID);
                VMuktiHelper.RegisterEvent("SetDispositionEnable").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(CtlDispositionRender_VMuktiEvent_SetDispositionEnable);
                VMuktiHelper.RegisterEvent("SetDispositionButtonClickEvent").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(CtlDispositionRender_VMuktiEvent_SetDispositionButtonClickEvent);
                VMuktiHelper.RegisterEvent("SetDispositiForDetectedTone").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(CtlDispositionRender_VMuktiEvent_SetDispositiForDetectedTone);
                cnvDispoButtons.IsEnabled = false;
                Application.Current.Exit += new ExitEventHandler(Current_Exit);

                this.btnEnterDispReason.Click += new RoutedEventHandler(btnEnterDispReason_Click);
                this.btnCancelDispReason.Click += new RoutedEventHandler(btnCancelDispReason_Click);

                this.btnEnterCallBackReason.Click += new RoutedEventHandler(btnEnterCallBackReason_Click);
                this.btnCancelCallBackReason.Click += new RoutedEventHandler(btnCancelCallBackReason_Click);

                this.btnCancelOtherDispReason.Click += new RoutedEventHandler(btnCancelOtherDispReason_Click);
                this.btnEnterOtherDispReason.Click += new RoutedEventHandler(btnEnterOtherDispReason_Click);

                //this.Unloaded += new RoutedEventHandler(CtlDispositionRender_Unloaded);
                VMuktiHelper.CallEvent("AllModulesLoaded", this, null);
                VMuktiHelper.CallEvent("AllModulesLoadedForPredictive", this, null);
                VMuktiAPI.VMuktiHelper.RegisterEvent("SignOut").VMuktiEvent += new VMuktiEvents.VMuktiEventHandler(CtlDispositionRender_VMuktiEvent);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlDispositionRender()", "CtlDispositionRender.xaml.cs");
            }
        }

        void FncPermissionsReview()
        {
            try
            {
                this.Visibility = Visibility.Visible;

                for (int i = 0; i < _MyPermissions.Length; i++)
                {
                    if (_MyPermissions[i] == ModulePermissions.View)
                    {
                        this.Visibility = Visibility.Visible;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FncPermissionsReview()", "CtlDispositionRender.xaml.cs");
            }
        }

        #region Other Call Back Notes Buttons
      
        //Submit Disposition for the particular call
        void btnEnterOtherDispReason_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtCallNote.Text = string.Empty;
                cnvDispoButtons.Visibility = Visibility.Visible;
                cnvDispoButtons.IsEnabled = false;
                cnvCallBack.Visibility = Visibility.Hidden;
                cnvOtherDispositon.Visibility = Visibility.Hidden;
                cnvDispositon.Visibility = Visibility.Hidden;
                blApplicationExit = false;

                //Automatic dialing
                if (sCallingType == "AutoMatic")
                {
                    VMuktiHelper.CallEvent("SetDisposition", this, new VMuktiEventArgs(sCurrentDispostion, txtCallNote.Text.Trim(), false, null, sCurrentChannelID));
                    VMuktiHelper.CallEvent("SetDialerEnable", this, new VMuktiEventArgs(true));
                }

                //Predictive dialing
                else if (sCallingType == "Predictive")
                {
                    //VMuktiHelper.CallEvent("SetDispositionForPredictive", this, new VMuktiEventArgs(sCurrentDispostion, txtCallNote.Text.Trim(), false, null, sCurrentChannelID));
                    VMuktiHelper.CallEvent("SetDispositionForPredictive", this, new VMuktiEventArgs(sCurrentDispostion, txtCallNote.Text.Trim(), false, null, sCurrentChannelID));
                    VMuktiHelper.CallEvent("SetPredictiveDialerEnable", this, new VMuktiEventArgs(true, sCurrentChannelID));
                }

                //VMuktiHelper.CallEvent("SetSoftPhoneEnable", this, new VMuktiEventArgs(true));
                //VMuktiAPI.VMuktiHelper.CallEvent("entDipositionRenderStatus", this, new VMuktiEventArgs(false));
                //}
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnEnterOtherDispReason_Click()", "CtlDispositionRender.xaml.cs");
            }
        }

        //cancle to submit disposition reason
        void btnCancelOtherDispReason_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cnvDispositon.Visibility = Visibility.Hidden;
                cnvCallBack.Visibility = Visibility.Hidden;
                cnvOtherDispositon.Visibility = Visibility.Hidden;
                cnvDispoButtons.Visibility = Visibility.Visible;
                cnvDispoButtons.IsEnabled = true;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnCancelOtherDispReason_Click()", "CtlDispositionRender.xaml.cs");
            }
        }
        #endregion

        #region  Call Back  Buttons
        //Call back disposition for the particular call
        void btnEnterCallBackReason_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (!CallBackInfoAvailable())
                {
                    MessageBox.Show("Fill Proper Values For Call Back");
                    return;
                }
                else
                {
                    //set callback time
                    string sCallBackDateTime = monthPicker.SelectedDate.Value.ToShortDateString();
                    sCallBackDateTime += " " + cmbHour.SelectionBoxItem.ToString();
                    sCallBackDateTime += ":" + cmbMin.SelectionBoxItem.ToString();
                    sCallBackDateTime += ":00";
                    sCallBackDateTime += " " + cmbAMPM.SelectionBoxItem.ToString();

                    if (sCallingType == "AutoMatic")
                    {
                        VMuktiHelper.CallEvent("SetDisposition", this, new VMuktiEventArgs(sCurrentDispostion, txtCallBackReason.Text.ToString(), chkIsPublic.IsChecked, sCallBackDateTime, sCurrentChannelID));
                        VMuktiHelper.CallEvent("SetDialerEnable", this, new VMuktiEventArgs(true));
                    }
                    else if (sCallingType == "Predictive")
                    {
                        //VMuktiHelper.CallEvent("SetDispositionForPredictive", this, new VMuktiEventArgs(sCurrentDispostion, txtCallBackReason.Text.ToString(), chkIsPublic.IsChecked, sCallBackDateTime, sCurrentChannelID));
                        VMuktiHelper.CallEvent("SetDispositionForPredictive", this, new VMuktiEventArgs(sCurrentDispostion, txtCallBackReason.Text.ToString(), chkIsPublic.IsChecked, sCallBackDateTime, sCurrentChannelID));
                        VMuktiHelper.CallEvent("SetPredictiveDialerEnable", this, new VMuktiEventArgs(true, sCurrentChannelID));
                    }

                    cnvDispoButtons.Visibility = Visibility.Visible;
                    cnvDispoButtons.IsEnabled = false;
                    cnvDispositon.Visibility = Visibility.Hidden;
                    cnvCallBack.Visibility = Visibility.Hidden;
                    blApplicationExit = false;
                    txtCallBackReason.Text = string.Empty;
                    txtCallBackNo.Text = string.Empty;
                    chkIsPublic.IsChecked = false;
                    monthPicker.SelectedDate.GetValueOrDefault();
                    cmbAMPM.SelectedIndex = 0;
                    cmbHour.SelectedIndex = 0;
                    cmbMin.SelectedIndex = 0;

                }
                VMuktiAPI.VMuktiHelper.CallEvent("entDipositionRenderStatus", this, new VMuktiEventArgs(false));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnEnterCallBackReason_Click()", "CtlDispositionRender.xaml.cs");
            }
        }

        //function to check callback info is available or not
        bool CallBackInfoAvailable()
        {
            try
            {
                bool isAllValid = true;
                //if (txtCallBackReason.Text.Trim() == string.Empty)
                //{
                //    isAllValid = false;                
                //}          
                if (cmbMin.SelectedItem == null || cmbHour.SelectedItem == null || cmbAMPM.SelectedItem == null)
                {
                    isAllValid = false;
                }
                else if (object.Equals(monthPicker.SelectedDate, null))
                {
                    isAllValid = false;
                }
                else if (!object.Equals(monthPicker.SelectedDate, null))
                {
                    if (cmbMin.SelectedItem != null && cmbHour.SelectedItem != null && cmbAMPM.SelectedItem != null)
                    {
                        string sCallBackDateTime = monthPicker.SelectedDate.Value.ToShortDateString();
                        sCallBackDateTime += " " + cmbHour.SelectionBoxItem.ToString();
                        sCallBackDateTime += ":" + cmbMin.SelectionBoxItem.ToString();
                        sCallBackDateTime += ":00";
                        sCallBackDateTime += " " + cmbAMPM.SelectionBoxItem.ToString();
                        int i = DateTime.Compare(DateTime.Now, DateTime.Parse(sCallBackDateTime));
                        if (i > 0)
                        {
                            isAllValid = false;
                        }
                    }
                }
                return isAllValid;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CallBackInfoAvailable()", "CtlDispositionRender.xaml.cs");
                return false;
            }

        }
        //To cancle callback reason
        void btnCancelCallBackReason_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cnvDispositon.Visibility = Visibility.Hidden;
                cnvCallBack.Visibility = Visibility.Hidden;
                cnvOtherDispositon.Visibility = Visibility.Hidden;
                cnvDispoButtons.Visibility = Visibility.Visible;
                cnvDispoButtons.IsEnabled = true;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnCancelCallBackReason_Click()", "CtlDispositionRender.xaml.cs");
            }
        }
        #endregion

        #region DNC Buttons
        //To cancle Disposition
        void btnCancelDispReason_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                cnvDispositon.Visibility = Visibility.Hidden;
                cnvCallBack.Visibility = Visibility.Hidden;
                cnvOtherDispositon.Visibility = Visibility.Hidden;
                cnvDispoButtons.Visibility = Visibility.Visible;
                cnvDispoButtons.IsEnabled = true;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnCancelDispReason_Click()", "CtlDispositionRender.xaml.cs");
            }
        }
        //To enter Disposition Reason
        void btnEnterDispReason_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (txtDNCReason.Text.Trim() == string.Empty)
                //{

                //    MessageBox.Show("Enter Desposition Reason");
                //    return;
                //}
                //else
                //{
                if (sCallingType == "AutoMatic")
                {
                    VMuktiHelper.CallEvent("SetDisposition", this, new VMuktiEventArgs(sCurrentDispostion, txtDNCReason.Text.ToString(), true));
                    VMuktiHelper.CallEvent("SetDialerEnable", this, new VMuktiEventArgs(true));
                }
                else if (sCallingType == "Predictive")
                {
                    VMuktiHelper.CallEvent("SetDispositionForPredictive", this, new VMuktiEventArgs(sCurrentDispostion, txtDNCReason.Text.Trim(), false, null, sCurrentChannelID));
                    VMuktiHelper.CallEvent("SetPredictiveDialerEnable", this, new VMuktiEventArgs(true, sCurrentChannelID));
                }

                cnvDispoButtons.Visibility = Visibility.Visible;
                cnvDispoButtons.IsEnabled = false;
                cnvDispositon.Visibility = Visibility.Hidden;
                cnvCallBack.Visibility = Visibility.Hidden;
                blApplicationExit = false;
                txtDNCReason.Text = string.Empty;
                txtPhoneNo.Text = string.Empty;
                //}
                VMuktiAPI.VMuktiHelper.CallEvent("entDipositionRenderStatus", this, new VMuktiEventArgs(false));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnEnterDispReason_Click()", "CtlDispositionRender.xaml.cs");
            }
        }
        #endregion

        #region Application Events
        void CtlDispositionRender_Unloaded(object sender, RoutedEventArgs e)
        {
            VMuktiHelper.UnRegisterEvent("SetCampaignID");
            VMuktiHelper.UnRegisterEvent("SetDispositionEnable");

            try
            {
                if (blApplicationExit == true)
                {
                    if (sCallingType == "AutoMatic")
                    {
                        VMuktiHelper.CallEvent("SetDisposition", this, new VMuktiEventArgs(100, "None", false, null, sCurrentChannelID));
                    }
                    else if (sCallingType == "Predictive")
                    {
                        VMuktiHelper.CallEvent("SetDispositionForPredictive", this, new VMuktiEventArgs(100, "None", false, null, sCurrentChannelID));
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlDispositionRender_Unloaded()", "CtlDispositionRender.xaml.cs");
            }

        }

        public void ClosePod()
        {
            try
            {
                if (blApplicationExit == true)
                {
                    if (sCallingType == "AutoMatic")
                    {
                        VMuktiHelper.CallEvent("SetDisposition", this, new VMuktiEventArgs(100, "None", false, null, sCurrentChannelID));
                    }
                    else if (sCallingType == "Predictive")
                    {
                        VMuktiHelper.CallEvent("SetDispositionForPredictive", this, new VMuktiEventArgs(100, "None", false, null, sCurrentChannelID));
                    }
                }
                VMuktiHelper.UnRegisterEvent("SetCampaignID");
                VMuktiHelper.UnRegisterEvent("SetDispositionEnable");
                VMuktiHelper.UnRegisterEvent("SetDispositionButtonClickEvent");
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ClosePod()", "CtlDispositionRender.xaml.cs");
            }
        }

        void CtlDispositionRender_VMuktiEvent(object sender, VMuktiEventArgs e)
        {
            try
            {
                if (blApplicationExit == true)
                {
                    if (sCallingType == "AutoMatic")
                    {
                        VMuktiHelper.CallEvent("SetDisposition", this, new VMuktiEventArgs(100, "None", false, null, sCurrentChannelID));
                    }
                    else if (sCallingType == "Predictive")
                    {
                        VMuktiHelper.CallEvent("SetDispositionForPredictive", this, new VMuktiEventArgs(100, "None", false, null, sCurrentChannelID));
                    }
                }
                VMuktiHelper.UnRegisterEvent("SetCampaignID");
                VMuktiHelper.UnRegisterEvent("SetDispositionEnable");
                VMuktiHelper.UnRegisterEvent("SetDispositionButtonClickEvent");
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlDispositionRender_VMuktiEvent()", "CtlDispositionRender.xaml.cs");
            }
        }

        void Current_Exit(object sender, ExitEventArgs e)
        {
            try
            {
                if (blApplicationExit == true)
                {
                    if (sCallingType == "AutoMatic")
                    {
                        VMuktiHelper.CallEvent("SetDisposition", this, new VMuktiEventArgs(100, "None", false, null, sCurrentChannelID));
                    }
                    else if (sCallingType == "Predictive")
                    {
                        VMuktiHelper.CallEvent("SetDispositionForPredictive", this, new VMuktiEventArgs(100, "None", false, null, sCurrentChannelID));
                    }
                }
                VMuktiHelper.UnRegisterEvent("SetCampaignID");
                VMuktiHelper.UnRegisterEvent("SetDispositionEnable");
                VMuktiHelper.UnRegisterEvent("SetDispositionButtonClickEvent");
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Current_Exit()", "CtlDispositionRender.xaml.cs");
            }
        }
        #endregion

        #region VMukti Events
        void CtlDispositionRender_VMuktiEvent_SetDispositionEnable(object sender, VMuktiEventArgs e)
        {
            try
            {
                blApplicationExit = bool.Parse(e._args[0].ToString());
                if (bool.Parse(e._args[0].ToString()))
                {
                    VMuktiAPI.VMuktiHelper.CallEvent("entDipositionRenderStatus", this, new VMuktiEventArgs(e._args[0].ToString()));
                    cnvDispoButtons.IsEnabled = bool.Parse(e._args[0].ToString());
                    cnvDispoButtons.Visibility = Visibility.Visible;
                }
                //else if(!bool.Parse(e._args[0].ToString()))
                else
                {
                    cnvDispoButtons.IsEnabled = bool.Parse(e._args[0].ToString());
                    cnvDispoButtons.Visibility = Visibility.Visible;
                }
                sPhoneNumber = e._args[1].ToString();
                sCurrentChannelID = e._args[2].ToString();
                sCallingType = e._args[3].ToString();

            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlDispositionRender_VMuktiEvent_SetDispositionEnable()", "CtlDispositionRender.xaml.cs");
            }
        }
        void CtlDispositionRender_VMuktiEventCampaignID(object sender, VMuktiEventArgs e)
        {
            try
            {
                //MessageBox.Show("Event Called" + e._args[0].ToString());
                Int64 CampID = Int64.Parse(e._args[0].ToString());
                cnvDispoButtons.Children.Clear();
                //GetAll() function to get data related to campaignID and Disposition List
                objDispColl = ClsDispositionRenderCollection.GetAll(CampID);
                double top = 5;
                double left = 5;
                for (int i = 0; i < objDispColl.Count; i++)
                {
                    Button btnDisp = new Button();
                    btnDisp.Content = objDispColl[i].DespositionName;
                    btnDisp.Tag = objDispColl[i].ID.ToString();
                    btnDisp.FontSize = 14;
                    btnDisp.Width = 90;
                    btnDisp.Height = 25;
                    //btnDisp.HorizontalAlignment = "Center";
                    //btnDisp.Margin = new Thickness(left, top, 0, 0);
                    btnDisp.SetValue(Canvas.TopProperty, top);
                    btnDisp.SetValue(Canvas.LeftProperty, left);
                    btnDisp.Click += new RoutedEventHandler(btnDisp_Click);
                    cnvDispoButtons.Children.Add(btnDisp);
                    if (i == 0)
                    {
                        left = left + 100;
                    }
                    else
                    {
                        if (i % 2 == 0)
                        {
                            if (i == 2)
                            {
                                left = left + 100;
                            }
                            else
                            {
                                left = left + 100;
                            }
                        }
                        else
                        {
                            if (i == 1)
                            {
                                top = top + 30;
                                left = left - 100;
                            }
                            else
                            {
                                top = top + 30;
                                left = left - 100;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlDispositionRender_VMuktiEventCampaignID()", "CtlDispositionRender.xaml.cs");
            }
        }
        void CtlDispositionRender_VMuktiEvent_SetDispositionButtonClickEvent(object sender, VMuktiEventArgs e)
        {
            sCurrentDispostion = e._args[0].ToString();
            SetDispositionCanvas(sCurrentDispostion);
        }
        void CtlDispositionRender_VMuktiEvent_SetDispositiForDetectedTone(object sender, VMuktiEventArgs e)
        {
            try
            {
                //objDispColl.GetDispoId(e._args[0].ToString())
                if (sCallingType == "AutoMatic")
                {
                VMuktiHelper.CallEvent("SetDialerEnable", this, new VMuktiEventArgs(true));
                }
                if (sCallingType == "Predictive")
                {
                    VMuktiHelper.CallEvent("SetPredictiveDialerEnable", this, new VMuktiEventArgs(true, sCurrentChannelID));
                }
                sCurrentDispostion = objDispColl.GetDispoId(e._args[0].ToString()).ToString();
                sPhoneNumber = e._args[1].ToString();
                sCurrentChannelID = e._args[2].ToString();
                sCallingType = e._args[3].ToString();
                txtCallNote.Text = string.Empty;
                cnvDispoButtons.Visibility = Visibility.Visible;
                cnvDispoButtons.IsEnabled = false;
                cnvCallBack.Visibility = Visibility.Hidden;
                cnvOtherDispositon.Visibility = Visibility.Hidden;
                cnvDispositon.Visibility = Visibility.Hidden;
                blApplicationExit = false;

                //Automatic dialing
                if (sCallingType == "AutoMatic")
                {
                    VMuktiHelper.CallEvent("SetDisposition", this, new VMuktiEventArgs(sCurrentDispostion, txtCallNote.Text.Trim(), false, null, sCurrentChannelID));
                }
                if (sCallingType == "Predictive")
                {
                    VMuktiHelper.CallEvent("SetDispositionForPredictive", this, new VMuktiEventArgs(sCurrentDispostion, txtCallNote.Text.Trim(), false, null, sCurrentChannelID));
                }
            }
            catch (Exception ex)
            {
            }
        }
        #endregion

        void btnDisp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //sCurrentDispostion = ((Button)sender).Tag.ToString();
                //cnvDispoButtons.Visibility = Visibility.Hidden;

                //if (((Button)sender).Tag.ToString() == "11")
                //{
                //    txtPhoneNo.Text = sPhoneNumber;
                //    cnvDispositon.Visibility = Visibility.Visible;
                //    // cnvDispoButtons.IsEnabled = false;
                //}
                //else if (((Button)sender).Tag.ToString() == "6")
                //{
                //    txtCallBackNo.Text = sPhoneNumber;
                //    // cnvDispoButtons.IsEnabled = false;
                //    cnvCallBack.Visibility = Visibility.Visible;
                //}
                //else
                //{
                //    txtOtherPhoneNo.Text = sPhoneNumber;
                //    //  cnvDispoButtons.IsEnabled = false;
                //    cnvCallBack.Visibility = Visibility.Hidden;
                //    cnvDispositon.Visibility = Visibility.Hidden;
                //    cnvOtherDispositon.Visibility = Visibility.Visible;
                //}
                
                //Get pressed button for dispostion
                sCurrentDispostion = ((Button)sender).Tag.ToString();
                SetDispositionCanvas(sCurrentDispostion);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnDisp_Click()", "CtlDispositionRender.xaml.cs");
            }
        }

        //set new canvas for particular disposition
        void SetDispositionCanvas(string strDispID)
        {
            try
            {
            if (strDispID == "11")
            {
                txtPhoneNo.Text = sPhoneNumber;
                cnvDispositon.Visibility = Visibility.Visible;
                cnvDispoButtons.IsEnabled = false;
                cnvDispoButtons.Visibility = Visibility.Hidden;
            }
            else if (strDispID == "6")
            {
                txtCallBackNo.Text = sPhoneNumber;
                cnvDispoButtons.IsEnabled = false;
                cnvCallBack.Visibility = Visibility.Visible;
                cnvDispoButtons.Visibility = Visibility.Hidden;
            }
            else
            {
                txtOtherPhoneNo.Text = sPhoneNumber;
                cnvDispoButtons.IsEnabled = false;
                cnvCallBack.Visibility = Visibility.Hidden;
                cnvDispositon.Visibility = Visibility.Hidden;
                cnvOtherDispositon.Visibility = Visibility.Visible;
                cnvDispoButtons.Visibility = Visibility.Hidden;
            }


        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "SetDispositionCanvas()", "CtlDispositionRender.xaml.cs");
            }
        }
    }
}
