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
