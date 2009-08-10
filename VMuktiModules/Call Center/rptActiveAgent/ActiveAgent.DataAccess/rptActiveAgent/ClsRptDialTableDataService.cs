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
using System.Data.SqlServerCe;

namespace rptActiveAgent.DataAccess
   
{
    public class ClsRptActiveAgentDataService : ClsDataServiceBase
    {
        //public ClsRptActiveAgentDataService() : base() { }

        //public ClsRptActiveAgentDataService(IDbTransaction txn) : base(txn) { }

        //public DataSet rptActiveAgent_GetHistoryDataOfDates()
        //{
        //    return ExecuteDataSet("spDialTable", CommandType.StoredProcedure);
            
        //}

        public ClsRptActiveAgentDataService() : base() { }

        public ClsRptActiveAgentDataService(IDbTransaction txn) : base(txn) { }

        string strClientConnectionString = string.Empty;

        public DataTable rptActiveAgent_GetHistoryDataOfDates()
        {
          //  strClientConnectionString = AppDomain.CurrentDomain.BaseDirectory + "\\VMukti.sdf";
           // strClientConnectionString = @"Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\VMukti.sdf";
           // System.Reflection.Assembly asm = System.Reflection.Assembly.GetExecutingAssembly();
            strClientConnectionString = @"Data Source=" + AppDomain.CurrentDomain.BaseDirectory + "\\VMukti.sdf";
            SqlCeConnection ce = new SqlCeConnection(strClientConnectionString);
            ce.Open();
            DataTable dt4ActiveRecord = new DataTable();
            // string strSelectCommand = "Select UserName  as AgentName , CampaignID as Campaign , GroupID as Group , RoleID as Role , Starttime as SessionStartTime From AgentStatusInfo";

            string strSelectCommand = "Select UserName as AgentName,CampaignName as Campaign,GroupName as [Group],RoleName as Role, starttime as SessionStart From AgentStatusInfo"; 
            
            SqlCeDataAdapter da4ActiveAgent = new SqlCeDataAdapter(strSelectCommand, ce);
            da4ActiveAgent.Fill(dt4ActiveRecord);
            return dt4ActiveRecord;
            
         //   return ExecuteDataSet("spDialTable", CommandType.StoredProcedure, CreateParameter("@dtStart", SqlDbType.DateTime, dtStartDate), CreateParameter("@dtEnd", SqlDbType.DateTime, dtEndDate));

        }
    }
}
