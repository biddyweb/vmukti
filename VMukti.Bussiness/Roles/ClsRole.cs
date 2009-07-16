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
using VMukti.DataAccess;
using VMukti.Common;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using VMukti.Business.CommonDataContracts;
using System;
using VMuktiAPI;
using System.Text;

namespace VMukti.Business
{
    public class ClsRole : ClsBaseObject
    {
        #region Fields        
    
        private Int64 _ID = VMukti.Common.ClsConstants.NullLong;
        private string _RoleName = VMukti.Common.ClsConstants.NullString;
        private string _Description = VMukti.Common.ClsConstants.NullString;
        private bool _IsAdmin;
        private Int64 _CreatedBy = VMukti.Common.ClsConstants.NullLong;
        private DateTime _CreatedDate = VMukti.Common.ClsConstants.NullDateTime;
        private Int64 _ModifiedBy = VMukti.Common.ClsConstants.NullLong;
        private DateTime _ModifiedDate = VMukti.Common.ClsConstants.NullDateTime;

        #endregion 

        #region Properties

        public Int64 ID
        {
            get{return _ID;}
            set{_ID = value;}
        }

        public string RoleName
        {
            get { return _RoleName; }
            set { _RoleName = value;}
        }

        public Int64 CreatedBy
        {
            get { return _CreatedBy; }
            set { _CreatedBy = value; }
        }

        public Int64 ModifiedBy
        {
            get { return _ModifiedBy; }
            set { _ModifiedBy = value; }
        }

        public string Description
        {
            get { return _Description; }
            set { _Description = value; }
        }

        public bool IsAdmin
        {
            get { return _IsAdmin; }
            set { _IsAdmin = value; }
        }

        public DateTime CreatedDate
        {
            get { return _CreatedDate;}
            set { _CreatedDate = value; }
        }

        public DateTime ModifiedDate
        {
            get { return _ModifiedDate; }
            set { _ModifiedDate = value; }
        }

       
        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {
                ID = GetLong(row, "ID");
                RoleName = GetString(row, "RoleName");
                Description = GetString(row, "Description");
                IsAdmin = GetBool(row, "IsAdmin");
                CreatedBy = GetLong(row, "CreatedBy");
                CreatedDate = GetDateTime(row, "CreatedDate");
                ModifiedBy = GetLong(row, "ModifiedBy");
                ModifiedDate = GetDateTime(row, "ModifiedDate");

                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapData", "ClsRole.cs");               
                return false;
            }
        }

        public static ClsRole GetByRoleID(Int64 ID)
        {
            ClsRole obj = new ClsRole();

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
                DataSet ds = null;
                try
                {
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGRole", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetByRoleID(Int64 ID)", "ClsRole.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGRole", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetByRoleID(Int64 ID)", "ClsRole.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGRole", CSqlInfo).dsInfo;
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
                DataSet ds=null;
                try
                {
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGRole", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetByRoleID(Int64 ID)", "ClsRole.cs");                   
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGRole", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetByRoleID(Int64 ID)", "ClsRole.cs"); 
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGRole", CSqlInfo).dsInfo;
                }
                
                
                if (!obj.MapData(ds.Tables[0])) obj = null;
            }
            else
            {
                DataSet ds = new VMukti.DataAccess.ClsRoleDataService().Role_GetByID(ID);
                if (!obj.MapData(ds.Tables[0])) obj = null;
            }
            }
            return obj;
        }

        public static void Delete(Int64 ID)
        {
            try
            {
                Delete(ID, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Delete(Int64 ID)", "ClsRole.cs");             
            }
        }

        public static void Delete(Int64 ID, IDbTransaction txn)
        {

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                objInfo.Direction = "Input";
                objInfo.PName = "@pID";
                objInfo.PValue = ID;
                objInfo.PDBType = "BigInt";
                objInfo.PSize = 200;

                lstSP.Add(objInfo);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;

                try
                {
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDRoles", CSqlInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Delete(Int64, IDbTransaction)", "ClsRole.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDRoles", CSqlInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Delete(Int64, IDbTransaction)", "ClsRole.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDRoles", CSqlInfo);
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
                objInfo.PDBType = "BigInt";
                objInfo.PSize = 200;

                lstSP.Add(objInfo);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;

                try
                {
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDRoles", CSqlInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Delete(Int64, IDbTransaction)", "ClsRole.cs"); 
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDRoles", CSqlInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Delete(Int64, IDbTransaction)", "ClsRole.cs"); 
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDRoles", CSqlInfo);
                }
            }
            else
            {
                new VMukti.DataAccess.ClsRoleDataService(txn).Role_Delete(ID);
            }
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
                VMuktiHelper.ExceptionHandler(ex, "Delete", "ClsRole.cs");              
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
                VMuktiHelper.ExceptionHandler(ex, "Delete(IDbTransaction txn)", "ClsRole.cs");              
            }
        }

        public Int64 Save()
        {
            try
            {
                return (Save(null));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Save", "ClsRole.cs");                  
                return 0;
            }
        }

        public Int64 Save(IDbTransaction txn)
        {
           
            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                objInfo.Direction = "Input";
                objInfo.PName = "@pID";
                objInfo.PValue = _ID;
                objInfo.PDBType = "BigInt";
                objInfo.PSize = 200;

                clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                objInfo2.Direction = "Input";
                objInfo2.PName = "@pRoleName";
                objInfo2.PValue = _RoleName;
                objInfo2.PDBType = "NVarChar";
                objInfo2.PSize = 50;


                clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                objInfo3.Direction = "Input";
                objInfo3.PName = "@pDescription";
                objInfo3.PValue = _Description;
                objInfo3.PDBType = "NVarChar";
                objInfo3.PSize = 100;

                clsSqlParametersInfo objInfo4 = new clsSqlParametersInfo();
                objInfo4.Direction = "Input";
                objInfo4.PName = "@pIsAdmin";
                objInfo4.PValue = _IsAdmin;
                objInfo4.PDBType = "Bit";
                objInfo4.PSize = 200;

                clsSqlParametersInfo objInfo5 = new clsSqlParametersInfo();
                objInfo5.Direction = "Input";
                objInfo5.PName = "@pUserID";
                objInfo5.PValue = _CreatedBy;
                objInfo5.PDBType = "BigInt";
                objInfo5.PSize = 200;

                clsSqlParametersInfo objInfo6 = new clsSqlParametersInfo();
                objInfo6.Direction = "InputOutput";
                objInfo6.PName = "@pReturnMaxID";
                objInfo6.PValue = -1;
                objInfo6.PDBType = "BigInt";
                objInfo6.PSize = 200;


                lstSP.Add(objInfo);
                lstSP.Add(objInfo2);
                lstSP.Add(objInfo3);
                lstSP.Add(objInfo4);
                lstSP.Add(objInfo5);
                lstSP.Add(objInfo6);


                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;

                try
                {
                    return (Int64)int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAERoles", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Save(IDbTransaction txn)", "ClsRole.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return (Int64)int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAERoles", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Save(IDbTransaction txn)", "ClsRole.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return (Int64)int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAERoles", CSqlInfo).ToString());
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
                objInfo.PDBType = "BigInt";
                objInfo.PSize = 200;

                clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                objInfo2.Direction = "Input";
                objInfo2.PName = "@pRoleName";
                objInfo2.PValue = _RoleName;
                objInfo2.PDBType = "NVarChar";
                objInfo2.PSize = 50;


                clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                objInfo3.Direction = "Input";
                objInfo3.PName = "@pDescription";
                objInfo3.PValue = _Description;
                objInfo3.PDBType = "NVarChar";
                objInfo3.PSize = 100;

                clsSqlParametersInfo objInfo4 = new clsSqlParametersInfo();
                objInfo4.Direction = "Input";
                objInfo4.PName = "@pIsAdmin";
                objInfo4.PValue = _IsAdmin;
                objInfo4.PDBType = "Bit";
                objInfo4.PSize = 200;

                clsSqlParametersInfo objInfo5 = new clsSqlParametersInfo();
                objInfo5.Direction = "Input";
                objInfo5.PName = "@pUserID";
                objInfo5.PValue = _CreatedBy;
                objInfo5.PDBType = "BigInt";
                objInfo5.PSize = 200;                

                clsSqlParametersInfo objInfo6 = new clsSqlParametersInfo();
                objInfo6.Direction = "InputOutput";
                objInfo6.PName = "@pReturnMaxID";
                objInfo6.PValue = -1;
                objInfo6.PDBType = "BigInt";
                objInfo6.PSize = 200;


                lstSP.Add(objInfo);
                lstSP.Add(objInfo2);
                lstSP.Add(objInfo3);
                lstSP.Add(objInfo4);
                lstSP.Add(objInfo5);
                lstSP.Add(objInfo6);
                

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;

                try
                {
                    return (Int64)int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAERoles", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Save(IDbTransaction txn)", "ClsRole.cs");                   
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return (Int64)int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAERoles", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Save(IDbTransaction txn)", "ClsRole.cs");   
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return (Int64)int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAERoles", CSqlInfo).ToString());
                }
            }
            else
            {
                return (new VMukti.DataAccess.ClsRoleDataService(txn).Role_Save(ref _ID, _RoleName, _Description, _IsAdmin, _CreatedBy));
                }
            }
        }

        #endregion 
    }
}
