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
using DispositionRender.DataAccess;
using VMuktiAPI;
using System.Text;
namespace DispositionRender.Business
{
    public class ClsDispositionRenderCollection : ClsBaseCollection<ClsDispositionRender>
    {
        ClsDispositionRenderDataService clsDataService = new ClsDispositionRenderDataService();

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
        //Disposition_GetAll() function to get data related to campaignID and Disposition List
        public static ClsDispositionRenderCollection GetAll(Int64 CampID)
        {
            try
            {
                ClsDispositionRenderCollection obj = new ClsDispositionRenderCollection();
                

                obj.MapObjects(new ClsDispositionRenderDataService().Disposition_GetAll(CampID));
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetAll()", "ClsDispositionRenderCollection.cs");
                return null;
            } 
        }

        public int GetDispoId(string DispositionName)
        {
            try
            {
                return clsDataService.Disposition_GetByName(DispositionName);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetDispoId()", "ClsDispositionRenderCollection.cs");
                return 100;
            } 
        }

    }
}
