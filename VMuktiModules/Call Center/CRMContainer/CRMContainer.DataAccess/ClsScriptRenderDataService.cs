/* VMukti 1.0 -- An Open Source Unified Communications Engine
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
using CRMContainer.Common;
using System.Text;


/// <summary>
/// Summary description for DemoDataService
/// </summary>

namespace CRMContainer.DataAccess
{

    public class ClsCRMContainerDataService : ClsDataServiceBase
    {
        //public static StringBuilder sb1;
        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsCRMContainerDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsCRMContainerDataService(IDbTransaction txn) : base(txn) { }


       /* public DataSet Script_GetAll()
        {
            return ExecuteDataSet("Select * from vTrendWestLead;", CommandType.Text, null);

        }*/

        //public DataSet Script_GetByID(Int64 LeadID)
        //{
        //    return ExecuteDataSet("spGTrendWestLead", CommandType.StoredProcedure, CreateParameter("@pLeadID", SqlDbType.BigInt, LeadID));
        //}

        //public static StringBuilder CreateTressInfo()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("User Is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
        //    sb.AppendLine();
        //    sb.Append("Peer Type is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString());
        //    sb.AppendLine();
        //    sb.Append("User's SuperNode is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
        //    sb.AppendLine();
        //    sb.Append("User's Machine Ip Address : " + VMuktiAPI.GetIPAddress.ClsGetIP4Address.GetIP4Address());
        //    sb.AppendLine();
        //    sb.AppendLine("----------------------------------------------------------------------------------------");
        //    return sb;
        //}

        //This function call return the zip file name of the CRM from the database.
        public string File_GetByID(Int64 CampaignID)
        {
            try
            {
                //Execuating the stroed procedure to retrive the zip file name
                //of the CRM from database.
                DataSet ds = ExecuteDataSet("spGFileName1", CommandType.StoredProcedure, CreateParameter("@pCampaignID", SqlDbType.BigInt, CampaignID));
                DataTable dt = ds.Tables[0];
                DataRow dr = dt.Rows[0];
                return dr[0].ToString();
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "File_GetByID()", "ClsCRMContainerDataService.cs");
                return null;
            }
        }
    }
}