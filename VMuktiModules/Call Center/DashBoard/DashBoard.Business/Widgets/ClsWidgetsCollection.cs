using System;
using VMuktiAPI;
using DashBoard.DataAccess;
using DashBoard.Business;

namespace DashBoard.Business
{
    public class ClsWidgetsCollection : ClsBaseCollection<ClsWidgets>
    {
        public static ClsWidgetsCollection GetAllWidgets()
        {
            try
            {
                ClsWidgetsCollection obj = new ClsWidgetsCollection();

                //if (bool.Parse(VMuktiAPI.VMuktiInfo.Port80) && VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType != PeerType.BootStrap)
                //{
                //    try
                //    {
                //        obj.MapObjects(clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("Select * from Module where IsCollaborative='true'").dsInfo);
                //    }
                //    catch (Exception exp)
                //    {
                //        exp.Data.Add("My Key", "GetCollWidgets()--:--ClsWidgetsCollection.cs--:--" + exp.Message + " :--:--");
                //        ClsException.LogError(exp);
                //        ClsException.WriteToErrorLogFile(exp);
                //    }
                //}
                //else
                //{
                //    if (VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType == VMuktiAPI.PeerType.NodeWithHttp)
                //    {
                //        try
                //        {
                //            obj.MapObjects(clsMailDBClient.chHttpMailDBService.svcExecuteDataSet("Select * from Module where IsCollaborative='true'").dsInfo);
                //        }
                //        catch (Exception exp)
                //        {
                //            exp.Data.Add("My Key", "GetCollWidgets()--:--ClsWidgetsCollection.cs--:--" + exp.Message + " :--:--");
                //            ClsException.LogError(exp);
                //            ClsException.WriteToErrorLogFile(exp);
                //        }
                //    }
                //else
                //{
                //obj.MapObjects(new ClsWidgetsDataService().GetAllWidgets());
                //    }
                //}

                obj.MapObjects(new ClsWidgetsDataService().GetAllWidgets());
                return obj;
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "GetAllWidgets", "ClsWidgetsCollection");
                return null;
            }
        }
    }
}
