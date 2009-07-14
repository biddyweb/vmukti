using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace PlayFile.DataAccess
{
    public class ClsSearchResult:ClsDataServiceBase
    {
       

        public DataSet SearchResult(string searchQry)
        {
            try
            {
                return ExecuteDataSet(searchQry, CommandType.Text, null);
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }

     

        public DataSet PhoneNo(string phno)
        {
            try
            {
                return ExecuteDataSet(phno, CommandType.Text, null);
            }
            catch(Exception ex)
            {
                return null;
            }
        }
    }
}
