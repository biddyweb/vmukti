using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PlayFile.DataAccess
{
    public class ClsCamp4Phno : ClsDataServiceBase
    {
       
        

        public DataSet Camp4Phno(string PhoneNumber)
        {
            try
            {
                return ExecuteDataSet("select name from campaign where id in(select campaignid from campaigncallinglist where listid in(select listid from leads where phoneno='" + PhoneNumber + "'))", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public DataSet Camp4Agent(string Campaign)
        {
            try
            {
                return ExecuteDataSet("select firstname from userinfo where id in(select generatedby from call where campaignid in(select id from campaign where name='" + Campaign + "'))", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
