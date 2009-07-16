using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VMuktiAPI;
using ImportLeads.DataAccess;

namespace ImportLeads.Business
{
    public class ClsFilterNameCollection:ClsBaseCollection<ClsFilterName>
    {
        public static StringBuilder sb1;
        public static StringBuilder CreateTressInfo()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append("User Is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
            sb.AppendLine();
            sb.Append("Peer Type is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString());
            sb.AppendLine();
            sb.Append("User's SuperNode is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
            sb.AppendLine();
            sb.Append("User's Machine Ip Address : " + VMuktiAPI.GetIPAddress.ClsGetIP4Address.GetIP4Address());
            sb.AppendLine();
            sb.AppendLine("----------------------------------------------------------------------------------------");
            return sb;
        }
        public static ClsFilterNameCollection Filter_GetName(string FormatName)
        {
            try
            {
                ClsFilterNameCollection obj = new ClsFilterNameCollection();
                obj.MapObjects(new ClsImportLeadsDataService().Filter_GetName(FormatName));
                return obj;
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--ImportLeads--:--ImportLeads.Business--:--ClsFilterTypeCollection.cs--:--Filter_GetAll()--");
                ClsException.LogError(ex);
                ClsException.WriteToErrorLogFile(ex);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(ex.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + ex.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + ex.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
                return null;
            }
        }
    
    }
}
