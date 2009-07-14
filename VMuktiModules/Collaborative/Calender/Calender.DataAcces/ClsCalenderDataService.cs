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
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;
using VMuktiAPI;


namespace Calender.DataAccess
{
    public class ClsCalenderDataService : ClsDataServiceBase
    {
        //public static StringBuilder sb1;
        public ClsCalenderDataService() : base() { }

        public ClsCalenderDataService(IDbTransaction txn) : base(txn) { }

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

        public DataSet getAllUserInfo()
        {
            try
            {
                return ExecuteDataSet("Select ID,DisplayName,Email From vUserInfo", CommandType.Text, null);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getAllUserInfo()", "ClsCalenderDataService.xaml.cs");
                return null;
            }
        }

        public DataSet getConferenceDetails(Int64 conferenceID)
        {
            try
            {
                return ExecuteDataSet("Select * from tblConference where ID='" + conferenceID.ToString() + "'", CommandType.Text, null);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getConferenceDetails()", "ClsCalenderDataService.xaml.cs");
                return null;
            }
        }

        public DataSet getAllConferences(Int64 userID, string date)
        {
            try
            {
                // return ExecuteDataSet("Select * from tblConference where CreatedBy='" + userID + "' or HostID='"+userID+"'", CommandType.Text, null);

                //                return ExecuteDataSet("select * from tblConference where (CreatedBy ='"+userID+"' or HostID='"+userID+"') and  ((StartDateTime >= '"+date+" 12:00:00 AM' and StartDateTime <= '"+date+" 12:59:00 PM') or (EndDateTime >= '"+date+" 12:00:00 AM' and EndDateTime <= '"+date+" 12:59:00 PM'))", CommandType.Text, null);
                return ExecuteDataSet("select * from tblConference where (CreatedBy ='" + userID + "' or HostID='" + userID + "') and  ((StartDateTime >= '" + date + " 12:00:00 AM' and StartDateTime <= '" + date + " 11:59:00 PM') or (EndDateTime >= '" + date + " 12:00:00 AM' and EndDateTime <= '" + date + " 12:59:00 PM'))", CommandType.Text, null);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getAllConferences()", "ClsCalenderDataService.xaml.cs");
                return null;
            }
        }

        // to be added

        //#####  29-01-2008 ##### START
        public bool deleteConference(Int64 ConferenceID)
        {
            try
            {
                ExecuteDataSet("delete from tblConference where ID='" + ConferenceID + "'", CommandType.Text, null);
                return true;
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "deleteConference()", "ClsCalenderDataService.xaml.cs");
                return false;
            }

        }

        public DataSet getConferenceGuest(string name, string email)
        {
            try
            {
                return ExecuteDataSet("select * from tblConferenceGuests where GuestName='" + name + "' and Email='" + email + "'", CommandType.Text, null);

            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getConferenceGuest()", "ClsCalenderDataService.xaml.cs");
                return null;
            }
        }

        public void setEmailStatusOfConferenceGuest(string name, string email, string status)
        {
            try
            {
                ExecuteDataSet("update tblConferenceGuests set EmailStatus='" + status + "' where GuestName='" + name + "' and Email='" + email + "'", CommandType.Text, null);

            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "setEmailStatusOfConferenceGuest()", "ClsCalenderDataService.xaml.cs");
            }
        }

        public bool deleteUnInvitedConferenceGuest(string name, string email)
        {
            try
            {
                DataSet ds = ExecuteDataSet("select * from tblConferenceGuests where GuestName='" + name + "' and Email='" + email + "'", CommandType.Text, null);
                if (ds.Tables[0].Rows.Count != 0)
                {
                    ExecuteDataSet("delete from tblConferenceGuests where GuestName='" + name + "' and Email='" + email + "'", CommandType.Text, null);
                    return true;
                }
                return false;
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "deleteUnInvitedConferenceGuest()", "ClsCalenderDataService.xaml.cs");
                return false;
            }
        }


        public void addConferenceUsers(Int64 conferenceID, Int64 guestID)
        {
            try
            {
                ExecuteDataSet("insert into tblConferenceUsers values('" + conferenceID + "','" + guestID + "','NoResponse')", CommandType.Text, null);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "addConferenceUsers()", "ClsCalenderDataService.xaml.cs");
            }
        }

        public void updateConferenceUsers(Int64 conferenceID, Int64 guestID, string attendence)
        {
            try
            {
                ExecuteDataSet("update tblConferenceUsers set Attendence='" + attendence + "' where ConferenceID='" + conferenceID + "' and UserId='" + guestID + "'", CommandType.Text, null);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "updateConferenceUsers()", "ClsCalenderDataService.xaml.cs");
            }
        }

        public void deleteConferenceUsers(Int64 conferenceID, Int64 guestID)
        {
            try
            {
                ExecuteDataSet("delete from tblConferenceUsers where ConferenceID='" + conferenceID + "' and UserId='" + guestID + "'", CommandType.Text, null);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "deleteConferenceUsers()", "ClsCalenderDataService.xaml.cs");
            }
        }

        public void deleteConferenceUsers(Int64 conferenceID)
        {
            try
            {
                ExecuteDataSet("delete from tblConferenceUsers where ConferenceID='" + conferenceID + "'", CommandType.Text, null);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "deleteConferenceUsers()", "ClsCalenderDataService.xaml.cs");
            }
        }

        public string getConferenceUserMyAttendence(Int64 conferenceID, Int64 userID)
        {
            try
            {
                DataTable dt = ExecuteDataSet("select * from tblConferenceUsers where ConferenceID='" + conferenceID + "' and UserId='" + userID + "'", CommandType.Text, null).Tables[0];
                if (dt.Rows.Count != 0)
                {
                    DataRow dr = dt.Rows[0];
                    return dr["Attendence"].ToString();
                }

                return null;

            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getConferenceUserMyAttendence()", "ClsCalenderDataService.xaml.cs");
                return null;
            }

        }

        public DataSet getConferenceUsers(Int64 conferenceID)
        {
            try
            {
                return ExecuteDataSet("select * from tblConferenceUsers where ConferenceID='" + conferenceID + "'", CommandType.Text, null);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getConferenceUsers()", "ClsCalenderDataService.xaml.cs");
                return null;
            }

        }


        public Int64 getUserID(string name, string email)
        {
            try
            {
                DataSet ds = ExecuteDataSet("Select * From vUserInfo where DisplayName='" + name + "' and Email='" + email + "'", CommandType.Text, null);
                DataRow dr = ds.Tables[0].Rows[0];
                return Int64.Parse(dr["ID"].ToString());
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getUserID()", "ClsCalenderDataService.xaml.cs");
                return -1;
            }
        }

        public DataSet getMyConferences(Int64 userID)
        {
            try
            {
                return ExecuteDataSet("Select * From tblConferenceUsers where UserId='" + userID + "'", CommandType.Text, null);

            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getMyConferences()", "ClsCalenderDataService.xaml.cs");
                return null;
            }
        }

        public DataSet getConference(Int64 conferenceID, string date)
        {
            try
            {
                return ExecuteDataSet("select * from tblConference where (ID ='" + conferenceID + "') and  ((StartDateTime >= '" + date + " 12:00:00 AM' and StartDateTime <= '" + date + " 11:59:00 PM') or (EndDateTime >= '" + date + " 12:00:00 AM' and EndDateTime <= '" + date + " 12:59:00 PM'))", CommandType.Text, null);

            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getConference()", "ClsCalenderDataService.xaml.cs");
                return null;
            }
        }

        public string getUserInfo(Int64 userId)
        {
            try
            {
                DataTable dt = ExecuteDataSet("Select DisplayName From vUserInfo where ID='" + userId + "'", CommandType.Text, null).Tables[0];
                if (dt.Rows.Count != 0)
                {
                    DataRow dr = dt.Rows[0];
                    return dr["DisplayName"].ToString();
                }
                return null;
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getUserInfo()", "ClsCalenderDataService.xaml.cs");
                return null;
            }
        }

        public DataSet getConferenceGuests(Int64 conferenceID)
        {
            try
            {
                return ExecuteDataSet("select * from tblConferenceGuests where ConfId='" + conferenceID + "'", CommandType.Text, null);

            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getConferenceGuests()", "ClsCalenderDataService.xaml.cs");
                return null;
            }

        }

        public string getUserInfoEmail(Int64 userId)
        {
            try
            {
                DataTable dt = ExecuteDataSet("Select Email From vUserInfo where ID='" + userId + "'", CommandType.Text, null).Tables[0];
                if (dt.Rows.Count != 0)
                {
                    DataRow dr = dt.Rows[0];
                    return dr["Email"].ToString();
                }
                return null;
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getUserInfoEmail()", "ClsCalenderDataService.xaml.cs");
                return null;
            }
        }

        public DataSet getSMTPCredentials()
        {
            try
            {
                return ExecuteDataSet("select FieldName,FieldValue from Config", CommandType.Text, null);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getSMTPCredentials()", "ClsCalenderDataService.xaml.cs");
                return null;
            }

        }

        public Int64 addNewUser(string username, string userid, string password, string email)
        {
            try
            {
                DataRow dr = ExecuteDataSet("SELECT ISNULL( MAX( ID ) ,0 ) + 1 FROM UserInfo", CommandType.Text, null).Tables[0].Rows[0];
                Int64 id = Int64.Parse(dr[0].ToString());
                ExecuteDataSet("insert into UserInfo(ID,DisplayName,RoleID,FirstName,LastName,EMail,Password,IsActive,IsDeleted,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy) values('" + id + "','" + userid + "','2','" + username + "','','" + email + "','" + password + "','True','False','" + DateTime.Now.ToString() + "','1','" + DateTime.Now.ToString() + "','1')", CommandType.Text, null);

                return id;
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "addNewUser()", "ClsCalenderDataService.xaml.cs");
                return -1;

            }

        }

        public bool IsUserIDExist(string userid)
        {
            try
            {
                DataTable dt = ExecuteDataSet("select * from vUserInfo where DisplayName='" + userid + "'", CommandType.Text, null).Tables[0];

                if (dt.Rows.Count > 0)
                    return true;

                return false;
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "IsUserIDExist()", "ClsCalenderDataService.xaml.cs");
                return false;

            }

        }

        public DataSet getUserInfoForMail(Int64 userId)
        {
            try
            {
                return ExecuteDataSet("Select * From vUserInfo where ID='" + userId + "'", CommandType.Text, null);

            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getUserInfoForMail()", "ClsCalenderDataService.xaml.cs");
                return null;
            }
        }

        public DataSet getMeetingsBetween(Int64 hostID, DateTime startDateTime, DateTime endDateTime)
        {
            try
            {
                return ExecuteDataSet("select * from tblconference where ((StartDateTime between '" + startDateTime + "' and '" + endDateTime + "') or (EndDateTIme between '" + startDateTime + "' and '" + endDateTime + "')) and HostID='" + hostID + "'", CommandType.Text, null);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getMeetingsBetween()", "ClsCalenderDataService.xaml.cs");
                return null;
            }

        }

        // For TimeZone
        public DataSet getCountry_Timezone()
        {
            try
            {
                return ExecuteDataSet("select * from timezone;", CommandType.Text, null);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getCountry_Timezone()", "ClsCalenderDataService.xaml.cs");
                return null;
            }

        }


        //#####  29-01-2008 ##### END ---------------------------------

        public DataSet getConferenceGuest(Int64 conferenceID)
        {
            try
            {
                return ExecuteDataSet("Select * from tblConferenceGuests where ConfId='" + conferenceID.ToString() + "'", CommandType.Text, null);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getConferenceGuest()", "ClsCalenderDataService.xaml.cs");
                return null;
            }

        }

        public DataSet getConferenceReminder(Int64 conferenceID)
        {
            try
            {
                return ExecuteDataSet("Select * from tblReminder where ConferenceID='" + conferenceID.ToString() + "'", CommandType.Text, null);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getConferenceReminder()", "ClsCalenderDataService.xaml.cs");
                return null;
            }

        }


        public DataSet Calender_GetAll()
        {
            return ExecuteDataSet("Select * from vtblConference;", CommandType.Text, null);
            //return ExecuteDataSet("Select * from vtblReminder;", CommandType.Text, null);

        }

        public DataSet Calender_All()
        {
            return ExecuteDataSet("Select * from vtblReminder", CommandType.Text, null);
        }

        //public DataSet ScriptType_GetAll()
        //{
        //    return ExecuteDataSet("Select * from vScriptType;", CommandType.Text, null);

        //}

        public DataSet Calender_GetByID(int ID)
        {
            return ExecuteDataSet("spGCalender", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.BigInt, ID));
        }

        public int Calender_Save(ref Int64 ID, string ConfTitle, DateTime StartDateTime, DateTime EndDateTime, bool IsAllDay, string RepeatType, Int64 HostId, string ConferenceLocation, string ConferenceDetail, string ConferenceType, string strTimezone, string ConfPassword, DateTime CreatedDate, Int64 CreatedBy, DateTime ModifiedDate, Int64 ModifiedBy, string ReminderType, string ConfResponse, string TimeBeforeConf, string GuestName, string UserEmailID)
        {
            int RetID = 0;

            SqlCommand cmd1;

            ExecuteNonQuery(out cmd1, "spAEtblConference",

                CreateParameter("@pID", SqlDbType.BigInt, ID, ParameterDirection.Input),
                CreateParameter("@pConfTitle", SqlDbType.NVarChar, ConfTitle),
                CreateParameter("@pStartDateTime", SqlDbType.DateTime, StartDateTime),
                CreateParameter("@pEndDateTime", SqlDbType.DateTime, EndDateTime),
                CreateParameter("@pAllDay", SqlDbType.Bit, IsAllDay),
                CreateParameter("@pRepeatType", SqlDbType.NVarChar, RepeatType),
                //CreateParameter("@pRepeatType", SqlDbType.NVarChar, Repeat),
                CreateParameter("@pHostID", SqlDbType.BigInt, HostId),
                CreateParameter("@pConferenceLocation", SqlDbType.NVarChar, ConferenceLocation),
                CreateParameter("@pConferenceDetail", SqlDbType.NVarChar, ConferenceDetail),
                CreateParameter("@pConferenceType", SqlDbType.NVarChar, ConferenceType),
                // CreateParameter("@pCountry", SqlDbType.NVarChar,Country),
                CreateParameter("@pTimezone", SqlDbType.NVarChar, strTimezone),
                CreateParameter("@pConfPassword", SqlDbType.NVarChar, ConfPassword),
                //CreateParameter("@pIsDeleted", SqlDbType.Bit, IsDeleted),
                CreateParameter("@pCreatedBy", SqlDbType.BigInt, CreatedBy),
                CreateParameter("@pModifiedBy", SqlDbType.BigInt, ModifiedBy),
                CreateParameter("@pCreatedDate", SqlDbType.DateTime, CreatedDate),
                CreateParameter("@pModifiedDate", SqlDbType.DateTime, ModifiedDate),
                CreateParameter("@pReminderType", SqlDbType.NVarChar, ReminderType),
                CreateParameter("@pConfResponse", SqlDbType.NVarChar, ConfResponse),
                CreateParameter("@pReturnMaxId", SqlDbType.BigInt, ParameterDirection.Output));


            RetID = int.Parse(cmd1.Parameters["@pReturnMaxId"].Value.ToString());

            cmd1.Dispose();

            // return RetID;


            SqlCommand cmd2;

            ExecuteNonQuery(out cmd2, "spAEtblReminder",
                CreateParameter("@pID", SqlDbType.BigInt, ID, ParameterDirection.Input),
                CreateParameter("@pConferenceID", SqlDbType.BigInt, RetID, ParameterDirection.Input),
                CreateParameter("@pConferenceType", SqlDbType.NVarChar, ConferenceType),
                CreateParameter("@pReminderType", SqlDbType.NVarChar, ReminderType),
                CreateParameter("@pTimeBeforeConf", SqlDbType.NVarChar, TimeBeforeConf),

                CreateParameter("@pCreatedBy", SqlDbType.BigInt, CreatedBy),
                CreateParameter("@pModifiedBy", SqlDbType.BigInt, ModifiedBy),
                CreateParameter("@pCreatedDate", SqlDbType.DateTime, CreatedDate),
                CreateParameter("@pModifiedDate", SqlDbType.DateTime, ModifiedDate),
                CreateParameter("@pReturnMaxId", SqlDbType.BigInt, ParameterDirection.Output));

            // RetID = int.Parse(cmd2.Parameters["@pReturnMaxId"].Value.ToString());

            return RetID;
        }

        public int Calender_ConferenceGuestsSave(Int64 ConfId, string GuestName, string UserEmailID, DateTime CreatedDate, Int64 CreatedBy, DateTime ModifiedDate, Int64 ModifiedBy)
        {
            SqlCommand cmd3;
            Int64 ID = -1;

            ExecuteNonQuery(out cmd3, "sptblConferenceGuests",
            CreateParameter("@pID", SqlDbType.BigInt, ID, ParameterDirection.Input),
            CreateParameter("@pConfId", SqlDbType.BigInt, ConfId, ParameterDirection.Input),
            CreateParameter("@pUserName", SqlDbType.NVarChar, GuestName, ParameterDirection.Input),
            CreateParameter("@pEmail", SqlDbType.NVarChar, UserEmailID, ParameterDirection.Input),
            CreateParameter("@pCreatedDate", SqlDbType.DateTime, CreatedDate),
            CreateParameter("@pCreatedBy", SqlDbType.BigInt, CreatedBy),
            CreateParameter("@pModifiedDate", SqlDbType.DateTime, ModifiedDate),
            CreateParameter("@pModifiedBy", SqlDbType.BigInt, ModifiedBy));

            cmd3.Dispose();

            return 1;
        }

        public string Calender_Email(string Request, Int64 ConferenceId, string GuestName, string Email)
        {
            try
            {

                SqlCommand cmd;

                ExecuteNonQuery(out cmd, "spEmailSentNotSent",
                CreateParameter("@pRequest", SqlDbType.NVarChar, Request, ParameterDirection.Input),
                CreateParameter("@pConfId", SqlDbType.BigInt, ConferenceId, ParameterDirection.Input),
                CreateParameter("@pUserName", SqlDbType.NVarChar, GuestName, ParameterDirection.Input),
                CreateParameter("@pEmail", SqlDbType.NVarChar, Email, ParameterDirection.Input),
                CreateParameter("@pEmailStatus", SqlDbType.NVarChar, ParameterDirection.Output));

                return cmd.Parameters["@pEmailStatus"].Value.ToString();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Calender_Email()", "ClsCalenderDataService.xaml.cs");
                return null;
            }
        }




        public void Calender_Delete(int ID)
        {
            ExecuteNonQuery("spDCalender", CreateParameter("@pID", SqlDbType.BigInt, ID));
        }
    }

}