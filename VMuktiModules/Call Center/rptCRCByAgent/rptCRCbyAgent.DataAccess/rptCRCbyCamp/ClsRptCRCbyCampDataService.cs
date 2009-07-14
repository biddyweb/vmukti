using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace rptCRCbyAgent.DataAccess
{
    public class ClsRptCRCbyAgentDataService : ClsDataServiceBase
    {
        public ClsRptCRCbyAgentDataService() : base() { }

        public ClsRptCRCbyAgentDataService(IDbTransaction txn) : base(txn) { }

        public DataSet rptCRCbyCamp_GetHistoryDataOfDates(DateTime dtStartDate, DateTime dtEndDate)
        {
            //Access database using spCRCbyAgentOld stored procedure
            return ExecuteDataSet("spCRCbyAgentOld", CommandType.StoredProcedure, CreateParameter("@dtStart", SqlDbType.DateTime, dtStartDate), CreateParameter("@dtEnd", SqlDbType.DateTime, dtEndDate));
        }
    }
}
