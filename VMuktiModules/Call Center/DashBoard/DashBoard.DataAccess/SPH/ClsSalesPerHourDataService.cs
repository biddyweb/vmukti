using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace DashBoard.DataAccess.SPH
{
    public class ClsSalesPerHourDataService : ClsDataServiceBase 
    {
        public double GetAllSales()
        {
            try
            {
                SqlCommand cmd;
                int ReturnSales = 0;

                ExecuteNonQuery(out cmd, "spGetSalesPerHour", CreateParameter("@sales", SqlDbType.Int, -1, ParameterDirection.InputOutput));
                cmd.Dispose();
                ReturnSales = Convert.ToInt32(cmd.Parameters["@sales"].Value.ToString());
                return ReturnSales;

            }
            catch (Exception ex)
            {
                return 0.0;
            }
        }
    }
}
