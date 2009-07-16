using System;
using SIPUserInfo.DataAccess;
using VMuktiAPI;
namespace SIPUserInfo.Business
{
    public class ClsSIPUserInfoCollection : ClsBaseCollection<ClsSIPUserInfo>
    {
        public static ClsSIPUserInfoCollection GetAll()
        {
            try
            {
                ClsSIPUserInfoCollection obj = new ClsSIPUserInfoCollection();
                obj.MapObjects(new ClsSIPUserInfoDataService().SIPUserInfo_GetAll());
                return obj;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "GetAll()", "ClsSIPUserInfoCollection.cs");
                return null;
            }
        }

    }
}
