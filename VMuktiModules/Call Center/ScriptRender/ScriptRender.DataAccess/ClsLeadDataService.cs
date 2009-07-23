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
