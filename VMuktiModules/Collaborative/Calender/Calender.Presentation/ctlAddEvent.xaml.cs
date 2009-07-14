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
using Calender.Business;
using System.Data;
using System.Net.Mail;
using System.Net.Security;
using System.Threading;
using System.Timers;
using System.IO;
using System.Reflection;
using Calender.Business.Service;
using VMuktiAPI;


namespace Calender.Presentation
{
    /// <summary>
    /// Interaction logic for ctlAddEvent.xaml
    /// </summary>
    public partial class ctlAddEvent : UserControl
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

        System.Windows.Threading.DispatcherTimer dt = new System.Windows.Threading.DispatcherTimer();

        //StringBuilder sb1 = CreateTressInfo();
        Int64 confID = -1;
        string MCType = "";
        Int64 userID = 10;

        string SMTPServer = "";
        int SMTPPort = -1;
        string SMTPUserName = "";
        string SMTPPassword = "";
        DataSet dsTimeZone;
        //StringBuilder sb1 = CreateTressInfo();

        public ctlAddEvent()
        {
            try
            {
            InitializeComponent();
            userID = VMuktiAPI.VMuktiInfo.CurrentPeer.ID;
            AddExistingUsers();

                ///// Populates the Country list for timezone
                dsTimeZone = ClsCalender.getCountry_Timezone();

                //foreach (DataRow drTimeZone in dsTimeZone.Tables[0].Rows)
                //{
                //    if (!cmbCountryTimeZone.Items.Contains(drTimeZone["Country"]))
                //        cmbCountryTimeZone.Items.Add(drTimeZone["Country"]);

                //}
                //cmbCountryTimeZone.SelectedIndex = 0;


            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlAddEvent()", "ctlAddEvent.xaml.cs");
            }

        }

        private bool isvalidate()
        {
            try
            {
            if (dtpStartDate.Value == null || dtpendate.Value == null)
            {
                MessageBox.Show("please select start date and end date");
                return false;
            }
            else if (cmbstarttimeh.SelectedValue == null && cmbstarttimem.SelectedValue == null && cmbstartap.SelectedValue == null)
            {
                MessageBox.Show("Select a proper value of time schedule");
                return false;
            }
            else if (cmbstarttimeh.SelectedValue != null && cmbstarttimem.SelectedValue != null && cmbstartap.SelectedValue != null)
            {
               
                DateTime dsStart = new DateTime(int.Parse(dtpStartDate.Value.Value.Year.ToString()), int.Parse(dtpStartDate.Value.Value.Month.ToString()), int.Parse(dtpStartDate.Value.Value.Day.ToString()), int.Parse(cmbstarttimeh.Text), int.Parse(cmbstarttimem.Text), 0);
                string strDTStart = dsStart.ToString();
                string strReplaceText = " " + cmbendtimeap.Text.ToUpper();
                strDTStart = strDTStart.Replace("AM", strReplaceText);
                dsStart = DateTime.Parse(strDTStart);
                int i = DateTime.Compare(DateTime.Now, dsStart);
                if (i > 0)
                {
                    MessageBox.Show("select proper start time schedule");
                    return false;
                }
                else
                {
                    return true;
                }

            }
            else if (cmbendtimeh.SelectedValue == null && cmbendtimem.SelectedValue == null && cmbendtimeap.SelectedValue == null)
            {
                MessageBox.Show("select proper end time schedule");
                return false;
            }
            else if (dtpStartDate.Value > dtpendate.Value)
            {
                MessageBox.Show("Incorrect End date ");
                return false;
            }
            else if (lstInvitedUserList.Items.Count == 0)
            {
                MessageBox.Show("Select at least one user for conference");
                return false;
            }
            //else if (cmbremindertype.SelectedItem == null)
            //{
            //    MessageBox.Show("PLease select at list one Reminder Type");
            //    return false;
            //}
            //else if (cmbremindertime.SelectedValue == null)
            //{
            //    MessageBox.Show("Please select a valid Reminder Time");
            //    return false;
            //}
            //else
            {
                return true;
            }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "isvalidate()", "ctlAddEvent.xaml.cs");
                return false;
            }
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            if (isvalidate())
            {
     //           DateTime dsStart = new DateTime(int.Parse(dtpStartDate.Value.Value.Year.ToString()), int.Parse(dtpStartDate.Value.Value.Month.ToString()), int.Parse(dtpStartDate.Value.Value.Day.ToString()), int.Parse(cmbstarttimeh.Text), int.Parse(cmbstarttimem.Text), 0);
    //            DateTime dsEnd = new DateTime(int.Parse(dtpendate.Value.Value.Year.ToString()), int.Parse(dtpendate.Value.Value.Month.ToString()), int.Parse(dtpendate.Value.Value.Day.ToString()), int.Parse(cmbendtimeh.Text), int.Parse(cmbendtimem.Text), 0);
                ClsCalender objCal = new ClsCalender();

                
                    objCal.ID = -1;
                    objCal.ConfTitle = txtWhat.Text;
                    //  MessageBox.Show(dtpStartDate.Text);
                    string tmpDate = dtpStartDate.Text;
                    string[] splittmpDate = tmpDate.Split('/');
                    tmpDate = splittmpDate[1] + "/" + splittmpDate[0] + "/" + splittmpDate[2];
                    objCal.StartDateTime = DateTime.Parse(tmpDate + " " + cmbstarttimeh.Text + ":" + cmbstarttimem.Text + " " + cmbstartap.Text);
                    //   objCal.StartDateTime = dsStart;
                    DateTime dsStart = objCal.StartDateTime;
                    tmpDate = dtpendate.Text;
                    splittmpDate = tmpDate.Split('/');
                    tmpDate = splittmpDate[1] + "/" + splittmpDate[0] + "/" + splittmpDate[2];
                    objCal.EndDateTime = DateTime.Parse(tmpDate + " " + cmbendtimeh.Text + ":" + cmbendtimem.Text + " " + cmbendtimeap.Text);
                    DateTime dsEnd = objCal.EndDateTime;
                    //       objCal.EndDateTime = dsEnd;
                    DataTable dtMeetingBetween = ClsCalender.getMeetingsBetween(userID, dsStart, dsEnd).Tables[0];

                    if (dtMeetingBetween.Rows.Count == 0)
                    {

                        if (chkallday.IsChecked == true)
                        {
                            objCal.IsAllDay = true;
                        }
                        else
                            objCal.IsAllDay = false;

                        objCal.RepeatType = cmbrepeats.Text;
                        objCal.HostId = userID;
                        objCal.ConferenceLocation = txtwhere.Text;
                        objCal.ConferenceDetail = txtdescription.Text;
                        //objCal.Country = cmbCountryTimeZone.SelectedValue.ToString().Trim();
                        objCal.Timezone = cmbTimeZone.SelectedValue.ToString().Trim();
                        objCal.ModifiedBy = userID;
                        objCal.CreatedBy = userID;
                        objCal.IsDeleted = false;
                        //                objCal.TimeBeforeConf = cmbremindertime.Text;
                        //                objCal.ReminderType = cmbremindertype.Text;

                        if (rbndefault.IsChecked == true)
                        {
                            //objCal.ReminderType = "Default";
                            objCal.ConferenceType = "Default";
                            MCType = "Default";
                        }

                        else if (rbnprivate.IsChecked == true)
                        {
                            //objCal.ReminderType = "Private";
                            objCal.ConferenceType = "Private";
                            MCType = "Private";

                        }
                        else if (rbnpublic.IsChecked == true)
                        {
                            //objCal.ReminderType = "Public";
                            objCal.ConferenceType = "Public";
                            MCType = "Public";
                        }

                        objCal.ConfResponse = "Yes";

                        confID = objCal.Save();
                        string myDisplayName = ClsCalender.getUserInfo(userID);
                        string myEmail = ClsCalender.getUserInfoEmail(userID);
                        objCal.GuestName = myDisplayName;
                        objCal.Email = myEmail;
                        objCal.SaveGuest(confID);
                        ClsCalender.setEmailStatusOfConferenceGuest(myDisplayName, myEmail, "Organized");
                        ClsCalender.addConferenceUsers(confID, userID);
                        ClsCalender.updateConferenceUsers(confID, userID, "Yes");
                        sendInvitesToAll();
                        MessageBox.Show("Event Created Successfully");

                        //txtWhat.Text = "";
                        //dtpStartDate.Value = null;
                        //cmbstarttimeh.SelectedIndex = 0;
                        //cmbstarttimem.SelectedIndex = 0;
                        //cmbstartap.SelectedIndex = 0;
                        //dtpendate.Value = null;
                        //cmbendtimeh.SelectedIndex = 0;
                        //cmbendtimem.SelectedIndex = 0;
                        //cmbendtimeap.SelectedIndex = 0;
                        //cmbrepeats.SelectedIndex = 0;
                        //txtwhere.Text = "";
                        //txtdescription.Text = "";
                        ////                cmbremindertype.SelectedIndex = 0;
                        ////                cmbremindertime.SelectedIndex = 0;
                        //rbnprivate.IsChecked = true;
                        //txtUserName.Text = "";
                        //txtEmail.Text = "";
                        //lstInvitedUserList.Items.Clear();
                        //lstExistingUserList.Items.Clear();
                        //AddExistingUsers();

                        ((ctlCalContainer)this.Tag).objAddEvent.Visibility = Visibility.Collapsed;
                        ((ctlCalContainer)this.Tag).objConfCalander.Visibility = Visibility.Visible;
                        ((ctlCalContainer)this.Tag).objAddEvent = null;
                        ((Grid)this.Parent).Children.Remove(this);
                }
                else
                {
                    int countRows = dtMeetingBetween.Rows.Count;
                    string msg = "";
                    int i =0;
                    foreach (DataRow drMeetingBetween in dtMeetingBetween.Rows)
                    {
                        DateTime ds = DateTime.Parse(drMeetingBetween["StartDateTime"].ToString());
                        DateTime de = DateTime.Parse(drMeetingBetween["EndDateTime"].ToString());

                        string[] startDateTimeSplit = ds.ToString().Split(' ');
                        string[] endDateTimeSplit = de.ToString().Split(' ');
                        string confTitle = drMeetingBetween["ConfTitle"].ToString();

                        string[] stSplit = startDateTimeSplit[1].Split(':');
                        string[] etSplit = endDateTimeSplit[1].Split(':');

                        startDateTimeSplit[1] = stSplit[0] + ":" + stSplit[1];
                        endDateTimeSplit[1] = etSplit[0] + ":" + etSplit[1];

                        if(startDateTimeSplit[0].Equals(endDateTimeSplit[0]))
                        {
                            msg += ++i + ". " + confTitle +"        " + startDateTimeSplit[0] + "  -  " + startDateTimeSplit[1]+" "+startDateTimeSplit[2]+" To "+endDateTimeSplit[1]+" "+endDateTimeSplit[2] +"\n"; 
                        }
                        else
                            msg += ++i + ". " + confTitle +"        " + startDateTimeSplit[0] + "  -  " + startDateTimeSplit[1]+" "+startDateTimeSplit[2]+" To "+ startDateTimeSplit[0] + "  -  " +endDateTimeSplit[1]+" "+endDateTimeSplit[2] +"\n"; 

                    }

                    MessageBox.Show(countRows + " Meetings between the selected time already exist. Please select different time. \n\n"+msg);
                }
            }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "button1_Click()", "ctlAddEvent.xaml.cs");
            }


        }
           
        

        private void btncancle_Click(object sender, RoutedEventArgs e)
        {
            
            ((ctlCalContainer)this.Tag).objAddEvent.Visibility = Visibility.Collapsed;
            ((ctlCalContainer)this.Tag).objConfCalander.Visibility = Visibility.Visible;
            ((ctlCalContainer)this.Tag).objAddEvent = null;
            ((Grid)this.Parent).Children.Remove(this);
            
            
        }

        

        private void btnAddNewUser_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            if (txtUserName.Text.Trim() != string.Empty && txtEmail.Text.Trim() != string.Empty)
            {
                if (IsValidName(txtUserName.Text.Trim()) && IsValidEmailAddress(txtEmail.Text.Trim()))
                {
                ListBoxItem lstItem = new ListBoxItem();
                lstItem.Content = txtUserName.Text;
                lstItem.Tag = txtEmail.Text + ":-99";
                lstItem.ToolTip = txtEmail.Text;

                lstInvitedUserList.Items.Add(lstItem);
                txtEmail.Text = string.Empty;
                txtUserName.Text = string.Empty;
            }
                else
                    {
                    MessageBox.Show("Please Enter Valid UserName and Password (Name can contain only space");
            }

                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "btnAddNewUser_Click()", "ctlAddEvent.xaml.cs");
            }
        }



        private void AddExistingUsers()
        {
            try
            {
            System.Data.DataSet dsUserInfo = ClsCalender.getAllUsersInfo();


            foreach (DataRow dr in dsUserInfo.Tables[0].Rows)
            {
                ListBoxItem lstItem = new ListBoxItem();
                Int64 myid = Int64.Parse(dr["ID"].ToString());

                if (myid != userID)
                {
                lstItem.Content = dr["DisplayName"];
                lstItem.Tag = dr["EMail"];
                lstItem.Tag += ":" + dr["ID"];
                lstItem.ToolTip = dr["EMail"];

                lstExistingUserList.Items.Add(lstItem);
            }
        }
        }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "AddExistingUsers()", "ctlAddEvent.xaml.cs");
            }
        }


        private void btnAddToInvitedUserList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            if (lstExistingUserList.SelectedItems.Count > 0)
            {
                int count = lstExistingUserList.SelectedItems.Count;
                for (int i = 0; i < count; i++)
                {
                    ListBoxItem lstItem = new ListBoxItem();
                    lstItem.Content = ((ListBoxItem)lstExistingUserList.SelectedItems[0]).Content;
                    lstItem.Tag = ((ListBoxItem)lstExistingUserList.SelectedItems[0]).Tag;
                    lstItem.ToolTip = ((ListBoxItem)lstExistingUserList.SelectedItems[0]).ToolTip;
                    lstExistingUserList.Items.Remove(lstExistingUserList.SelectedItems[0]);
                    lstInvitedUserList.Items.Add(lstItem);

                }
            }
        }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "btnAddToInvitedUserList_Click()", "ctlAddEvent.xaml.cs");
            }
        }
        private void btnRemoveFromInvitedUserList_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            if (lstInvitedUserList.SelectedItems.Count > 0)
            {
                int count = lstInvitedUserList.SelectedItems.Count;
                for (int i = 0; i < count; i++)
                {
                    ListBoxItem lstItem = new ListBoxItem();
                    lstItem.Content = ((ListBoxItem)lstInvitedUserList.SelectedItems[0]).Content;
                    lstItem.Tag = ((ListBoxItem)lstInvitedUserList.SelectedItems[0]).Tag;
                    lstItem.ToolTip = ((ListBoxItem)lstInvitedUserList.SelectedItems[0]).ToolTip;
                    lstInvitedUserList.Items.Remove(lstInvitedUserList.SelectedItems[0]);
                    //  lstInvitedUserList.Items.RemoveAt(0);
                    lstExistingUserList.Items.Add(lstItem);

                }
            }
        }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "btnRemoveFromInvitedUserList_Click()", "ctlAddEvent.xaml.cs");
            }
        }

       public void SendEmailTo(string To, string Subject, string msg)
        {
           try
           {
               ClsException.WriteToLogFile("Calender: User sending  Email");
               ClsException.WriteToLogFile("Email Sending," + To + "," + Subject + "," + msg);
                if (SMTPServer.Equals("") || SMTPPort != -1 || SMTPUserName.Equals("") || SMTPPassword.Equals(""))
                {
                    DataSet dsSMTP = ClsCalender.getSMTPCredentials();

                    foreach (DataRow drSMTP in dsSMTP.Tables[0].Rows)
                    {
                        string fieldName = drSMTP["FieldName"].ToString();

                        switch (fieldName)
                        {
                            case "SMTPServer":
                                {
                                    SMTPServer = drSMTP["FieldValue"].ToString();
                                    break;
                                }
                            case "SMTPPort":
                                {
                                    SMTPPort = int.Parse(drSMTP["FieldValue"].ToString());
                                    break;
                                }

                            case "SMTPUserName":
                                {
                                    SMTPUserName = drSMTP["FieldValue"].ToString();
                                    break;
                                }

                            case "SMTPPassword":
                                {
                                    SMTPPassword = drSMTP["FieldValue"].ToString();
                                    break;
                                }

                            default:
                                 break;
                        }
                    }
                }

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    clsMailInfo objMail = new clsMailInfo();
                    objMail.strFrom = SMTPUserName;
                    objMail.strTo = To;
                    objMail.strSubject = Subject;
                    objMail.strMsg = msg;
                    objMail.strServer = SMTPServer;
                    objMail.intPort = SMTPPort;
                    objMail.strPwd = SMTPPassword;
                    clsMailDBClient.chHttpMailDBService.svcSendMail(objMail);
                }
                else
                {
                System.Net.Mail.MailMessage mailMsg = new System.Net.Mail.MailMessage();
                mailMsg.From = new System.Net.Mail.MailAddress(SMTPUserName);
                mailMsg.To.Add(To);
                mailMsg.Subject = Subject;
                mailMsg.SubjectEncoding = System.Text.Encoding.UTF8;
                mailMsg.Body = msg;
                mailMsg.BodyEncoding = System.Text.Encoding.UTF8;
                mailMsg.Priority = System.Net.Mail.MailPriority.High;
                mailMsg.IsBodyHtml = true;




                System.Net.Mail.SmtpClient SmtpMail = new System.Net.Mail.SmtpClient(SMTPServer, SMTPPort);
                SmtpMail.Credentials = new System.Net.NetworkCredential(SMTPUserName, SMTPPassword);
                SmtpMail.EnableSsl = true;
                SmtpMail.Send(mailMsg);
                }


            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "SendEmailTo()", "ctlAddEvent.xaml.cs");
            }
        }


       public void sendInvitesToAll()
       {
          
          
           ClsCalender objCal = new ClsCalender();

           DataRow drConf = ClsCalender.getConferenceDetails(confID).Tables[0].Rows[0];

           for (int i = 0; i < lstInvitedUserList.Items.Count; i++)
           {
               ListBoxItem lstItem = new ListBoxItem();
               lstItem = (ListBoxItem)lstInvitedUserList.Items.GetItemAt(i);
               string guestname = lstItem.Content.ToString();
               string[] tagSplit = lstItem.Tag.ToString().Split(':');
               Int64 guestID = Int64.Parse(tagSplit[1]);
               string email = tagSplit[0];
               objCal.ConferenceID = confID;

               objCal.Email = email;
               objCal.CreatedBy = userID;
               objCal.ModifiedBy = userID;

               if (guestID == -99)
               {
                   try
                   {
                       Int64 uid = CreateNewUser(guestname, email);
                       DataRow drUinfo = ClsCalender.getUserInfoForMail(uid).Tables[0].Rows[0];
                       string newuserid = drUinfo["DisplayName"].ToString();
                       string pwd = drUinfo["Password"].ToString();
                       objCal.GuestName = newuserid;

                       string[] stotaltimeSplit = drConf["StartDateTime"].ToString().Split(' ');
                       string sdate1 = stotaltimeSplit[0];
                       string stime1 = stotaltimeSplit[1] + " " + stotaltimeSplit[2];

                       string[] etotaltimeSplit = drConf["EndDateTime"].ToString().Split(' ');
                       string edate1 = etotaltimeSplit[0];
                       string etime1 = etotaltimeSplit[1] + " " + etotaltimeSplit[2];
                       string strUri = VMuktiAPI.VMuktiInfo.ZipFileDownloadLink.ToString();
                       if (dtpStartDate.Text.Equals(dtpendate.Text))
                           SendEmailTo(email, "VMukti Meeting/Conference Invitation: " + drConf["ConfTitle"].ToString(), "<br/><br/>You are invited for the following Conference/Meeting. <br/><br/><br/><b>Title:</b> " + drConf["ConfTitle"].ToString() + "<br/><br/>" + "<b>Description:</b> " + drConf["ConferenceDetail"].ToString() + "<br/><br/><b>Location:</b> " + drConf["ConferenceLocation"].ToString() + "<br/><br/><b>Meeting/Conference Type:</b> " + drConf["ConferenceType"].ToString() + "<br/><br/><br/><br/><b>Date:</b> " + sdate1 + "<br/><br/><b>Start Time:</b> " + stime1 + "<br/><br/><b>End Time:</b> " + etime1 +   " <br/><br/><b>Default TimeZone:</b> " + drConf["Timezone"] + "<br/><br/><b>Your Login Information is as follows:</b><br/><b>Username:</b> " + newuserid + "<br><b>Password:</b> " + pwd + "<br><br><b>URI:</b> "+strUri+"<br><br><br>");
                       else
                           SendEmailTo(email, "VMukti Meeting/Conference Invitation: " + drConf["ConfTitle"].ToString(), "<br/><br/>You are invited for the following Conference/Meeting. <br/><br/><br/><b>Title:</b> " + drConf["ConfTitle"].ToString() + "<br/><br/>" + "<b>Description:</b> " + drConf["ConferenceDetail"].ToString() + "<br/><br/><b>Location:</b> " + drConf["ConferenceLocation"].ToString() + "<br/><br/><b>Meeting/Conference Type:</b> " + drConf["ConferenceType"].ToString() + "<br/><br/><br/><br/><b>Start Date:</b> " + sdate1 + "<br/><br/><b>Start Time:</b> " + stime1 + "<br/><br/><br/><b>End Date:</b> " + edate1 + "<br/><br/><b>End Time:</b> " + etime1 + " <br/><br/><b>Default TimeZone:</b> " + drConf["Timezone"] + "<br/><br/><b>Your Login Information is as follows:</b><br/><b>Username:</b> " + newuserid + "<br><b>Password:</b> " + pwd + "<br><br><b>URI:</b> " + strUri + "<br><br><br>");

                       ClsCalender.setEmailStatusOfConferenceGuest(newuserid, email, "Sent");
                       //      if (guestID != -99)
                       ClsCalender.addConferenceUsers(confID, uid);
                       objCal.SaveGuest(confID);
                   }
                   catch (Exception exp)
                   {
                       VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "sendInvitesToAll()--if--", "ctlAddEvent.xaml.cs");
                   }
               }
               else
               {
                   objCal.GuestName = guestname;
                   objCal.SaveGuest(confID);

                   DataRow drConfGuest = ClsCalender.getConferenceGuest(guestname, email).Tables[0].Rows[0];

                   string emailStatus = drConfGuest["EmailStatus"].ToString();


                   try
                   {
                       objCal.GuestName = guestname;
                       string[] stotaltimeSplit = drConf["StartDateTime"].ToString().Split(' ');
                       string sdate1 = stotaltimeSplit[0];
                       string stime1 = stotaltimeSplit[1] + " " + stotaltimeSplit[2];

                       string[] etotaltimeSplit = drConf["EndDateTime"].ToString().Split(' ');
                       string edate1 = etotaltimeSplit[0];
                       string etime1 = etotaltimeSplit[1] + " " + etotaltimeSplit[2];
                       string strUri = VMuktiAPI.VMuktiInfo.ZipFileDownloadLink.ToString();
                       if (dtpStartDate.Text.Equals(dtpendate.Text))
                           //Email sended to invited users(email, ConfTitle)
                           SendEmailTo(email, "VMukti Meeting/Conference Invitation: " + drConf["ConfTitle"].ToString(), "<br/><br/>You are invited for the following Conference/Meeting. <br/><br/><br/><b>Title:</b> " + drConf["ConfTitle"].ToString() + "<br/><br/>" + "<b>Description:</b> " + drConf["ConferenceDetail"].ToString() + "<br/><br/><b>Location:</b> " + drConf["ConferenceLocation"].ToString() + "<br/><br/><b>Meeting/Conference Type:</b> " + drConf["ConferenceType"].ToString() + "<br/><br/><br/><br/><b>Date:</b> " + sdate1 + "<br/><br/><b>Start Time:</b> " + stime1 + "<br/><br/><b>End Time:</b> " + etime1 + "<br/><br/><b>Default TimeZone:</b> " + drConf["Timezone"] + "<br><br><b>URI:</b> " + strUri + "<br/><br/><br/>");
                       else
                           SendEmailTo(email, "VMukti Meeting/Conference Invitation: " + drConf["ConfTitle"].ToString(), "<br/><br/>You are invited for the following Conference/Meeting. <br/><br/><br/><b>Title:</b> " + drConf["ConfTitle"].ToString() + "<br/><br/>" + "<b>Description:</b> " + drConf["ConferenceDetail"].ToString() + "<br/><br/><b>Location:</b> " + drConf["ConferenceLocation"].ToString() + "<br/><br/><b>Meeting/Conference Type:</b> " + drConf["ConferenceType"].ToString() + "<br/><br/><br/><br/><b>Start Date:</b> " + sdate1 + "<br/><br/><b>Start Time:</b> " + stime1 + "<br/><br/><br/><b>End Date:</b> " + edate1 + "<br/><br/><b>End Time:</b> " + etime1 + "<br/><br/><b>Default TimeZone:</b> " + drConf["Timezone"] + "<br><br><b>URI:</b> " + strUri + "<br/><br/><br/>");

                       ClsCalender.setEmailStatusOfConferenceGuest(guestname, email, "Sent");
                       //      if (guestID != -99)
                       ClsCalender.addConferenceUsers(confID, guestID);
                       objCal.SaveGuest(confID);
                   }
                   catch (Exception exp)
                   {
                       VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "sendInvitesToAll()--else--", "ctlAddEvent.xaml.cs");
                   }
               }
               
           }
           for (int j = 0; j < lstExistingUserList.Items.Count; j++)
           {
               ListBoxItem lstItem1 = new ListBoxItem();
               lstItem1 = (ListBoxItem)lstExistingUserList.Items.GetItemAt(j);
               string guestname1 = lstItem1.Content.ToString();
               string[] tagSplit1 = lstItem1.Tag.ToString().Split(':');
               Int64 guestID1 = Int64.Parse(tagSplit1[1]);
               string email1 = tagSplit1[0];

               ClsCalender.deleteUnInvitedConferenceGuest(guestname1, email1);
               //       if (guestID != -99)
               //       {
               ClsCalender.deleteConferenceUsers(confID, guestID1);
               //       }
           }

           MessageBox.Show("Mail has been sent to all the invited users");

       }


        private Int64 CreateNewUser(string username, string email)
        {
            string userName = username;
            string Email = email;
            string newUserId = "";
            string password = "";

            string[] userNameSplit = userName.Trim().Split(' ');
            try
            {
            if (userNameSplit.Length == 1)
            {
                newUserId = userName.Trim().ToLower();

            }
            else if (userNameSplit.Length == 2)
            {
                if (userNameSplit[0].Trim().Length > 1)
                    newUserId = userNameSplit[0].Trim().ToLower() + "." + userNameSplit[1].Trim().ToLower();
                else
                    newUserId = userNameSplit[0].Trim().ToLower() + userNameSplit[1].Trim().ToLower();

            }
            else if (userNameSplit.Length == 3)
            {
                if (userNameSplit[0].Trim().Length > 1 && userNameSplit[1].Trim().Length >= 1)
                    newUserId = userNameSplit[0].Trim().ToLower() + "." + userNameSplit[2].Trim().ToLower();
                else if (userNameSplit[1].Trim().Length == 1)
                    newUserId = userNameSplit[0].Trim().ToLower() + userNameSplit[1].Trim().ToLower() + userNameSplit[2].Trim().ToLower();
            }

            

            newUserId = newUserId.ToLower();
            password = GetPassword();
            string userIdFinal = newUserId + GenerateRandomNumber();
            bool UserIDExists = true;
            while (UserIDExists == true)
            {
                if (ClsCalender.IsUserIDExist(userIdFinal))
                {
                    userIdFinal = newUserId + GenerateRandomNumber();
                    UserIDExists = true;
                }
                else
                {
                    UserIDExists = false;
                    return ClsCalender.addNewUser(userName, userIdFinal, password, Email);

                }
            }
            return -1;
        }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "CreateNewUser()", "ctlAddEvent.xaml.cs");
                return -1;
            }
        }

        private int GenerateRandomNumber()
        {
            Random random = new Random();
            return random.Next(99, 1000);
        }

        private int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }

        public static string GetRandomNumber(int numChars, int seed)
        {
            string[] chars = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            try
            {
            Random rnd = new Random(seed);
            string random = string.Empty;
            for (int i = 0; i < numChars; i++)
            {
                random += chars[rnd.Next(0, 10)];
            }
            return random;
        }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "GetRandomNumber()", "ctlAddEvent.xaml.cs");
                return string.Empty;
            }
        }

        public static string GetRandomAlphabet(int numChars, int seed)
        {
            string[] chars = { "a", "b", "c", "d", "e", "f", "g", "h", "i", "j", "k", "l", "m", "n", "o","p", "q", "r", "s",
            "t", "u", "v", "w", "x", "y", "z" };

            try
            {
            Random rnd = new Random(seed);
            string random = string.Empty;
            for (int i = 0; i < numChars; i++)
            {
                random += chars[rnd.Next(0, 26)];
            }
            return random;
        }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "GetRandomAlphabet()", "ctlAddEvent.xaml.cs");
                return string.Empty;
            }

        }

        public string GetPassword()
        {
            int a = RandomNumber(1, 10);
            string pwd1 = GetRandomAlphabet(3, a);
            int b = RandomNumber(10, 100);
            string pwd2 = GetRandomNumber(3, b);
            int c = RandomNumber(100, 200);
            string pwd3 = GetRandomAlphabet(3, c);
            int d = RandomNumber(200, 300);
            string pwd4 = GetRandomNumber(3, d);

            try
            {
            string password = pwd1 + pwd2 + pwd3 + pwd4;
            return password;
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "GetPassword()", "ctlAddEvent.xaml.cs");
                return string.Empty;
            }
        }



        private bool MatchString(string str, string regexstr)
        {
            str = str.Trim();
            System.Text.RegularExpressions.Regex pattern = new System.Text.RegularExpressions.Regex(regexstr);

            return pattern.IsMatch(str);
        }


        private bool IsValidName(string strName)
        {
            // Allows alphabetical chars, single quote, dash and space
            // must be at least two characters long and caps out at 128 (database size)
            string regExPattern = @"^[a-zA-Z\s]{1,128}$";
            return MatchString(strName, regExPattern);
        }

        private bool IsValidEmailAddress(string strEmail)
        {
            // Allows common email address that can start with a alphanumeric char and contain word, dash and period characters
            // followed by a domain name meeting the same criteria followed by a alpha suffix between 2 and 9 character lone
            string regExPattern = @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$";
            return MatchString(strEmail, regExPattern);
        }


        #region logging function

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

        #endregion

        //private void cmbCountryTimeZone_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //{
        //   // string selectedCountry = cmbCountryTimeZone.SelectedValue.ToString().Trim();
        //   // cmbTimeZone.Items.Clear();
        //   // foreach (DataRow drTimeZone in dsTimeZone.Tables[0].Rows)
        //   // {
        //       // if (drTimeZone["Country"].ToString().Trim().Equals(selectedCountry))
        //            //cmbTimeZone.Items.Add(drTimeZone["TimeZoneName"]);
        //    cmbTimeZone.Items.Add(dsTimeZone["TimeZoneName"]);
        //   // }
        //    cmbTimeZone.SelectedIndex = 0;
        //}


      
    }
}
