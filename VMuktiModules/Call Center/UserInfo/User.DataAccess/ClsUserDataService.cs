/* VMukti 2.0 -- An Open Source Video Communications Suite
*
* Copyright (C) 2008 - 2009, VMukti Solutions Pvt. Ltd.
*
* Hardik Sanghvi <hardik@vmukti.com>
*
* See http://www.vmukti.com for more information about
* the VMukti project. Please do not directly contact
* any of the maintainers of this project for assistance;
<<<<<<< HEAD:VMuktiModules/Call Center/UserInfo/User.DataAccess/ClsUserDataService.cs
* the project provides a web site, forums and mailing lists      
=======
* the project provides a web site, forums and mailing lists
>>>>>>> b74131bacb80d82c79711cf70fe93af3fb611b40:VMuktiModules/Call Center/UserInfo/User.DataAccess/ClsUserDataService.cs
* for your use.

* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
<<<<<<< HEAD:VMuktiModules/Call Center/UserInfo/User.DataAccess/ClsUserDataService.cs
* the Free Software Foundation, either version 3 of the License, or
=======
* the Free Software Foundation, either version 2 of the License, or
>>>>>>> b74131bacb80d82c79711cf70fe93af3fb611b40:VMuktiModules/Call Center/UserInfo/User.DataAccess/ClsUserDataService.cs
* (at your option) any later version.

* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU General Public License for more details.

* You should have received a copy of the GNU General Public License
* along with this program. If not, see <http://www.gnu.org/licenses/>
<<<<<<< HEAD:VMuktiModules/Call Center/UserInfo/User.DataAccess/ClsUserDataService.cs
 
=======

>>>>>>> b74131bacb80d82c79711cf70fe93af3fb611b40:VMuktiModules/Call Center/UserInfo/User.DataAccess/ClsUserDataService.cs
*/
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace User.DataAccess
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
            return ExecuteDataSet("Select * from vUserInfo;", CommandType.Text, null);
        }

        public DataSet Role_GetAll()
        {
            return ExecuteDataSet("Select * from vRoles;", CommandType.Text, null);
        }

        public DataSet User_GetByID(int ID)
        {
            return ExecuteDataSet("spGUserInfoPayroll", CommandType.StoredProcedure, 
                CreateParameter("@pID", SqlDbType.Int, ID),
                CreateParameter("@pUName", SqlDbType.VarChar, ""),
                CreateParameter("@pPass", SqlDbType.VarChar, ""));
        }

        public int User_Save(ref int ID, string displayame,int roleID,string firstName,string lastName,string eMail,string password,bool isActive,int byUserId,float ratePerHour,float overTimeRate,float doubleRatePerHour,float doubleOverTimeRate )
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

        public void User_Delete(int ID)
        {
            ExecuteNonQuery("spDUser", CreateParameter("@pID", SqlDbType.Int, ID));
        }

    }
}
