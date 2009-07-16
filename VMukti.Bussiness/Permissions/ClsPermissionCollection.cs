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
using VMuktiAPI;
using System.Text;

namespace VMukti.Business
{
    public class ClsPermissionCollection : ClsBaseCollection<ClsPermissionLogic>
    {        
      
        public static ClsPermissionCollection GetAll()
        {
            ClsPermissionCollection obj = new ClsPermissionCollection();

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                try
                {
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from permissions").dsInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetAll()", "ClsPermissionCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from permissions").dsInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetAll()", "ClsPermissionCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from permissions").dsInfo);
                }
            }
            else
            {
            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
            {
                try
                {
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from permissions").dsInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetAll()", "ClsPermissionCollection.cs");                   
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from permissions").dsInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetAll()", "ClsPermissionCollection.cs"); 
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from permissions").dsInfo);
                }
            }
            else
            {
                obj.MapObjects(new ClsPermissionDataService().GetPermissions());
            }
            }
            return obj;
        }

        public static ClsPermissionCollection Get_PermissionRefModule(int ModuleId, int RoleId)
        {
            ClsPermissionCollection obj = new ClsPermissionCollection();
            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                objInfo.Direction = "Input";
                objInfo.PName = "@pModuleId";
                objInfo.PValue = ModuleId;
                objInfo.PDBType = "BigInt";
                objInfo.PSize = 200;

                clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                objInfo2.Direction = "Input";
                objInfo2.PName = "@pRoleId";
                objInfo2.PValue = RoleId;
                objInfo2.PDBType = "BigInt";
                objInfo2.PSize = 200;

                lstSP.Add(objInfo);
                lstSP.Add(objInfo2);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;
                try
                {
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGPermissionRefModule", CSqlInfo).dsInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_PermissionRefModule(int ModuleId, int RoleId)", "ClsPermissionCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGPermissionRefModule", CSqlInfo).dsInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_PermissionRefModule(int ModuleId, int RoleId)", "ClsPermissionCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGPermissionRefModule", CSqlInfo).dsInfo);
                }
            }
            else
            {
            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
            {
                
                List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                objInfo.Direction = "Input";
                objInfo.PName = "@pModuleId";
                objInfo.PValue = ModuleId;
                objInfo.PDBType = "BigInt";
                objInfo.PSize = 200;

                clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                objInfo2.Direction = "Input";
                objInfo2.PName = "@pRoleId";
                objInfo2.PValue = RoleId;
                objInfo2.PDBType = "BigInt";
                objInfo2.PSize = 200;

                lstSP.Add(objInfo);
                lstSP.Add(objInfo2);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;
                try
                {
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGPermissionRefModule", CSqlInfo).dsInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_PermissionRefModule(int ModuleId, int RoleId)", "ClsPermissionCollection.cs");                    
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGPermissionRefModule", CSqlInfo).dsInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_PermissionRefModule(int ModuleId, int RoleId)", "ClsPermissionCollection.cs"); 
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGPermissionRefModule", CSqlInfo).dsInfo);
                }
            }
            else
            {
                obj.MapObjects(new ClsPermissionDataService().GetPermissionRefModule(ModuleId, RoleId));
            }
            }
            return obj;
        }

        public static ClsPermissionCollection Get_ModPermission(int intModuleId)
        {
            ClsPermissionCollection obj = new ClsPermissionCollection();
            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                try
                {
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet(("select * from permissions where ModuleID=" + intModuleId)).dsInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_ModPermission(int intModuleId)", "ClsPermissionCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet(("select * from permissions where ModuleID=" + intModuleId)).dsInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_ModPermission(int intModuleId)", "ClsPermissionCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet(("select * from permissions where ModuleID=" + intModuleId)).dsInfo);
                }
            }
            else
            {
            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
            {
                
                try
                {
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet(("select * from permissions where ModuleID=" + intModuleId)).dsInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_ModPermission(int intModuleId)", "ClsPermissionCollection.cs");                   
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet(("select * from permissions where ModuleID=" + intModuleId)).dsInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Get_ModPermission(int intModuleId)", "ClsPermissionCollection.cs"); 
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet(("select * from permissions where ModuleID=" + intModuleId)).dsInfo);
                }
            }
            else
            {
                obj.MapObjects(new ClsPermissionDataService().GetAllModPermissions(intModuleId));
                }
            }
            return obj;
        }

    }
}
