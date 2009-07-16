using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace rptDialTable.DataAccess
   
{
    public class ClsRptDialTableDataService : ClsDataServiceBase
    {
        public ClsRptDialTableDataService() : base() { }

        public ClsRptDialTableDataService(IDbTransaction txn) : base(txn) { }

        public DataSet rptDialTable_GetHistoryDataOfDates(DateTime dtStartDate, DateTime dtEndDate)
        {
            try
            {
                return ExecuteDataSet("spDialTable", CommandType.StoredProcedure, CreateParameter("@pdtStart", SqlDbType.DateTime, dtStartDate), CreateParameter("@pdtEnd", SqlDbType.DateTime, dtEndDate));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "rptDialTable_GetHistoryDataOfDates()", "ClsRptDialTableDataService.cs");
                return null;
            }
            
        }
    }
}
