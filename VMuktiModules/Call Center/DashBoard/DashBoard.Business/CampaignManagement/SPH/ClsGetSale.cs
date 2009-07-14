using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DashBoard.Business.SPH
{
    public class ClsGetSale
    {

        public static double GetAllSales()
        {
            try
            {
                return (new DashBoard.DataAccess.SPH.ClsSalesPerHourDataService().GetAllSales());
            }
            catch (Exception ex)
            {
                return 0.0;
            }
        }
    }
}
