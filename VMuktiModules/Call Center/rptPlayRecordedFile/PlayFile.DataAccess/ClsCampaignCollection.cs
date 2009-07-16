using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PlayFile.DataAccess
{
   public class ClsCampaignCollection : ClsDataServiceBase
    {
        public DataSet CampaignCollection()
        {
            try
            {
                return ExecuteDataSet("select Name from campaign", CommandType.Text, null);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
