using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VMuktiAPI;
using System.Data;

namespace ImportLeads.Business
{
    public class ClsFilterName:ClsBaseObject
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
        private Int64 _ID = ImportLeads.Common.ClsConstants.NullLong;
        private string _FilterName = ImportLeads.Common.ClsConstants.NullString;

        public Int64 ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        public string FilterName
        {
            get { return _FilterName; }
            set { _FilterName = value; }
        }
        
        public override bool MapData(DataRow row)
        {
            try
            {
                ID = GetLong(row, "ID");
                FilterName = GetString(row, "FilterName");
                return base.MapData(row);
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--Call Center--:--ImportLeads--:--ImportLeads.Business--:--ClsFilter.cs--:--MapData(DataRow)--");
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
                return false;
            }
        }  
    }
}
