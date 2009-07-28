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
using System.Collections.Generic;
//using System.Xml.Linq;
using System.Text;
using System.Data;
using Script.Common;
using System.Data.SqlClient;
using VMuktiAPI;

namespace Script.DataAccess
{
    public class ClsScriptDataService : ClsDataServiceBase
    {
        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService
        /// </summary>
        ///////////////////////////////////////////////////////////////////////
        public ClsScriptDataService() : base() { }

         ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsScriptDataService(IDbTransaction txn) : base(txn) { }

        public DataSet Script_GetAll()
        {
            //return ExecuteDataSet("select * from vScript_ScriptType;", CommandType.Text, null);
            return ExecuteDataSet("select * from vScript_ScriptType;", CommandType.Text, null);
        }
        public DataSet ScriptType_GetAll()
        {
            return ExecuteDataSet("Select * from vScriptType;", CommandType.Text, null);

        }
        
        
      



        public DataSet Script_GetByID(int ID)
        {
            return ExecuteDataSet("spGScript", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.BigInt, ID));
        }

        public int Script_Save(ref int ID, string ScriptURL, string ScriptName, int ScriptTypeID,bool IsActive, int CreatedBy)
        {
            {
                SqlCommand cmd;
                ExecuteNonQuery(out cmd, "spAEScript",
                    CreateParameter("@pID", SqlDbType.Int, ID, ParameterDirection.Input),
                    CreateParameter("@pScriptURL", SqlDbType.VarChar, ScriptURL),
                    CreateParameter("@pScriptName", SqlDbType.NVarChar, ScriptName),
                    CreateParameter("@pScriptTypeID", SqlDbType.BigInt, ScriptTypeID),
                    CreateParameter("@pIsActive", SqlDbType.Bit, IsActive),
                    CreateParameter("@pGivenID", SqlDbType.BigInt, CreatedBy),

                    CreateParameter("@pReturnMaxId", SqlDbType.BigInt, ParameterDirection.Output));


                ID = int.Parse(cmd.Parameters["@pReturnMaxId"].Value.ToString());

                cmd.Dispose();
                return ID;
            }
           
        }

        
        public void Script_Delete(int ID)
        {
            ExecuteNonQuery("spDScript", CreateParameter("@pID", SqlDbType.BigInt, ID));
        }

    } //class
}


















    

