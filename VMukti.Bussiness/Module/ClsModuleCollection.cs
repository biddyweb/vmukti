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
using System.Drawing;
using System.IO;



namespace VMukti.Business
{
    public class ClsModuleCollection : ClsBaseCollection<ClsModuleLogic>
    {

        public static ClsModuleCollection GetAll()
        {
            ClsModuleCollection obj = new ClsModuleCollection();

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                try
                {
                    if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")  ///Show Only collaborative modules who contains ModuleVersion=1.0.0.0
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                    }
                    else ///show all modules present in Vmukti database
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module order by ModuleName").dsInfo);
                    }
                }

                #region Catch block
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetAll()", "clsModuleCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")  ///Show Only collaborative modules who contains ModuleVersion=1.0.0.0
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                    }
                    else ///show all modules present in Vmukti database
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module order by ModuleName").dsInfo);
                    }
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetAll()", "clsModuleCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")  ///Show Only collaborative modules who contains ModuleVersion=1.0.0.0
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                    }
                    else ///show all modules present in Vmukti database
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module order by ModuleName").dsInfo);
                    }
                }
                #endregion
            }
            else
            {

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    try
                    {
                        if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")  ///Show Only collaborative modules who contains ModuleVersion=1.0.0.0
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                        }
                        else
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module order by ModuleName").dsInfo);
                        }
                    }

                    #region Catch Block
                    catch (System.ServiceModel.EndpointNotFoundException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "GetAll()", "clsModuleCollection.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")  ///Show Only collaborative modules who contains ModuleVersion=1.0.0.0
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                        }
                        else
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module order by ModuleName").dsInfo);
                        }
                    }
                    catch (System.ServiceModel.CommunicationException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "GetAll()", "clsModuleCollection.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")  ///Show Only collaborative modules who contains ModuleVersion=1.0.0.0
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                        }
                        else
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module order by ModuleName").dsInfo);
                        }
                    }
                    #endregion
                }
                else
                {
                    obj.MapObjects(new ClsModuleDataService().GetModules());
                }
            }
            return obj;
        }

        public static ClsModuleCollection GetNCMod()
        {
            ClsModuleCollection obj = new ClsModuleCollection();

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                try
                {
                    if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName, id, IsCollaborative, ZipFile, AssemblyFile,ImageFile from Module where NeedsAuthentication='true' and IsCollaborative='false' and ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                    }
                    else
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName, id, IsCollaborative, ZipFile, AssemblyFile,ImageFile from Module where NeedsAuthentication='true' and IsCollaborative='false' order by ModuleName").dsInfo);
                    }
                }
                #region Catch Block
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetNCMod()", "clsModuleCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile from Module where NeedsAuthentication='true' and IsCollaborative='false' and ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                    }
                    else
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile from Module where NeedsAuthentication='true' and IsCollaborative='false' order by ModuleName").dsInfo);
                    }
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetNCMod()", "clsModuleCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile from Module where NeedsAuthentication='true'and IsCollaborative='false' and ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                    }
                    else
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile from Module where NeedsAuthentication='true'and IsCollaborative='false' order by ModuleName").dsInfo);
                    }
                }
                #endregion
            }
            else
            {

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    try
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName, id, IsCollaborative, ZipFile, AssemblyFile,ImageFile from Module where NeedsAuthentication='true'and IsCollaborative='false' order by ModuleName").dsInfo);
                    }

                    #region Catch Block
                    catch (System.ServiceModel.EndpointNotFoundException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "GetNCMod()", "clsModuleCollection.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile from Module where NeedsAuthentication='true'and IsCollaborative='false' and ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                        }
                        else
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile from Module where NeedsAuthentication='true'and IsCollaborative='false' order by ModuleName").dsInfo);
                        }
                    }
                    catch (System.ServiceModel.CommunicationException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "GetNCMod()", "clsModuleCollection.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile from Module where NeedsAuthentication='true'and IsCollaborative='false' and ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                        }
                        else
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile from Module where NeedsAuthentication='true'and IsCollaborative='false' order by ModuleName").dsInfo);
                        }
                    }
                    #endregion
                }
                else
                {
                    obj.MapObjects(new ClsModuleDataService().GetNonCollaborativeModules());
                }
            }
            return obj;
        }

        public static ClsModuleCollection GetNonAuthenticatedMod()
        {
            ClsModuleCollection obj = new ClsModuleCollection();

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                try
                {
                    if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where NeedsAuthentication='false' and IsDeleted='false' and ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                    }
                    else
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where NeedsAuthentication='false' and IsDeleted='false' order by ModuleName").dsInfo);
                    }
                }
                #region Catch Block
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetNonAuthenticatedMod()", "clsModuleCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile from Module where NeedsAuthentication='false' and ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                    }
                    else
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile from Module where NeedsAuthentication='false' order by ModuleName").dsInfo);
                    }
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetNonAuthenticatedMod()", "clsModuleCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile from Module where NeedsAuthentication='false' and ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                    }
                    else
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile from Module where NeedsAuthentication='false' order by ModuleName").dsInfo);
                    }
                }
                #endregion
            }
            else
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    try
                    {
                        if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where NeedsAuthentication='false' and IsDeleted='false' and ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                        }
                        else
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where NeedsAuthentication='false' and IsDeleted='false' order by ModuleName").dsInfo);
                        }
                    }

                    #region Catch Block
                    catch (System.ServiceModel.EndpointNotFoundException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "GetNonAuthenticatedMod()", "clsModuleCollection.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile from Module where NeedsAuthentication='false' and ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                        }
                        else
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile from Module where NeedsAuthentication='false' order by ModuleName").dsInfo);
                        }
                    }
                    catch (System.ServiceModel.CommunicationException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "GetNonAuthenticatedMod()", "clsModuleCollection.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile from Module where NeedsAuthentication='false' and ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                        }
                        else
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile from Module where NeedsAuthentication='false' order by ModuleName").dsInfo);
                        }
                    }
                    #endregion
                }
                else
                {
                    obj.MapObjects(new ClsModuleDataService().GetNonAuthenticatedModules());
                }
            }
            return obj;
        }


        public static ClsModuleCollection GetCMod(int intRoleId)
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

                clsSqlParametersInfo objVersionInfo = new clsSqlParametersInfo();
                objVersionInfo.Direction = "Input";
                objVersionInfo.PName = "@pVersion";
                if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                {
                    objVersionInfo.PValue = "1.0.%";
                }
                else
                {
                    objVersionInfo.PValue = " ";
                }
                objVersionInfo.PDBType = "varchar";
                objVersionInfo.PSize = 10;
                lstSP.Add(objVersionInfo);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;

                try
                {
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGCModules", CSqlInfo).dsInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetCMod(int intRoleId)", "clsModuleCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGCModules", CSqlInfo).dsInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetCMod(int intRoleId)", "clsModuleCollection.cs");
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

                    clsSqlParametersInfo objVersionInfo = new clsSqlParametersInfo();
                    objVersionInfo.Direction = "Input";
                    objVersionInfo.PName = "@pVersion";
                    if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                    {
                        objVersionInfo.PValue = "1.0.%";
                    }
                    else
                    {
                        objVersionInfo.PValue = " ";
                    }
                    objVersionInfo.PDBType = "varchar";
                    objVersionInfo.PSize = 10;
                    lstSP.Add(objVersionInfo);

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;

                    try
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGCModules", CSqlInfo).dsInfo);
                    }
                    catch (System.ServiceModel.EndpointNotFoundException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "GetCMod(int intRoleId)", "clsModuleCollection.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGCModules", CSqlInfo).dsInfo);
                    }
                    catch (System.ServiceModel.CommunicationException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "GetCMod(int intRoleId)", "clsModuleCollection.cs");
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

        public static ClsModuleCollection GetCModCountCheck(int intRoleId,int intMCount)
        {
            ClsModuleCollection obj = new ClsModuleCollection();
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

                clsSqlParametersInfo objInfoCount = new clsSqlParametersInfo();
                objInfoCount.Direction = "Input";
                objInfoCount.PName = "@pCount";
                objInfoCount.PValue = intMCount;
                objInfoCount.PDBType = "BigInt";
                objInfoCount.PSize = 200;
                lstSP.Add(objInfoCount);

                clsSqlParametersInfo objVersionInfo = new clsSqlParametersInfo();
                objVersionInfo.Direction = "Input";
                objVersionInfo.PName = "@pVersion";
                if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                {
                    objVersionInfo.PValue = "1.0.%";
                }
                else
                {
                    objVersionInfo.PValue = " ";
                }
                objVersionInfo.PDBType = "varchar";
                objVersionInfo.PSize = 10;
                lstSP.Add(objVersionInfo);

                clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                CSqlInfo.objParam = lstSP;

                try
                {
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGCModulesCount", CSqlInfo).dsInfo);
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetCModCountCheck(int intRoleId,int intMCount)", "clsModuleCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGCModulesCount", CSqlInfo).dsInfo);
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetCModCountCheck(int intRoleId,int intMCount)", "clsModuleCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGCModulesCount", CSqlInfo).dsInfo);
                }

            }
            else
            {
                obj.MapObjects(new ClsModuleDataService().GetCollaborativeCheckingModuleCount(intRoleId, intMCount));
            }
            return obj;
        }

        public static ClsModuleCollection GetOnlyCollMod()
        {
            ClsModuleCollection obj = new ClsModuleCollection();

            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                try
                {
                    if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where IsCollaborative='true' and NeedsAuthentication='true' and IsDeleted='False' and ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                    }
                    else
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where IsCollaborative='true' and NeedsAuthentication='true' and IsDeleted='False' order by ModuleName").dsInfo);
                    }
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetOnlyCollMod", "clsModuleCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where IsCollaborative='true' and NeedsAuthentication='true' and IsDeleted='False' and ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                    }
                    else
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where IsCollaborative='true' and NeedsAuthentication='true' and IsDeleted='False' order by ModuleName").dsInfo);
                    }
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetOnlyCollMod", "clsModuleCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where IsCollaborative='true' and NeedsAuthentication='true' and IsDeleted='False' and ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                    }
                    else
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where IsCollaborative='true' and NeedsAuthentication='true' and IsDeleted='False' order by ModuleName").dsInfo);
                    }
                }
            }
            else
            {

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    try
                    {
                        if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where IsCollaborative='true' and NeedsAuthentication='true' and IsDeleted='False' and ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                        }
                        else
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where IsCollaborative='true' and NeedsAuthentication='true' and IsDeleted='False' order by ModuleName").dsInfo);
                        }
                    }
                    #region Catch Block
                    catch (System.ServiceModel.EndpointNotFoundException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "GetOnlyCollMod", "clsModuleCollection.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where IsCollaborative='true' and NeedsAuthentication='true' and IsDeleted='False' and ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                        }
                        else
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where IsCollaborative='true' and NeedsAuthentication='true' and IsDeleted='False' order by ModuleName").dsInfo);
                        }
                    }
                    catch (System.ServiceModel.CommunicationException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "GetOnlyCollMod", "clsModuleCollection.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where IsCollaborative='true' and NeedsAuthentication='true' and IsDeleted='False' and ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                        }
                        else
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where IsCollaborative='true' and NeedsAuthentication='true' and IsDeleted='False' order by ModuleName").dsInfo);
                        }
                    }
                    #endregion
                }
                else
                {
                    obj.MapObjects(new ClsModuleDataService().GetOnlyCollaborativeModules());
                }
            }
            return obj;
        }

        public static ClsModuleCollection GetNonAuthenticatedNonCollMod()
        {
            ClsModuleCollection obj = new ClsModuleCollection();
            if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
            {
                try
                {
                    if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where NeedsAuthentication='false' and IsDeleted='false' and IsCollaborative='false' and ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                    }
                    else
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where NeedsAuthentication='false' and IsDeleted='false' and IsCollaborative='false' order by ModuleName").dsInfo);
                    }
                }
                catch (System.ServiceModel.EndpointNotFoundException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetNonAuthenticatedNonCollMod", "clsModuleCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where NeedsAuthentication='false' and IsDeleted='false' and IsCollaborative='false' and ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                    }
                    else
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where NeedsAuthentication='false' and IsDeleted='false' and IsCollaborative='false' order by ModuleName").dsInfo);
                    }
                }
                catch (System.ServiceModel.CommunicationException e)
                {
                    VMuktiHelper.ExceptionHandler(e, "GetNonAuthenticatedNonCollMod", "clsModuleCollection.cs");
                    VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                    if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where NeedsAuthentication='false' and IsDeleted='false' and IsCollaborative='false' and ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                    }
                    else
                    {
                        obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where NeedsAuthentication='false' and IsDeleted='false' and IsCollaborative='false' order by ModuleName").dsInfo);
                    }
                }
            }
            else
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    try
                    {
                        if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where NeedsAuthentication='false' and IsDeleted='false' and IsCollaborative='false' and ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                        }
                        else
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where NeedsAuthentication='false' and IsDeleted='false' and IsCollaborative='false' order by ModuleName").dsInfo);
                        }
                    }
                    #region Catch
                    catch (System.ServiceModel.EndpointNotFoundException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "GetNonAuthenticatedNonCollMod", "clsModuleCollection.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where NeedsAuthentication='false' and IsDeleted='false' and IsCollaborative='false' and ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                        }
                        else
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where NeedsAuthentication='false' and IsDeleted='false' and IsCollaborative='false' order by ModuleName").dsInfo);
                        }
                    }
                    catch (System.ServiceModel.CommunicationException e)
                    {
                        VMuktiHelper.ExceptionHandler(e, "GetNonAuthenticatedNonCollMod", "clsModuleCollection.cs");
                        VMuktiAPI.VMuktiHelper.CallEvent("GetSuperNodeIP", null, null);
                        if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where NeedsAuthentication='false' and IsDeleted='false' and IsCollaborative='false' and ModuleVersion like '1.0.%' order by ModuleName").dsInfo);
                        }
                        else
                        {
                            obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where NeedsAuthentication='false' and IsDeleted='false' and IsCollaborative='false' order by ModuleName").dsInfo);
                        }
                    }
                    #endregion

                }
                else
                {
                    obj.MapObjects(new ClsModuleDataService().GetNonAuthenticatedNonCollMod());
                }
            }
            return obj;
        }

    }
}
