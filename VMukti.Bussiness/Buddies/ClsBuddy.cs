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
using System.Net;
using System;
using VMuktiAPI;
using VMuktiService;
using VMukti;
using System.Text;



namespace VMukti.Business
{
    public class ClsBuddy : ClsBaseObject
    {
        #region Fields       
       

        private int _ID = VMukti.Common.ClsConstants.NullInt;
        private int _MyUserID = VMukti.Common.ClsConstants.NullInt;
        private int _UserID = VMukti.Common.ClsConstants.NullInt;
        private string _DisplayName = VMukti.Common.ClsConstants.NullString;

        private string _MeshID = VMukti.Common.ClsConstants.NullString;
        private string _Status = VMukti.Common.ClsConstants.NullString;

        #endregion 

        #region Properties

        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }

        public int MyUserID
        {
            get { return _MyUserID; }
            set 
            { 
                _MyUserID = value;
                _MeshID = "VMukti" + _MyUserID.ToString();
            }
        }

        public int UserID
        {
            get { return _UserID; }
            set { _UserID = value; }
        }

        public string MeshID
        {
            get { return _MeshID; }
        }

        public string DisplayName
        {
            get { return _DisplayName; }
            set { _DisplayName = value; }
        }
        
        public string Status
        {
            get { return _Status; }
            set { _Status = value; }
        }

        #endregion 

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {
                ID = GetInt(row, "ID");
                MyUserID = GetInt(row, "MyUserID");
                UserID = GetInt(row, "UserID");
                DisplayName = GetString(row, "DisplayName");

                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapData", "ClsBuddy.cs");
                return false;
            }
        }

        public static ClsBuddy GetByBuddyID(int ID)
        {
            ClsBuddy obj = new ClsBuddy();
            ClsPeer _Peer = VMuktiAPI.VMuktiInfo.CurrentPeer;

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


                //super node checking
                DataSet ds;
                try
                {
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGMyBuddy", CSqlInfo).dsInfo;

                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetByBuddyId(int ID)", "clsBuddy.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGMyBuddy", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetByBuddyId(int ID)", "clsBuddy.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGMyBuddy", CSqlInfo).dsInfo;
                }


                if (!obj.MapData(ds.Tables[0])) obj = null;
            }
            else
            {

            if (_Peer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
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

              
                //super node checking
                DataSet ds;
                try
                {
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGMyBuddy", CSqlInfo).dsInfo;
                    
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetByBuddyId(int ID)", "clsBuddy.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGMyBuddy", CSqlInfo).dsInfo;
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetByBuddyId(int ID)", "clsBuddy.cs");                 
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGMyBuddy", CSqlInfo).dsInfo;  
                }
               
                
                if (!obj.MapData(ds.Tables[0])) obj = null;                
            }
            else
            {
                DataSet ds = new VMukti.DataAccess.ClsBuddyDataService().Buddy_GetByID(ID);
                if (!obj.MapData(ds.Tables[0])) obj = null;
            }
            }
            return obj;
        }

        public static string  GetIP4Address()
        {
            string IP4Address = String.Empty;
            try
            {
                foreach (IPAddress IPA in Dns.GetHostAddresses(System.Net.Dns.GetHostName()))
                {
                    if (IPA.AddressFamily.ToString() == "InterNetwork")
                    {
                        IP4Address = IPA.ToString();
                        break;
                    }
                }
                return IP4Address;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetIP4Address", "ClsBuddy.cs");                
                return IP4Address;
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
                VMuktiHelper.ExceptionHandler(ex, "Delete(int ID)", "clsBuddy.cs");
            }
        }

        public static void Delete(int ID, IDbTransaction txn)
        {
            ClsPeer _Peer = VMuktiAPI.VMuktiInfo.CurrentPeer;

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

                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDMyBuddy", CSqlInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Delete(int ID, IDbTransaction txn)", "clsBuddy.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDMyBuddy", CSqlInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Delete(int ID, IDbTransaction txn)", "clsBuddy.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDMyBuddy", CSqlInfo);
                }
            }
            else
            {

            if (_Peer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
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

                VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDMyBuddy", CSqlInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Delete(int ID, IDbTransaction txn)", "clsBuddy.cs");                   
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDMyBuddy", CSqlInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Delete(int ID, IDbTransaction txn)", "clsBuddy.cs");                  
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spDMyBuddy", CSqlInfo);
                }
               
            }
            else
            {
                new VMukti.DataAccess.ClsBuddyDataService(txn).Buddy_Delete(ID);
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
                VMuktiHelper.ExceptionHandler(ex, "Delete()", "ClsBuddy.cs");
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
                VMuktiHelper.ExceptionHandler(ex,  "Delete(IDbTransaction txn)","ClsBuddy.cs");
            }
        }

        public void Save()
        {
            try
            {
                Save(null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Save()","ClsBuddy.cs");                
            }
        }

        public void Save(IDbTransaction txn)
        {
            ClsPeer _Peer = VMuktiAPI.VMuktiInfo.CurrentPeer;

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
                objInfo2.PName = "@pMyUserID";
                objInfo2.PValue = _MyUserID;
                objInfo2.PDBType = "Int";
                objInfo2.PSize = 200;

                clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                objInfo3.Direction = "Input";
                objInfo3.PName = "@pUserID";
                objInfo3.PValue = _UserID;
                objInfo3.PDBType = "Int";
                objInfo3.PSize = 200;


                lstSP.Add(objInfo);
                lstSP.Add(objInfo2);
                lstSP.Add(objInfo3);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;
                try
                {
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spAEMyBuddy", CSqlInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Save(IDbTransaction txn)", "clsBuddy.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spAEMyBuddy", CSqlInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Save(IDbTransaction txn)", "clsBuddy.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spAEMyBuddy", CSqlInfo);
                }
            }
            else
            {
            if (_Peer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
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
                objInfo2.PName = "@pMyUserID";
                objInfo2.PValue = _MyUserID;                
                objInfo2.PDBType = "Int";
                objInfo2.PSize = 200;
                
                clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                objInfo3.Direction = "Input";
                objInfo3.PName = "@pUserID";
                objInfo3.PValue = _UserID;                
                objInfo3.PDBType = "Int";
                objInfo3.PSize = 200;

                
                lstSP.Add(objInfo);
                lstSP.Add(objInfo2);
                lstSP.Add(objInfo3);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;
                  try
                  {
                VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spAEMyBuddy", CSqlInfo);
                  }
                  catch (System.ServiceModel.EndpointNotFoundException e)
                  {
                      VMuktiHelper.ExceptionHandler(e, "Save(IDbTransaction txn)", "clsBuddy.cs");                     
                      VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                      VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spAEMyBuddy", CSqlInfo);
                  }
                  catch (System.ServiceModel.CommunicationException e)
                  {
                      VMuktiHelper.ExceptionHandler(e, "Save(IDbTransaction txn)", "clsBuddy.cs"); 
                      VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                      VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteNonQuery("spAEMyBuddy", CSqlInfo);
                  }
                
            }
            else
            {
                new VMukti.DataAccess.ClsBuddyDataService(txn).Buddy_Save(ref _ID, _MyUserID, _UserID);
                }
            }
        }

        #endregion 
    }
}
