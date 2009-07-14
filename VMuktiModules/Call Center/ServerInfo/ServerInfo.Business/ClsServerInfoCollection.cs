using System;
using ServerInfo.DataAccess;
using VMuktiAPI;
namespace ServerInfo.Business
{
    public class ClsServerInfoCollection : ClsBaseCollection<ClsServerInfo>
    {
        public static ClsServerInfoCollection GetAll()
        {
            try
            {
                ClsServerInfoCollection obj = new ClsServerInfoCollection();
                obj.MapObjects(new ClsServerInfoDataService().ServerInfo_GetAll());
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetAll()", "ClsServerInfoCollection.cs");
                return null;
            }
        }

    }
}
