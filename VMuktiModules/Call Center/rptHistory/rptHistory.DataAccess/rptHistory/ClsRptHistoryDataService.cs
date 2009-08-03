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
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace rptHistory.DataAccess
{
    public class ClsRptHistoryDataService : ClsDataServiceBase
    {
        public ClsRptHistoryDataService() : base() { }

        public ClsRptHistoryDataService(IDbTransaction txn) : base(txn) { }

        public DataSet rptHistory_GetHistoryDataOfDates(DateTime dtStartDate, DateTime dtEndDate)
        {
            //Access database using spHistory stored procedure
            return ExecuteDataSet("spHistory", CommandType.StoredProcedure, CreateParameter("@dtStart", SqlDbType.DateTime, dtStartDate), CreateParameter("@dtEnd", SqlDbType.DateTime, dtEndDate));
        }
    }
}
