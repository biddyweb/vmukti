using System;
using System.Data;
using Calender.DataAccess;
using Calender.Common;
using System.Collections.Generic;
using Calender.Business.Service;
using VMuktiAPI;
using System.Text;

namespace Calender.Business
{
    public class ClsCalender : ClsBaseObject
    {
        public static StringBuilder sb1;
        //StringBuilder sb1 = CreateTressInfo();
        
        #region Fields
        //Table: tblConference
        private Int64 _ID = Calender.Common.ClsConstants.NullLong;
        private string _ConfTitle = Calender.Common.ClsConstants.NullString;
        private DateTime _StartDateTime = Calender.Common.ClsConstants.NullDateTime;
        private DateTime _EndDateTime = Calender.Common.ClsConstants.NullDateTime;
        private bool _IsAllDay = false;
        private string _RepeatType = Calender.Common.ClsConstants.NullString;
        private Int64 _HostId = Calender.Common.ClsConstants.NullLong;
        private string _ConferenceDetail = Calender.Common.ClsConstants.NullString;
        private string _ConferenceLocation = Calender.Common.ClsConstants.NullString;
        private string _ConferenceType = Calender.Common.ClsConstants.NullString;
        private string _ReminderType = Calender.Common.ClsConstants.NullString;
        private string _ConfPassword = Calender.Common.ClsConstants.NullString;
        private bool _IsDeleted = false;
        private DateTime _CreatedDate = Calender.Common.ClsConstants.NullDateTime;
        private Int64 _CreatedBy = Calender.Common.ClsConstants.NullLong;
        private DateTime _ModifiedDate = Calender.Common.ClsConstants.NullDateTime;
        private Int64 _ModifiedBy = Calender.Common.ClsConstants.NullLong;
        private Int64 _ConferenceID = Calender.Common.ClsConstants.NullLong;
        private string _ConfResponse = Calender.Common.ClsConstants.NullString;
        // private string _ConferencfeType = ClsConstants.NullString;
       
        //Table: tblConferenceGuests
        private string _GuestName = Calender.Common.ClsConstants.NullString;
        private string _Email = Calender.Common.ClsConstants.NullString;
        private string _EmailStatus = Calender.Common.ClsConstants.NullString;

        //private string _Country = Calender.Common.ClsConstants.NullString;
        private string _Timezone = Calender.Common.ClsConstants.NullString;

        //Table: tblReminder
        private string _TimeBeforeConf = Calender.Common.ClsConstants.NullString;
        #endregion 

        #region Properties

        public Int64 ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public string ConfTitle
        {
            get { return _ConfTitle; }
            set { _ConfTitle = value; }
        }

        public DateTime StartDateTime
        {
            get { return _StartDateTime; }
            set { _StartDateTime = value; }
        }

        public DateTime EndDateTime
        {
            get { return _EndDateTime; }
            set { _EndDateTime = value; }
        }

        public bool IsAllDay
        {
            get { return _IsAllDay; }
            set { _IsAllDay = value; }
        }

        public string RepeatType
        {
            get { return _RepeatType; }
            set { _RepeatType = value; }
        }

        public Int64 HostId
        {
            get { return _HostId; }
            set { _HostId = value; }
        }

        public string ConferenceDetail
        {
            get { return _ConferenceDetail; }
            set { _ConferenceDetail = value; }
        }

        public string ConferenceLocation
        {
            get { return _ConferenceLocation; }
            set { _ConferenceLocation = value; }
        }

        public string ConferenceType
        {
            get { return _ConferenceType; }
            set { _ConferenceType = value; }
        }

        public string ConfPassword
        {
            get { return _ConfPassword; }
            set { _ConfPassword = value; }
        }
 
        public bool IsDeleted
        {
            get { return _IsDeleted; }
            set { _IsDeleted = value; }
        }

        public DateTime CreatedDate
        {
            get { return _CreatedDate; }
            set { _CreatedDate = value; }
        }

        public Int64 CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }

        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { _ModifiedDate = value; }
        }

        public Int64 ModifiedBy
        {
            get { return _ModifiedBy; }
            set { _ModifiedBy = value; }
        }

        public Int64 ConferenceID
        {
            get { return _ConferenceID; }
            set { _ConferenceID = value; }
        }
        //public string ConferencfeType
        //{
        //    get { return _ConferencfeType; }
        //    set { _ConferencfeType = value; }
        //}

        public string ReminderType
        {
            get { return _ReminderType; }
            set { _ReminderType = value; }
        }

        public string ConfResponse
        {
            get { return _ConfResponse; }
            set { _ConfResponse = value; }
        }

        public string GuestName
        {
            get { return _GuestName; }
            set { _GuestName = value; }
        }
               

        public string Email
        {
            get { return _Email ; }
            set { _Email = value; }
        }

        public string EmailStatus
        {
            get { return _EmailStatus; }
            set { _EmailStatus = value; }
        }


        public string TimeBeforeConf
        {
            get { return _TimeBeforeConf; }
            set { _TimeBeforeConf = value; }
        }

        //public string Country
        //{
        //    get { return _Country; }
        //    set { _Country = value; }
        //}

        public string Timezone
        {
            get { return _Timezone; }
            set { _Timezone = value; }
        }

        #endregion 

        #region Methods

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

        public override bool MapData(DataRow row)
        {
            try
            {
                ID = GetInt(row, "ID");
                ConfTitle = GetString(row, "ConfTitle");
                StartDateTime = GetDateTime(row, "StartDateTime");
                EndDateTime = GetDateTime(row, "EndDateTime");
                IsAllDay = GetBool(row, "IsAllDay");
                RepeatType = GetString(row, "RepeatType");
                HostId = GetInt(row, "HostID");
                ConferenceLocation = GetString(row, "ConferenceLocation");
                ConferenceDetail = GetString(row, "ConferenceDetail");
                ConferenceType = GetString(row, "ConferenceType");
                ConfPassword = GetString(row, "ConfPassword");
                IsDeleted = GetBool(row, "IsDeleted");
                CreatedDate = GetDateTime(row, "CreatedDate");
                CreatedBy = GetInt(row, "CreatedBy");
                ModifiedDate = GetDateTime(row, "ModifiedDate");
                ModifiedBy = GetInt(row, "ModifiedBy");
                ReminderType = GetString(row, "ReminderType");

                GuestName = GetString(row, "GuestName");
                Email = GetString(row, "Email");
                EmailStatus = GetString(row, "EmailStatus");
                ConferenceID = GetInt(row, "ConferenceID");
                //ConferencfeType = GetString(row, "ConferencfeType");
                TimeBeforeConf = GetString(row, "TimeBeforeConf");

                //Country = GetString(row, "Country");
                Timezone = GetString(row, "Timezone");
                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "MapData()", "ClsCalender.xaml.cs");
                return false;
            }

        }

        public static ClsCalender GetByCtlCalenderID(int ID)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    ClsCalender obj = new ClsCalender();
                    DataSet ds = (clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("spGCalender").dsInfo);
                    if (!obj.MapData(ds)) obj = null;
                    return obj;
                }
                else
                {
                    ClsCalender obj = new ClsCalender();
                    DataSet ds = new Calender.DataAccess.ClsCalenderDataService().Calender_GetByID(ID);
                    if (!obj.MapData(ds)) obj = null;
                    return obj;
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "GetByCtlCalenderID()", "ClsCalender.xaml.cs");
                return null;
            }
        }
        //Delete the UserID(ID)
        public static void Delete(int ID)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("calander Module.");
                sb.AppendLine("Delete the UserID:" + ID.ToString());
                sb.Append(sb.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
                

                Delete(ID, null);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "Delete()", "ClsCalender.xaml.cs");
            }
        }
        // Delete the user id and transaction(ID, txn)
        public static void Delete(int ID, IDbTransaction txn)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    List<clsSqlParametersInfo> lstParamInfo = new List<clsSqlParametersInfo>();
                    clsSqlParameterContract clsDataContract = new clsSqlParameterContract();
                    
                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pID";
                    objInfo.PDBType = "BigInt";
                    objInfo.PValue = ID;
                    //objInfo.PSize = 200";
                   lstParamInfo.Add(objInfo);

                   clsDataContract.objParam = lstParamInfo;

                    clsMailDBClient.chHttpMailDBService. svcExecuteNonQuery("spDCalender",clsDataContract);

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("calander Module.");
                    sb.AppendLine("Delete the UserID:" + ID.ToString());
                    sb.AppendLine("Delete the transaction:" + txn.ToString());
                    sb.Append(sb.ToString());
                    VMuktiAPI.ClsLogging.WriteToTresslog(sb);

                }
                else
                {
                    new Calender.DataAccess.ClsCalenderDataService(txn).Calender_Delete(ID);
                }
                
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "Delete(int ID, IDbTransaction txn)", "ClsCalender.xaml.cs");
            }
        }

        //public void Delete()
        //{
        //    Delete(ID);
        //}

        //public void Delete(IDbTransaction txn)
        //{
        //    Delete(ID, txn);
        //}

        public int Save()
        {
            try
            {
                return (Save(null));
               
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "Save()", "ClsCalender.xaml.cs");
                return -1;
            }
        }
        
        public int Save(IDbTransaction txn)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    List<clsSqlParametersInfo> lstParamInfo = new List<clsSqlParametersInfo>();
                    clsSqlParameterContract clsDataContract = new clsSqlParameterContract();

                    clsSqlParametersInfo objPID = new clsSqlParametersInfo();
                    objPID.Direction = "Input";
                    objPID.PName = "@pID";
                    objPID.PDBType = "BigInt";
                    objPID.PValue = ID;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objPID);

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Output";
                    objInfo.PName = "@pReturnMaxId";
                    objInfo.PDBType = "BigInt";
                   // objInfo.PValue = ID;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objInfo);


                    clsSqlParametersInfo objInfo1 = new clsSqlParametersInfo();
                    objInfo1.Direction = "Input";
                    objInfo1.PName = "@pConfTitle";
                    objInfo1.PDBType = "NVarChar";
                    objInfo1.PValue = _ConfTitle;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objInfo1);

                    clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                    objInfo2.Direction = "Input";
                    objInfo2.PName = "@pStartDateTime";
                    objInfo2.PDBType = "DateTime";
                    objInfo2.PValue = _StartDateTime;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objInfo2);

                    clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                    objInfo3.Direction = "Input";
                    objInfo3.PName = "@pEndDateTime";
                    objInfo3.PDBType = "DateTime";
                    objInfo3.PValue = EndDateTime;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objInfo3);

                    clsSqlParametersInfo objInfo4 = new clsSqlParametersInfo();
                    objInfo4.Direction = "Input";
                    objInfo4.PName = "@pAllDay";
                    objInfo4.PDBType = "Bit";
                    objInfo4.PValue = IsAllDay;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objInfo4);

                    clsSqlParametersInfo objInfo5 = new clsSqlParametersInfo();
                    objInfo5.Direction = "Input";
                    objInfo5.PName = "@pRepeatType";
                    objInfo5.PDBType = "NVarChar";
                    objInfo5.PValue = RepeatType;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objInfo5);

                   clsSqlParametersInfo objInfo6 = new clsSqlParametersInfo();
                    objInfo6.Direction = "Input";
                    objInfo6.PName = "@pHostId";
                    objInfo6.PDBType = "BigInt";
                    objInfo6.PValue = HostId;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objInfo6);

                   clsSqlParametersInfo objInfo7 = new clsSqlParametersInfo();
                    objInfo7.Direction = "Input";
                    objInfo7.PName = "@pConferenceLocation";
                    objInfo7.PDBType = "NVarChar";
                    objInfo7.PValue = ConferenceLocation;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objInfo7);

                    clsSqlParametersInfo objInfo8 = new clsSqlParametersInfo();
                    objInfo8.Direction = "Input";
                    objInfo8.PName = "@pConferenceDetail";
                    objInfo8.PDBType = "NVarChar";
                    objInfo8.PValue = ConferenceDetail;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objInfo8);

                    clsSqlParametersInfo objInfo9 = new clsSqlParametersInfo();
                    objInfo9.Direction = "Input";
                    objInfo9.PName = "@pConferenceType";
                    objInfo9.PDBType = "NVarChar";
                    objInfo9.PValue = ConferenceType;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objInfo9);

                    clsSqlParametersInfo objInfo10 = new clsSqlParametersInfo();
                    objInfo10.Direction = "Input";
                    objInfo10.PName = "@pConfPassword";
                    objInfo10.PDBType = "NVarChar";
                    objInfo10.PValue = ConfPassword;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objInfo10);

                    //clsSqlParametersInfo objInfo11 = new clsSqlParametersInfo();
                    //objInfo11.Direction = "Input";
                    //objInfo11.PName = "@pCountry";
                    //objInfo11.PDBType = "NVarChar";
                    //objInfo11.PValue = Country;
                    ////objInfo.PSize = 200";
                    //lstParamInfo.Add(objInfo11);

                    clsSqlParametersInfo objInfo12 = new clsSqlParametersInfo();
                    objInfo12.Direction = "Input";
                    objInfo12.PName = "@pTimezone";
                    objInfo12.PDBType = "NVarChar";
                    objInfo12.PValue = Timezone;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objInfo12);



                    clsSqlParametersInfo objInfo13 = new clsSqlParametersInfo();
                    objInfo13.Direction = "Input";
                    objInfo13.PName = "@pCreatedDate";
                    objInfo13.PDBType = "DateTime";
                    objInfo13.PValue = DateTime.Now;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objInfo13);

                    clsSqlParametersInfo objInfo14 = new clsSqlParametersInfo();
                    objInfo14.Direction = "Input";
                    objInfo14.PName = "@pCreatedBy";
                    objInfo14.PDBType = "BigInt";
                    objInfo14.PValue = CreatedBy;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objInfo14);

                    clsSqlParametersInfo objInfo15 = new clsSqlParametersInfo();
                    objInfo15.Direction = "Input";
                    objInfo15.PName = "@pModifiedDate";
                    objInfo15.PDBType = "DateTime";
                    objInfo15.PValue = DateTime.Now;
                    //objInf.PSize = 200";
                    lstParamInfo.Add(objInfo15);

                    clsSqlParametersInfo objInfo16 = new clsSqlParametersInfo();
                    objInfo16.Direction = "Input";
                    objInfo16.PName = "@pModifiedBy";
                    objInfo16.PDBType = "BigInt";
                    objInfo16.PValue = ModifiedBy;
                    //objInf.PSize = 200";
                    lstParamInfo.Add(objInfo16);

                   

                    clsSqlParametersInfo objInfo17 = new clsSqlParametersInfo();
                    objInfo17.Direction = "Input";
                    objInfo17.PName = "@pReminderType";
                    objInfo17.PDBType = "NVarChar";
                    objInfo17.PValue = ReminderType;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objInfo17);

                    clsSqlParametersInfo objInfo18 = new clsSqlParametersInfo();
                    objInfo18.Direction = "Input";
                    objInfo18.PName = "@pConfResponse";
                    objInfo18.PDBType = "NVarChar";
                    objInfo18.PValue = ConfResponse;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objInfo18);

                    clsDataContract.objParam = lstParamInfo;

                    int NewConfID = clsMailDBClient.chHttpMailDBService.svcExecuteReturnNonQuery("spAEtblConference", clsDataContract);

                    //int NewConfID = int.Parse(objInfo.PValue.ToString());
                    lstParamInfo = new List<clsSqlParametersInfo>();
                    clsDataContract = new clsSqlParameterContract();

                    objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pID";
                    objInfo.PDBType = "BigInt";
                    objInfo.PValue = -1;
                    lstParamInfo.Add(objInfo);


                    objInfo1 = new clsSqlParametersInfo();
                    objInfo1.Direction = "Input";
                    objInfo1.PName = "@pConferenceID";
                    objInfo1.PDBType = "BigInt";
                    objInfo1.PValue = NewConfID;
                    lstParamInfo.Add(objInfo1);


                    objInfo2 = new clsSqlParametersInfo();
                    objInfo2.Direction = "Input";
                    objInfo2.PName = "@pConferenceType";
                    objInfo2.PDBType = "NVarChar";
                    objInfo2.PValue = ConferenceType;
                    lstParamInfo.Add(objInfo2);

                    objInfo3 = new clsSqlParametersInfo();
                    objInfo3.Direction = "Input";
                    objInfo3.PName = "@pReminderType";
                    objInfo3.PDBType = "NVarChar";
                    objInfo3.PValue = ReminderType;
                    lstParamInfo.Add(objInfo3);

                    objInfo4 = new clsSqlParametersInfo();
                    objInfo4.Direction = "Input";
                    objInfo4.PName = "@pTimeBeforeConf";
                    objInfo4.PDBType = "NVarChar";
                    objInfo4.PValue = TimeBeforeConf;
                    lstParamInfo.Add(objInfo4);

                    objInfo5 = new clsSqlParametersInfo();
                    objInfo5.Direction = "Input";
                    objInfo5.PName = "@pCreatedBy";
                    objInfo5.PDBType = "BigInt";
                    objInfo5.PValue = CreatedBy;
                    lstParamInfo.Add(objInfo5);

                    objInfo6 = new clsSqlParametersInfo();
                    objInfo6.Direction = "Input";
                    objInfo6.PName = "@pModifiedBy";
                    objInfo6.PDBType = "BigInt";
                    objInfo6.PValue = ModifiedBy;
                    lstParamInfo.Add(objInfo6);

                    objInfo8 = new clsSqlParametersInfo();
                    objInfo8.Direction = "Input";
                    objInfo8.PName = "@pCreatedDate";
                    objInfo8.PDBType = "DateTime";
                    objInfo8.PValue = DateTime.Now;
                    lstParamInfo.Add(objInfo8);

                    objInfo9 = new clsSqlParametersInfo();
                    objInfo9.Direction = "Input";
                    objInfo9.PName = "@pModifiedDate";
                    objInfo9.PDBType = "DateTime";
                    objInfo9.PValue = DateTime.Now;
                    lstParamInfo.Add(objInfo9);

                    objInfo10 = new clsSqlParametersInfo();
                    objInfo10.Direction = "Output";
                    objInfo10.PName = "@pReturnMaxId";
                    objInfo10.PDBType = "BigInt";
                    //objInfo10.PValue = ModifiedDate;
                    lstParamInfo.Add(objInfo10);

                    clsDataContract.objParam = lstParamInfo;

                    clsMailDBClient.chHttpMailDBService.svcExecuteReturnNonQuery("spAEtblReminder", clsDataContract);

                    return NewConfID;
                    //clsMailDBClient.chHttpMailDBService.svcExecuteReturnNonQuery
                              
                    
                    
                }
                else
                {

                    return (new Calender.DataAccess.ClsCalenderDataService(txn).Calender_Save(ref _ID, _ConfTitle, _StartDateTime, _EndDateTime, _IsAllDay, _RepeatType, _HostId, _ConferenceLocation, _ConferenceDetail, _ConferenceType,_Timezone, _ConfPassword, _CreatedDate, _CreatedBy, _ModifiedDate, _ModifiedBy, _ReminderType, _ConfResponse,_TimeBeforeConf,_GuestName,_Email));
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "Save(IDbTransaction txn)", "ClsCalender.xaml.cs");
                return -1;
            }
        }
        //Saving the guest id(ID)
        public int SaveGuest(Int64 id)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendLine("calander Module.");
                sb.AppendLine("Saving the guest:" + ID.ToString());
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);

                ID = id;
                return (SaveGuest(null));
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "SaveGuest()", "ClsCalender.xaml.cs");
                return -1;
            }
        }

        //Saving the guest transaction(txn)
        public int SaveGuest(IDbTransaction txn)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {

                    StringBuilder sb = new StringBuilder();
                    sb.AppendLine("calander Module.");
                    sb.AppendLine("Saving the guest transaction:");
                    sb.Append(sb1.ToString());
                    VMuktiAPI.ClsLogging.WriteToTresslog(sb);

                    List<clsSqlParametersInfo> lstParamInfo = new List<clsSqlParametersInfo>();
                    clsSqlParameterContract clsDataContract = new clsSqlParameterContract();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pID";
                    objInfo.PDBType = "BigInt";
                    objInfo.PValue = -1;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objInfo);

                    clsSqlParametersInfo objConfID = new clsSqlParametersInfo();
                    objConfID.Direction = "Input";
                    objConfID.PName = "@pConfId";
                    objConfID.PDBType = "BigInt";
                    objConfID.PValue = ID;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objConfID);

                    clsSqlParametersInfo objInfo1 = new clsSqlParametersInfo();
                    objInfo1.Direction = "Input";
                    objInfo1.PName = "@pUserName";
                    objInfo1.PDBType = "NVarChar";
                    objInfo1.PValue = GuestName;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objInfo1);

                    clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                    objInfo2.Direction = "Input";
                    objInfo2.PName = "@pEmail";
                    objInfo2.PDBType = "NVarChar";
                    objInfo2.PValue = Email;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objInfo2);

                    clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                    objInfo3.Direction = "Input";
                    objInfo3.PName = "@pCreatedDate";
                    objInfo3.PDBType = "DateTime";
                    objInfo3.PValue = DateTime.Now;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objInfo3);

                    clsSqlParametersInfo objInfo4 = new clsSqlParametersInfo();
                    objInfo4.Direction = "Input";
                    objInfo4.PName = "@pCreatedBy";
                    objInfo4.PDBType = "BigInt";
                    objInfo4.PValue = CreatedBy;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objInfo4);

                    clsSqlParametersInfo objInfo5 = new clsSqlParametersInfo();
                    objInfo5.Direction = "Input";
                    objInfo5.PName = "@pModifiedDate";
                    objInfo5.PDBType = "DateTime";
                    objInfo5.PValue = DateTime.Now;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objInfo5);

                    clsSqlParametersInfo objInfo6 = new clsSqlParametersInfo();
                    objInfo6.Direction = "Input";
                    objInfo6.PName = "@pModifiedBy";
                    objInfo6.PDBType = "BigInt";
                    objInfo6.PValue = ModifiedBy;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objInfo6);

                    clsDataContract.objParam = lstParamInfo;

                   return clsMailDBClient.chHttpMailDBService.svcExecuteReturnNonQuery("sptblConferenceGuests", clsDataContract);



                }
                else
                {

                    return (new Calender.DataAccess.ClsCalenderDataService(txn).Calender_ConferenceGuestsSave(_ID, _GuestName, _Email, _CreatedDate, _CreatedBy, _ModifiedDate, _ModifiedBy));
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "SaveGuest()", "ClsCalender.xaml.cs");
                return -1;
            }
        }
        // Sending the Email notification
        public string getEmailStatus()
        {
            try
            {
                return (getEmailStatus(null));
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getEmailStatus()", "ClsCalender.xaml.cs");
                return string.Empty;
            }
        }

        // Sending the Email notification not sent(txn)
        public string getEmailStatus(IDbTransaction txn)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    List<clsSqlParametersInfo> lstParamInfo = new List<clsSqlParametersInfo>();
                    clsSqlParameterContract clsDataContract = new clsSqlParameterContract();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Output";
                    objInfo.PName = "@pEmailStatus";
                    objInfo.PDBType = "NVarChar";
                    objInfo.PValue = _ID;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objInfo);

                    clsSqlParametersInfo objInfo1 = new clsSqlParametersInfo();
                    objInfo1.Direction = "Input";
                    objInfo1.PName = "@pConfId";
                    objInfo1.PDBType = "BigInt";
                    objInfo1.PValue = _ConferenceID;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objInfo1);

                    clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                    objInfo2.Direction = "Input";
                    objInfo2.PName = "@pUserName";
                    objInfo2.PDBType = "NVarChar";
                    objInfo2.PValue = _GuestName;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objInfo2);

                    clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                    objInfo3.Direction = "Input";
                    objInfo3.PName = "@pEmail";
                    objInfo3.PDBType = "NVarChar";
                    objInfo3.PValue = _Email;
                    //objInfo.PSize = 200";
                    lstParamInfo.Add(objInfo3);

                    clsDataContract.objParam = lstParamInfo;

                   clsMailDBClient.chHttpMailDBService.svcExecuteNonQuery("spEmailSentNotSent", clsDataContract);

                   return (string)(objInfo.PValue);

                }
                else
                {

                    return (new Calender.DataAccess.ClsCalenderDataService(txn).Calender_Email(_EmailStatus, _ConferenceID, _GuestName, _Email));
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getEmailStatus()", "ClsCalender.xaml.cs");
                return string.Empty;
            }
        } 

        //Getting UserInformation
        public static DataSet getAllUsersInfo()
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    return (clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("Select ID,DisplayName,Email From vUserInfo").dsInfo);
            
                }   
                else
                {
                    return (new Calender.DataAccess.ClsCalenderDataService().getAllUserInfo());
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getAllUsersInfo()", "ClsCalender.xaml.cs");
                return null;
            }
        }

        //Getting the Conference Details( conferenceID)
        public static DataSet getConferenceDetails(Int64 conferenceID)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    return (clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("Select * from tblConference where ID='" + conferenceID.ToString() + "'").dsInfo);

                }
                else
                {
                    return (new Calender.DataAccess.ClsCalenderDataService().getConferenceDetails(conferenceID));
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getConferenceDetails()", "ClsCalender.xaml.cs");
                return null;
            }
        }

        // Diaplay Conference Guest(conferenceID)
        public static DataSet getConferenceGuests(Int64 conferenceID)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    return (clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("select * from tblConferenceGuests where ConfId='" + conferenceID + "'").dsInfo);
                }
                else
                {
                    return (new Calender.DataAccess.ClsCalenderDataService().getConferenceGuests(conferenceID));
                }
            }
                
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getConferenceGuests()", "ClsCalender.xaml.cs");
                return null;
            }
            
        }
        //Displaying type of Reminder(conference ID)
        public static DataSet getConferenceReminder(Int64 conferenceID)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    return (clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("Select * from tblReminder where ConferenceID='" + conferenceID.ToString()).dsInfo);
                }
                else
                {
                    return (new Calender.DataAccess.ClsCalenderDataService().getConferenceReminder(conferenceID));
                }
            }
            catch(Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getConferenceReminder()", "ClsCalender.xaml.cs");
                return null;
            }
            
        }

        // Information regarding all the conferences(userID, date)
        public static DataSet getAllConferences(Int64 userID, string date)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    return (clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("select * from tblConference where (CreatedBy ='" + userID + "' or HostID='" + userID + "') and  ((StartDateTime >= '" + date + " 12:00:00 AM' and StartDateTime <= '" + date + " 11:59:00 PM') or (EndDateTime >= '" + date + " 12:00:00 AM' and EndDateTime <= '" + date + " 12:59:00 PM'))").dsInfo);
                }
                else
                {
                    return (new Calender.DataAccess.ClsCalenderDataService().getAllConferences(userID, date));
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getAllConferences()", "ClsCalender.xaml.cs");
                return null;
            }
            
        }

//#####  29-01-2008 ##### START
        // For dealiting a conference(conferenceID)
        public static bool deleteConference(Int64 conferenceID)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("delete from tblConference where ID='" + conferenceID + "'");
                    return true;
                }
                else
                {
                    return (new Calender.DataAccess.ClsCalenderDataService().deleteConference(conferenceID));
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "deleteConference()", "ClsCalender.xaml.cs");
                return false;
            }
        }

        // Displaying conference Guest(name, email)
        public static DataSet getConferenceGuest(string name, string email)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    return (clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("select * from tblConferenceGuests where GuestName='" + name + "' and Email='" + email + "'").dsInfo);
                }
                else
                {
                    return (new Calender.DataAccess.ClsCalenderDataService().getConferenceGuest(name, email));
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getConferenceGuest()", "ClsCalender.xaml.cs");
                return null;
            }
            
        }
        // Email notification for gonference guests(name, email, status)
        public static void setEmailStatusOfConferenceGuest(string name, string email, string status)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("update tblConferenceGuests set EmailStatus='" + status + "' where GuestName='" + name + "' and Email='" + email + "'");
                }
                else
                {
                    new Calender.DataAccess.ClsCalenderDataService().setEmailStatusOfConferenceGuest(name, email, status);
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "setEmailStatusOfConferenceGuest()", "ClsCalender.xaml.cs");
            }
        }
        // Removing the invited Guests(name, email)
        public static bool deleteUnInvitedConferenceGuest(string name, string email)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    DataSet ds = clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("select * from tblConferenceGuests where GuestName='" + name + "' and Email='" + email + "'").dsInfo;
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("delete from tblConferenceGuests where GuestName='" + name + "' and Email='" + email + "'");
                        return true;
                    }
                    return false;
                }
                else
                {
                    return (new Calender.DataAccess.ClsCalenderDataService().deleteUnInvitedConferenceGuest(name, email));
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "deleteUnInvitedConferenceGuest()", "ClsCalender.xaml.cs");
                return false;
            }
        }
        //Adding the conference Users(conferenceID, guestID)
        public static void addConferenceUsers(Int64 conferenceID, Int64 guestID)
        {
             try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("insert into tblConferenceUsers values('" + conferenceID + "','" + guestID + "','NoResponse')");
                }
                else
                {
                    new Calender.DataAccess.ClsCalenderDataService().addConferenceUsers(conferenceID, guestID);
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "addConferenceUsers()", "ClsCalender.xaml.cs");
            }
        }

        public static void deleteConferenceUsers(Int64 conferenceID, Int64 guestID)
        {
            try
            {

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("delete from tblConferenceUsers where ConferenceID='" + conferenceID + "' and UserId='" + guestID + "'");
                }
                else
                {
                    new Calender.DataAccess.ClsCalenderDataService().deleteConferenceUsers(conferenceID, guestID);
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "deleteConferenceUsers()", "ClsCalender.xaml.cs");
            }
        }

        //Getting the userID(name, email)
        public static Int64 getUserID(string name, string email)
        {
            try
            {

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    DataSet ds = clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("Select * From vUserInfo where DisplayName='" + name + "' and Email='" + email + "'").dsInfo;
                    DataRow dr = ds.Tables[0].Rows[0];
                    return Int64.Parse(dr["ID"].ToString());
                }
                else
                {
                    return (new Calender.DataAccess.ClsCalenderDataService().getUserID(name, email));
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getUserID()", "ClsCalender.xaml.cs");
                return -1;
            }
        }
        //Getting the conference details(userID)
        public static DataSet getMyConferences(Int64 userID)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    return (clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("Select * From tblConferenceUsers where UserId='" + userID + "'").dsInfo);
                }
                else
                {
                    return (new Calender.DataAccess.ClsCalenderDataService().getMyConferences(userID));
                }
            }
             catch (Exception exp)
             {
                 VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getMyConferences()", "ClsCalender.xaml.cs");
                 return null;
             }
        }
        // Getting conference infomartion(conferenceID, date)
        public static DataSet getConference(Int64 conferenceID, string date)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    return (clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("select * from tblConference where (ID ='" + conferenceID + "') and  ((StartDateTime >= '" + date + " 12:00:00 AM' and StartDateTime <= '" + date + " 11:59:00 PM') or (EndDateTime >= '" + date + " 12:00:00 AM' and EndDateTime <= '" + date + " 12:59:00 PM'))").dsInfo);
                }
                else
                {
                    return (new Calender.DataAccess.ClsCalenderDataService().getConference(conferenceID, date));
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getConference()", "ClsCalender.xaml.cs");
                return null;
            }

        }

        public static void updateConferenceUsers(Int64 conferenceID, Int64 guestID, string attendence)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("update tblConferenceUsers set Attendence='" + attendence + "' where ConferenceID='" + conferenceID + "' and UserId='" + guestID + "'");
                }
                else
                {
                    new Calender.DataAccess.ClsCalenderDataService().updateConferenceUsers(conferenceID, guestID, attendence);
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "updateConferenceUsers()", "ClsCalender.xaml.cs");
            }
        }
        // Conference user attending information(conferenceID, userID)
        public static string getConferenceUserMyAttendence(Int64 conferenceID, Int64 userID)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    DataTable dt = clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("select * from tblConferenceUsers where ConferenceID='" + conferenceID + "' and UserId='" + userID + "'").dsInfo.Tables[0];
                    if (dt.Rows.Count != 0)
                    {
                        DataRow dr = dt.Rows[0];
                        return dr["Attendence"].ToString();
                    }
                    return null;
                }
                else
                {
                    return (new Calender.DataAccess.ClsCalenderDataService().getConferenceUserMyAttendence(conferenceID, userID));
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getConferenceUserMyAttendence()", "ClsCalender.xaml.cs");
                return null;
            }

        }

        public static string getUserInfo(Int64 userId)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    DataTable dt = clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("Select DisplayName From vUserInfo where ID='" + userId + "'").dsInfo.Tables[0];
                    if (dt.Rows.Count != 0)
                    {
                        DataRow dr = dt.Rows[0];
                        return dr["DisplayName"].ToString();
                    }
                    return null;
                }
                return (new Calender.DataAccess.ClsCalenderDataService().getUserInfo(userId));
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getUserInfo()", "ClsCalender.xaml.cs");
                return null;
            }
        }

        public static DataSet getConferenceUsers(Int64 conferenceID)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    return (clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("select * from tblConferenceUsers where ConferenceID='" + conferenceID + "'").dsInfo);
                }
                else
                {
                    return (new Calender.DataAccess.ClsCalenderDataService().getConferenceUsers(conferenceID));
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getConferenceUsers()", "ClsCalender.xaml.cs");
                return null;
            }
        }

        // Deleting cnference users(conferenceID)
        public static void deleteConferenceUsers(Int64 conferenceID)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("delete from tblConferenceUsers where ConferenceID='" + conferenceID + "'");
                }
                else
                {
                    new Calender.DataAccess.ClsCalenderDataService().deleteConferenceUsers(conferenceID);
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "deleteConferenceUsers()", "ClsCalender.xaml.cs"); 
            }
        }

        public static DataSet getConferenceGuest(Int64 conferenceID)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    return (clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("select * from tblConferenceGuests where ConfId='" + conferenceID + "'").dsInfo);
                }
                else
                {
                    return (new Calender.DataAccess.ClsCalenderDataService().getConferenceGuests(conferenceID));
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getConferenceGuest()", "ClsCalender.xaml.cs");
                return null;
            }
        }

        public static string getUserInfoEmail(Int64 userId)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    DataTable dt = clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("Select Email From vUserInfo where ID='" + userId + "'").dsInfo.Tables[0];
                    if (dt.Rows.Count != 0)
                    {
                        DataRow dr = dt.Rows[0];
                        return dr["Email"].ToString();
                    }
                    return null;
                }
                else
                {
                    return (new Calender.DataAccess.ClsCalenderDataService().getUserInfoEmail(userId));
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getUserInfoEmail()", "ClsCalender.xaml.cs");
                return null;
            }
        }
        // Gettng the information for SMTP credentails
        public static DataSet getSMTPCredentials()
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    return (clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("select FieldName,FieldValue from Config").dsInfo);
                }
                else
                {
                    return (new Calender.DataAccess.ClsCalenderDataService().getSMTPCredentials());
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getSMTPCredentials()", "ClsCalender.xaml.cs");
                return null;
            }
        }
        //Add new user(username, userid, password, email)
        public static Int64 addNewUser(string username, string userid, string password, string email)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {

                    DataRow dr = clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("SELECT ISNULL( MAX( ID ) ,0 ) + 1 FROM UserInfo").dsInfo.Tables[0].Rows[0];
                    Int64 id = Int64.Parse(dr[0].ToString());
                    clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("insert into UserInfo(ID,DisplayName,RoleID,FirstName,LastName,EMail,Password,IsActive,IsDeleted,CreatedDate,CreatedBy,ModifiedDate,ModifiedBy) values('" + id + "','" + userid + "','2','" + username + "','','" + email + "','" + password + "','True','False','" + DateTime.Now.ToString() + "','1','" + DateTime.Now.ToString() + "','1')");
                    return id;               
                }
                else
                {
                    return (new Calender.DataAccess.ClsCalenderDataService().addNewUser(username, userid, password, email));
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "addNewUser()", "ClsCalender.xaml.cs");
                return -1;
            }
        }
        // Checking for existing of userID(userID)
        public static bool IsUserIDExist(string userid)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    DataTable dt = clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("select * from vUserInfo where DisplayName='" + userid + "'").dsInfo.Tables[0];

                    if (dt.Rows.Count > 0)
                        return true;

                    return false;
                }
                else
                {
                    return (new Calender.DataAccess.ClsCalenderDataService().IsUserIDExist(userid));
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "IsUserIDExist()", "ClsCalender.xaml.cs");
                return false;
            }
        }

        public static DataSet getUserInfoForMail(Int64 userId)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    return (clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("Select * From vUserInfo where ID='" + userId + "'").dsInfo);
                   // return Int64.Parse(dr["ConfID"].ToString());
                }
                else
                {
                    return (new Calender.DataAccess.ClsCalenderDataService().getUserInfoForMail(userId));
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getUserInfoForMail()", "ClsCalender.xaml.cs");
                return null;
            }
        }

        // Meeting timings(hostID, startDateTime, endDateTime)
        public static DataSet getMeetingsBetween(Int64 hostID, DateTime startDateTime, DateTime endDateTime)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    return (clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("select * from tblconference where ((StartDateTime between '" + startDateTime + "' and '" + endDateTime + "') or (EndDateTIme between '" + startDateTime + "' and '" + endDateTime + "')) and HostID='" + hostID + "'").dsInfo);
                }
                else
                {
                    return (new Calender.DataAccess.ClsCalenderDataService().getMeetingsBetween(hostID, startDateTime, endDateTime));
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getMeetingsBetween()", "ClsCalender.xaml.cs");
                return null;
            }
        }

        public static DataSet getCountry_Timezone()
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    return (clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("select * from timezone").dsInfo);
                }
                else
                {
                    return (new Calender.DataAccess.ClsCalenderDataService().getCountry_Timezone());
                }
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "getCountry_Timezone()", "ClsCalender.xaml.cs");
                return null;
            }
        }

 //#########  29-01-2008 ##### END ---------------------------------

    #endregion


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
    }
}
