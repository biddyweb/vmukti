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
using System.Data;
using VMukti.Common;
using VMukti.DataAccess;
using System.Data.SqlClient;
using System.Collections.Generic;
using VMukti.Business.CommonDataContracts;
using VMukti;
using VMuktiAPI;
using System.Text;
using System;

namespace VMukti.Business
{
    public class ClsBuddyReq : ClsBaseObject
    {
        #region Fields       
      
        private int _ID = VMukti.Common.ClsConstants.NullInt;
        private int _UserID = VMukti.Common.ClsConstants.NullInt;
        private string _DisplayName = VMukti.Common.ClsConstants.NullString;

        private int _ReqUserID = VMukti.Common.ClsConstants.NullInt;
        private string _ReqDisplayName = VMukti.Common.ClsConstants.NullString;

        private string _MeshID = VMukti.Common.ClsConstants.NullString;
        private string _Status = VMukti.Common.ClsConstants.NullString;

        #endregion 

        #region Properties

        public int ID
        {
            get
            {
                return _ID;
            }
            set
            {
                _ID = value;
            }
        }

        public int UserID
        {
            get
            {
                return _UserID;
            }
            set
            {
                _UserID = value;
            }
        }

        public string DisplayName
        {
            get
            {
                return _DisplayName;
            }
            set
            {
                _DisplayName = value;
            }
        }

        public int ReqUserID
        {
            get
            {
                return _ReqUserID;
            }
            set
            {
                _ReqUserID = value;
            }
        }

        public string ReqDisplayName
        {
            get
            {
                return _ReqDisplayName;
            }
            set
            {
                _ReqDisplayName = value;
            }
        }

        public string MeshID
        {
            get
            {
                return _MeshID;
            }
            set
            {
                _MeshID = value;
            }
        }

        public string Status
        {
            get
            {
                return _Status;
            }
            set
            {
                _Status = value;
            }
        }

        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {
                ID = GetInt(row, "ID");
                UserID = GetInt(row, "UserID");
                DisplayName = GetString(row, "UDisplayName");
                ReqUserID = GetInt(row, "ReqUserID");
                ReqDisplayName = GetString(row, "RDisplayName");

                return base.MapData(row);
            }
            catch (Exception ex)
            {

                VMuktiHelper.ExceptionHandler(ex, "MapData()","ClsBuddyReq.cs");
                return false;
            }
        }

        public static ClsBuddy GetByBuddyID(int ID)
        {
            ClsBuddy obj = new ClsBuddy();

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                objInfo.Direction = "Input";
                objInfo.PName = "@pID";
                objInfo.PValue = ID;
                objInfo.PDBType = "BigInt";
                objInfo.PSize = 200;

                clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                objInfo2.Direction = "Input";
                objInfo2.PName = "@pUserID";
                objInfo2.PValue = -1;
                objInfo2.PDBType = "BigInt";
                objInfo2.PSize = 200;

                lstSP.Add(objInfo);
                lstSP.Add(objInfo2);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;
                DataSet ds;
                try
                {
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGBuddyRequest", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetByBuddyID(int ID)", "clsBuddyReq.s");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGBuddyRequest", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetByBuddyID(int ID)", "clsBuddyReq.s");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGBuddyRequest", CSqlInfo).dsInfo;
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
                objInfo.PDBType = "BigInt";
                objInfo.PSize = 200;

                clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                objInfo2.Direction = "Input";
                objInfo2.PName = "@pUserID";
                objInfo2.PValue = -1;
                objInfo2.PDBType = "BigInt";
                objInfo2.PSize = 200;

                lstSP.Add(objInfo);
                lstSP.Add(objInfo2);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;
                DataSet ds;
                try
                {
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGBuddyRequest", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetByBuddyID(int ID)", "clsBuddyReq.s");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                     ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGBuddyRequest", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetByBuddyID(int ID)", "clsBuddyReq.s");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                     ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGBuddyRequest", CSqlInfo).dsInfo;
                }
                if (!obj.MapData(ds.Tables[0])) obj = null;
            }
            else
            {
                DataSet ds = new VMukti.DataAccess.ClsBuddyReqDataService().BuddyReq_GetByID(ID);

                if (!obj.MapData(ds.Tables[0])) obj = null;
            }
            }
            return obj;
        }

        public static void Delete(int ID)
        {
            try
            {
                Delete(ID, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Delete(int ID)","ClsBuddyReq.cs");
            }
        }

        public static void Delete(int ID, IDbTransaction txn)
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

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;

                try
                {
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDBuddyRequest", CSqlInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Delete(int ID, IDbTransaction txn)", "clsBuddyReq.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDBuddyRequest", CSqlInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Delete(int ID, IDbTransaction txn)", "clsBuddyReq.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDBuddyRequest", CSqlInfo);
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

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;
                
                try
                {
                VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDBuddyRequest", CSqlInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Delete(int ID, IDbTransaction txn)", "clsBuddyReq.cs");                  
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDBuddyRequest", CSqlInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Delete(int ID, IDbTransaction txn)", "clsBuddyReq.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDBuddyRequest", CSqlInfo);
                }
            }
            else
            {
                new VMukti.DataAccess.ClsBuddyReqDataService(txn).BuddyReq_Delete(ID);
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
                VMuktiHelper.ExceptionHandler(ex, "Delete()","ClsBuddyReq.cs");
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
                VMuktiHelper.ExceptionHandler(ex, "Delete(IDbTransaction txn)", "ClsBuddyReq.cs");
            }
        }

        public void Save(ref int Result)
        {
            try
            {
                Save(null, ref Result);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Save(ref int Result)","ClsBuddyReq.cs");
            }
        }

        public void Save(IDbTransaction txn, ref int Result)
        {

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                objInfo.Direction = "InputOutput";
                objInfo.PName = "@pID";
                objInfo.PValue = _ID;
                objInfo.PDBType = "Int";
                objInfo.PSize = 200;


                clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                objInfo2.Direction = "Input";
                objInfo2.PName = "@pUserID";
                objInfo2.PValue = _UserID;
                objInfo2.PDBType = "Int";
                objInfo2.PSize = 200;

                clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                objInfo3.Direction = "Input";
                objInfo3.PName = "@pDisplayName";
                objInfo3.PValue = _DisplayName;
                objInfo3.PDBType = "VarChar";
                objInfo3.PSize = 200;

                clsSqlParametersInfo objInfo4 = new clsSqlParametersInfo();
                objInfo4.Direction = "Input";
                objInfo4.PName = "@pReqUserID";
                objInfo4.PValue = _ReqUserID;
                objInfo4.PDBType = "Int";
                objInfo4.PSize = 200;

                clsSqlParametersInfo objInfo5 = new clsSqlParametersInfo();
                objInfo5.Direction = "InputOutput";
                objInfo5.PName = "@pResult";
                objInfo5.PValue = Result;
                objInfo5.PDBType = "Int";
                objInfo5.PSize = 200;

                lstSP.Add(objInfo);
                lstSP.Add(objInfo2);
                lstSP.Add(objInfo3);
                lstSP.Add(objInfo4);
                lstSP.Add(objInfo5);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;
                try
                {
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spAEBuddyRequest", CSqlInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Save(IDbTransaction txn, ref int Result)", "clsBuddyReq.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spAEBuddyRequest", CSqlInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Save(IDbTransaction txn, ref int Result)", "clsBuddyReq.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spAEBuddyRequest", CSqlInfo);
                }
            }
            else
            {

            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
            {                
                List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                objInfo.Direction = "InputOutput";
                objInfo.PName = "@pID";
                objInfo.PValue = _ID;
                objInfo.PDBType = "Int";
                objInfo.PSize = 200;


                clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                objInfo2.Direction = "Input";
                objInfo2.PName = "@pUserID";
                objInfo2.PValue = _UserID;
                objInfo2.PDBType = "Int";
                objInfo2.PSize = 200;

                clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                objInfo3.Direction = "Input";
                objInfo3.PName = "@pDisplayName";
                objInfo3.PValue = _DisplayName;
                objInfo3.PDBType = "VarChar";
                objInfo3.PSize = 200;

                clsSqlParametersInfo objInfo4 = new clsSqlParametersInfo();
                objInfo4.Direction = "Input";
                objInfo4.PName = "@pReqUserID";
                objInfo4.PValue = _ReqUserID;
                objInfo4.PDBType = "Int";
                objInfo4.PSize = 200;

                clsSqlParametersInfo objInfo5 = new clsSqlParametersInfo();
                objInfo5.Direction = "InputOutput";
                objInfo5.PName = "@pResult";
                objInfo5.PValue = Result;
                objInfo5.PDBType = "Int";
                objInfo5.PSize = 200;

                lstSP.Add(objInfo);
                lstSP.Add(objInfo2);
                lstSP.Add(objInfo3);
                lstSP.Add(objInfo4);
                lstSP.Add(objInfo5);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;
                try
                {
                VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spAEBuddyRequest", CSqlInfo);                
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Save(IDbTransaction txn, ref int Result)", "clsBuddyReq.cs"); 
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spAEBuddyRequest", CSqlInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Save(IDbTransaction txn, ref int Result)", "clsBuddyReq.cs");                    
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spAEBuddyRequest", CSqlInfo);
                }
            }
            else
            {
                new VMukti.DataAccess.ClsBuddyReqDataService(txn).BuddyReq_Save(ref _ID, _UserID, _DisplayName, _ReqUserID, ref Result);
                }
            }
        }

        #endregion 
    }
}
