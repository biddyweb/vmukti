using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PlayFile.DataAccess;

namespace PlayFile.Business
{
    public class ClsGetSearchResult:ClsBaseObject
    {
        public static System.Data.DataSet GetSearchResult(string searchQry)
        {
            try
            {
                return (new ClsSearchResult().SearchResult(searchQry));
            }
            catch(Exception ex)
            {
                return null;
            }
        }


        public static DataSet GetPhoneNo(string phno)
        {
            try
            {
                return (new ClsSearchResult().PhoneNo(phno));
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
