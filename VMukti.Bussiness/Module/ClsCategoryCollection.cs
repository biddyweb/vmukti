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
using System.Data.SqlClient;
using System.Collections.Generic;
using VMukti.Business.CommonDataContracts;
using VMuktiAPI;
using System.Text;

namespace VMukti.Business
{
    public class ClsCategoryCollection : ClsBaseCollection<ClsCategoryLogic>
    {        
      
        public static ClsCategoryCollection GetMainCategory(int intRoleId)
        {
            ClsCategoryCollection obj = new ClsCategoryCollection();

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                try
                {
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from Category").dsInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetCMod(int intRoleId)", "ClsModuleCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from Category").dsInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetMainCategory(int intRoleId)", "clsCategoryCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from Category").dsInfo);
                }
            }
            else
            {
            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
            {
                try
                {
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from Category").dsInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetCMod(int intRoleId)", "ClsModuleCollection.cs"); 
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from Category").dsInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetMainCategory(int intRoleId)", "clsCategoryCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select * from Category").dsInfo);
                }
            }
            else
            {
                try
                {
                    obj.MapObjects(new ClsCategoryDataService().GetMainCategory());
                }
                catch (System.Exception e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetMainCategory(int intRoleId)", "clsCategoryCollection.cs");
                }
            }
            }
            return obj;
        }

        public static ClsModuleCollection SubCategory(int intRoleId)
        {
            ClsModuleCollection obj = new ClsModuleCollection();

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                objInfo.Direction = "Input";
                objInfo.PName = "@pRoleId";
                objInfo.PValue = intRoleId;
                objInfo.PDBType = "BigInt";
                objInfo.PSize = 200;
                lstSP.Add(objInfo);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;

                try
                {
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGCModules", CSqlInfo).dsInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "SubCategory(int intRoleId)", "clsCategoryCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGCModules", CSqlInfo).dsInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "SubCategory(int intRoleId)", "clsCategoryCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGCModules", CSqlInfo).dsInfo);
                }
            }
            else
            {

            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
            {
                List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                objInfo.Direction = "Input";
                objInfo.PName = "@pRoleId";
                objInfo.PValue = intRoleId;
                objInfo.PDBType = "BigInt";
                objInfo.PSize = 200;
                lstSP.Add(objInfo);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;

                try
                {
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGCModules", CSqlInfo).dsInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "SubCategory(int intRoleId)", "clsCategoryCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGCModules", CSqlInfo).dsInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "SubCategory(int intRoleId)", "clsCategoryCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGCModules", CSqlInfo).dsInfo);
                }

            }
            else
            {
                obj.MapObjects(new ClsModuleDataService().GetCollaborativeModules(intRoleId));
                }
            }
            return obj;
        }
        
    }
}
