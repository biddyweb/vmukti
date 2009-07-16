using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VMuktiAPI;
using System.Security.Permissions;
using System.Reflection;
using System.Globalization;

namespace Desktop_Sharing.Presentation
{
    [Serializable]
    public class DummyClient
    {
        public string UserName;
        public int MyId;
        List<AppDomain> appDummyDomains = new List<AppDomain>();

        public static List<object> objDesktopDummies = new List<object>();

        public DummyClient(string uname)
        {
            try
            {
                this.UserName = uname;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DummyClient", "DummyClient.cs");
            }
        }

        public string DesktopClient(int ID, string netP2pUri, string strDesktopSNodeIp)
        {
            try
            {
                 string httpUri = "http://" + strDesktopSNodeIp + ":80/VMukti/Desktop" + (objDesktopDummies.Count + 1).ToString() + "/" + DateTime.Now.ToUniversalTime().Millisecond.ToString();
                //string httpUri = "http://" + strDesktopSNodeIp + ":80/VMukti/Desktop"; 
                AppDomainSetup setup = new AppDomainSetup();
                setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
                appDummyDomains.Add(AppDomain.CreateDomain("DummyDesktop" + ID.ToString(), null, setup, new System.Security.PermissionSet(PermissionState.Unrestricted)));
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.ExtraInfo = AppDomain.CurrentDomain.ApplicationTrust.ExtraInfo;
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.DefaultGrantSet = new System.Security.Policy.PolicyStatement(new System.Security.PermissionSet(PermissionState.Unrestricted));
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.IsApplicationTrustedToRun = true;
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.Persist = true;

                objDesktopDummies.Add(InstantiateDecimal(appDummyDomains[appDummyDomains.Count - 1], new DomainBinder(), new CultureInfo("en-US"), UserName, "", ID, netP2pUri, httpUri));
                return httpUri;

            }
            catch (Exception ex)
            {

                VMuktiHelper.ExceptionHandler(ex, "DesktopClient", "DummyClient.cs");
                return null;
            }
        }



        static object InstantiateDecimal(AppDomain appDomain, Binder binder, CultureInfo cultureInfo, string MyName, string UName, int Id, string netP2pUri, string httpUri)
        {
            try
            {

                object instance = appDomain.CreateInstance(
                   "Desktop_Sharing.Presentation",
                   "Desktop_Sharing.Presentation.DesktopDummy",

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

                VMuktiHelper.ExceptionHandler(ex, "InstantiateDecimal", "DummyClient.cs");
                return null;
            }
        }   

    }
}
