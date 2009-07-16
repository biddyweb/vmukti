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
    public class ClsPermissionLogic : ClsBaseObject
    {
        #region Fields       
      
        private int intPermissionId = VMukti.Common.ClsConstants.NullInt;
        private int intModuleId = VMukti.Common.ClsConstants.NullInt;
        private string strPermissionName = VMukti.Common.ClsConstants.NullString;
        private int strPermissionValue = VMukti.Common.ClsConstants.NullInt;

        #endregion

        #region Properties

        public int PermissionId
        {
            get { return intPermissionId; }
            set { intPermissionId = value; }
        }

        public int ModuleId
        {
            get { return intModuleId; }
            set { intModuleId = value; }
        }

        public string PermissionName
        {
            get { return strPermissionName; }
            set { strPermissionName = value; }
        }

        public int PermissionValue
        {
            get { return strPermissionValue; }
            set { strPermissionValue = value; }
        }

        #endregion

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {
                intPermissionId = GetInt(row, "ID");
                intModuleId = GetInt(row, "ModuleID");
                strPermissionName = GetString(row, "PermissionName");
                strPermissionValue = GetInt(row, "PermissionValue");
                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapData()","ClsPermissionLogic.cs");
                return false;
            }
        }

        public  int Add_Permission(int permissionId, int moduleId, string permissionName, int permissionValue)
        {

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                objInfo.Direction = "Input";
                objInfo.PName = "@pID";
                objInfo.PValue = permissionId;
                objInfo.PDBType = "BigInt";
                objInfo.PSize = 200;

                clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                objInfo2.Direction = "Input";
                objInfo2.PName = "@pModuleId";
                objInfo2.PValue = moduleId;
                objInfo2.PDBType = "BigInt";
                objInfo2.PSize = 200;

                clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                objInfo3.Direction = "Input";
                objInfo3.PName = "@pPermissionName";
                objInfo3.PValue = permissionName;
                objInfo3.PDBType = "NVarChar";
                objInfo3.PSize = 200;

                clsSqlParametersInfo objInfo4 = new clsSqlParametersInfo();
                objInfo4.Direction = "Input";
                objInfo4.PName = "@pPermissionValue";
                objInfo4.PValue = permissionValue;
                objInfo4.PDBType = "BigInt";
                objInfo4.PSize = 200;

                clsSqlParametersInfo objInfo5 = new clsSqlParametersInfo();
                objInfo5.Direction = "Output";
                objInfo5.PName = "@pReturnMaxID";
                objInfo5.PValue = -1;
                objInfo5.PDBType = "BigInt";
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
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEPermissions", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Add_Permission(int permissionId, int moduleId...)", "ClsPermissionLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEPermissions", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Add_Permission(int permissionId, int moduleId...)", "ClsPermissionLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEPermissions", CSqlInfo).ToString());
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
                objInfo.PValue = permissionId;
                objInfo.PDBType = "BigInt";
                objInfo.PSize = 200;

                clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                objInfo2.Direction = "Input";
                objInfo2.PName = "@pModuleId";
                objInfo2.PValue = moduleId;
                objInfo2.PDBType = "BigInt";
                objInfo2.PSize = 200;

                clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                objInfo3.Direction = "Input";
                objInfo3.PName = "@pPermissionName";
                objInfo3.PValue = permissionName;
                objInfo3.PDBType = "NVarChar";
                objInfo3.PSize = 200;

                clsSqlParametersInfo objInfo4 = new clsSqlParametersInfo();
                objInfo4.Direction = "Input";
                objInfo4.PName = "@pPermissionValue";
                objInfo4.PValue = permissionValue;
                objInfo4.PDBType = "BigInt";
                objInfo4.PSize = 200;

                clsSqlParametersInfo objInfo5 = new clsSqlParametersInfo();
                objInfo5.Direction = "Output";
                objInfo5.PName = "@pReturnMaxID";
                objInfo5.PValue = -1;
                objInfo5.PDBType = "BigInt";
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
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEPermissions", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Add_Permission(int permissionId, int moduleId...)", "ClsPermissionLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEPermissions", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Add_Permission(int permissionId, int moduleId...)", "ClsPermissionLogic.cs");
                     VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEPermissions", CSqlInfo).ToString());
                }
            }
            else
            {
                return (new ClsPermissionDataService().AddPermission(permissionId, moduleId, permissionName, permissionValue));
            }
        }
        }

        public  int Add_ModulePermission(int permissionId, int intRoleId)
        {

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                objInfo.Direction = "Input";
                objInfo.PName = "@pPermissionID";
                objInfo.PValue = permissionId;
                objInfo.PDBType = "BigInt";
                objInfo.PSize = 200;

                clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                objInfo2.Direction = "Input";
                objInfo2.PName = "@pRoleID";
                objInfo2.PValue = intRoleId;
                objInfo2.PDBType = "BigInt";
                objInfo2.PSize = 200;

                clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                objInfo3.Direction = "Output";
                objInfo3.PName = "@pReturnMaxID";
                objInfo3.PValue = -1;
                objInfo3.PDBType = "BigInt";
                objInfo3.PSize = 200;


                lstSP.Add(objInfo);
                lstSP.Add(objInfo2);
                lstSP.Add(objInfo3);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;

                try
                {
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEModulePermission", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Add_ModulePermission(int permissionId, int intRoleId)", "ClsPermissionLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEModulePermission", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Add_ModulePermission(int permissionId, int intRoleId)", "ClsPermissionLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEModulePermission", CSqlInfo).ToString());
                }
            }
            else
            {

            if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
            {                
                List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                objInfo.Direction = "Input";
                objInfo.PName = "@pPermissionID";
                objInfo.PValue = permissionId;
                objInfo.PDBType = "BigInt";
                objInfo.PSize = 200;

                clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                objInfo2.Direction = "Input";
                objInfo2.PName = "@pRoleID";
                objInfo2.PValue = intRoleId;
                objInfo2.PDBType = "BigInt";
                objInfo2.PSize = 200;                                

                clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                objInfo3.Direction = "Output";
                objInfo3.PName = "@pReturnMaxID";
                objInfo3.PValue = -1;
                objInfo3.PDBType = "BigInt";
                objInfo3.PSize = 200;


                lstSP.Add(objInfo);
                lstSP.Add(objInfo2);
                lstSP.Add(objInfo3);
                
                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;

                try
                {
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEModulePermission", CSqlInfo).ToString());                
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Add_ModulePermission(int permissionId, int intRoleId)", "ClsPermissionLogic.cs");                    
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEModulePermission", CSqlInfo).ToString());                
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "Add_ModulePermission(int permissionId, int intRoleId)", "ClsPermissionLogic.cs");                    
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEModulePermission", CSqlInfo).ToString());                
                }
            }
            else
            {
                return (new ClsPermissionDataService().AddModulePermission(permissionId, intRoleId));
                }
            }
        }

        #endregion
    }
}
