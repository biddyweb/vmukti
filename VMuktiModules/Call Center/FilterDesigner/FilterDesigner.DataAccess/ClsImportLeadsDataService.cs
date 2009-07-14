using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.SqlTypes;
//using System.Linq;
using System.Text;
using System.Xml;
using VMuktiAPI;
namespace FilterDesigner.DataAccess
{
    public class ClsImportLeadsDataService : ClsDataServiceBase
    {

        public ClsImportLeadsDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsImportLeadsDataService(IDbTransaction txn) : base(txn) { }


        public DataSet List_GetAll(bool state)
        {
            try
            {
                return ExecuteDataSet("Select ID,ListName,IsActive,IsDNCList from CallingList where IsDNCList ='" + state + "' and IsDeleted=0 order by IsActive Desc,ListName Asc;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "List_GetAll()", "ClsImportLeadsDataService.cs");
                return null;
            }
        }

        public DataSet GetLeadFormat()
        {
            try
            {
                return ExecuteDataSet("Select distinct(LeadFormatName) from LeadFormat order by LeadFormatName", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetLeadFormat()", "ClsImportLeadsDataService.cs");
                return null;
            }
        }

        public DataSet Country_GetAll()
        {
            try
            {
                return ExecuteDataSet("Select CountryCode as ID, Name from vCountry order by Name;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Country_GetAll()", "ClsImportLeadsDataService.cs");
                return null;
            }
        }

        public DataSet PhoneNumbers_GetAll(Int64 ListID)
        {
            try
            {
                return ExecuteDataSet("select phoneNO from Leads,CampaignCallingList where Leads.ListID = CampaignCallingList.ListID and CampaignCallingList.CampaignID IN (select CampaignID from CampaignCallingList where ListID ='" + ListID + "');", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "PhoneNumbers_GetAll()", "ClsImportLeadsDataService.cs");
                return null;
            }
        }

        public DataSet GetLeadFields(Int64 FormatID)
        {
            try
            {
                return ExecuteDataSet("select StartPosition,CustomFieldID from LeadFields,LeadFormat where LeadFields.LeadformatID = LeadFormat.ID and LeadformatID = " + FormatID + " order by StartPosition;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetLeadFields()", "ClsImportLeadsDataService.cs");
                return null;
            }
        }

        public int GetMaxIDLeads()
        {
            try
            {
                DataSet ds = ExecuteDataSet("Select IsNull(Max(ID),0)+1 from Leads;", CommandType.Text, null);
                return (int.Parse(ds.Tables[0].Rows[0][0].ToString()));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetMaxIDLeads()", "ClsImportLeadsDataService.cs");
                return 0;
            }
        }

        public int GetMaxIDTrendWestID()
        {
            try
            {
                DataSet ds = ExecuteDataSet("Select IsNull(Max(ID),0)+1 from TrendWestLead;", CommandType.Text, null);
                return (int.Parse(ds.Tables[0].Rows[0][0].ToString()));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetMaxIDTrendWestID()", "ClsImportLeadsDataService.cs");
                return 0;
            }
        }

        public int GetMaxLeadDetailID()
        {
            try
            {
                DataSet ds = ExecuteDataSet("Select IsNull(Max(ID),0)+1 from LeadDetail;", CommandType.Text, null);
                return (int.Parse(ds.Tables[0].Rows[0][0].ToString()));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetMaxLeadDetailID()", "ClsImportLeadsDataService.cs");
                return 0;
            }
        }

        public int GetMaxLocationID()
        {
            try
            {
                DataSet ds = ExecuteDataSet("Select IsNull(Max(ID),0)+1 from TrendWestLead;", CommandType.Text, null);
                return (int.Parse(ds.Tables[0].Rows[0][0].ToString()));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetMaxLocationID()", "ClsImportLeadsDataService.cs");
                return 0;
            }
        }



        public DataSet Timezone_GetAll()
        {
            try
            {
                return ExecuteDataSet("Select distinct(TimezoneName) as TimezoneName,ID from TimeZone order by TimezoneName;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Timezone_GetAll()", "ClsImportLeadsDataService.cs");
                return null;
            }
        }


        public DataSet ReteriveCustomeFieldDetails(Int64 LeadFormatId)
        {
            try
            {
                return ExecuteDataSet("select leadfields.ID,leadcustomfields.ID as CustomFieldID,FieldName,FieldType,StartPosition,Length,LeadFormatID from leadfields,leadcustomfields where leadformatid=" + LeadFormatId + "and customfieldid = leadcustomfields.ID order by StartPosition", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ReteriveCustomeFieldDetails()", "ClsImportLeadsDataService.cs");
                return null;
            }

        }

        public DataSet AreaCode_GetAll()
        {
            try
            {
                return ExecuteDataSet("Select distinct(Areacode) as Areacode,ID  from Areacode order by Areacode;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AreaCode_GetAll()", "ClsImportLeadsDataService.cs");
                return null;
            }
        }

        public DataSet State_GetAll()
        {
            try
            {
                return ExecuteDataSet("select distinct(StateName),Abbreviation,ID from State order by StateName;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "State_GetAll()", "ClsImportLeadsDataService.cs");
                return null;
            }
        }

        public DataSet FileFormat_GetAll()
        {
            try
            {
                return ExecuteDataSet("Select * from LeadFormat order by LeadFormatName;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FileFormat_GetAll()", "ClsImportLeadsDataService.cs");
                return null;
            }
        }

        public DataSet FormatField_GetAll(int FormatID)
        {
            try
            {
                return ExecuteDataSet("Select CustomFieldID as CustomFieldID,FieldName as LeadFieldName from LeadFields,LeadCustomFields where LeadFields.CustomFieldID = LeadCustomFields.ID and LeadFormatID = " + FormatID + ";", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FormatField_GetAll()", "ClsImportLeadsDataService.cs");
                return null;
            }
        }
        public DataSet FormatField_GetOtherDetail()
        {
            try
            {
                return ExecuteDataSet("Select ID as CustomFieldID,FieldName as LeadFieldName from LeadCustomFields where id in (4,5,6); ", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FormatField_GetOtherDetail()", "ClsImportLeadsDataService.cs");
                return null;
            }
        }

        public DataSet Treatment_Save(string TreatmentName, string Description, string TreatmentType,Int64 LeadFormatID,Int64 FieldID, string @Operator,string FieldValues,Int64 UserID)
        {
            try
            {
               DataSet ds = new DataSet();

                ds = ExecuteDataSet("spInsertTreatment", CommandType.StoredProcedure, CreateParameter("@TreatmentName", SqlDbType.VarChar, TreatmentName, 50),
                    CreateParameter("@Description", SqlDbType.VarChar, Description, 100),
                    CreateParameter("@TreatmentType", SqlDbType.VarChar, TreatmentType, 15),
                    CreateParameter("@LeadFormatID", SqlDbType.BigInt, LeadFormatID),
                    CreateParameter("@FieldID ", SqlDbType.BigInt, FieldID),
                    CreateParameter("@Operator", SqlDbType.VarChar, @Operator,50),
                    CreateParameter("@FieldValues", SqlDbType.VarChar, FieldValues,1000),
                    CreateParameter("@UserID", SqlDbType.BigInt, UserID)); 
                return ds;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Treatment_Save()", "ClsImportLeadsDataService.cs");
                return null;
            }
        }

        }
       
    }

