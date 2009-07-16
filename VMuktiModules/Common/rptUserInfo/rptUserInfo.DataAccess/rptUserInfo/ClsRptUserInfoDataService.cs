using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using VMuktiAPI;


namespace rptUserInfo.DataAccess
{
    public class ClsRptUserInfo : ClsDataServiceBase                                                                                                             
    {
        public static StringBuilder sb1;
        public ClsRptUserInfo() : base() { }

        public ClsRptUserInfo(IDbTransaction txn) : base(txn) { }

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

        public DataSet rptUserInfo_GetAllUserByName()
        {
            try
            {
                //get users' name from database table UserInfo1... 
                return ExecuteDataSet("select DisplayName from UserInfo", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--User Info--:--RptUserInfo--:--RptUserInfo.Presentation--:--ctlRptUserInfo.xaml.cs--:--ctlRptUserInfo(ModulePermissions[] MyPermissions)--");
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

        public DataSet rptUserInfo_GetAllUsersByName()
        {
            try
            {
                //Get all users' DisplayName and ID from database table UserInfo1
                return ExecuteDataSet("Select DisplayName,ID from UserInfo", CommandType.Text, null);
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--User Info--:--RptUserInfo--:--RptUserInfo.Presentation--:--ctlRptUserInfo.xaml.cs--:--ctlRptUserInfo(ModulePermissions[] MyPermissions)--");
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
              
        
        public DataSet rptUserInfo_GetAllUserDetails(int ID)
        {
            try
            {
                //it will call UserDetails stored procedure and returns ActivityName and ActivityTime of selected user
                return ExecuteDataSet("UserDetails", CommandType.StoredProcedure, CreateParameter("@id", SqlDbType.Int, ID));
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--User Info--:--RptUserInfo--:--RptUserInfo.Presentation--:--ctlRptUserInfo.xaml.cs--:--ctlRptUserInfo(ModulePermissions[] MyPermissions)--");
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

        public DataSet rptUserInfo_GetUserLoginDetails(int ID)
        {
            try
            {
                //It will call UserLoginInfo stored procedure that will returns LoginTime, LogoutTime, DifferenceTime in Hour and DiffernceTime in Min
                return ExecuteDataSet("UserLoginInfo", CommandType.StoredProcedure, CreateParameter("@Id", SqlDbType.Int, ID));
            }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "VMukti--:--VmuktiModules--:--User Info--:--RptUserInfo--:--RptUserInfo.Presentation--:--ctlRptUserInfo.xaml.cs--:--ctlRptUserInfo(ModulePermissions[] MyPermissions)--");
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
