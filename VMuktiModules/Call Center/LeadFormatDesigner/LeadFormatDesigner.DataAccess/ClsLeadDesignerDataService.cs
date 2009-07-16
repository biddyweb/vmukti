using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using LeadFormatDesigner.Common;
using VMuktiAPI;

namespace LeadFormatDesigner.DataAccess
{
    public class ClsLeadDesignerDataService : ClsDataServiceBase 
    {
        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService 
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsLeadDesignerDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsLeadDesignerDataService(IDbTransaction txn) : base(txn) { }

        public int LeadDesigner_Save(Int64 ID,string FieldName, string FieldType, Int64 FieldSize, bool required)
        {
            try
            {

                int RowInserted;
                ExecuteNonQuery(out RowInserted, "spInsertCustomeFields",
                    CreateParameter("@pID", SqlDbType.BigInt, ID),
                    CreateParameter("@pFieldName", SqlDbType.VarChar, FieldName, 50),
                    CreateParameter("@pFieldType", SqlDbType.VarChar, FieldType, 50),
                    CreateParameter("@FieldSize", SqlDbType.BigInt, FieldSize),
                    CreateParameter("@pIsRequired", SqlDbType.Bit, required));
                
                //cmd.Dispose();
                return RowInserted;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "LeadDesigner_Save()", "ClsLeadDesignerDataService.cs");
                return 0;
            }
        }

        public int LeadFormat_Save(Int64 ID, string LeadFormatName, string FormatType, string Description)
        {
            try
            {

                int RowInserted=-1;
                RowInserted=ExecuteNonQuery(out RowInserted,"spInsertLeadFormat", CreateParameter("@pid", SqlDbType.BigInt, RowInserted),
                    CreateParameter("@pLeadFormatName", SqlDbType.NVarChar, LeadFormatName, 50),
                    CreateParameter("@pLeadFormatType", SqlDbType.NVarChar, FormatType, 50),
                    CreateParameter("@pDescription", SqlDbType.VarChar,Description,150));

                //ID = ExecuteNonQuery(out ID, "spInsertLeadFormat",
                //    CreateParameter("@pid", SqlDbType.BigInt, ID),
                //    CreateParameter("@pLeadFormatName", SqlDbType.NVarChar, LeadFormatName, 50),
                //    CreateParameter("@pLeadFormatType", SqlDbType.NVarChar, FormatType, 50),
                //    CreateParameter("@pDescription", SqlDbType.VarChar,Description,150)
                //    );
                //cmd.Dispose();
                //RowInserted = int.Parse(ID.ToString());

                return RowInserted;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "LeadFormat_Save()", "ClsLeadDesignerDataService.cs");
                return 0;
            }
        }

        public int LeadFormatDesigner_Save(Int64 ID,Int64 LeadFormatID,Int64 CustomeFieldID, string DefaultValue,bool IsRequired,int StartPosition,int Length,string Delimiter)
        {
            try
              {

                int RowInserted;
                ExecuteNonQuery(out RowInserted, "spInsertLeadFormatDesigner",
                    CreateParameter("@pID", SqlDbType.BigInt, ID),
                    CreateParameter("@pLeadformatID", SqlDbType.BigInt, LeadFormatID),
                    CreateParameter("@pCustomeFieldID", SqlDbType.BigInt, CustomeFieldID),
                    CreateParameter("@pDefaultValue", SqlDbType.VarChar, DefaultValue, 50),
                    CreateParameter("@pIsRequired", SqlDbType.Bit, IsRequired),
                    CreateParameter("@pStartPosition", SqlDbType.BigInt, StartPosition),
                    CreateParameter("@pLength", SqlDbType.BigInt, Length),
                    CreateParameter("@pDelimiters", SqlDbType.VarChar, Delimiter, 20));

                //cmd.Dispose();
                return RowInserted;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "LeadFormatDesigner_Save()", "ClsLeadDesignerDataService.cs");
                return 0;
            }
        }

        
        public DataSet LeadDesignerFields_Get()
        {
            try
            {
                return ExecuteDataSet("Select distinct(FieldName),ID from LeadCustomFields order by FieldName;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "LeadDesignerFields_Get()", "ClsLeadDesignerDataService.cs");
                return null;
            }
        }

        public DataSet LeadFormatFields_Get()
        {
            try
            {
                return ExecuteDataSet("Select * from LeadFormat order by ID;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "LeadFormatFields_Get()", "ClsLeadDesignerDataService.cs");
                return null;
            }

        }

        public DataSet LeadFormat_GetByID(Int64 ID)
        {
            try
            {
                return ExecuteDataSet("spSelectLeadFormat", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.BigInt, ID));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "LeadFormat_GetByID()", "ClsLeadDesignerDataService.cs");
                return null;
            }
        }
        public int DeleteFormatField_ByID(Int64 ForamtFieldID)
        {
            int RowAffedted=-1;
            RowAffedted = ExecuteNonQuery(out RowAffedted, "spDLeadFields", CreateParameter("@pID", SqlDbType.BigInt, ForamtFieldID), CreateParameter("@cntLeadFormatField", SqlDbType.BigInt, RowAffedted));
            return RowAffedted;
        }
        public int DeleteFormat_ByID(Int64 FormatID)
        {
            try
            {
                int RowAffedted=-1;
                RowAffedted = ExecuteNonQuery(out RowAffedted, "spDLeadFormat", CreateParameter("@pID", SqlDbType.BigInt, FormatID), CreateParameter("@cntLeadFormat", SqlDbType.BigInt, RowAffedted));
                return RowAffedted;
                //return ExecuteNonQuery("spDLeadFormat", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.BigInt, FormatID), CreateParameter("@cntLeadFormat", SqlDbType.BigInt, RowAffedted));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DeleteFormat_ByID()", "ClsLeadDesignerDataService.cs");
                return -1;
            }
        }
        public DataSet LeadFields_GetByID(Int64 ID)
        {
            try
            {
                return ExecuteDataSet("spSelectLeadFields", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.BigInt, ID));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "LeadFields_GetByID()", "ClsLeadDesignerDataService.cs");
                return null;
            }
        }

        public DataSet LeadFields_GetByFormatID(Int64 ID)
        {
            try
            {
                return ExecuteDataSet("spSelectLeadFieldsByFormatID", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.BigInt, ID));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "LeadFields_GetByFormatID()", "ClsLeadDesignerDataService.cs");
                return null;
            }
        }
    }
}
