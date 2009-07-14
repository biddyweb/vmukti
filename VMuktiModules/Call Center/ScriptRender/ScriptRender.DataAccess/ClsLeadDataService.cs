using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace ScriptRender.DataAccess
{
    public class ClsLeadDataService : ClsDataServiceBase
    {

        public ClsLeadDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsLeadDataService(IDbTransaction txn) : base(txn) { }

        
        public DataSet Lead_GetAll(int LeadID, string FieldName, string LeadFormat)
        {
            try
            {
                if (FieldName.Equals("PhoneNumber"))
                {
                    return ExecuteDataSet("Select Leads.ID,PhoneNo as PropertyValue from Leads where Leads.ID =" + LeadID + ";", CommandType.Text, null);
                }
                else if (FieldName.Equals("State"))
                {
                    return ExecuteDataSet("Select Leads.ID,StateName as PropertyValue from Leads,Location,State where Leads.LocationID = Location.ID and Location.StateID = State.ID and Leads.ID =" + LeadID + ";", CommandType.Text, null);
                }
                else if (FieldName.Equals("Zip"))
                {
                    return ExecuteDataSet("Select Leads.ID,ZipCode as PropertyValue from Leads,Location,Zipcode where Leads.LocationID = Location.ID and Location.ZipCodeID = Zipcode.ID and Leads.ID =" + LeadID + ";", CommandType.Text, null);
                }
                else
                {
                    return ExecuteDataSet("select leadDetail.ID  as ID, leadDetail.PropertyValue as PropertyValue from LeadFormat leadFormat, LeadCustomFields leadFields, LeadDetail leadDetail, LeadFields lfields where leadFormat.ID = lfields.LeadFormatID and lfields.CustomFieldID = leadDetail.LeadFieldID and leadFields.ID = lfields.CustomFieldID and leadDetail.LeadID='"+LeadID+"' and leadFields.FieldName='"+FieldName+"' and leadFormat.LeadFormatName='"+LeadFormat+"'", CommandType.Text, null);
                }
            }
            catch(Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Lead_GetAll", "ClsLeadDataservice.cs");
            }

            return null;
        }

        public int getLeadFieldID(string FieldName)
        {
            try
            {
                return int.Parse(ExecuteDataSet("Select ID from LeadCustomFields where FieldName='"+ FieldName.Trim() + "';", CommandType.Text, null).Tables[0].Rows[0].ItemArray[0].ToString());
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "getLeadfieldID", "ClsLeadDataservice.cs");
            }
            return -1;
        }


        public string getWebScriptURL(int ScriptID)
        {
            try
            {
                return ExecuteDataSet("select ScriptURL from Script where ID=" + ScriptID + "", CommandType.Text, null).Tables[0].Rows[0].ItemArray[0].ToString();
            }
            catch(Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "getWebScriptURL", "ClsLeadDataservice.cs");
            }
            return null;
        }


    }
}
