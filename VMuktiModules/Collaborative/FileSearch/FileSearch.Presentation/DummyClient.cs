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
using System.Reflection;
using System.Globalization;
using VMuktiAPI;
using System.Security.Permissions;

namespace FileSearch.Presentation
{
    [Serializable]
    public class DummyClient
    {
        //public static StringBuilder sb1;
        //public static StringBuilder CreateTressInfo()
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("User Is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName);
        //    sb.AppendLine();
        //    sb.Append("Peer Type is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.CurrPeerType.ToString());
        //    sb.AppendLine();
        //    sb.Append("User's SuperNode is : " + VMuktiAPI.VMuktiInfo.CurrentPeer.SuperNodeIP);
        //    sb.AppendLine();
        //    sb.Append("User's Machine Ip Address : " + VMuktiAPI.GetIPAddress.ClsGetIP4Address.GetIP4Address());
        //    sb.AppendLine();
        //    sb.AppendLine("----------------------------------------------------------------------------------------");
        //    return sb;
        //}
        public string UserName;
        List<AppDomain> appDummyDomains = new List<AppDomain>();
        public static List<object> objFileSearchDummies = new List<object>();

        public DummyClient(string uName)
        {
            UserName = uName;
        }

        public string FileSearchClient(int ID, string netP2pUri, string strFileSearchSNodeIp)
        {
            try
            {
                string httpUri = "http://" + strFileSearchSNodeIp + ":80/VMukti/FileSearch" + (objFileSearchDummies.Count + 1).ToString() + "/" + DateTime.Now.ToUniversalTime().Millisecond.ToString();
                AppDomainSetup setup = new AppDomainSetup();
                setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;

                appDummyDomains.Add(AppDomain.CreateDomain("DummyFileSearch" + ID.ToString(), null, setup, new System.Security.PermissionSet(PermissionState.Unrestricted)));
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.ExtraInfo = AppDomain.CurrentDomain.ApplicationTrust.ExtraInfo;
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.DefaultGrantSet = new System.Security.Policy.PolicyStatement(new System.Security.PermissionSet(PermissionState.Unrestricted));
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.IsApplicationTrustedToRun = true;
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.Persist = true;
                objFileSearchDummies.Add(InstantiateFileSearch(appDummyDomains[appDummyDomains.Count - 1], new DomainBinder(), new CultureInfo("en-US"), UserName, "", ID, netP2pUri, httpUri));
                return httpUri;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearchClient", "DummyClient.cs");
                return null;
            }
            //appDummyDomains.Add(AppDomain.CreateDomain("DummyFileSearch" + ID.ToString(), null, setup));
            //objFileSearchDummies.Add(InstantiateFileSearch(appDummyDomains[appDummyDomains.Count - 1], new DomainBinder(), new CultureInfo("en-US"), UserName, "", ID, netP2pUri, httpUri));
            //return httpUri;
        }

        public string FileSearchClientWithoutDummy(int ID, string netP2pUri, string strQASNodeIp)
        {
            try
            {
                string httpUri = "http://" + strQASNodeIp + ":80/VMukti/FileSearch" + (objFileSearchDummies.Count + 1).ToString() + "/" + DateTime.Now.ToUniversalTime().Millisecond.ToString();
                objFileSearchDummies.Add(new FileSearchDummy(UserName, "", ID, netP2pUri, httpUri));
                return httpUri;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FileSearchClientWithoutDummy", "DummyClient.cs");
                return null;
            }
        }

        static object InstantiateFileSearch(AppDomain appDomain, Binder binder, CultureInfo cultureInfo, string MyName, string UName, int Id, string netP2pUri, string httpUri)
        {
            try
            {
                object instance = appDomain.CreateInstance(
                   "FileSearch.Presentation",
                   "FileSearch.Presentation.FileSearchDummy",
                   false,
                   BindingFlags.Default,
                   binder,
                   new object[] { MyName, UName, Id, netP2pUri, httpUri },
                   cultureInfo,
                   null,
                   null
                );
                return instance;
            }
            catch (Exception exp)
            {
                if (exp.InnerException != null)
                {
                   // System.Windows.MessageBox.Show(exp.InnerException.Message);
                }
                else
                {
                    //System.Windows.MessageBox.Show("exp.Message " + exp.Message);
                }
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "InstantiateFileSearch", "DummyClient.cs");
                return null;
            }
        }
    }
}
