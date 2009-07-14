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
using System.Text;
using VMuktiAPI;

namespace VMukti.DataAccess
{
    public class ClsUserDataService : ClsDataServiceBase
    {        
       
        public ClsUserDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsUserDataService(IDbTransaction txn) : base(txn) { }


        public DataSet User_GetAll()
        {
            try
            {
                return ExecuteDataSet("Select UserInfo.*,Payroll.* from UserInfo left outer join Payroll on UserInfo.Id=Payroll.UserId where UserInfo.IsDeleted=0;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "User_GetAll()", "clsUserDataService.cs");               
                return null;
            }
        }

        public DataSet User_GetByID(int ID)
        {
            try
            {
                return ExecuteDataSet("spGUserInfoPayroll", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.Int, ID));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "User_GetById()", "clsUserDataService.cs");               
                return null;
            }
        }

        public DataSet User_GetByUNamePass(string UName, string Pass, ref bool Result)
        {
            try
            {
                SqlCommand cmd;
                DataSet ds = ExecuteDataSet(out cmd, "spGUserInfoPayroll", CommandType.StoredProcedure,
                    CreateParameter("@pID", SqlDbType.Int, -1),
                    CreateParameter("@pUName", SqlDbType.VarChar, UName),
                    CreateParameter("@pPass", SqlDbType.VarChar, Pass),
                    CreateParameter("@pResult", SqlDbType.Bit, false, ParameterDirection.InputOutput));

                Result = (bool)cmd.Parameters["@pResult"].Value;
                cmd.Dispose();
                return ds;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "User_GetByUNamePass()", "clsUserDataService.cs");               
                return null;
            }
        }

        public void User_LogOff(Int64 UserID)
        {
            try
            {
                ExecuteDataSet("spDOnlineUsers", CommandType.StoredProcedure,
                    CreateParameter("@pID", SqlDbType.BigInt, -1),
                    CreateParameter("@pUserID", SqlDbType.BigInt, UserID));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "User_LogOff()", "clsUserDataService.cs");               
            }
        }

        public int User_Save(ref int ID, string displayame,int roleID,string firstName,string lastName,string eMail,string password,bool isActive,int byUserId,float ratePerHour,float overTimeRate,float doubleRatePerHour,float doubleOverTimeRate )
        {
            try
            {
                SqlCommand cmd;
                ExecuteNonQuery(out cmd, "spAEUserInfoPayroll",
                    CreateParameter("@pID", SqlDbType.Int, ID),
                    CreateParameter("@pDisplayName", SqlDbType.NVarChar, displayame, 100),
                    CreateParameter("@pRoleID", SqlDbType.BigInt, roleID),
                    CreateParameter("@pFirstName", SqlDbType.NVarChar, firstName, 50),
                    CreateParameter("@pLastName", SqlDbType.NVarChar, lastName, 50),
                    CreateParameter("@pEMail", SqlDbType.NVarChar, eMail, 256),
                    CreateParameter("@pPassword", SqlDbType.NVarChar, password, 50),
                    CreateParameter("@pIsActive", SqlDbType.Bit, isActive),
                    CreateParameter("@pByUserID", SqlDbType.BigInt, byUserId),
                    CreateParameter("@pRatePerHour", SqlDbType.Float, ratePerHour),
                    CreateParameter("@pOverTimeRate", SqlDbType.Float, overTimeRate),
                    CreateParameter("@pDoubleRatePerHour", SqlDbType.Float, doubleRatePerHour),
                    CreateParameter("@pDoubleOverTimeRate", SqlDbType.Float, doubleOverTimeRate),
                    CreateParameter("@pReturnId", SqlDbType.BigInt, ParameterDirection.Output));
                int retID = int.Parse(cmd.Parameters["@pReturnId"].Value.ToString());

                cmd.Dispose();
                return retID;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "User_Save()", "clsUserDataService.cs");               
                return 0;
            }
        }

        public void User_Delete(int ID)
        {
            try
            {
                ExecuteNonQuery("spDUserInfoPayroll", CreateParameter("@pID", SqlDbType.Int, ID));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "User_Delete()", "clsUserDataService.cs");                           
            }
        }
       
        //Following 3 functions has been added by Alpa for UserActivity.
        public void User_InsertRecord(int uid)
        {
            try
            {
                ExecuteDataSet("spLogin", CommandType.StoredProcedure,
                    CreateParameter("@uid", SqlDbType.Int, uid));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "User_InsertRecord()", "clsUserDataService.cs");               
            }
        }

        public void User_AddRecord()
        {
            try
            {
                ExecuteDataSet("spSignUp", CommandType.StoredProcedure, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "User_AddRecord()", "clsUserDataService.cs");               
            }
        }

        public void User_AddNewRecord(int uid)
        {
            try
            {
                ExecuteDataSet("spLogout", CommandType.StoredProcedure,
                    CreateParameter("@uid", SqlDbType.Int, uid));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "User_AddNewRecord()", "clsUserDataService.cs");               
            }
        }

        public DataSet User_FindBuddy(string username, string email)
        //public DataSet User_FindBuddy()
        {
            try
            {
                return ExecuteDataSet("spFindBuddy", CreateParameter("@uname", SqlDbType.NVarChar, username, 100), CreateParameter("@email", SqlDbType.NVarChar, email, 256));

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "User_FindBuddy", "ClsUserDataService");
                return null;

            }
        }

    }
}
