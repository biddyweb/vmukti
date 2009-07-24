<<<<<<< HEAD:VMuktiModules/Call Center/ScriptRender/ScriptRender.DataAccess/ClsScriptRenderDataService.cs
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
=======
>>>>>>> b74131bacb80d82c79711cf70fe93af3fb611b40:VMuktiModules/Call Center/ScriptRender/ScriptRender.DataAccess/ClsScriptRenderDataService.cs
using System;
using System.Data;
using System.Data.SqlClient;
using ScriptRender.Common;


/// <summary>
/// Summary description for DemoDataService
/// </summary>

namespace ScriptRender.DataAccess
{

    public class ClsScriptRenderDataService : ClsDataServiceBase
    {
        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsScriptRenderDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsScriptRenderDataService(IDbTransaction txn) : base(txn) { }


       /* public DataSet Script_GetAll()
        {
            return ExecuteDataSet("Select * from vTrendWestLead;", CommandType.Text, null);

        }*/

        public string File_GetByID(Int64 CampaignID)
        {
            DataSet ds = ExecuteDataSet("spGFileName", CommandType.StoredProcedure, CreateParameter("@pCampaignID", SqlDbType.BigInt, CampaignID));
            DataTable dt = ds.Tables[0];
            DataRow dr = dt.Rows[0];
            return dr[0].ToString(); 
        }

        public DataSet Script_GetByID(Int64 LeadID)
        {
            return ExecuteDataSet("spGTrendWestLead", CommandType.StoredProcedure, CreateParameter("@pLeadID", SqlDbType.BigInt, LeadID));
        }

        public string Script_GetScriptType(long ScriptID)
        {
            try
            {
                return ExecuteDataSet("select ST.Scripttype from ScriptType ST, Script S where ST.ID = S.ScriptTypeID and S.ID ='"+ScriptID +"'", CommandType.Text, null).Tables[0].Rows[0].ItemArray[0].ToString();
            }
            catch(Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "script_GetScriptType", "ClsScriptRenderDataService.cs");
                return null;
            }
            
        }
    }
}