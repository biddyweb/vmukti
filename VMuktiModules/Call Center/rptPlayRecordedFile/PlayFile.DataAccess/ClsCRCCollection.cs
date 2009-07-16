using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PlayFile.DataAccess
{
    public class ClsCRCCollection:ClsDataServiceBase
    {
       
        public DataSet CRCCollection(string Campaign)
        {
            try
            {
               
               //return ExecuteDataSet("select * from Disposition where ID in(select DespositionListID from CampaignDespoList where CampaignID in(select ID from Campaign where Name='"+Campaign+"') )",CommandType.Text,null);
               return ExecuteDataSet("select despositionname from disposition where id in (Select dispositionid from DispListDisp where DispositionListId in (select DespositionListid from CampaignDespoList where Campaignid in (select id from campaign where name='" + Campaign + "')))", CommandType.Text, null);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
