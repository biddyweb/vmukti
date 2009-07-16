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
using VMukti.Business.CommonDataContracts;
using VMuktiAPI;

namespace VMukti.Business
{
    public class clsProfileCollection : ClsBaseCollection<clsProfileMap>
    {        
     

        public static clsProfileCollection GetUserProfile(int userID)
        {
            try
            {

                clsProfileCollection obj = new clsProfileCollection();

                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();
                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pUserID";
                    objInfo.PValue = userID;
                    objInfo.PDBType = "BigInt";
                    objInfo.PSize = 200;
                    lstSP.Add(objInfo);
                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGUserProfile", CSqlInfo).dsInfo);                    
                }
                else
                {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    List<clsSqlParametersInfo> lstSP = new List<clsSqlParametersInfo>();
                    clsSqlParametersInfo objInfo = new clsSqlParametersInfo();
                    objInfo.Direction = "Input";
                    objInfo.PName = "@pUserID";
                    objInfo.PValue = userID;
                    objInfo.PDBType = "BigInt";
                    objInfo.PSize = 200;
                    lstSP.Add(objInfo);
                    clsSqlParameterContract CSqlInfo = new clsSqlParameterContract();
                    CSqlInfo.objParam = lstSP;
                    obj.MapObjects(VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("spGUserProfile", CSqlInfo).dsInfo);
                }
                else
                {
                    obj.MapObjects(new VMukti.DataAccess.clsProfile().GetUserProfile(userID));
                    }
                }
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetUserProfile", "clsProfileCollection.cs");               
                return null;
            }
        }
    }
}
