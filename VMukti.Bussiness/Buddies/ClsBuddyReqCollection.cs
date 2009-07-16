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
using VMuktiAPI;
using System.Text;

namespace VMukti.Business
{ 
    public class ClsBuddyReqCollection : ClsBaseCollection<ClsBuddyReq>
    {        
     
        public static ClsBuddyReqCollection GetAll()
        {
            
            ClsBuddyReqCollection obj = new ClsBuddyReqCollection();

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                try
                {
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("Select * vMyBuddy").dsInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetAll", "ClsBuddyReqCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("Select * vMyBuddy").dsInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetAll", "ClsBuddyReqCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("Select * vMyBuddy").dsInfo);
                }
            }
            else
            {

            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
            {
                try
                {
                obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("Select * vMyBuddy").dsInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetAll", "ClsBuddyReqCollection.cs");                    
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("Select * vMyBuddy").dsInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetAll", "ClsBuddyReqCollection.cs");                     
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("Select * vMyBuddy").dsInfo);
                }
            }
            else
            {
                obj.MapObjects(new ClsBuddyReqDataService().BuddyReq_GetAll());
            }
            }
            return obj;
        }

        public static ClsBuddyReqCollection GetByUserID(int UserID)
        {
            ClsBuddyReqCollection obj = new ClsBuddyReqCollection();

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                objInfo.Direction = "Input";
                objInfo.PName = "@pID";
                objInfo.PValue = -1;
                objInfo.PDBType = "BigInt";
                objInfo.PSize = 200;

                clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                objInfo2.Direction = "Input";
                objInfo2.PName = "@pUserID";
                objInfo2.PValue = UserID;
                objInfo2.PDBType = "BigInt";
                objInfo2.PSize = 200;

                lstSP.Add(objInfo);
                lstSP.Add(objInfo2);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;


                try
                {
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGBuddyRequest", CSqlInfo).dsInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetByUserID(int UserID)", "ClsBuddyReqCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGBuddyRequest", CSqlInfo).dsInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetByUserID(int UserID)", "ClsBuddyReqCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGBuddyRequest", CSqlInfo).dsInfo);
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

                clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                objInfo2.Direction = "Input";
                objInfo2.PName = "@pUserID";
                objInfo2.PValue = UserID;
                objInfo2.PDBType = "BigInt";
                objInfo2.PSize = 200;

                lstSP.Add(objInfo);
                lstSP.Add(objInfo2);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;

                
                try
                {
                obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGBuddyRequest", CSqlInfo).dsInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetByUserID(int UserID)", "ClsBuddyReqCollection.cs");                    
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGBuddyRequest", CSqlInfo).dsInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetByUserID(int UserID)", "ClsBuddyReqCollection.cs");                    
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGBuddyRequest", CSqlInfo).dsInfo);
                }
                

            }
            else
            {
                obj.MapObjects(new VMukti.DataAccess.ClsBuddyReqDataService().BuddyReq_GetByUserID(UserID));
                }
            }
            return obj;
        }
    }
}
