using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Permissions;
using System.Reflection;
using System.Globalization;
using VMuktiAPI;

namespace ImageSharing.Presentation
{
    [Serializable]
    public class DummyClient
    {
        public string UserName;
        public int MyId;
        List<AppDomain> appDummyDomains = new List<AppDomain>();

        public static List<object> objImageSharingDummies = new List<object>();

        public DummyClient(string uname)
        {
            try
            {
                this.UserName = uname;
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "DummyClient", "DummyClient.cs");
            }
        }

        public string ImageSharingClient(int ID, string netP2pUri, string strVideoSNodeIp)
        {
            try
            {

                string httpUri = "http://" + strVideoSNodeIp + ":80/VMukti/ImageSharing" + (objImageSharingDummies.Count + 1).ToString() + "/" + DateTime.Now.ToUniversalTime().Millisecond.ToString();
                //string httpUri = "http://" + strVideoSNodeIp + ":80/VMukti/ImageSharing";
                AppDomainSetup setup = new AppDomainSetup();
                setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
                appDummyDomains.Add(AppDomain.CreateDomain("DummyImageSharing" + ID.ToString(), null, setup, new System.Security.PermissionSet(PermissionState.Unrestricted)));
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.ExtraInfo = AppDomain.CurrentDomain.ApplicationTrust.ExtraInfo;
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.DefaultGrantSet = new System.Security.Policy.PolicyStatement(new System.Security.PermissionSet(PermissionState.Unrestricted));
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.IsApplicationTrustedToRun = true;
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.Persist = true;

                objImageSharingDummies.Add(InstantiateDecimal(appDummyDomains[appDummyDomains.Count - 1], new DomainBinder(), new CultureInfo("en-US"), UserName, "", ID, netP2pUri, httpUri));
                return httpUri;

            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ImageSharingClient", "DummyClient.cs");
                return null;
            }
        }

        static object InstantiateDecimal(AppDomain appDomain, Binder binder, CultureInfo cultureInfo, string MyName, string UName, int Id, string netP2pUri, string httpUri)
        {
            try
            {

                object instance = appDomain.CreateInstance(
                   "ImageSharing.Presentation",
                   "ImageSharing.Presentation.ImageSharingDummy",
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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "InstantiateDecimal", "DummyClient.cs");
                return null;
            }
        }   
    }
}
