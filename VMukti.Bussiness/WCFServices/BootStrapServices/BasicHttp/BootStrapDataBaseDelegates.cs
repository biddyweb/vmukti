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

using System.Collections.Generic;
using System.ServiceModel;
using VMukti.Business.CommonDataContracts;
using VMukti.Business.WCFServices.BootStrapServices.DataContracts;
using System.Data;

namespace VMukti.Business.WCFServices.BootStrapServices.BasicHttp
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class BootStrapDataBaseDelegates : IHttpBootStrapDataBaseService
    {
        public delegate void DelHttpsvcJoin();
        public delegate clsDataBaseInfo DelHttpExecuteDataSet(string querystring);
        public delegate clsDataBaseInfo DelHttpExecuteStoredProcedure(string spName, clsSqlParameterContract objSParam);
        public delegate void DelHttpExecuteNonQuery(string spName, clsSqlParameterContract objSParam);
        public delegate int DelHttpExecuteReturnNonQuery(string spName, clsSqlParameterContract objSParam);

        public event DelHttpsvcJoin EntHttpsvcjoin;
        public event DelHttpExecuteDataSet EntHttpExecuteDataSet;
        public event DelHttpExecuteStoredProcedure EntHttpExecuteStoredProcedure;
        public event DelHttpExecuteNonQuery EntHttpExecuteNonQuery;
        public event DelHttpExecuteReturnNonQuery EntHttpExecuteReturnNonQuery;
        

        public void svcJoin()
        {
            if (EntHttpsvcjoin != null)
            {
                EntHttpsvcjoin();
            }
        }

        public clsDataBaseInfo svcExecuteDataSet(string querystring)
        {
            if (EntHttpExecuteDataSet != null)
            {
                return EntHttpExecuteDataSet(querystring);
            }
            else
            {
                return null;
            }
        }

        public clsDataBaseInfo svcExecuteDataSet(string spName,clsSqlParameterContract objSParam)
        {
            if (EntHttpExecuteStoredProcedure != null)
            {
                return EntHttpExecuteStoredProcedure(spName, objSParam);
            }
            else
            {
                return null;
            }
        }

        public void svcExecuteNonQuery(string spName, clsSqlParameterContract objSParam)
        {
            if (EntHttpExecuteNonQuery != null)
            {
                EntHttpExecuteNonQuery(spName, objSParam);
            }           
        }

        public int svcExecuteReturnNonQuery(string spName, clsSqlParameterContract objSParam)
        {
            if (EntHttpExecuteReturnNonQuery != null)
            {
                return EntHttpExecuteReturnNonQuery(spName, objSParam);
            }
            else
            {
                return -1;                
            }
        }

    }
}
