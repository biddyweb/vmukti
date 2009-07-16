using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace CRMDesigner.DataAccess
{
    public class ClsCRMDataService : ClsDataServiceBase
    {
        public ClsCRMDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsCRMDataService(IDbTransaction txn) : base(txn) { }

        public DataSet CRM_GetByID(int ID)
        {
            //Returns the Record of Particular Id from CRM table through stored procedure.
            return ExecuteDataSet("spGCRM", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.BigInt, ID));
        }

        public int CRM_Save(ref int ID, string CRMURL, string CRMName, bool IsActive, int CreatedBy)
        {
            {
                //This function save the CRM.
                //Following paramateres must be passed to the Stored Procedure.
                SqlCommand cmd;
                ExecuteNonQuery(out cmd, "spAECRM",
                    CreateParameter("@pID", SqlDbType.Int, ID, ParameterDirection.Input),
                    CreateParameter("@pCRMURL", SqlDbType.VarChar, CRMURL),
                    CreateParameter("@pCRMName", SqlDbType.NVarChar, CRMName),
                    CreateParameter("@pIsActive", SqlDbType.Bit, IsActive),
                    CreateParameter("@pGivenID", SqlDbType.BigInt, CreatedBy),
                    CreateParameter("@pReturnMaxId", SqlDbType.BigInt, ParameterDirection.Output));


                ID = int.Parse(cmd.Parameters["@pReturnMaxId"].Value.ToString());

                cmd.Dispose();
                return ID;
            }

        }


        public void CRM_Delete(int ID)
        {
            //This function deletes the crm from the CRM table.
            ExecuteNonQuery("spDCRM", CreateParameter("@pID", SqlDbType.BigInt, ID));
        }

        public DataSet CRM_GetAll()
        {
            //return ExecuteDataSet("Select ID,CRMName from vCRM where IsDeleted=0;", CommandType.Text, null);
            return ExecuteDataSet("Select * from vCRM", CommandType.Text, null);
        }
    }
}
