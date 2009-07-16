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


namespace Calender.Presentation
{
    /// <summary>
    /// Interaction logic for ctlAddEvent.xaml
    /// </summary>
    public partial class ctlEditEventReadOnly : UserControl
    {
        //public static StringBuilder sb1;
 //       System.Windows.Threading.DispatcherTimer dt = new System.Windows.Threading.DispatcherTimer();

        Int64 confID = -1;

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

        public ctlEditEventReadOnly()
        {
            InitializeComponent();
           
           
                System.Data.DataSet dsUserInfo = ClsCalender.getConferenceDetails(GlobalVariables.ConferenceID);
                confID = GlobalVariables.ConferenceID;
                DataTable table = dsUserInfo.Tables[0];
                DataRow dr = table.Rows[0];
                txtWhat.Content = dr["ConfTitle"].ToString();
                txtStartDate.Content =dr["StartDateTime"].ToString();
                txtEndDate.Content = dr["EndDateTime"].ToString();
                txtAllDay.Content = dr["IsAllDay"].ToString();

                txtCreatedBy.Content = ClsCalender.getUserInfo(Int64.Parse((dr["CreatedBy"].ToString())));
                txtTimezone.Content = dr["Timezone"].ToString();
                txtRepeatType.Content = dr["RepeatType"].ToString();
                txtwhere.Content = dr["ConferenceLocation"].ToString();
                
                txtdescription.AppendText(dr["ConferenceDetail"].ToString());
                txtdescription.IsReadOnly = true;
     //           txtReminderType.Content = dr["ReminderType"].ToString();
                               

                System.Data.DataSet dsConferenceReminder = ClsCalender.getConferenceReminder(GlobalVariables.ConferenceID);
                
                DataRow drConferenceReminder = dsConferenceReminder.Tables[0].Rows[0];
    //            txtReminderTime.Content = drConferenceReminder["TimeBeforeConf"].ToString();

                string conferenceType = dr["ConferenceType"].ToString();

                if (conferenceType.Equals("Default"))
                {
                    txtPrivacy.Content = "Default";
                }
                else if (conferenceType.Equals("Public"))
                {
                    txtPrivacy.Content = "Public";
                }
                else if (conferenceType.Equals("Private"))
                {
                    txtPrivacy.Content = "Private";
                }
  
           DataSet dsUsers = ClsCalender.getConferenceUsers(GlobalVariables.ConferenceID);
           int countUsersAttendingConference = 0;
           foreach (DataRow drUsers in dsUsers.Tables[0].Rows)
           {
               try
               {
                   string response = drUsers["Attendence"].ToString().Trim();
                   if (response.Equals("Yes"))
                   {
                       countUsersAttendingConference++;
                   }
                   else if (response.Equals("MayBe"))
                   {
                       countUsersAttendingConference++;
                   }

               }
               catch (Exception ex)
               {
                   VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlEditEventReadOnly()", "ctlEditEventReadOnly.xaml.cs");  
               }
           }
           
           txtGuestsNo.Content = countUsersAttendingConference.ToString();
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            ((ctlCalContainer)this.Tag).objReadEvent.Visibility = Visibility.Collapsed;
            ((ctlCalContainer)this.Tag).objConfCalander.Visibility = Visibility.Visible;
            ((ctlCalContainer)this.Tag).objReadEvent = null;
            ((Grid)this.Parent).Children.Remove(this);
        }
    }
    
}
