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
using Recycle.Common;
using System.Data;
using Recycle.DataAccess;
using System.Text;

namespace Recycle.Business
{
    public class ClsRecycle : ClsBaseObject
    {
        //public static StringBuilder sb1;
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

        public static DataSet Campaign_GetAll()
        {
            try
            {
                return (Campaign_GetAll(null));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Campaign_GetAll()", "ClsRecycle.cs");
                return null;
            }
        }

        public static DataSet Campaign_GetAll(IDbTransaction txn)
        {
            try
            {
                return (new Recycle.DataAccess.ClsRecycleDataService(txn).Campaign_GetAll());
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Campaign_GetAll()", "ClsRecycle.cs");
                return null;
            }
        }

        public static DataSet List_GetAll(Int64 CampID)
        {
            try
            {
                return (List_GetAll(CampID, null));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "List_GetAll()", "ClsRecycle.cs");
                return null;
            }
        }

        public static DataSet List_GetAll(Int64 CampID, IDbTransaction txn)
        {
            try
            {
                return (new Recycle.DataAccess.ClsRecycleDataService(txn).List_GetAll(CampID));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "List_GetAll()", "ClsRecycle.cs");
                return null;
            }
        }

        public static DataSet Recycle_Leads(Int64 DispIDList, Int64 ListID)
        {
            try
            {
                return (Recycle_Leads(DispIDList, ListID, null));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Recycle_Leads()", "ClsRecycle.cs");
                return null;
            }
        }

        public static DataSet Recycle_Leads(Int64 DispIDList, Int64 ListID, IDbTransaction txn)
        {
            try
            {
                return (new Recycle.DataAccess.ClsRecycleDataService(txn).Recycle_Leads(DispIDList, ListID));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Recycle_Leads()", "ClsRecycle.cs");
                return null;
            }
        }

        public static DataSet Disposition_GetAll(Int64 CampID)
        {
            try
            {
                return (Disposition_GetAll(CampID, null));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Disposition_GetAll()", "ClsRecycle.cs");
                return null;
            }
        }

        public static DataSet Disposition_GetAll(Int64 CampID, IDbTransaction txn)
        {
            try
            {
                return (new Recycle.DataAccess.ClsRecycleDataService(txn).Disposition_GetAll(CampID));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Disposition_GetAll()", "ClsRecycle.cs");
                return null;
            }
        }

        //#endregion 
    }
}
