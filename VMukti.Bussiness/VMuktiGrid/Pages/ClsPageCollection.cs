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
using VMukti.DataAccess.VMuktiGrid;
using System.Data.SqlClient;
using System.Collections.Generic;
using VMukti.Business.CommonDataContracts;
using System;
using VMuktiAPI;
using System.Text;
//using VMukti.Business.CommonDataContracts;


namespace VMukti.Business.VMuktiGrid
{
    public class ClsPageCollection : ClsBaseCollection<ClsPage>
    {       
       
        public static ClsPageCollection GetAll()
        {
            try
            {
                ClsPageCollection obj = new ClsPageCollection();

                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select pagetitle,id from page where isdeleted='false'").dsInfo);
                }
                else
                {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("select pagetitle,id from page where isdeleted='false'").dsInfo);
                }
                else
                {
                    obj.MapObjects(new ClsPageDataService().GetPages());
                }
                }
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetAll", "ClsPageCollection.cs");   
                return null;
            }
        }

        public static ClsPageCollection GetUPageAllocated(int intUserId)
        {
            try
            {
                ClsPageCollection obj = new ClsPageCollection();
                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pUserID";
                    objInfo.PValue = intUserId;
                    objInfo.PDBType = "BigInt";
                    objInfo.PSize = 200;

                    lstSP.Add(objInfo);

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;

                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGetUsersPageAllocated", CSqlInfo).dsInfo);
                    
                }
                else
                {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();

                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pUserID";
                    objInfo.PValue = intUserId;
                    objInfo.PDBType = "BigInt";
                    objInfo.PSize = 200;

                    lstSP.Add(objInfo);

                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;

                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGetUsersPageAllocated", CSqlInfo).dsInfo);
                }
                else
                {
                    obj.MapObjects(new ClsPageDataService().GetPageAllocated(intUserId));
                    }
                }
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetUPageAllocated", "ClsPageCollection.cs");             
                return null;
            }
        }
    }
}
