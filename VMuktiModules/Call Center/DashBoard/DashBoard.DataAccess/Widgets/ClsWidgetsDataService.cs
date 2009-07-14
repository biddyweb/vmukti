using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;


namespace DashBoard.DataAccess
{
    public class ClsWidgetsDataService : ClsDataServiceBase
    {
        public ClsWidgetsDataService() : base() { }

        public ClsWidgetsDataService(IDbTransaction txn) : base(txn) { }

        public DataSet GetAllWidgets()
        {
            try
            {
                return ExecuteDataSet("Select * from Module where IsDeleted='False'", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetAllWidgets", "ClsWidgetsDataService.cs");
                return new DataSet();
            }
        }
    }
}
