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
using System.Text;
using VMuktiAPI;

namespace VMukti.Bussiness.PBXConfiguration
{
    public class PBXConfiguration
    {        
      
        VMukti.DataAccess.PBXConfiguration.ClsPBXDataService PBXDataService = new VMukti.DataAccess.PBXConfiguration.ClsPBXDataService();

        public void FncPBXBusinessInsertCredential(string PBXUserName, string PBXPassword, string PBXDomain)
        {
            try
            {
                if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                {
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("update PBXConfig set FieldValue= '" + PBXUserName + "' where Field='ProviderUserName'");
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("update PBXConfig set FieldValue= '" + PBXPassword + "' where Field='ProviderPassword'");
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("update PBXConfig set FieldValue= '" + PBXDomain + "' where Field='ProvideDomain'");
                  
                }
                else
                {

                if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                {
                    
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("update PBXConfig set FieldValue= '" + PBXUserName + "' where Field='ProviderUserName'");
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("update PBXConfig set FieldValue= '" + PBXPassword + "' where Field='ProviderPassword'");
                    VMukti.Business.clsDataBaseChannel.chHttpDataBaseService.svcExecuteDataSet("update PBXConfig set FieldValue= '" + PBXDomain + "' where Field='ProvideDomain'");
                }
                else
                {
                    PBXDataService.FncDataServicePBXConfigInsertValue(PBXUserName, PBXPassword, PBXDomain);
                }
            }
            }
            catch (Exception ex)
            {
                
                VMuktiHelper.ExceptionHandler(ex, "FncPBXBusinessInsertCredential(string PBXUserName, string PBXPassword, string PBXDomain)","PBXConfiguration.cs");
            }
        }
    }
}
