using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace rptCRCbyCamp.DataAccess
{
    public class ClsRptCRCbyCampDataService : ClsDataServiceBase
    {
        public ClsRptCRCbyCampDataService() : base() { }

        public ClsRptCRCbyCampDataService(IDbTransaction txn) : base(txn) { }

        public DataSet rptCRCbyCamp_GetHistoryDataOfDates(DateTime dtStartDate, DateTime dtEndDate)
        {
            //Access database using spCRCbyCamp stored procedure
            return ExecuteDataSet("spCRCbyCamp", CommandType.StoredProcedure, CreateParameter("@dtStart", SqlDbType.DateTime, dtStartDate), CreateParameter("@dtEnd", SqlDbType.DateTime, dtEndDate));
        }
    }
}
