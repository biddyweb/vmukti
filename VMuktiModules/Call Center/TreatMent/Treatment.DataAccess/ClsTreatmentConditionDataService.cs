using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Treatment.DataAccess
{
    public class ClsTreatmentConditionDataService : ClsDataServiceBase
    {

        public ClsTreatmentConditionDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsTreatmentConditionDataService(IDbTransaction txn) : base(txn) { }


        //public DataSet TreatmentCondition_GetAll(int TreatmentID)
        //{
        //    return ExecuteDataSet("Select * from TreatmentCondition where TreatmentId=" + TreatmentID + ";", CommandType.Text, null);
        //}

        public DataSet TreatmentCondition_GetByTreatmentID(int TreatmentID)
        {
            return ExecuteDataSet("spGTreatmentCondition", CommandType.StoredProcedure, CreateParameter("@pTreatmentID", SqlDbType.Int, TreatmentID));
        }
        public DataSet TreatmentDisposition_GetByTreatmentID(int TreatmentID)
        {
            return ExecuteDataSet("spGTreatmentDisposition", CommandType.StoredProcedure, CreateParameter("@pTreatmentID", SqlDbType.Int, TreatmentID));
        }

        public void TreatmentCondition_DeleteByTreatmentID(int TreatmentID)
        {            
        }
        public void TreatmentCondition_SaveDisposition(ref int ID, int TreatmentID, string Duration, string FieldValues)
        {
            SqlCommand cmd;
            ExecuteNonQuery(out cmd, "spAETreatmentDisposition",
                CreateParameter("@pID", SqlDbType.Int, ID),
                CreateParameter("@pTreatmentID", SqlDbType.BigInt, TreatmentID),
                CreateParameter("@pDuration", SqlDbType.VarChar, Duration, 50),                
                CreateParameter("@pFieldValues", SqlDbType.VarChar, FieldValues));
            cmd.Dispose();
        }

        public void TreatmentCondition_Save(ref int ID, int TreatmentID, string LeadFormatName, string FieldName ,string Operator, string DataType, string FieldValues)        {
            SqlCommand cmd;
            ExecuteNonQuery(out cmd, "spAETreatmentCondition",
                CreateParameter("@pID", SqlDbType.Int, ID),
                CreateParameter("@pTreatmentID", SqlDbType.BigInt , TreatmentID),
                CreateParameter("@pLeadFormatID", SqlDbType.NVarChar, LeadFormatName),
                CreateParameter("@pFieldName", SqlDbType.VarChar , FieldName,50),
                CreateParameter("@pOperator", SqlDbType.VarChar ,Operator,50 ),
                CreateParameter("@pDataType", SqlDbType.VarChar , DataType,50 ),
                CreateParameter("@pFieldValues", SqlDbType.VarChar ,FieldValues));
            cmd.Dispose();
        }
        public void TreatmentCondition_Delete(int ID)
        {
            ExecuteNonQuery("spDTreatmentCondition", CreateParameter("@pID", SqlDbType.Int, ID));
        }
    }
}
