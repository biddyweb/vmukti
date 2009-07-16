using System;
using ServerInfo.DataAccess;
using VMuktiAPI;
namespace ServerInfo.Business
{
    public class ClsUserCollection : ClsBaseCollection<ClsUser>
    {
        public static ClsUserCollection GetAll()
        {
            try
            {
                ClsUserCollection obj = new ClsUserCollection();
                obj.MapObjects(new ClsServerInfoDataService().User_GetAll());
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetAll()", "ClsUserCollection.cs");
                return null;
            }
        }

    }
}
