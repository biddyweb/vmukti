/* VMukti 1.0 -- An Open Source Unified Communications Engine
*
* Copyright (C) 2008 - 2009, VMukti Solutions Pvt. Ltd.
*
* Hardik Sanghvi <hardik@vmukti.com>
*
* See http://www.vmukti.com for more information about
* the VMukti project. Please do not directly contact
* any of the maintainers of this project for assistance;
* the project provides a web site, forums and mailing lists
* for your use.

* This program is free software: you can redistribute it and/or modify
* it under the terms of the GNU General Public License as published by
* the Free Software Foundation, either version 3 of the License, or
* (at your option) any later version.

* This program is distributed in the hope that it will be useful,
* but WITHOUT ANY WARRANTY; without even the implied warranty of
* MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
* GNU General Public License for more details.

* You should have received a copy of the GNU General Public License
* along with this program. If not, see <http://www.gnu.org/licenses/>

*/
using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;
using System.ServiceModel;


namespace Calender.Business.Service
{
    [ServiceContract]
    public interface IClsMailDBService
    {

        [OperationContract(IsOneWay = false)]
        void svcJoin(string uName);

        [OperationContract(IsOneWay = false)]
        clsDataBaseInfo svcExecuteDataSet(string querystring);

        [OperationContract(IsOneWay = false, Name = "ExecuteStoredProcedure")]
        clsDataBaseInfo svcExecuteDataSet(string spName, clsSqlParameterContract objSParam);

        [OperationContract(IsOneWay = true)]
        void svcExecuteNonQuery(string spName, clsSqlParameterContract objSParam);

        [OperationContract(IsOneWay = false)]
        int svcExecuteReturnNonQuery(string spName, clsSqlParameterContract objSParam);

        [OperationContract(IsOneWay = true)]
        void svcSendMail(clsMailInfo objMail);

        [OperationContract(IsOneWay = true)]
        void svcUnJoin(string uName);
    }

    public interface IClsMailDBServiceChannel : IClientChannel, IClsMailDBService
    { }


    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    class MailDBService:IClsMailDBService
    {
        public static StringBuilder sb1;
        public delegate void DelsvcJoin(string uname);
        public delegate clsDataBaseInfo DelsvcExecuteDataSet(string querystring);
        public delegate clsDataBaseInfo DelsvcExecuteStoredProcedure(string spName, clsSqlParameterContract objSParam);
        public delegate void DelsvcExecuteNonQuery(string spName, clsSqlParameterContract objSParam);
        public delegate int DelsvcExecuteReturnNonQuery(string spName, clsSqlParameterContract objSParam);
        public delegate void DelsvcSendMail(clsMailInfo objMail);
        public delegate void DelsvcUnJoin(string uname);

        public event DelsvcJoin EntsvcJoin;
        public event DelsvcExecuteDataSet EntsvcExecuteDataSet;
        public event DelsvcExecuteStoredProcedure EntsvcExecuteStoredProcedure;
        public event DelsvcExecuteNonQuery EntsvcExecuteNonQuery;
        public event DelsvcExecuteReturnNonQuery EntsvcExecuteReturnNonQuery;
        public event DelsvcSendMail EntsvcSendMail;
        public event DelsvcUnJoin EntsvcUnJoin;       
       

        #region IClsMailDBService Members

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

        public void svcJoin(string uName)
        {
            try
            {
            if (EntsvcJoin != null)
            {
                EntsvcJoin(uName);
            }
        }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "svcJoin()--:--clsHttpAudio.cs--:--" + ex.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
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
            }
        }
        public clsDataBaseInfo svcExecuteDataSet(string querystring)
        {
            try
            {
            if (EntsvcExecuteDataSet != null)
            {
                return (EntsvcExecuteDataSet(querystring));
            }
            else
            {
                return null;
            }
        }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "svcExecuteDataSet()--:--clsHttpAudio.cs--:--" + ex.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
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
        public clsDataBaseInfo svcExecuteDataSet(string spName, clsSqlParameterContract objSParam)
        {
            try
            {
            if (EntsvcExecuteStoredProcedure != null)
            {
                return (EntsvcExecuteStoredProcedure(spName, objSParam));
            }
            else
            {
                return null;
            }
        }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "svcExecuteDataSet(string spName, clsSqlParameterContract objSParam)--:--clsHttpAudio.cs--:--" + ex.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
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

        public void svcExecuteNonQuery(string spName, clsSqlParameterContract objSParam)
        {
            try
            {
            if (EntsvcExecuteNonQuery != null)
            {
                EntsvcExecuteNonQuery(spName, objSParam);
            }                
        }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "svcExecuteNonQuery()--:--clsHttpAudio.cs--:--" + ex.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
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
            }
        }
        public int svcExecuteReturnNonQuery(string spName, clsSqlParameterContract objSParam)
        {
            try
            {
            if(EntsvcExecuteReturnNonQuery!=null)
            {
                return (EntsvcExecuteReturnNonQuery(spName, objSParam));
            }
            else
            {
                return -1;
            }
        }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "svcExecuteReturnNonQuery()--:--clsHttpAudio.cs--:--" + ex.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
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
                return 0;
            }
        }

        public void svcSendMail(clsMailInfo objMail)
        {
            try
            {
            if (EntsvcSendMail != null)
            {
                EntsvcSendMail(objMail);
            }
        }
            catch (Exception ex)
            {
                ex.Data.Add("My Key", "svcSendMail()--:--clsHttpAudio.cs--:--" + ex.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
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
               
            }
        }
        public void svcUnJoin(string uName)
        {
            try
            {
            if (EntsvcUnJoin != null)
            {
                EntsvcUnJoin(uName);
            }
        }

         
         catch (Exception ex)
            {
                ex.Data.Add("My Key", "svcUnJoin()--:--clsHttpAudio.cs--:--" + ex.Message + " :--:--");
                //ClsException.LogError(ex);
                //ClsException.WriteToErrorLogFile(ex);
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
                
            }
        }
 #endregion
}
}