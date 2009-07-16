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
using System.Windows.Media.Imaging;
using System;




namespace VMukti.Business
{
    public class ClsModuleLogic : ClsBaseObject
    {
        public static StringBuilder sb1 = new StringBuilder();


        #region Fields

        private int intModuleId = VMukti.Common.ClsConstants.NullInt;
        //private int intCategoryId = VMukti.Common.ClsConstants.NullInt;
        private string strModuleTitle = VMukti.Common.ClsConstants.NullString;
        private string strIsCollaborative = VMukti.Common.ClsConstants.NullString;
        private string strZipName = VMukti.Common.ClsConstants.NullString;
        private string strAssemblyFile = VMukti.Common.ClsConstants.NullString;
        private byte[] byImageFile;
        #endregion

        #region Properties

        public byte[] ImageFile
        {
            get { return byImageFile; }
            set { byImageFile = value; }
        }

        public int ModuleId
        {
            get { return intModuleId; }
            set { intModuleId = value; }
        }

        public string ModuleTitle
        {
            get { return strModuleTitle; }
            set { strModuleTitle = value; }
        }

        public string IsCollaborative
        {
            get { return strIsCollaborative; }
            set { strIsCollaborative = value; }
        }

        public string ZipFile
        {
            get { return strZipName; }
            set { strZipName = value; }
        }

        public string AssemblyFile
        {
            get { return strAssemblyFile; }
            set { strAssemblyFile = value; }
        }

        #endregion

        #region Methods

        public override bool MapData(DataRow row)
        {
            try
            {
                intModuleId = GetInt(row, "ID");
                strModuleTitle = GetString(row, "ModuleName");
                strIsCollaborative = GetString(row, "IsCollaborative");
                strZipName = GetString(row, "ZipFile");
                strAssemblyFile = GetString(row, "AssemblyFile");
                byImageFile = GetImage(row, "ImageFile");
                return base.MapData(row);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "MapData()", "ClsModuleLogic.cs");
                return false;
            }
        }

        public int AddModule(int intModId, string strModName, string strModVersion, string strDesc, string strAssFile, string strClassFile, string strZipFile, int intUserid, bool blnIsCollaborative, bool blnNeedAuth, byte[] ImageFile)
        {

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                objInfo.Direction = "Input";
                objInfo.PName = "@pID";
                objInfo.PValue = intModId;
                objInfo.PDBType = "BigInt";
                objInfo.PSize = 200;

                clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                objInfo2.Direction = "Input";
                objInfo2.PName = "@pModuleName";
                objInfo2.PValue = strModName;
                objInfo2.PDBType = "NVarChar";
                objInfo2.PSize = 200;

                clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                objInfo3.Direction = "Input";
                objInfo3.PName = "@pModuleVersion";
                objInfo3.PValue = strModVersion;
                objInfo3.PDBType = "NVarChar";
                objInfo3.PSize = 200;

                clsSqlParametersInfo objInfo4 = new clsSqlParametersInfo();
                objInfo4.Direction = "Input";
                objInfo4.PName = "@pDescription";
                objInfo4.PValue = strDesc;
                objInfo4.PDBType = "NVarChar";
                objInfo4.PSize = 200;

                clsSqlParametersInfo objInfo5 = new clsSqlParametersInfo();
                objInfo5.Direction = "Input";
                objInfo5.PName = "@pAssemblyFile";
                objInfo5.PValue = strAssFile;
                objInfo5.PDBType = "NVarChar";
                objInfo5.PSize = 200;

                clsSqlParametersInfo objInfo6 = new clsSqlParametersInfo();
                objInfo6.Direction = "Input";
                objInfo6.PName = "@pClassName";
                objInfo6.PValue = strClassFile;
                objInfo6.PDBType = "NVarChar";
                objInfo6.PSize = 200;

                clsSqlParametersInfo objInfo7 = new clsSqlParametersInfo();
                objInfo7.Direction = "Input";
                objInfo7.PName = "@pZipFile";
                objInfo7.PValue = strZipFile;
                objInfo7.PDBType = "NVarChar";
                objInfo7.PSize = 200;

                clsSqlParametersInfo objInfo8 = new clsSqlParametersInfo();
                objInfo8.Direction = "Input";
                objInfo8.PName = "@pUserID";
                objInfo8.PValue = intUserid;
                objInfo8.PDBType = "BigInt";
                objInfo8.PSize = 200;

                clsSqlParametersInfo objInfo9 = new clsSqlParametersInfo();
                objInfo9.Direction = "Input";
                objInfo9.PName = "@pIsCollaborative";
                objInfo9.PValue = blnIsCollaborative;
                objInfo9.PDBType = "Bit";
                objInfo9.PSize = 200;

                clsSqlParametersInfo objInfo10 = new clsSqlParametersInfo();
                objInfo10.Direction = "Input";
                objInfo10.PName = "@pNeedsAuthentication";
                objInfo10.PValue = blnNeedAuth;
                objInfo10.PDBType = "Bit";
                objInfo10.PSize = 200;

                clsSqlParametersInfo objInfo11 = new clsSqlParametersInfo();
                objInfo11.Direction = "Input";
                objInfo11.PName = "@pImageFile";
                objInfo11.PValue = ImageFile;
                objInfo11.PDBType = "Image";


                clsSqlParametersInfo objInfo12 = new clsSqlParametersInfo();
                objInfo12.Direction = "Output";
                objInfo12.PName = "@pReturnMaxID";
                objInfo12.PValue = -1;
                objInfo12.PDBType = "BigInt";
                objInfo12.PSize = 200;


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

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;
                try
                {
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEModule", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "AddModule(int intModId, string strModName, str..)", "ClsModuleLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEModule", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "AddModule(int intModId, string strModName, str..)", "ClsModuleLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEModule", CSqlInfo).ToString());
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
                    objInfo.PValue = intModId;
                    objInfo.PDBType = "BigInt";
                    objInfo.PSize = 200;

                    clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                    objInfo2.Direction = "Input";
                    objInfo2.PName = "@pModuleName";
                    objInfo2.PValue = strModName;
                    objInfo2.PDBType = "NVarChar";
                    objInfo2.PSize = 200;

                    clsSqlParametersInfo objInfo3 = new clsSqlParametersInfo();
                    objInfo3.Direction = "Input";
                    objInfo3.PName = "@pModuleVersion";
                    objInfo3.PValue = strModVersion;
                    objInfo3.PDBType = "NVarChar";
                    objInfo3.PSize = 200;

                    clsSqlParametersInfo objInfo4 = new clsSqlParametersInfo();
                    objInfo4.Direction = "Input";
                    objInfo4.PName = "@pDescription";
                    objInfo4.PValue = strDesc;
                    objInfo4.PDBType = "NVarChar";
                    objInfo4.PSize = 200;

                    clsSqlParametersInfo objInfo5 = new clsSqlParametersInfo();
                    objInfo5.Direction = "Input";
                    objInfo5.PName = "@pAssemblyFile";
                    objInfo5.PValue = strAssFile;
                    objInfo5.PDBType = "NVarChar";
                    objInfo5.PSize = 200;

                    clsSqlParametersInfo objInfo6 = new clsSqlParametersInfo();
                    objInfo6.Direction = "Input";
                    objInfo6.PName = "@pClassName";
                    objInfo6.PValue = strClassFile;
                    objInfo6.PDBType = "NVarChar";
                    objInfo6.PSize = 200;

                    clsSqlParametersInfo objInfo7 = new clsSqlParametersInfo();
                    objInfo7.Direction = "Input";
                    objInfo7.PName = "@pZipFile";
                    objInfo7.PValue = strZipFile;
                    objInfo7.PDBType = "NVarChar";
                    objInfo7.PSize = 200;

                    clsSqlParametersInfo objInfo8 = new clsSqlParametersInfo();
                    objInfo8.Direction = "Input";
                    objInfo8.PName = "@pUserID";
                    objInfo8.PValue = intUserid;
                    objInfo8.PDBType = "BigInt";
                    objInfo8.PSize = 200;

                    clsSqlParametersInfo objInfo9 = new clsSqlParametersInfo();
                    objInfo9.Direction = "Input";
                    objInfo9.PName = "@pIsCollaborative";
                    objInfo9.PValue = blnIsCollaborative;
                    objInfo9.PDBType = "Bit";
                    objInfo9.PSize = 200;

                    clsSqlParametersInfo objInfo10 = new clsSqlParametersInfo();
                    objInfo10.Direction = "Input";
                    objInfo10.PName = "@pNeedsAuthentication";
                    objInfo10.PValue = blnNeedAuth;
                    objInfo10.PDBType = "Bit";
                    objInfo10.PSize = 200;

                    clsSqlParametersInfo objInfo11 = new clsSqlParametersInfo();
                    objInfo11.Direction = "Input";
                    objInfo11.PName = "@pImageFile";
                    objInfo11.PValue = ImageFile;
                    objInfo11.PDBType = "Image";


                    clsSqlParametersInfo objInfo12 = new clsSqlParametersInfo();
                    objInfo12.Direction = "Output";
                    objInfo12.PName = "@pReturnMaxID";
                    objInfo12.PValue = -1;
                    objInfo12.PDBType = "BigInt";
                    objInfo12.PSize = 200;



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

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;
                    try
                    {
                        return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEModule", CSqlInfo).ToString());
                    }
                    catch (System.ServiceModel.EndpointNotFoundException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "AddModule(int intModId, string strModName, str..)", "ClsModuleLogic.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEModule", CSqlInfo).ToString());
                    }
                    catch (System.ServiceModel.CommunicationException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "AddModule(int intModId, string strModName, str..)", "ClsModuleLogic.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        return int.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spAEModule", CSqlInfo).ToString());
                    }

                }
                else
                {
                    return (new ClsModuleDataService().Add_Module(intModId, strModName, strModVersion, strDesc, strAssFile, strClassFile, strZipFile, intUserid, blnIsCollaborative, blnNeedAuth, ImageFile));
                }
            }
        }

        public ClsModuleLogic GetPodModule(int intModId)
        {
            ClsModuleLogic obj = new ClsModuleLogic();

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                DataSet ds = null;

                try
                {
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ID,ModuleName,IsCollaborative,ZipFile,AssemblyFile from Module where ID=" + intModId + "order by ModuleName").dsInfo;
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetPodModule(int intModId)", "ClsModuleLogic.cs");

                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ID,ModuleName,IsCollaborative,ZipFile,AssemblyFile from Module where ID=" + intModId + "order by ModuleName").dsInfo;
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetPodModule(int intModId)", "ClsModuleLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ID,ModuleName,IsCollaborative,ZipFile,AssemblyFile from Module where ID=" + intModId + "order by ModuleName").dsInfo;
                }
                if (!obj.MapData(ds.Tables[0])) obj = null;
            }
            else
            {

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    DataSet ds = null;

                    try
                    {
                        ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ID,ModuleName,IsCollaborative,ZipFile,AssemblyFile from Module where ID=" + intModId + "order by ModuleName").dsInfo;
                    }
                    catch (System.ServiceModel.EndpointNotFoundException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "GetPodModule(int intModId)", "ClsModuleLogic.cs");

                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ID,ModuleName,IsCollaborative,ZipFile,AssemblyFile from Module where ID=" + intModId + "order by ModuleName").dsInfo;
                    }
                    catch (System.ServiceModel.CommunicationException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "GetPodModule(int intModId)", "ClsModuleLogic.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        ds = VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ID,ModuleName,IsCollaborative,ZipFile,AssemblyFile from Module where ID=" + intModId + "order by ModuleName").dsInfo;
                    }
                    if (!obj.MapData(ds.Tables[0])) obj = null;

                }
                else
                {

                    DataSet ds = new ClsModuleDataService().Get_PodModule(intModId);
                    if (!obj.MapData(ds.Tables[0])) obj = null;
                }
            }
            return obj;
        }

        public bool ModuleExists(string strModName)
        {

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                objInfo.Direction = "Input";
                objInfo.PName = "@pModuleName";
                objInfo.PValue = strModName;
                objInfo.PDBType = "NVarChar";
                objInfo.PSize = 200;

                clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                objInfo2.Direction = "Output";
                objInfo2.PName = "@Result";
                objInfo2.PValue = -1;
                objInfo2.PDBType = "Bit";
                objInfo2.PSize = 200;

                lstSP.Add(objInfo);
                lstSP.Add(objInfo2);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;
                try
                {
                    return Convert.ToBoolean(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spModuleExists", CSqlInfo));
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "ModuleExists(string strModName)", "ClsModuleLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return bool.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spModuleExists", CSqlInfo).ToString());
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "ModuleExists(string strModName)", "ClsModuleLogic.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    return bool.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spModuleExists", CSqlInfo).ToString());
                }
            }
            else
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {

                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pModuleName";
                    objInfo.PValue = strModName;
                    objInfo.PDBType = "NVarChar";
                    objInfo.PSize = 200;

                    clsSqlParametersInfo objInfo2 = new clsSqlParametersInfo();
                    objInfo2.Direction = "Output";
                    objInfo2.PName = "@Result";
                    objInfo2.PValue = -1;
                    objInfo2.PDBType = "Bit";
                    objInfo2.PSize = 200;

                    lstSP.Add(objInfo);
                    lstSP.Add(objInfo2);

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;
                    try
                    {
                        return Convert.ToBoolean(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spModuleExists", CSqlInfo));
                    }
                    catch (System.ServiceModel.EndpointNotFoundException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "ModuleExists(string strModName)", "ClsModuleLogic.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        return bool.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spModuleExists", CSqlInfo).ToString());
                    }
                    catch (System.ServiceModel.CommunicationException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "ModuleExists(string strModName)", "ClsModuleLogic.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        return bool.Parse(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteReturnNonQuery("spModuleExists", CSqlInfo).ToString());
                    }
                }
                else
                {
                    return (new ClsModuleDataService().Module_Exists(strModName));
                }
            }
        }
        #endregion

    }
}
