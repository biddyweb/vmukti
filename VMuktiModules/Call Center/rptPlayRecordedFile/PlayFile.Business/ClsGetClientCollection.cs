using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlayFile.DataAccess;

namespace PlayFile.Business
{
    public class ClsGetClientCollection : ClsBaseCollection<ClsClientDetail>
    {
       public static ClsGetClientCollection GetClientCollection(string Campaign)
        {
           try
            {
                ClsGetClientCollection obj = new ClsGetClientCollection();
                obj.MapObjects(new ClsClientCollection().ClientCollection(Campaign));
                return obj;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
