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
//using System.Linq;
using System.Text;
using System.Reflection;
using System.Globalization;
using System.Runtime.Remoting;
using System.Collections.Generic;
using System.Runtime.Serialization;
using VMuktiAPI;


namespace Video.Presentation
{
    [Serializable]
    public class UserVideoDummyClient
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
        public int MyId;
        List<AppDomain> appDummyDomains = new List<AppDomain>();

        public static List<object> objUserVideoClientDummies = new List<object>();

        public UserVideoDummyClient(string UName)
        {
            try
            {
                this.UserName = UName;
            }
             catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UserVideoDummyClient", "UserVideoDummyClient.cs");             
            }
        }

        public string UserVideoClientWithDomain(int ID, string netP2pUri, string strUserVideoSNodeIp)
        {
            try
            {
                string httpUri = "http://" + strUserVideoSNodeIp + ":80/UserVideo" + (objUserVideoClientDummies.Count + 1).ToString();
                AppDomainSetup setup = new AppDomainSetup();
                setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
                appDummyDomains.Add(AppDomain.CreateDomain("UserVideoDummy" + ID.ToString(), null, setup));
                objUserVideoClientDummies.Add(InstantiateDecimal(appDummyDomains[appDummyDomains.Count - 1], new DomainBinder(), new CultureInfo("en-US"), UserName, "", ID, netP2pUri, httpUri));
                return httpUri;
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UserVideoClientWithDomain", "UserVideoDummyClient.cs");             
                return null;
            }
        }

        public string UserVideoClientWithoutDomain(int ID, string netP2pUri, string strUserVideoSNodeIp)
        {
            try
            {
                string httpUri = "http://" + strUserVideoSNodeIp + ":80/UserVideo" + (objUserVideoClientDummies.Count + 1).ToString();
                objUserVideoClientDummies.Add(new UserVideoDummies(UserName, "", ID, netP2pUri, httpUri));
                return httpUri;
            }

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UserVideoClientWithoutDomain", "UserVideoDummyClient.cs");             
                return null;
            }
        }

        //static object InstantiateDecimal(AppDomain appDomain, Binder binder, CultureInfo cultureInfo, string MyName, string UName, int Id, string netP2pUri, string httpUri)
        //{
        //    try
        //    {
        //        object instance = appDomain.CreateInstance(
        //           "Weblink.Presentation",
        //           "Weblink.Presentation.WebLinkDummies",
        //           false,
        //           BindingFlags.Default,
        //           binder,
        //           new object[] { MyName, UName, Id, netP2pUri, httpUri },
        //           cultureInfo,
        //           null,
        //           null
        //        );
        //        return instance;
        //    }
        //    catch (Exception exp)
        //    {
        //        System.Windows.MessageBox.Show("InstantiateDecimal" + exp.Message);
        //        if (exp.InnerException != null)
        //        {
        //            System.Windows.MessageBox.Show("InstantiateDecimal " + exp.InnerException.Message);
        //        }
        //        throw exp;
        //        return null;
        //    }
        //}

        static object InstantiateDecimal(AppDomain appDomain, Binder binder, CultureInfo cultureInfo, string MyName, string UName, int Id, string netP2pUri, string httpUri)
        {
            try
            {

                object instance = appDomain.CreateInstance(
                   "Video.Presentation",
                   "Video.Presentation.UserVideoDummies",
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

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "InstantiatedDecimal", "UserVideoDummyClient.cs");             
                return null;
            }
        }
    }
}
