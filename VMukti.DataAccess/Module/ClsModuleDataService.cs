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
using System.Data.SqlClient;
using System.Text;
using System;
using System.Windows.Media.Imaging;
using System.Drawing;
using System.IO;
using VMuktiAPI;

namespace VMukti.DataAccess
{
    public class ClsModuleDataService : ClsDataServiceBase
    {                
       
        public ClsModuleDataService() : base() { }

        public ClsModuleDataService(IDbTransaction txn) : base(txn) { }

        public DataSet GetModules()
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                {
                    return ExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where ModuleVersion like '1.0.%' order by ModuleName", CommandType.Text, null);
                }
                else
                {
                    return ExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module order by ModuleName", CommandType.Text, null);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetModules()", "clsModuleDataService.cs");
                return null;
            }
        }

        public DataSet GetNonCollaborativeModules()
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                {
                    return ExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where NeedsAuthentication='true'and IsCollaborative='false' and ModuleVersion like '1.0.' order by ModuleName", CommandType.Text, null);
                }
                else
                {
                    return ExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where NeedsAuthentication='true'and IsCollaborative='false' order by ModuleName", CommandType.Text, null);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetNonCollaborativeModules()", "clsModuleDataService.cs");               
                return null;
            }
        }

        public DataSet GetNonAuthenticatedModules()
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                {
                    return ExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where NeedsAuthentication='false' and IsDeleted='false' and ModuleVersion like '1.0.%' order by ModuleName", CommandType.Text, null);
                }
                else
                {
                    return ExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where NeedsAuthentication='false' and IsDeleted='false' order by ModuleName", CommandType.Text, null);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetNonAuthenticatedModules()", "clsModuleDataService.cs");                 
                return null;
            }
        }

        public DataSet GetCollaborativeModules(int intRoleId)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                {

                    return ExecuteDataSet("spGCModules",
                        CreateParameter("@pRoleId", SqlDbType.BigInt, intRoleId),
                        CreateParameter("@pVersion", SqlDbType.NVarChar, "1.0.%"));
                }
                else
                {
                    return ExecuteDataSet("spGCModules",
                       CreateParameter("@pRoleId", SqlDbType.BigInt, intRoleId),
                       CreateParameter("@pVersion", SqlDbType.NVarChar, " "));
                }

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetCollaborativeModules()", "clsModuleDataService.cs");                
                return null;
            }
        }

        public DataSet GetOnlyCollaborativeModules()
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                {
                    return ExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where IsCollaborative='true' and NeedsAuthentication='true' and IsDeleted='False' and ModuleVersion like '1.0.%' order by ModuleName", CommandType.Text, null);
                }
                else
                {
                    return ExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where IsCollaborative='true' and NeedsAuthentication='true' and IsDeleted='False' order by ModuleName", CommandType.Text, null);
                }

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetOnlyCollaborativeModules()", "clsModuleDataService.cs");                 
                return null;
            }
        }

        public DataSet GetCollaborativeCheckingModuleCount(int intRoleID, int ModuleCount)
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                {
                    return ExecuteDataSet("spGCModulesCount",
                        CreateParameter("@pRoleId", SqlDbType.BigInt, intRoleID),
                        CreateParameter("@pCount", SqlDbType.BigInt, ModuleCount),
                        CreateParameter("@pVersion", SqlDbType.NVarChar, "1.0.%"));
                }
                else
                {
                    return ExecuteDataSet("spGCModulesCount",
                         CreateParameter("@pRoleId", SqlDbType.BigInt, intRoleID),
                         CreateParameter("@pCount", SqlDbType.BigInt, ModuleCount),
                         CreateParameter("@pVersion", SqlDbType.NVarChar, " "));
                }
            }
            catch(Exception ex)
            {
                return null;
            }
        }
        public int Add_Module(int ModuleId, string strModName, string strModVersion, string strDesc, string strAssFile, string strClassFile, string strZipFile, int intUserid, bool blnIsCollaborative, bool blnNeedAuth, byte[] ImageFile)
        {
            try
            {
                SqlCommand cmd;
                int id;
                ExecuteNonQuery(out cmd, "spAEModule",
                CreateParameter("@pID", SqlDbType.BigInt, ModuleId),
                CreateParameter("@pModuleName", SqlDbType.NVarChar, strModName),
                CreateParameter("@pModuleVersion", SqlDbType.NVarChar, strModVersion),
                CreateParameter("@pDescription", SqlDbType.NVarChar, strDesc),
                CreateParameter("@pAssemblyFile", SqlDbType.NVarChar, strAssFile),
                CreateParameter("@pClassName", SqlDbType.NVarChar, strClassFile),
                CreateParameter("@pZipFile", SqlDbType.NVarChar, strZipFile),
                CreateParameter("@pUserID", SqlDbType.BigInt, intUserid),
                CreateParameter("@pIsCollaborative", SqlDbType.Bit, blnIsCollaborative),
                CreateParameter("@pNeedsAuthentication", SqlDbType.Bit, blnNeedAuth),
                CreateParameter("@pImageFile",SqlDbType.Image,ImageFile),
                CreateParameter("@pReturnMaxID", SqlDbType.BigInt, ParameterDirection.Output));

                id = int.Parse(cmd.Parameters["@pReturnMaxID"].Value.ToString());
                cmd.Dispose();
                return id;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Add_Module()", "ClsModuleDataService.cs");
                return 0;
            }
        }

        public DataSet Get_PodModule(int intModId)
        {
            try
            {
                return ExecuteDataSet("select ID,ModuleName,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where ID=" + intModId + "order by ModuleName", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Get_PodModule()", "ClsModuleDataService.cs");            
                return null;
            }
        }

        public bool Module_Exists(string strModName)
        {
            try
            {
                SqlCommand cmd;
                bool id;
                ExecuteNonQuery(out cmd, "spModuleExists",
                CreateParameter("@pModuleName", SqlDbType.NVarChar, strModName),
                CreateParameter("@Result", SqlDbType.Bit, ParameterDirection.Output));
                id = bool.Parse(cmd.Parameters["@Result"].Value.ToString());
                cmd.Dispose();
                return id;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Module_Exists", "clsModuleDataService.cs");
                return false;
            }
        }

        public DataSet GetNonAuthenticatedNonCollMod()
        {
            try
            {
                if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.0")
                {
                    return ExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where NeedsAuthentication='false' and IsDeleted='false' and IsCollaborative='false' and ModuleVersion like '1.0.%' order by ModuleName", CommandType.Text, null);
                }
                else
                {
                    return ExecuteDataSet("select ModuleName,id,IsCollaborative,ZipFile,AssemblyFile,ImageFile from Module where NeedsAuthentication='false' and IsDeleted='false' and IsCollaborative='false' order by ModuleName", CommandType.Text, null);
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetNonAuthenticatedNonCollMod", "clsModuleDataService.cs");
                return null;
            }
        }
     
    }
}