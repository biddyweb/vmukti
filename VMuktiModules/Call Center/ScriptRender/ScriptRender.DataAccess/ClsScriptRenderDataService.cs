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