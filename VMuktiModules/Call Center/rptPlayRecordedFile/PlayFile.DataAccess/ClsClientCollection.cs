using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PlayFile.DataAccess
{
    public class ClsClientCollection : ClsDataServiceBase
    {
       
        public DataSet ClientCollection(string Campaign)
        {
            try
            {
                return ExecuteDataSet("select PropertyValue from LeadDetail where LeadFieldID='7' and LeadID in(select LeadID from Call where CampaignID in(select ID from Campaign where Name='" + Campaign + "'))", CommandType.Text, null);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
