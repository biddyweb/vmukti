using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PlayFile.DataAccess
{
   public class ClsPlayList:ClsDataServiceBase
    {
        public DataSet PlayList()
        {
            try
            {
                return ExecuteDataSet("select name from campaign", CommandType.Text, null);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
