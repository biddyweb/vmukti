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
