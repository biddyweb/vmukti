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
using System.Data.SqlClient;
using VMuktiAPI;
using System.Text;

namespace VMukti.DataAccess
{
    public class ClsRecordingDataService : ClsRecordingDataServiceBase
    {
      
        public void User_Save(ref int ID, string ServerIP, string ServerPort, string UserName, string DirectoryName, string Password,string strTag)
        {
            try
            {
                SqlCommand cmd;
                ExecuteNonQuery(out cmd, "spAERecordingFTPServer",
                    CreateParameter("@pID", SqlDbType.Int, ID),
                    CreateParameter("@pServerIP", SqlDbType.NVarChar, ServerIP),
                    CreateParameter("@pServerPort", SqlDbType.NVarChar, ServerPort),
                    CreateParameter("@pUserName", SqlDbType.NVarChar, UserName),
                    CreateParameter("@pDirectoryName", SqlDbType.NVarChar, DirectoryName),
                    CreateParameter("@pPassword", SqlDbType.NVarChar, Password),
                    CreateParameter("@pTag", SqlDbType.NVarChar, strTag));

                cmd.Dispose();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name, "clsRecordingDataServiceBase.cs");               
            }
        }

        public void SetFTPDetails(string strTag)
        {
            try
            { 
                SqlCommand cmd;
                DataSet myDS = ExecuteDataSet(out cmd, "spAEGetRecordingFTPDetail",
                    CreateParameter("@pTag", SqlDbType.NVarChar, strTag, ParameterDirection.Input));
                cmd.Dispose();

                if (myDS.Tables[0].Rows.Count > 0)
                {
                    VMuktiInfo.FTPServerIP = myDS.Tables[0].Rows[0][1].ToString();
                    VMuktiInfo.FTPPort= myDS.Tables[0].Rows[0][2].ToString();
                    VMuktiInfo.FTPUserName = myDS.Tables[0].Rows[0][3].ToString();
                    VMuktiInfo.FTPDirPath = myDS.Tables[0].Rows[0][4].ToString();
                    VMuktiInfo.FTPPassword = myDS.Tables[0].Rows[0][5].ToString();
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name, "clsUserDataService.cs");
            }
        }
    }
}
