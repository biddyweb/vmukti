using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VMuktiAPI;
using System.Security.Permissions;
using System.Globalization;
using System.Reflection;

namespace Desktop_Sharing.Presentation
{
    [Serializable]
    public class P2PDesktopDummyClient
    {

        public string UserName;
        public int MyId;
        List<AppDomain> appDummyDomains = new List<AppDomain>();

        public static List<object> objDesktopDummies = new List<object>();

        public P2PDesktopDummyClient(string uname)
		{
			try
			{
				this.UserName = uname;
			}
			catch (Exception exp)
			{
                VMuktiHelper.ExceptionHandler(exp, "P2PDesktopDummyClient", "P2PDesktopDummyClient.cs");
			}
        }

        public void DesktopP2PClient(string ID, string P2PUri)
        {
            try
            {
                AppDomainSetup setup = new AppDomainSetup();
                setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
                appDummyDomains.Add(AppDomain.CreateDomain("Desktopp2pClient" + ID.ToString(), null, setup, new System.Security.PermissionSet(PermissionState.Unrestricted)));
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.ExtraInfo = AppDomain.CurrentDomain.ApplicationTrust.ExtraInfo;
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.DefaultGrantSet = new System.Security.Policy.PolicyStatement(new System.Security.PermissionSet(PermissionState.Unrestricted));
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.IsApplicationTrustedToRun = true;
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.Persist = true;
                objDesktopDummies.Add(InstantiateDecimal(appDummyDomains[appDummyDomains.Count - 1], new DomainBinder(), new CultureInfo("en-US"), UserName, P2PUri));

            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "DesktopP2PClient", "P2PDesktopDummyClient.cs");
            }
        }

        static object InstantiateDecimal(AppDomain appDomain, Binder binder, CultureInfo cultureInfo, string UName, string P2PUri)
        {
            try
            {
                object instance = appDomain.CreateInstance(
                   "Desktop_Sharing.Presentation",
                   "Desktop_Sharing.Presentation.P2PDesktopClient",
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

                VMuktiHelper.ExceptionHandler(exp, "InstantiateDecimal", "P2PDesktopDummyClient.cs");
                return null;
            }
        }   

    }
}
