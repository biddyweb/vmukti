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
using System;
using System.Text;


namespace VMukti.Business
{
    public class ClsCategoryLogic : ClsBaseObject
    {        
      
        #region Fields

        private int intCategoryId = VMukti.Common.ClsConstants.NullInt;
        private string strCategoryTitle = VMukti.Common.ClsConstants.NullString;


        #endregion

        #region Properties

        public int CategoryId
        {
            get { return intCategoryId; }
            set { intCategoryId = value; }
        }

        public string CategoryTitle
        {
            get { return strCategoryTitle; }
            set { strCategoryTitle = value; }
        }
        #endregion

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {
                intCategoryId = GetInt(row, "Id");
                strCategoryTitle = GetString(row, "CategoryName");
                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapData()","ClsCategoryLogic.cs");
                return false;
            }
        }

        public int EditCategory(int id,string name)
        {
            try
            {
                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pID";
                    objInfo.PValue = id;
                    objInfo.PDBType = "BigInt";
                    objInfo.PSize = 200;

                    clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                    objInfo2.Direction = "Input";
                    objInfo2.PName = "@pModuleName";
                    objInfo2.PValue = name;
                    objInfo2.PDBType = "NVarChar";
                    objInfo2.PSize = 200;


                    lstSP.Add(objInfo);
                    lstSP.Add(objInfo2);

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;
                    try
                    {
                        return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spEditCategory", CSqlInfo).ToString());
                    }
                    catch (System.ServiceModel.EndpointNotFoundException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "AddModule(int intModId, string strModName, str..)", "ClsModuleLogic.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spEditCategory", CSqlInfo).ToString());
                    }
                    catch (System.ServiceModel.CommunicationException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "AddModule(int intModId, string strModName, str..)", "ClsModuleLogic.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spEditCategory", CSqlInfo).ToString());
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
                    objInfo.PValue = id;
                    objInfo.PDBType = "BigInt";
                    objInfo.PSize = 200;

                    clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                    objInfo2.Direction = "Input";
                    objInfo2.PName = "@pModuleName";
                    objInfo2.PValue = name;
                    objInfo2.PDBType = "NVarChar";
                    objInfo2.PSize = 200;


                    lstSP.Add(objInfo);
                    lstSP.Add(objInfo2);

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;
                    try
                    {
                        return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spEditCategory", CSqlInfo).ToString());
                    }
                    catch (System.ServiceModel.EndpointNotFoundException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "AddModule(int intModId, string strModName, str..)", "ClsModuleLogic.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spEditCategory", CSqlInfo).ToString());
                    }
                    catch (System.ServiceModel.CommunicationException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "AddModule(int intModId, string strModName, str..)", "ClsModuleLogic.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spEditCategory", CSqlInfo).ToString());
                    }
                }
                else
                {
                    return (new ClsCategoryDataService().UpdateCategory(id, name));
                }
            }
            }
            catch
            {
                return 0;
            }
        }

        public void DeleteCategory(int id)
        {
            try
            {

                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pID";
                    objInfo.PValue = id;
                    objInfo.PDBType = "BigInt";
                    objInfo.PSize = 200;

                    lstSP.Add(objInfo);

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;
                    try
                    {
                        VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spDeleteCategory", CSqlInfo).ToString();
                    }
                    catch (System.ServiceModel.EndpointNotFoundException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "DeleteCategory(int Id)", "ClsModuleLogic.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spDeleteCategory", CSqlInfo);
                    }
                    catch (System.ServiceModel.CommunicationException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "DeleteCategory(int Id)", "ClsModuleLogic.cs"); VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spDeleteCategory", CSqlInfo).ToString();
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
                    objInfo.PValue = id;
                    objInfo.PDBType = "BigInt";
                    objInfo.PSize = 200;

                    lstSP.Add(objInfo);

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;
                    try
                    {
                        VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spDeleteCategory", CSqlInfo).ToString();
                    }
                    catch (System.ServiceModel.EndpointNotFoundException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "DeleteCategory(int Id)", "ClsModuleLogic.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spDeleteCategory", CSqlInfo);
                    }
                    catch (System.ServiceModel.CommunicationException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "DeleteCategory(int Id)", "ClsModuleLogic.cs"); VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spDeleteCategory", CSqlInfo).ToString();
                    }
                }
                else
                {
                    ClsCategoryDataService objClsCategoryDataService = new ClsCategoryDataService();
                    objClsCategoryDataService.Delete_Category(id);
                    
                }
            }
            }
            catch (Exception e)
            {
                VMuktiHelper.ExceptionHandler(e, "DeleteCategory(int Id)", "ClsModuleLogic.cs");
            }
        }

        public int UpdateCategory(int id,string CategoryName)
        {
            try
            {

                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pID";
                    objInfo.PValue = id;
                    objInfo.PDBType = "BigInt";
                    objInfo.PSize = 200;

                    clsSqlParametersInfo objInfo1 = new clsSqlParametersInfo();
                    objInfo1.Direction = "Input";
                    objInfo1.PName = "@pModuleName";
                    objInfo1.PValue = CategoryName;
                    objInfo1.PDBType = "NVarChar";
                    objInfo1.PSize = 200;

                    clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                    objInfo2.Direction = "Output";
                    objInfo2.PName = "@pReturnMaxID";
                    objInfo2.PValue = -1;
                    objInfo2.PDBType = "BigInt";
                    objInfo2.PSize = 200;

                    lstSP.Add(objInfo);
                    lstSP.Add(objInfo1);
                    lstSP.Add(objInfo2);

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;
                    try
                    {
                        return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAECategory", CSqlInfo).ToString());
                    }
                    catch (System.ServiceModel.EndpointNotFoundException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "UpdateCategory(int Id, string CategoryName)", "ClsModuleLogic.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAECategory", CSqlInfo).ToString());
                    }
                    catch (System.ServiceModel.CommunicationException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "UpdateCategory(int Id, string CategoryName)", "ClsModuleLogic.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAECategory", CSqlInfo).ToString());
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
                    objInfo.PValue = id;
                    objInfo.PDBType = "BigInt";
                    objInfo.PSize = 200;

                    clsSqlParametersInfo objInfo1 = new clsSqlParametersInfo();
                    objInfo1.Direction = "Input";
                    objInfo1.PName = "@pModuleName";
                    objInfo1.PValue = CategoryName;
                    objInfo1.PDBType = "NVarChar";
                    objInfo1.PSize = 200;

                    clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                    objInfo2.Direction = "Output";
                    objInfo2.PName = "@pReturnMaxID";
                    objInfo2.PValue = -1;
                    objInfo2.PDBType = "BigInt";
                    objInfo2.PSize = 200;

                    lstSP.Add(objInfo);
                    lstSP.Add(objInfo1);
                    lstSP.Add(objInfo2);

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;
                    try
                    {
                        return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAECategory", CSqlInfo).ToString());
                    }
                    catch (System.ServiceModel.EndpointNotFoundException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "UpdateCategory(int Id, string CategoryName)", "ClsModuleLogic.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAECategory", CSqlInfo).ToString());
                    }
                    catch (System.ServiceModel.CommunicationException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "UpdateCategory(int Id, string CategoryName)", "ClsModuleLogic.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAECategory", CSqlInfo).ToString());
                    }
                }
                else
                {
                    return (new ClsCategoryDataService().UpdateCategory(id, CategoryName));
                    }
                }
            }
            catch
            {
                return 0;
            }
        }
        #endregion 

    }
}

