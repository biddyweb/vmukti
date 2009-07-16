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
using Video.Business.Service.NetP2P;
using Video.Business.Service.DataContracts;
using Video.Business.Service.BasicHttp;
using VMuktiService;
using System.ServiceModel;
using System.Runtime.Remoting;
using System.Globalization;
using System.Reflection;
using VMuktiAPI;
using System.Security.Permissions;


namespace Video.Presentation
{
    [Serializable]
    public class P2PVideoDummyClient
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
        public string UserName;
        public int MyId;
        List<AppDomain> appDummyDomains = new List<AppDomain>();

        public static List<object> objVideoDummies = new List<object>();

        public P2PVideoDummyClient(string uname)
		{
			try
			{
				this.UserName = uname;
			}
			catch (Exception exp)
			{
                exp.Data.Add("My Key", "P2PVideoDummyClient()--:--P2PVideoDummyClient.cs--:--" + exp.Message + " :--:--");
				//ClsException.LogError(exp);
				//ClsException.WriteToErrorLogFile(exp);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(exp.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + exp.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + exp.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
			}
        }

        public void VideoP2PClient(string ID, string P2PUri)
		{
			try
			{              
                AppDomainSetup setup = new AppDomainSetup();
                setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory ;
                appDummyDomains.Add(AppDomain.CreateDomain("Videop2pClient" + ID.ToString(), null, setup, new System.Security.PermissionSet(PermissionState.Unrestricted)));
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.ExtraInfo = AppDomain.CurrentDomain.ApplicationTrust.ExtraInfo;
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.DefaultGrantSet = new System.Security.Policy.PolicyStatement(new System.Security.PermissionSet(PermissionState.Unrestricted));
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.IsApplicationTrustedToRun = true;
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.Persist = true;
                objVideoDummies.Add(InstantiateDecimal(appDummyDomains[appDummyDomains.Count - 1], new DomainBinder(), new CultureInfo("en-US"), UserName, P2PUri));

			}
			catch (Exception exp)
			{
                exp.Data.Add("My Key", "VideoP2PClient()--:--P2PVideoDummyClient.cs--:--" + exp.Message + " :--:--");
				//ClsException.LogError(exp);
				//ClsException.WriteToErrorLogFile(exp);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(exp.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + exp.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + exp.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
			}
        }

        static object InstantiateDecimal(AppDomain appDomain, Binder binder, CultureInfo cultureInfo,string UName, string P2PUri)
        {
            try
            {
                object instance = appDomain.CreateInstance(
                   "Video.Presentation",
                   "Video.Presentation.P2PVideoClient",
                   false,
                   BindingFlags.Default,
                   binder,
                   new object[] {UName,P2PUri },
                   cultureInfo,
                   null,
                   null
                );
                return instance;
            }
            catch (Exception exp)
            {
                exp.Data.Add("My Key", "InstantiateDecimal()--:--P2PVideoDummyClient.cs--:--" + exp.Message + " :--:--");
                //ClsException.LogError(exp);
                //ClsException.WriteToErrorLogFile(exp);
                System.Text.StringBuilder sb = new StringBuilder();
                sb.AppendLine(exp.Message);
                sb.AppendLine();
                sb.AppendLine("StackTrace : " + exp.StackTrace);
                sb.AppendLine();
                sb.AppendLine("Location : " + exp.Data["My Key"].ToString());
                sb.AppendLine();
                sb1 = CreateTressInfo();
                sb.Append(sb1.ToString());
                VMuktiAPI.ClsLogging.WriteToTresslog(sb);
                return null;
            }
        }   


    }
}
