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
using System.Data;
using System.Data.SqlClient;
using Treatment.Common;


/// <summary>
/// Summary description for DemoDataService
/// </summary>

namespace Treatment.DataAccess
{

    public class ClsTreatmentDataService : ClsDataServiceBase
    {
        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsTreatmentDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsTreatmentDataService(IDbTransaction txn) : base(txn) { }

        public DataSet GetFields()
        {
            try
            {
            return ExecuteDataSet("Select * from TrendWestLead", CommandType.Text, null);
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetFields()", "ClsTreatmentDataService.cs");
                return null;
            }
        }
        public DataSet GetCampaignDisp(string CampaignName)
        {
            try
            {
            return ExecuteDataSet("select Disposition.DespositionName from Disposition,Campaign,CampaignDespoList,DispListDisp where Campaign.Name='"+CampaignName+"' and  Campaign.ID=CampaignDespoList.CampaignID and CampaignDespoList.DespositionListID=DispListDisp.DispositionListId and DispListDisp.DispositionId=Disposition.ID", CommandType.Text, null);
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetCampaignDisp()", "ClsTreatmentDataService.cs");
                return null;
            }
        }
        public void Delete_Disposition(int TreatID)
        {
            ExecuteDataSet("Delete from TreatmentDisposition where TreatmentID = '" + TreatID + "'", CommandType.Text, null);

        }

        public void Delete_All(int TreatID)
        {
            ExecuteDataSet("spExDTreatmentCondition", CommandType.StoredProcedure, CreateParameter("@pTreatmentID", SqlDbType.Int, TreatID));
        }
        public DataSet GetCampaign()
        {
            try
            {
           return ExecuteDataSet("select Name from Campaign",CommandType.Text, null);
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetCampaign()", "ClsTreatmentDataService.cs");
                return null;
            }
        }

        public DataSet GetFieldValues(string FieldName)
        {
            try
            {
            if (FieldName == "PhoneNumber")
            {
                
                return ExecuteDataSet("Select PhoneNo as PropertyValue from Leads", CommandType.Text, null);
            }
            else if(FieldName=="State")
            {
                int LeadID = 2;
                //return ExecuteDataSet("Select StateName as PropertyValue from Leads,Location,State where Leads.LocationID = Location.ID and Location.StateID = State.ID and Leads.ID =" + LeadID + ";", CommandType.Text, null);
                return ExecuteDataSet("Select StateName as PropertyValue from Leads,Location,State where Leads.LocationID = Location.ID and Location.StateID = State.ID and Leads.ID =" + LeadID + ";", CommandType.Text, null);
            }
            else if (FieldName == "Zip")
            {
                int LeadID = 3;
                return ExecuteDataSet("Select ZipCode as PropertyValue from Leads,Location,Zipcode where Leads.LocationID = Location.ID and Location.ZipCodeID = Zipcode.ID and Leads.ID =" + LeadID + ";", CommandType.Text, null);
            }
            else
            {
                return ExecuteDataSet("spGFieldValue",CommandType.StoredProcedure,CreateParameter("@fname",SqlDbType.NVarChar,FieldName,50));
            }
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetFieldValues()", "ClsTreatmentDataService.cs");
                return null;
            }
        }

        public DataSet Treatment_GetAll()
        {
            try
            {
            return ExecuteDataSet("Select ID,TreatmentName,Description,Type,IsInclude,ModifiedBy,IsDeleted from Treatment where IsDeleted <> 1", CommandType.Text, null);
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Treatment_GetAll()", "ClsTreatmentDataService.cs");
                return null;
            }
        }


        public DataSet Treatment_GetByID(int ID)
        {
            try
            {
            return ExecuteDataSet("spGTreatment", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.Int, ID));
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Treatment_GetByID()", "ClsTreatmentDataService.cs");
                return null;
            }
        }

        public int Treatment_Save(ref int ID, string TreatmentName, string Description, string Type , bool IsInclude ,  int UserID)
        {
            SqlCommand cmd;
            ExecuteNonQuery(out cmd, "spAETreatment",
                CreateParameter("@pID", SqlDbType.Int, ID),
                CreateParameter("@pTreatmentName", SqlDbType.NVarChar, TreatmentName,50),
                CreateParameter("@pDescription", SqlDbType.VarChar, Description,100),
                CreateParameter("@pType", SqlDbType.VarChar, Type,100),
                CreateParameter("@pIsInclude", SqlDbType.Bit, IsInclude),
                CreateParameter("@pUserID", SqlDbType.BigInt, UserID),
                CreateParameter("@pReturnMaxId", SqlDbType.BigInt, ParameterDirection.Output));

            ID = int.Parse(cmd.Parameters["@pReturnMaxId"].Value.ToString());
            cmd.Dispose();
            return ID;
        }


        public void Treatment_Delete(int ID)
        {
            ExecuteNonQuery("spDTreatment", CreateParameter("@pID", SqlDbType.Int, ID));
        }
        public DataSet GetLeadFormat()
        {
            try
            {
            return ExecuteDataSet("Select * from LeadFormat order by LeadFormatName;", CommandType.Text, null);
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetLeadFormat()", "ClsTreatmentDataService.cs");
                return null;
            }
        }
        public DataSet FormatField_GetAll(int FormatID)
        {    
            try
            {
            return ExecuteDataSet("select CustomFieldID as CustomFieldID,FieldName as LeadFieldName from LeadFields,LeadCustomFields where LeadFields.CustomFieldID = LeadCustomFields.ID and LeadFormatID = " + FormatID + ";", CommandType.Text, null);
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FormatField_GetAll()", "ClsTreatmentDataService.cs");
                return null;
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Timezone_GetAll()", "ClsTreatmentDataService.cs");
                return null;
            }
        }

        public DataSet Country_GetAll()
        {
            try
            {
                return ExecuteDataSet("Select Name , CountryCode as ID from vCountry order by Name;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Country_GetAll()", "ClsTreatmentDataService.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "AreaCode_GetAll()", "ClsTreatmentDataService.cs");
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "State_GetAll()", "ClsTreatmentDataService.cs");
                return null;
            }
        }

        public DataSet GetOtherDetail()
        {
            try
            {
                //return ExecuteDataSet("Select ID as CustomFieldID,FieldName as LeadFieldName from LeadCustomFields where id in (4,5,6); ", CommandType.Text, null);
                return ExecuteDataSet("Select ID as CustomFieldID,FieldName as LeadFieldName from LeadCustomFields where id in (4,6); ", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetOtherDetail()", "ClsTreatmentDataService.cs");
                return null;
            }
        }
    } //class

} //namespace