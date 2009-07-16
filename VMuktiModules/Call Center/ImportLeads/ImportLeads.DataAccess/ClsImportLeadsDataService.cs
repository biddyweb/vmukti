using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Data.SqlTypes;
using VMuktiAPI;
namespace ImportLeads.DataAccess
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

        public DataSet Filter_GetAll(string Condition)
        {
            try
            {
                return ExecuteDataSet("select Treatment.ID,TreatmentName as FilterName,Description,Type,TreatmentCondition.ID as TreatmentCondID,FieldID,Operator,FieldValues from Treatment,TreatmentCondition where Treatment.IsDeleted = 0 and Treatment.Type = 'Filter' and Treatment.Id = TreatmentCondition.TreatmentID and Treatment.ID in ("+ Condition +");", CommandType.Text, null);
                //return ExecuteDataSet("select Treatment.ID,TreatmentName as FilterName,Description,Type,TreatmentConditionNew.ID as TreatmentCondID,FieldID,Operator,FieldValues from Treatment,TreatmentConditionNew where Treatment.IsDeleted = 0 and Treatment.Type = 'Filter' and Treatment.Id = TreatmentConditionNew.TreatmentID;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Filter_GetAll()", "ClsImportLeadsDataService.cs");
                return null;
            }
        }

        public DataSet Filter_GetName(string FormatName)
        {
            try
            {
                return ExecuteDataSet("select distinct Treatment.TreatmentName as FilterName, Treatment.ID from Treatment,TreatmentCondition where Treatment.ID=TreatmentCondition.TreatmentID and TreatmentCondition.LeadFormatID='"+FormatName+"';", CommandType.Text, null);
                //return ExecuteDataSet("select Treatment.ID,TreatmentName as FilterName,Description,Type,TreatmentConditionNew.ID as TreatmentCondID,FieldID,Operator,FieldValues from Treatment,TreatmentConditionNew where Treatment.IsDeleted = 0 and Treatment.Type = 'Filter' and Treatment.Id = TreatmentConditionNew.TreatmentID;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--ImportLeads--:--ImportLeads.DataAccess--:--ClsImportLeadsDataService.cs--:--Timezone_GetAll()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
                return null;
            }
        }


        public DataSet Country_GetAll()
        {
            try
            {
                return ExecuteDataSet("Select ID,Name,CountryCode from vCountry;", CommandType.Text, null);
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
                DataSet ds = ExecuteDataSet("Select IsNull(Max(ID),0)+1 from Location;", CommandType.Text, null);
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
                return ExecuteDataSet("Select distinct(TimezoneName),ID from TimeZone;", CommandType.Text, null);
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

        public DataSet AreaCode_GetAll(string strTimeZone, string flag)
        {
            try
            {
                return ExecuteDataSet("Select Areacode,TimeZoneName from Areacode where TimeZoneName " + flag + " (" + strTimeZone + ");", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AreaCode_GetAll()", "ClsImportLeadsDataService.cs");
                return null;
            }
        }

        public DataSet AreaCode_GetID(string strTimeZone)
        {
            try
            {
                return ExecuteDataSet("Select ID,TimeZoneName,Areacode from Areacode where AreaCode = '" + strTimeZone + "';", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AreaCode_GetID()", "ClsImportLeadsDataService.cs");
                return null;
            }
        }
        public DataSet StateCountry_GetAll()
        {
            try
            {
                return ExecuteDataSet("select state.ID as stateID ,CountryID,ZipCode.ID as ZipCodeID,zipcode from state left outer join zipcode on state.ID = zipcode.stateid;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "StateCountry_GetAll()", "ClsImportLeadsDataService.cs");
                return null;
            }
        }

        public DataSet FileFormat_GetAll()
        {
            try
            {
                return ExecuteDataSet("Select * from LeadFormat;", CommandType.Text, null);
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
                return ExecuteDataSet("Select * from LeadFields where LeadFormatID = " + FormatID + ";", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "FormatField_GetAll()", "ClsImportLeadsDataService.cs");
                return null;
            }
        }

        public void Lead_Save(string x)
        {
            try
            {
                DataSet ds = ExecuteDataSet("Select Isnull(Max(ID),0) + 1 from XMLTry;", CommandType.Text, null);
                ExecuteDataSet("Insert Into XMLTry values (" + int.Parse(ds.Tables[0].Rows[0][0].ToString()) + ",'" + x + "');", CommandType.Text, null);

                //SqlCommand cmd;
                //    ExecuteNonQuery(out cmd, "spAXML",
                //        CreateParameter("@pXML", SqlDbType.Xml, new SqlXml(new XmlTextReader(x, XmlNodeType.Document, null))));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Lead_Save()", "ClsImportLeadsDataService.cs");

            }
        }

        //public DataSet User_GetByID(int ID)
        //{
        //    return ExecuteDataSet("spGUserInfoPayroll", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.Int, ID));
        //}

        //public void User_Save(ref int ID, string displayame,int roleID,string firstName,string lastName,string eMail,string password,bool isActive,int byUserId,float ratePerHour,float overTimeRate,float doubleRatePerHour,float doubleOverTimeRate )
        //{
        //    SqlCommand cmd;
        //    ExecuteNonQuery(out cmd, "spAEUserInfoPayroll",
        //        CreateParameter("@pID", SqlDbType.Int, ID),
        //        CreateParameter("@pDisplayName", SqlDbType.NVarChar, displayame, 100),
        //        CreateParameter("@pRoleID", SqlDbType.BigInt, roleID),
        //        CreateParameter("@pFirstName", SqlDbType.NVarChar, firstName, 50),
        //        CreateParameter("@pLastName", SqlDbType.NVarChar, lastName, 50),
        //        CreateParameter("@pEMail", SqlDbType.NVarChar, eMail, 256),
        //        CreateParameter("@pPassword", SqlDbType.NVarChar, password, 50),
        //        CreateParameter("@pIsActive", SqlDbType.Bit, isActive),
        //        CreateParameter("@pByUserID", SqlDbType.BigInt, byUserId),
        //        CreateParameter("@pRatePerHour", SqlDbType.Float, ratePerHour),
        //        CreateParameter("@pOverTimeRate", SqlDbType.Float, overTimeRate),
        //        CreateParameter("@pDoubleRatePerHour", SqlDbType.Float, doubleRatePerHour),
        //        CreateParameter("@pDoubleOverTimeRate", SqlDbType.Float, doubleOverTimeRate));

        //    cmd.Dispose();
        //}

        //public void User_Delete(int ID)
        //{
        //    ExecuteNonQuery("spDUserInfoPayroll", CreateParameter("@pID", SqlDbType.Int, ID));
        //}

        public DataSet GetLocationID(Int64 Zip)
        {
            try
            {
                return ExecuteDataSet("select Location.ID as ID,Location.TimeZoneID as TimeZoneID,Location.CountryID as CountryID,Location.StateID as StateID,Location.AreaCodeID as AreaCodeID,Location.ZipCodeID as ZipeCodeID,Zipcode.ZipCode as ZipCode from Location, Zipcode where Location.ZipCodeID=Zipcode.ID and Zipcode.ZipCode=" + Zip + ";", CommandType.Text, null);
                //return ExecuteDataSet("select * from Location where ZipCodeID=" + Zip + ";", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetLocationID()", "ClsImportLeadsDataService.cs");
                return null;
            }
        }

 
        public DataSet AreaCode_GetAll()
        {
            try
            {
                return ExecuteDataSet("Select Areacode,TimeZoneName from Areacode;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AreaCode_GetAll()", "ClsImportLeadsDataService.cs");
                return null;
            }
        }

        public DataSet GetZipCode()
        {
            try
            {
                return ExecuteDataSet("select DISTINCT ZipCode from location,ZipCode where location.ZipCodeID=ZipCode.ID;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetZipCode()", "ClsImportLeadsDataService.cs");
                return null;
            }
        }

        public int GetZipCodeID(Int64 finalzipcode)
        {
            try
            {
                DataSet ds=ExecuteDataSet("Select ID from Zipcode where ZipCode="+ finalzipcode+";",CommandType.Text, null);
                return (int.Parse(ds.Tables[0].Rows[0][0].ToString()));
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetZipCodeID()", "ClsImportLeadsDataService.cs");
                return 0;
            }
        }

        public DataSet GetLocationDetail()
        {
            try
            {
                return ExecuteDataSet("spGLocationDetail", CommandType.StoredProcedure, null);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetLocationDetail()", "ClsImportLeadsDataService.cs");
                return null;
            }
        }
    }
}
