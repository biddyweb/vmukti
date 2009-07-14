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
using System.Data;

namespace VMukti.DataAccess.PBXConfiguration
{
    public class ClsPBXDataService : ClsPBXDataServiceBase
    {
        public void FncDataServicePBXConfigInsertValue(string PBXUserName, string PBXPassword, string PBXDomain)
        {
            try
             {
                ExecuteDataSet("update PBXConfig set FieldValue= '" + PBXUserName + "' where Field='ProviderUserName'", CommandType.Text, null);
                ExecuteDataSet("update PBXConfig set FieldValue= '" + PBXPassword + "' where Field='ProviderPassword'", CommandType.Text, null);
                ExecuteDataSet("update PBXConfig set FieldValue= '" + PBXDomain + "' where Field='ProvideDomain'", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }

        }
    }
}
