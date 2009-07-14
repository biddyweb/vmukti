
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
using System.Text;
using CoAuthering.Business.NetP2P;
using CoAuthering.Business.DataContracts;
using CoAuthering.Business.BasicHTTP;
using VMuktiService;
using System.ServiceModel;
using System.Runtime.Remoting;
using System.Globalization;
using System.Reflection;
using VMuktiAPI;
using System.Security.Permissions;

namespace CoAuthering.Presentation
{
    [Serializable]
    public class P2PCoAuthDummyClient
    {
        
        public string UserName;
        public int MyId;
        List<AppDomain> appDummyDomains = new List<AppDomain>();

        public static List<object> objCoAuthDummies = new List<object>();

        public P2PCoAuthDummyClient(string uname)
        {
            try
            {
                this.UserName = uname;
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "P2PCoAuthClient", "P2PCoAuthDummyClient.cs");

            }
        }

        public void CoAuthP2PClient(string ID, string P2PUri)
        {
            try
            {
                AppDomainSetup setup = new AppDomainSetup();
                setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
                appDummyDomains.Add(AppDomain.CreateDomain("CoAuthp2pClient" + ID.ToString(), null, setup, new System.Security.PermissionSet(PermissionState.Unrestricted)));
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.ExtraInfo = AppDomain.CurrentDomain.ApplicationTrust.ExtraInfo;
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.DefaultGrantSet = new System.Security.Policy.PolicyStatement(new System.Security.PermissionSet(PermissionState.Unrestricted));
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.IsApplicationTrustedToRun = true;
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.Persist = true;
                objCoAuthDummies.Add(InstantiateDecimal(appDummyDomains[appDummyDomains.Count - 1], new DomainBinder(), new CultureInfo("en-US"), UserName, P2PUri));

            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "CoAuthP2PClient", "P2PCoAuthDummyClient.cs");
            }
        }

        static object InstantiateDecimal(AppDomain appDomain, Binder binder, CultureInfo cultureInfo, string UName, string P2PUri)
        {
            try
            {
                object instance = appDomain.CreateInstance(
                   "CoAuthering.Presentation",
                   "CoAuthering.Presentation.P2PCoAuthClient",
                   false,
                   BindingFlags.Default,
                   binder,
                   new object[] { UName, P2PUri },
                   cultureInfo,
                   null,
                   null
                );
                return instance;
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "InstantiateDecimal", "P2PCoAuthDummyClient.cs");

                return null;
            }
        }
    }
}
