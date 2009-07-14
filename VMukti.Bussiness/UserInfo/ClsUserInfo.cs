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
    public class ClsUserInfo : ClsBaseObject
    {
        #region Fields       
        public static StringBuilder CreateTressInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("User Is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
            sb.AppendLine();
            sb.Append("Peer Type is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString());
            sb.AppendLine();
            sb.Append("User's SuperNode is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
            sb.AppendLine();
            sb.Append("User's Machine Ip Address : " + VMuktiAPI.GetIPAddress.ClsGetIP4Address.GetIP4Address());
            sb.AppendLine();
            sb.AppendLine("----------------------------------------------------------------------------------------");
            return sb;
        }
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

        #endregion 

        #region Properties    

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

                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name, "clsUserInfo.cs");
                return false;
            }
        }

        public ClsUserInfo User_GetByID(int ID)
        {
            try
            {
                ClsUserInfo obj = new ClsUserInfo();

                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();


                    DataSet ds;
                    try
                    {
                        ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from UserInfo where ID=" + ID).dsInfo;
                    }
                    catch (System.ServiceModel.EndpointNotFoundException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, System.Reflection.MethodInfo.GetCurrentMethod().Name, "clsUserInfo.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from UserInfo where ID=" + ID).dsInfo;
                    }
                    catch (System.ServiceModel.CommunicationException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, System.Reflection.MethodInfo.GetCurrentMethod().Name, "clsUserInfo.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from UserInfo where ID=" + ID).dsInfo;
                    }

                    if (!obj.MapData(ds.Tables[0])) obj = null;
                }
                else
                {


                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {

                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                
                    DataSet ds;
                    try
                    {
                        ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from UserInfo where ID=" + ID).dsInfo;
                    }
                    catch (System.ServiceModel.EndpointNotFoundException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, System.Reflection.MethodInfo.GetCurrentMethod().Name, "clsUserInfo.cs"); 
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from UserInfo where ID=" + ID).dsInfo;
                    }
                    catch (System.ServiceModel.CommunicationException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, System.Reflection.MethodInfo.GetCurrentMethod().Name, "clsUserInfo.cs"); 
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from UserInfo where ID=" + ID).dsInfo;
                    }

                    if (!obj.MapData(ds.Tables[0])) obj = null;
                }
                else
                {
                    DataSet ds = new VMukti.DataAccess.ClsUserInfoDataService().User_GetByID(ID);

                    if (!obj.MapData(ds.Tables[0])) obj = null;
                    }
                }
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name, "clsUserInfo.cs");
                return null;
            }
        }


        #endregion 
    }
}
