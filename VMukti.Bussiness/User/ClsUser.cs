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
using VMukti.DataAccess;
using VMukti.Common;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using VMukti.Business.CommonDataContracts;
using VMuktiAPI;
using System.Text;

namespace VMukti.Business
{
    public class ClsUser : ClsBaseObject
    {
        #region Fields        
    
        private int _ID = VMukti.Common.ClsConstants.NullInt;
        private string _DisplayName = VMukti.Common.ClsConstants.NullString;
        private int _RoleID = VMukti.Common.ClsConstants.NullInt;
        private string _FName = VMukti.Common.ClsConstants.NullString;
        private string _LName = VMukti.Common.ClsConstants.NullString;
        private string _EMail = VMukti.Common.ClsConstants.NullString;
        private string _PassWord = VMukti.Common.ClsConstants.NullString;
        private bool _IsActive;
        private DateTime _CreatedDate = VMukti.Common.ClsConstants.NullDateTime;
        private int _CreatedBy = VMukti.Common.ClsConstants.NullInt;
        private DateTime _ModifiedDate = VMukti.Common.ClsConstants.NullDateTime;
        private int _ModifiedBy = VMukti.Common.ClsConstants.NullInt;

        private float _RatePerHour = VMukti.Common.ClsConstants.NullFloat;
        private float _OverTimeRate = VMukti.Common.ClsConstants.NullFloat;
        private float _DoubleRatePerHour = VMukti.Common.ClsConstants.NullFloat;
        private float _DoubleOverTimeRate = VMukti.Common.ClsConstants.NullFloat;


        private long _CampaignID = VMukti.Common.ClsConstants.NullLong;
        private long _GroupID = VMukti.Common.ClsConstants.NullLong;
        private long _ActivityID = VMukti.Common.ClsConstants.NullLong;

        private string _CampaignName = VMukti.Common.ClsConstants.NullString;
        private string _GroupName = VMukti.Common.ClsConstants.NullString;
        private string _RoleName = VMukti.Common.ClsConstants.NullString;

        private DateTime _StartTime = VMukti.Common.ClsConstants.NullDateTime;
        private DateTime _EndTime = VMukti.Common.ClsConstants.NullDateTime;

        private long _ScriptID = VMukti.Common.ClsConstants.NullLong;

        #endregion 

        #region Properties

        public long CampaignID
        {
            get { return _CampaignID; }
            set { _CampaignID = value; }
        }

        public long GroupID
        {
            get { return _GroupID; }
            set { _GroupID = value; }
        }

        public long ActivityID
        {
            get { return _ActivityID; }
            set { _ActivityID = value; }
        }

        public DateTime StartTime
        {
            get
            {
                return _StartTime;
            }
            set
            {
                _StartTime = value;
            }
        }

        public DateTime EndTime
        {
            get
            {
                return _EndTime;
            }
            set
            {
                _EndTime = value;
            }
        }

        public string CampaignName
        {
            get { return _CampaignName; }
            set { _CampaignName = value; }
        }

        public string GroupName
        {
            get { return _GroupName; }
            set { _GroupName = value; }
        }
        public string RoleName
        {
            get { return _RoleName; }
            set { _RoleName = value; }
        }

        public int ID
        {
            get{return _ID;}
            set{_ID = value;}
        }

        public string DisplayName
        {
            get { return _DisplayName; }
            set { _DisplayName = value;}
        }

        public int RoleID
        {
            get { return _RoleID; }
            set { _RoleID = value; }
        }

        public string FName
        {
            get { return _FName; }
            set { _FName = value; }
        }

        public string LName
        {
            get { return _LName; }
            set { _LName = value; }
        }

        public string EMail
        {
            get { return _EMail; }
            set { _EMail = value; }
        }

        public string PassWord
        {
            get { return _PassWord; }
            set { _PassWord = value; }
        }

        public bool IsActive
        {
            get { return _IsActive; }
            set { _IsActive = value; }
        }

        public DateTime CreatedDate
        {
            get { return _CreatedDate;}
            set { _CreatedDate = value; }
        }

        public int CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }

        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { _ModifiedDate = value; }
        }

        public int ModifiedBy
        {
            get { return _ModifiedBy; }
            set { _ModifiedBy = value; }
        }

        public float RatePerHour
        {
            get { return _RatePerHour; }
            set { _RatePerHour = value; }
        }

        public float OverTimeRate
        {
            get { return _OverTimeRate; }
            set { _OverTimeRate = value; }
        }

        public float DoubleRatePerHour
        {
            get { return _DoubleRatePerHour; }
            set { _DoubleRatePerHour = value; }
        }

        public float DoubleOverTimeRate
        {
            get { return _DoubleOverTimeRate; }
            set { _DoubleOverTimeRate = value; }
        }

        public long ScriptID
        {
            get { return _ScriptID; }
            set { _ScriptID = value; }
        }

        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {
                ID = GetInt(row, "ID");
                DisplayName = GetString(row, "DisplayName");
                RoleID = GetInt(row, "RoleID");
                FName = GetString(row, "FirstName");
                LName = GetString(row, "LastName");
                EMail = GetString(row, "EMail");
                PassWord = GetString(row, "Password");
                IsActive = GetBool(row, "IsActive");
                CreatedDate = GetDateTime(row, "CreatedDate");
                CreatedBy = GetInt(row, "CreatedBy");
                ModifiedDate = GetDateTime(row, "ModifiedDate");
                ModifiedBy = GetInt(row, "ModifiedBy");

                RatePerHour = GetFloat(row, "RatePerHour");
                OverTimeRate = GetFloat(row, "OverTimeRate");
                DoubleRatePerHour = GetFloat(row, "DoubleRatePerHour");
                DoubleOverTimeRate = GetFloat(row, "DoubleOverTimeRate");

                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapData()", "clsUser.cs"); 
                return false;
            }
        }

        public static ClsUser GetByGroupID(int ID)
        {
            try
            {
                ClsUser obj = new ClsUser();

                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pID";
                    objInfo.PValue = ID;
                    objInfo.PDBType = "Int";
                    objInfo.PSize = 200;

                    lstSP.Add(objInfo);

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;
                    DataSet ds;
                    try
                    {
                        ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGUserInfoPayroll", CSqlInfo).dsInfo;
                    }
                    catch (System.ServiceModel.EndpointNotFoundException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "GetByGroupID(int ID)", "ClsUser.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGUserInfoPayroll", CSqlInfo).dsInfo;
                    }
                    catch (System.ServiceModel.CommunicationException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "GetByGroupID(int ID)", "ClsUser.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGUserInfoPayroll", CSqlInfo).dsInfo;
                    }

                    if (!obj.MapData(ds.Tables[0])) obj = null;
                }
                else
                {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pID";
                    objInfo.PValue = ID;
                    objInfo.PDBType = "Int";
                    objInfo.PSize = 200;

                    lstSP.Add(objInfo);

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;
                    DataSet ds;
                    try
                    {
                        ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGUserInfoPayroll", CSqlInfo).dsInfo;
                    }
                    catch (System.ServiceModel.EndpointNotFoundException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "GetByGroupID(int ID)", "ClsUser.cs");                       
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGUserInfoPayroll", CSqlInfo).dsInfo;
                    }
                    catch (System.ServiceModel.CommunicationException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "GetByGroupID(int ID)", "ClsUser.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGUserInfoPayroll", CSqlInfo).dsInfo;
                    }
                   
                    if (!obj.MapData(ds.Tables[0])) obj = null;
                }
                else
                {
                    DataSet ds = new VMukti.DataAccess.ClsUserDataService().User_GetByID(ID);

                    if (!obj.MapData(ds.Tables[0])) obj = null;
                }
                }
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetByGroupId()", "clsUser.cs");  
                return null;
            }
        }

        public void MapData4Campaign(DataRow row)
        {
            try
            {
                CampaignID = GetLong(row, "CampaignID");
                GroupID = GetLong(row, "GroupID");
                CampaignName = GetString(row, "CampaignName");
                GroupName = GetString(row, "GroupName");
                RoleName = GetString(row, "RoleName");
                ScriptID = GetLong(row, "ScriptID");
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetByGroupId()", "clsUser.cs");
            }
        }

        public static ClsUser GetByUNamePass(string UName, string Password, ref bool Result)
        {
            try
            {
                ClsUser obj = new ClsUser();

                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pID";
                    objInfo.PValue = -1;
                    objInfo.PDBType = "Int";
                    objInfo.PSize = 200;

                    clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                    objInfo2.Direction = "Input";
                    objInfo2.PName = "@pUName";
                    objInfo2.PValue = UName;
                    objInfo2.PDBType = "NVarChar";
                    objInfo2.PSize = 200;

                    clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                    objInfo3.Direction = "Input";
                    objInfo3.PName = "@pPass";
                    objInfo3.PValue = Password;
                    objInfo3.PDBType = "NVarChar";
                    objInfo3.PSize = 200;

                    clsSqlParametersInfo objInfo4 = new clsSqlParametersInfo();
                    objInfo4.Direction = "InputOutput";
                    objInfo4.PName = "@pResult";
                    objInfo4.PValue = false;
                    objInfo4.PDBType = "Bit";
                    objInfo4.PSize = 200;


                    lstSP.Add(objInfo);
                    lstSP.Add(objInfo2);
                    lstSP.Add(objInfo3);
                    lstSP.Add(objInfo4);

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;

                    DataSet ds;
                    try
                    {
                        ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGUserInfoPayroll", CSqlInfo).dsInfo;
                    }
                    catch (System.ServiceModel.EndpointNotFoundException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "GetByUNamePass(string UName, string Password, ref bool Result)", "clsUser.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGUserInfoPayroll", CSqlInfo).dsInfo;
                    }
                    catch (System.ServiceModel.CommunicationException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "GetByUNamePass(string UName, string Password, ref bool Result)", "clsUser.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGUserInfoPayroll", CSqlInfo).dsInfo;
                    }

                    if (!obj.MapData(ds.Tables[0])) obj = null;

                    if (obj != null && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        obj.MapData4Campaign(ds.Tables[1].Rows[0]);
                    }
                }
                else
                {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {                  
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pID";
                    objInfo.PValue = -1;
                    objInfo.PDBType = "Int";
                    objInfo.PSize = 200;

                    clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                    objInfo2.Direction = "Input";
                    objInfo2.PName = "@pUName";
                    objInfo2.PValue = UName;
                    objInfo2.PDBType = "NVarChar";
                    objInfo2.PSize = 200;

                    clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                    objInfo3.Direction = "Input";
                    objInfo3.PName = "@pPass";
                    objInfo3.PValue = Password;
                    objInfo3.PDBType = "NVarChar";
                    objInfo3.PSize = 200;

                    clsSqlParametersInfo objInfo4 = new clsSqlParametersInfo();
                    objInfo4.Direction = "InputOutput";
                    objInfo4.PName = "@pResult";
                    objInfo4.PValue = false;
                    objInfo4.PDBType = "Bit";
                    objInfo4.PSize = 200;                                                        
                    

                    lstSP.Add(objInfo);
                    lstSP.Add(objInfo2);
                    lstSP.Add(objInfo3);
                    lstSP.Add(objInfo4);
                    
                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;

                    DataSet ds;
                    try
                    {
                        ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGUserInfoPayroll", CSqlInfo).dsInfo;
                    }
                    catch (System.ServiceModel.EndpointNotFoundException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "GetByUNamePass(string UName, string Password, ref bool Result)", "clsUser.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGUserInfoPayroll", CSqlInfo).dsInfo;
                    }
                    catch (System.ServiceModel.CommunicationException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "GetByUNamePass(string UName, string Password, ref bool Result)", "clsUser.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGUserInfoPayroll", CSqlInfo).dsInfo;
                    }

                    if (!obj.MapData(ds.Tables[0])) obj = null;

                    if (obj != null && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        obj.MapData4Campaign(ds.Tables[1].Rows[0]);
                    }

                }
                else
                {
                    DataSet ds = new VMukti.DataAccess.ClsUserDataService().User_GetByUNamePass(UName, Password, ref Result);

                    if (!obj.MapData(ds.Tables[0])) obj = null;

                    if (obj != null && ds.Tables[1] != null && ds.Tables[1].Rows.Count > 0)
                    {
                        obj.MapData4Campaign(ds.Tables[1].Rows[0]);
                    }
                }
                }
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetByUNamePass()", "clsUser.cs");
                return null;
            }
        }

        public static void Logout(Int64 UserID)
        {
            try
            {               

                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pID";
                    objInfo.PValue = -1;
                    objInfo.PDBType = "BigInt";
                    objInfo.PSize = 200;

                    lstSP.Add(objInfo);


                    clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                    objInfo2.Direction = "Input";
                    objInfo2.PName = "@pUserID";
                    objInfo2.PValue = UserID;
                    objInfo2.PDBType = "BigInt";
                    objInfo2.PSize = 200;

                    lstSP.Add(objInfo2);


                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;

                    try
                    {
                        VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDOnlineUsers", CSqlInfo);
                    }
                    catch (System.ServiceModel.EndpointNotFoundException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "Logout(Int64 UserID)", "ClsUser.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDOnlineUsers", CSqlInfo);
                    }
                    catch (System.ServiceModel.CommunicationException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "Logout(Int64 UserID)", "ClsUser.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDOnlineUsers", CSqlInfo);
                    }
                }
                else
                {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {

                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pID";
                    objInfo.PValue = -1;
                    objInfo.PDBType = "BigInt";
                    objInfo.PSize = 200;

                    lstSP.Add(objInfo);

                   
                    clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                    objInfo2.Direction = "Input";
                    objInfo2.PName = "@pUserID";
                    objInfo2.PValue = UserID;
                    objInfo2.PDBType = "BigInt";
                    objInfo2.PSize = 200;

                    lstSP.Add(objInfo2);


                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;

                    try
                    {
                        VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDOnlineUsers", CSqlInfo);
                    }
                    catch (System.ServiceModel.EndpointNotFoundException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "Logout(Int64 UserID)", "ClsUser.cs");                      
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDOnlineUsers", CSqlInfo);
                    }
                    catch (System.ServiceModel.CommunicationException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "Logout(Int64 UserID)", "ClsUser.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDOnlineUsers", CSqlInfo);
                    }  
                }
                else
                {

                    new VMukti.DataAccess.ClsUserDataService().User_LogOff(UserID);
                }
            }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "LogOut()", "clsUser.cs");
            }
        }

        public static void Delete(int ID)
        {
            try
            {
                Delete(ID, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Delete(int)", "clsUser.cs");
            }
        }

        public static void Delete(int ID, IDbTransaction txn)
        {
            try
            {                

                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pID";
                    objInfo.PValue = ID;
                    objInfo.PDBType = "Int";
                    objInfo.PSize = 200;

                    lstSP.Add(objInfo);

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;


                    try
                    {
                        VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDOnlineUsers", CSqlInfo);
                    }
                    catch (System.ServiceModel.EndpointNotFoundException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "Delete(int ID, IDbTransaction txn)", "ClsUser.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDOnlineUsers", CSqlInfo);
                    }
                    catch (System.ServiceModel.CommunicationException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "Delete(int ID, IDbTransaction txn)", "ClsUser.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDOnlineUsers", CSqlInfo);
                    }

                }
                else
                {

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {

                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pID";
                    objInfo.PValue = ID;
                    objInfo.PDBType = "Int";
                    objInfo.PSize = 200;

                    lstSP.Add(objInfo);                   

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;


                    try
                    {
                        VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDOnlineUsers", CSqlInfo);
                    }
                    catch (System.ServiceModel.EndpointNotFoundException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "Delete(int ID, IDbTransaction txn)", "ClsUser.cs");                      
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDOnlineUsers", CSqlInfo);
                    }
                    catch (System.ServiceModel.CommunicationException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "Delete(int ID, IDbTransaction txn)", "ClsUser.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDOnlineUsers", CSqlInfo);
                    }  
                    
                }
                else
                {
                    new VMukti.DataAccess.ClsUserDataService(txn).User_Delete(ID);
                }
            }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Delete(int64,IdbTransaction", "clsUser.cs");
            }
        }

        public void Delete()
        {
            try
            {
                Delete(ID);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Delete()", "clsUser.cs");
            }
        }

        public void Delete(IDbTransaction txn)
        {
            try
            {
                Delete(ID, txn);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Delete(IDbTransaction)", "clsUser.cs");
            }
        }

        public int Save()
        {
            try
            {
               return(Save(null));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Save()", "clsUser.cs");
                return 0;
            }
        }

        public int Save(IDbTransaction txn)
        {
            try
            {
                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pID";
                    objInfo.PValue = _ID;
                    objInfo.PDBType = "Int";
                    objInfo.PSize = 200;

                    clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                    objInfo2.Direction = "Input";
                    objInfo2.PName = "@pDisplayName";
                    objInfo2.PValue = _DisplayName;
                    objInfo2.PDBType = "NVarChar";
                    objInfo2.PSize = 100;

                    clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                    objInfo3.Direction = "Input";
                    objInfo3.PName = "@pRoleID";
                    objInfo3.PValue = _RoleID;
                    objInfo3.PDBType = "BigInt";
                    objInfo3.PSize = 200;

                    clsSqlParametersInfo objInfo4 = new clsSqlParametersInfo();
                    objInfo4.Direction = "Input";
                    objInfo4.PName = "@pFirstName";
                    objInfo4.PValue = _FName;
                    objInfo4.PDBType = "NVarChar";
                    objInfo4.PSize = 50;

                    clsSqlParametersInfo objInfo5 = new clsSqlParametersInfo();
                    objInfo5.Direction = "Input";
                    objInfo5.PName = "@pLastName";
                    objInfo5.PValue = _LName;
                    objInfo5.PDBType = "NVarChar";
                    objInfo5.PSize = 50;

                    clsSqlParametersInfo objInfo6 = new clsSqlParametersInfo();
                    objInfo6.Direction = "Input";
                    objInfo6.PName = "@pEMail";
                    objInfo6.PValue = _EMail;
                    objInfo6.PDBType = "NVarChar";
                    objInfo6.PSize = 256;


                    clsSqlParametersInfo objInfo7 = new clsSqlParametersInfo();
                    objInfo7.Direction = "Input";
                    objInfo7.PName = "@pPassword";
                    objInfo7.PValue = _PassWord;
                    objInfo7.PDBType = "NVarChar";
                    objInfo7.PSize = 50;

                    clsSqlParametersInfo objInfo8 = new clsSqlParametersInfo();
                    objInfo8.Direction = "Input";
                    objInfo8.PName = "@pIsActive";
                    objInfo8.PValue = _IsActive;
                    objInfo8.PDBType = "Bit";
                    objInfo8.PSize = 200;

                    clsSqlParametersInfo objInfo9 = new clsSqlParametersInfo();
                    objInfo9.Direction = "Input";
                    objInfo9.PName = "@pByUserID";
                    objInfo9.PValue = _ModifiedBy;
                    objInfo9.PDBType = "BigInt";
                    objInfo9.PSize = 200;

                    clsSqlParametersInfo objInfo10 = new clsSqlParametersInfo();
                    objInfo10.Direction = "Input";
                    objInfo10.PName = "@pRatePerHour";
                    objInfo10.PValue = _RatePerHour;
                    objInfo10.PDBType = "Float";
                    objInfo10.PSize = 200;

                    clsSqlParametersInfo objInfo11 = new clsSqlParametersInfo();
                    objInfo11.Direction = "Input";
                    objInfo11.PName = "@pOverTimeRate";
                    objInfo11.PValue = _OverTimeRate;
                    objInfo11.PDBType = "Float";
                    objInfo11.PSize = 200;

                    clsSqlParametersInfo objInfo12 = new clsSqlParametersInfo();
                    objInfo12.Direction = "Input";
                    objInfo12.PName = "@pDoubleRatePerHour";
                    objInfo12.PValue = _DoubleRatePerHour;
                    objInfo12.PDBType = "Float";
                    objInfo12.PSize = 200;

                    clsSqlParametersInfo objInfo13 = new clsSqlParametersInfo();
                    objInfo13.Direction = "Input";
                    objInfo13.PName = "@pDoubleOverTimeRate";
                    objInfo13.PValue = _DoubleOverTimeRate;
                    objInfo13.PDBType = "Float";
                    objInfo13.PSize = 200;

                    clsSqlParametersInfo objInfo14 = new clsSqlParametersInfo();
                    objInfo14.Direction = "Output";
                    objInfo14.PName = "@pReturnId";
                    objInfo14.PValue = -1;
                    objInfo14.PDBType = "BigInt";
                    objInfo14.PSize = 200;


                    lstSP.Add(objInfo);
                    lstSP.Add(objInfo2);
                    lstSP.Add(objInfo3);
                    lstSP.Add(objInfo4);
                    lstSP.Add(objInfo5);
                    lstSP.Add(objInfo6);
                    lstSP.Add(objInfo7);
                    lstSP.Add(objInfo8);
                    lstSP.Add(objInfo9);
                    lstSP.Add(objInfo10);
                    lstSP.Add(objInfo11);
                    lstSP.Add(objInfo12);
                    lstSP.Add(objInfo13);
                    lstSP.Add(objInfo14);


                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;

                    try
                    {
                        return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEUserInfoPayroll", CSqlInfo).ToString());
                    }
                    catch (System.ServiceModel.EndpointNotFoundException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "Save(IDbTransaction txn)", "ClsUser.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEUserInfoPayroll", CSqlInfo).ToString());
                    }
                    catch (System.ServiceModel.CommunicationException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "Save(IDbTransaction txn)", "ClsUser.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEUserInfoPayroll", CSqlInfo).ToString());
                    }
                }
                else
                {

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {

                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pID";
                    objInfo.PValue = _ID;
                    objInfo.PDBType = "Int";
                    objInfo.PSize = 200;

                    clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                    objInfo2.Direction = "Input";
                    objInfo2.PName = "@pDisplayName";
                    objInfo2.PValue = _DisplayName;
                    objInfo2.PDBType = "NVarChar";
                    objInfo2.PSize = 100;

                    clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                    objInfo3.Direction = "Input";
                    objInfo3.PName = "@pRoleID";
                    objInfo3.PValue = _RoleID;
                    objInfo3.PDBType = "BigInt";
                    objInfo3.PSize = 200;

                    clsSqlParametersInfo objInfo4 = new clsSqlParametersInfo();
                    objInfo4.Direction = "Input";
                    objInfo4.PName = "@pFirstName";
                    objInfo4.PValue = _FName;
                    objInfo4.PDBType = "NVarChar";
                    objInfo4.PSize = 50;

                    clsSqlParametersInfo objInfo5 = new clsSqlParametersInfo();
                    objInfo5.Direction = "Input";
                    objInfo5.PName = "@pLastName";
                    objInfo5.PValue = _LName;
                    objInfo5.PDBType = "NVarChar";
                    objInfo5.PSize = 50;

                    clsSqlParametersInfo objInfo6 = new clsSqlParametersInfo();
                    objInfo6.Direction = "Input";
                    objInfo6.PName = "@pEMail";
                    objInfo6.PValue = _EMail;
                    objInfo6.PDBType = "NVarChar";
                    objInfo6.PSize = 256;

                    
                    clsSqlParametersInfo objInfo7 = new clsSqlParametersInfo();
                    objInfo7.Direction = "Input";
                    objInfo7.PName = "@pPassword";
                    objInfo7.PValue = _PassWord;
                    objInfo7.PDBType = "NVarChar";
                    objInfo7.PSize = 50;

                    clsSqlParametersInfo objInfo8 = new clsSqlParametersInfo();
                    objInfo8.Direction = "Input";
                    objInfo8.PName = "@pIsActive";
                    objInfo8.PValue = _IsActive;
                    objInfo8.PDBType = "Bit";
                    objInfo8.PSize = 200;

                    clsSqlParametersInfo objInfo9 = new clsSqlParametersInfo();
                    objInfo9.Direction = "Input";
                    objInfo9.PName = "@pByUserID";
                    objInfo9.PValue = _ModifiedBy;
                    objInfo9.PDBType = "BigInt";
                    objInfo9.PSize = 200;

                    clsSqlParametersInfo objInfo10 = new clsSqlParametersInfo();
                    objInfo10.Direction = "Input";
                    objInfo10.PName = "@pRatePerHour";
                    objInfo10.PValue = _RatePerHour;
                    objInfo10.PDBType = "Float";
                    objInfo10.PSize = 200;

                    clsSqlParametersInfo objInfo11 = new clsSqlParametersInfo();
                    objInfo11.Direction = "Input";
                    objInfo11.PName = "@pOverTimeRate";
                    objInfo11.PValue = _OverTimeRate;
                    objInfo11.PDBType = "Float";
                    objInfo11.PSize = 200;

                    clsSqlParametersInfo objInfo12 = new clsSqlParametersInfo();
                    objInfo12.Direction = "Input";
                    objInfo12.PName = "@pDoubleRatePerHour";
                    objInfo12.PValue = _DoubleRatePerHour;
                    objInfo12.PDBType = "Float";
                    objInfo12.PSize = 200;

                    clsSqlParametersInfo objInfo13 = new clsSqlParametersInfo();
                    objInfo13.Direction = "Input";
                    objInfo13.PName = "@pDoubleOverTimeRate";
                    objInfo13.PValue = _DoubleOverTimeRate;
                    objInfo13.PDBType = "Float";
                    objInfo13.PSize = 200;

                    clsSqlParametersInfo objInfo14 = new clsSqlParametersInfo();
                    objInfo14.Direction = "Output";
                    objInfo14.PName = "@pReturnId";
                    objInfo14.PValue = -1;
                    objInfo14.PDBType = "BigInt";
                    objInfo14.PSize = 200;
                    

                    lstSP.Add(objInfo);
                    lstSP.Add(objInfo2);
                    lstSP.Add(objInfo3);
                    lstSP.Add(objInfo4);
                    lstSP.Add(objInfo5);
                    lstSP.Add(objInfo6);
                    lstSP.Add(objInfo7);
                    lstSP.Add(objInfo8);
                    lstSP.Add(objInfo9);
                    lstSP.Add(objInfo10);
                    lstSP.Add(objInfo11);
                    lstSP.Add(objInfo12);
                    lstSP.Add(objInfo13);
                    lstSP.Add(objInfo14);


                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;

                    try
                    {
                        return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEUserInfoPayroll", CSqlInfo).ToString());
                    }
                    catch (System.ServiceModel.EndpointNotFoundException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "Save(IDbTransaction txn)", "ClsUser.cs");                     
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEUserInfoPayroll", CSqlInfo).ToString());
                    }
                    catch (System.ServiceModel.CommunicationException e)
                    {
                      VMuktiHelper.ExceptionHandler(e, "Save(IDbTransaction txn)", "ClsUser.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEUserInfoPayroll", CSqlInfo).ToString());
                    }  
                   

                }
                else
                {

                    return (new VMukti.DataAccess.ClsUserDataService(txn).User_Save(ref _ID, _DisplayName, _RoleID, _FName, _LName, _EMail, _PassWord, _IsActive, _ModifiedBy, _RatePerHour, _OverTimeRate, _DoubleRatePerHour, _DoubleOverTimeRate));
                }
            }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Save(IDbTransaction)", "clsUser.cs");
                return 0;
            }
        }

        //Following 3 functions has been added by Alpa for UserActivity.
        public static void InsertRecord(int uid)
        {
            try
            {

                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    List<clsSqlParametersInfo> lstSP1 = new List<clsSqlParametersInfo>();

                    ClsUser objUser = new ClsUser();

                    clsSqlParametersInfo objUserInfo1 = new clsSqlParametersInfo();
                    objUserInfo1.Direction = "Input";
                    objUserInfo1.PName = "@uid";
                    objUserInfo1.PValue = uid;
                    objUserInfo1.PDBType = "Int";
                    objUserInfo1.PSize = 200;

                    lstSP1.Add(objUserInfo1);


                    //clsSqlParametersInfo objUserInfo2 = new clsSqlParametersInfo();
                    //objUserInfo2.Direction = "Input";
                    //objUserInfo2.PName = "@dt";
                    //objUserInfo2.PValue = dt;
                    //objUserInfo2.PDBType = "DateTime";
                    //objUserInfo2.PSize = 200;

                    //lstSP1.Add(objUserInfo2);

                    clsSqlParameterContract CSqlUserInfo = new clsSqlParameterContract();
                    CSqlUserInfo.objParam = lstSP1;


                    //   VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEUserInfoPayroll", CSqlInfo).ToString();
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spLogin", CSqlUserInfo);
                   
                }
                else
                {

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    List<clsSqlParametersInfo> lstSP1 = new List<clsSqlParametersInfo>();

                    ClsUser objUser = new ClsUser();

                    clsSqlParametersInfo objUserInfo1 = new clsSqlParametersInfo();
                    objUserInfo1.Direction = "Input";
                    objUserInfo1.PName = "@uid";
                    objUserInfo1.PValue = uid;
                    objUserInfo1.PDBType = "Int";
                    objUserInfo1.PSize = 200;

                    lstSP1.Add(objUserInfo1);


                    //clsSqlParametersInfo objUserInfo2 = new clsSqlParametersInfo();
                    //objUserInfo2.Direction = "Input";
                    //objUserInfo2.PName = "@dt";
                    //objUserInfo2.PValue = dt;
                    //objUserInfo2.PDBType = "DateTime";
                    //objUserInfo2.PSize = 200;

                    //lstSP1.Add(objUserInfo2);

                    clsSqlParameterContract CSqlUserInfo = new clsSqlParameterContract();
                    CSqlUserInfo.objParam = lstSP1;


                    //   VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEUserInfoPayroll", CSqlInfo).ToString();
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spLogin", CSqlUserInfo);
                }
                else
                {
                    new VMukti.DataAccess.ClsUserDataService().User_InsertRecord(uid);
                }
            }
            }
            catch (System.ServiceModel.EndpointNotFoundException e)
            {
                VMuktiHelper.ExceptionHandler(e, "Save(IDbTransaction txn)", "ClsUser.cs");               
                VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
            }
        }

        public static void AddRecord()
        {
            try
            {

                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    List<clsSqlParametersInfo> lstSP1 = new List<clsSqlParametersInfo>();

                    ClsUser objUser = new ClsUser();

                    //clsSqlParametersInfo objUserInfo1 = new clsSqlParametersInfo();
                    //objUserInfo1.Direction = "Input";
                    //objUserInfo1.PName = "@dt";
                    //objUserInfo1.PValue = dt;
                    //objUserInfo1.PDBType = "DateTime";
                    //objUserInfo1.PSize = 200;
                    //lstSP1.Add(objUserInfo1);

                    clsSqlParameterContract CSqlUserInfo = new clsSqlParameterContract();
                    CSqlUserInfo.objParam = lstSP1;

                    //   VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEUserInfoPayroll", CSqlInfo).ToString();
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spSignUp", CSqlUserInfo);
                    
                }
                else
                {

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    List<clsSqlParametersInfo> lstSP1 = new List<clsSqlParametersInfo>();

                    ClsUser objUser = new ClsUser();

                    //clsSqlParametersInfo objUserInfo1 = new clsSqlParametersInfo();
                    //objUserInfo1.Direction = "Input";
                    //objUserInfo1.PName = "@dt";
                    //objUserInfo1.PValue = dt;
                    //objUserInfo1.PDBType = "DateTime";
                    //objUserInfo1.PSize = 200;
                    //lstSP1.Add(objUserInfo1);

                    clsSqlParameterContract CSqlUserInfo = new clsSqlParameterContract();
                    CSqlUserInfo.objParam = lstSP1;

                    //   VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEUserInfoPayroll", CSqlInfo).ToString();
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spSignUp", CSqlUserInfo);
                }
                else
                {
                    new VMukti.DataAccess.ClsUserDataService().User_AddRecord();
                }
            }
            }
            catch (System.ServiceModel.EndpointNotFoundException e)
            {
                VMuktiHelper.ExceptionHandler(e, "AddRecord", "ClsUser.cs");
                VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);

            }
        }

        public static DataSet FindBuddy(string username, string email)
        //public static DataSet FindBuddy()
        {
            try
            {

                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    List<clsSqlParametersInfo> lstSP1 = new List<clsSqlParametersInfo>();

                    ClsUser objUser = new ClsUser();

                    clsSqlParametersInfo objUserInfo1 = new clsSqlParametersInfo();
                    objUserInfo1.Direction = "Input";
                    objUserInfo1.PName = "@uname";
                    objUserInfo1.PValue = username;
                    objUserInfo1.PDBType = "nvarchar";
                    objUserInfo1.PSize = 100;

                    lstSP1.Add(objUserInfo1);

                    clsSqlParametersInfo objUserInfo2 = new clsSqlParametersInfo();
                    objUserInfo2.Direction = "Input";
                    objUserInfo2.PName = "@email";
                    objUserInfo2.PValue = email;
                    objUserInfo2.PDBType = "nvarchar";
                    objUserInfo2.PSize = 256;

                    lstSP1.Add(objUserInfo2);



                    clsSqlParameterContract CSqlUserInfo = new clsSqlParameterContract();
                    CSqlUserInfo.objParam = lstSP1;

                    DataSet ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spFindBuddy", CSqlUserInfo).dsInfo;

                    return ds;
                    //VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEUserInfoPayroll", CSqlInfo).ToString();
                    //VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spFindBuddy", CSqlUserInfo);
                    
                }
                else
                {


                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    List<clsSqlParametersInfo> lstSP1 = new List<clsSqlParametersInfo>();

                    ClsUser objUser = new ClsUser();

                    clsSqlParametersInfo objUserInfo1 = new clsSqlParametersInfo();
                    objUserInfo1.Direction = "Input";
                    objUserInfo1.PName = "@uname";
                    objUserInfo1.PValue = username;
                    objUserInfo1.PDBType = "nvarchar";
                    objUserInfo1.PSize = 100;

                    lstSP1.Add(objUserInfo1);

                    clsSqlParametersInfo objUserInfo2 = new clsSqlParametersInfo();
                    objUserInfo2.Direction = "Input";
                    objUserInfo2.PName = "@email";
                    objUserInfo2.PValue = email;
                    objUserInfo2.PDBType = "nvarchar";
                    objUserInfo2.PSize = 256;

                    lstSP1.Add(objUserInfo2);



                    clsSqlParameterContract CSqlUserInfo = new clsSqlParameterContract();
                    CSqlUserInfo.objParam = lstSP1;

                    DataSet ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spFindBuddy", CSqlUserInfo).dsInfo;

                    return ds;
                    //VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEUserInfoPayroll", CSqlInfo).ToString();
                    //VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spFindBuddy", CSqlUserInfo);
                }
                else
                {
                    //return new VMukti.DataAccess.ClsUserDataService().User_FindBuddy();
                    return new VMukti.DataAccess.ClsUserDataService().User_FindBuddy(username, email);
                }
            }
            }
            catch (System.ServiceModel.EndpointNotFoundException e)
            {
                VMuktiHelper.ExceptionHandler(e, "FindBuddy", "ClsUser.cs");
                VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                return null;
                //VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spLogin", CSqlUserInfo);
            }
        }

        public static void AddNewRecord(int uid)
        {

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                List<clsSqlParametersInfo> lstSP1 = new List<clsSqlParametersInfo>();

                ClsUser objUser = new ClsUser();

                clsSqlParametersInfo objUserInfo1 = new clsSqlParametersInfo();
                objUserInfo1.Direction = "Input";
                objUserInfo1.PName = "@uid";
                objUserInfo1.PValue = uid;
                objUserInfo1.PDBType = "Int";
                objUserInfo1.PSize = 200;

                lstSP1.Add(objUserInfo1);


                //clsSqlParametersInfo objUserInfo2 = new clsSqlParametersInfo();
                //objUserInfo2.Direction = "Input";
                //objUserInfo2.PName = "@dt";
                //objUserInfo2.PValue = dt;
                //objUserInfo2.PDBType = "DateTime";
                //objUserInfo2.PSize = 200;
                //lstSP1.Add(objUserInfo2);

                clsSqlParameterContract CSqlUserInfo = new clsSqlParameterContract();
                CSqlUserInfo.objParam = lstSP1;

                try
                {
                    //   VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEUserInfoPayroll", CSqlInfo).ToString();
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spLogout", CSqlUserInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Save(IDbTransaction txn)", "ClsUser.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                }
            }
            else
            {

            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
            {
                List<clsSqlParametersInfo> lstSP1 = new List<clsSqlParametersInfo>();

                ClsUser objUser = new ClsUser();

                clsSqlParametersInfo objUserInfo1 = new clsSqlParametersInfo();
                objUserInfo1.Direction = "Input";
                objUserInfo1.PName = "@uid";
                objUserInfo1.PValue = uid;
                objUserInfo1.PDBType = "Int";
                objUserInfo1.PSize = 200;

                lstSP1.Add(objUserInfo1);


                //clsSqlParametersInfo objUserInfo2 = new clsSqlParametersInfo();
                //objUserInfo2.Direction = "Input";
                //objUserInfo2.PName = "@dt";
                //objUserInfo2.PValue = dt;
                //objUserInfo2.PDBType = "DateTime";
                //objUserInfo2.PSize = 200;
                //lstSP1.Add(objUserInfo2);

                clsSqlParameterContract CSqlUserInfo = new clsSqlParameterContract();
                CSqlUserInfo.objParam = lstSP1;

                try
                {
                    //   VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEUserInfoPayroll", CSqlInfo).ToString();
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spLogout", CSqlUserInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Save(IDbTransaction txn)", "ClsUser.cs");                  
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                }
            }
            else
            {
                new VMukti.DataAccess.ClsUserDataService().User_AddNewRecord(uid);
                }
            }
        }

        #endregion 
    }
}
