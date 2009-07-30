﻿/* VMukti 2.0 -- An Open Source Video Communications Suite
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

namespace rptTeleNumAudit.DataAccess
{
    public class ClsRptTeleNumAudit : ClsDataServiceBase
    {
        public ClsRptTeleNumAudit() : base() { }

        public ClsRptTeleNumAudit(IDbTransaction txn) : base(txn) { }

        public DataSet rptTeleNumAudit_GetAllCampaign()
        {
            return ExecuteDataSet("select [Name],ID From vCampaign", CommandType.Text, null);
        }

        public DataSet rptTeleNumAudit_GetAllListByCapmID(int CampaignID)
        {
            //Access database table CallingList, CampaignCallingList
            return ExecuteDataSet("Select CallingList.ListName, CallingList.ID from CallingList, CampaignCallingList where CampaignCallingList.ListID=CallingList.ID and CampaignCallingList.CampaignID = " + CampaignID.ToString(), CommandType.Text, null);
        }

        public DataSet rptTeleNumAudit_GetAllNumberByListID(int ListID)
        {
            //Access database table Call,Leads
            return ExecuteDataSet("Select Distinct(Leads.PhoneNo), Leads.ID from Leads, [Call] where Call.LeadID=Leads.ID and Leads.ListID = " + ListID.ToString(), CommandType.Text, null);
        }

        public DataSet rptTeleNumAudit_GetTeleNumAudit(Int64 TelePhoneNumber)
        {
            //Access Datbase using spReportTelephoneAudit stored procedure
            return ExecuteDataSet("spReportTelephoneAudit", CommandType.StoredProcedure, CreateParameter("@pTelephoneNumber", SqlDbType.BigInt, TelePhoneNumber));
        }
    }
}
