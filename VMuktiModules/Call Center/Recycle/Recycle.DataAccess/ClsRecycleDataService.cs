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

namespace Recycle.DataAccess
{
    public class ClsRecycleDataService : ClsDataServiceBase
    {

        public ClsRecycleDataService() : base() { }

        ////////////////////////////////////////////////////////////////////////
        /// <summary>
        ///	Creates a new DataService and specifies a transaction with
        ///	which to operate
        /// </summary>
        ////////////////////////////////////////////////////////////////////////
        public ClsRecycleDataService(IDbTransaction txn) : base(txn) { }


        //public DataSet User_GetAll()
        //{
        //    return ExecuteDataSet("Select UserInfo.*,Payroll.* from UserInfo left outer join Payroll on UserInfo.Id=Payroll.UserId where UserInfo.IsDeleted=0;", CommandType.Text, null);
        //}

        //public DataSet User_GetByID(int ID)
        //{
        //    return ExecuteDataSet("spGUserInfoPayroll", CommandType.StoredProcedure, CreateParameter("@pID", SqlDbType.Int, ID));
        //}

        //public void User_Save(ref int ID, string displayame, int roleID, string firstName, string lastName, string eMail, string password, bool isActive, int byUserId, float ratePerHour, float overTimeRate, float doubleRatePerHour, float doubleOverTimeRate)
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

        public DataSet Recycle_Leads(Int64 DispListID, Int64 ListID)
        {
            try
            {
                return ExecuteDataSet("spRecycle", CommandType.StoredProcedure, CreateParameter("@pDispList", SqlDbType.BigInt, DispListID), CreateParameter("@pListID", SqlDbType.BigInt, ListID));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Recycle_Leads()", "ClsDataServiceBase.cs");   
                return null;
            }
        }

        public DataSet Disposition_GetAll(Int64 CampID)
        {
            try
            {
                return ExecuteDataSet("Select Disposition.ID,Disposition.DespositionName from Disposition,CampaignDespoList,DispListDisp where CampaignDespoList.DespositionListID=DispListDisp.DispositionListID and DispListDisp.DispositionID=Disposition.ID and CampaignDespolist.CampaignID=" + CampID + ";", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Disposition_GetAll()", "ClsDataServiceBase.cs");   
                return null;
            }
        }

        public DataSet Campaign_GetAll()
        {
            try
            {
                return ExecuteDataSet("Select * from vCampaign;", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Campaign_GetAll()", "ClsDataServiceBase.cs");   
                return null;
            }
        }

        public DataSet List_GetAll(Int64 CampID)
        {
            try
            {
                return ExecuteDataSet("Select CallingList.ID,CallingList.ListName from CallingList,CampaignCallingList where CallingList.ID = CampaignCallingList.ListID and CampaignCallingList.CampaignID=" + CampID + ";", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "List_GetAll()", "ClsDataServiceBase.cs");   
                return null;
            }
        }
    }
}
