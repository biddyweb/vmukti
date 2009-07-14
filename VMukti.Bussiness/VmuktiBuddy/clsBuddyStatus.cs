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
using System.Text;
using System.Data;
using VMuktiAPI;

namespace VMukti.Business
{
    public class clsBuddyStatus
    {        
       
        public static void AddBuddy(string uName, string buddyname, string status)
        {
            try
            {
                new VMukti.DataAccess.clsBuddyStatus(null).AddBuddy(uName, buddyname, status);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AddBuddy()", "clsBuddyStatus.cs");             
            }
        }

        public static void UpdateBuddy(string uName, string status)
        {
            try
            {
                new VMukti.DataAccess.clsBuddyStatus(null).UpdateBuddy(uName, status);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AddBuddy()", "UpdateBuddy.cs");               
            }
        }

        public static void DeleteBuddy(string uName, string buddyname)
        {
            try
            {
                new VMukti.DataAccess.clsBuddyStatus(null).DeleteBuddy(uName, buddyname);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DeleteBuddy()", "clsBuddyStatus.cs"); 
            }
        }

        public static List<object> GetAllBuddies()
        {
            try
            {
                List<VmuktiBuddy.VmuktiBuddyInfo> lstBuddyInfo = new List<VMukti.Business.VmuktiBuddy.VmuktiBuddyInfo>();
                List<VmuktiBuddy.BuddyStatus> lstBuddyStatus = new List<VMukti.Business.VmuktiBuddy.BuddyStatus>();

                List<object> objList = new List<object>();
                
                DataSet ds = new VMukti.DataAccess.clsBuddyStatus(null).GetAllBuddies();
                if (ds.Tables[0] != null)
                {
                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        VmuktiBuddy.VmuktiBuddyInfo BuddyInfo = new VMukti.Business.VmuktiBuddy.VmuktiBuddyInfo();
                        BuddyInfo.BuddyName = (string)dr["Buddy_Name"];
                        BuddyInfo.BuddyStatus = (string)dr["Buddy_Status"];
                        lstBuddyInfo.Add(BuddyInfo);
                    }
                }

                if (ds.Tables[1] != null)
                {
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        VmuktiBuddy.BuddyStatus objBuddyStatus = new VMukti.Business.VmuktiBuddy.BuddyStatus();
                        objBuddyStatus.UserName = (string)dr["UserName"];
                        objBuddyStatus.BuddyName = (string)dr["BuddyName"];
                        objBuddyStatus.Status = (string)dr["BuddyStatus"];
                        lstBuddyStatus.Add(objBuddyStatus);
                    }
                }
                objList.Add(lstBuddyInfo);
                objList.Add(lstBuddyStatus);
                return objList;

            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetAllBuddies", "clsBuddyStatus.cs");
                return null;
            }
        }

    }
}
