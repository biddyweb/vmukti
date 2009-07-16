using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using PlayFile.DataAccess;

namespace PlayFile.Business
{
    public class ClsGetCamp4Phno : ClsBaseObject
    {
        

      

        public static DataSet GetCamp4Phno(string PhoneNumber)
        {
            try
            {
                return (new ClsCamp4Phno().Camp4Phno(PhoneNumber));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        

        public static DataSet GetCamp4Agent(string Campaign)
        {
            try
            {
                return (new ClsCamp4Phno().Camp4Agent(Campaign));
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
