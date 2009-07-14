using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImageSharing.Business.Service.NetP2P;
using ImageSharing.Business.Service.DataContracts;
using ImageSharing.Business.Service.BasicHttp;
using VMuktiService;
using System.ServiceModel;
using System.Runtime.Remoting;
using System.Globalization;
using System.Reflection;
using VMuktiAPI;
using System.Security.Permissions;


namespace ImageSharing.Presentation
{
    [Serializable]
    public class P2PImageSharingDummyClient
    {
        public string UserName;
        public int MyId;
        List<AppDomain> appDummyDomains = new List<AppDomain>();

        public static List<object> objImageSharingDummies = new List<object>();

        public P2PImageSharingDummyClient(string uname)
        {
            try
            {
                this.UserName = uname;
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "P2PImageSharingDummyClient", "P2PImageSharingDummyClient.cs");
            }
        }

        public void ImageSharingP2PClient(string ID, string P2PUri)
        {
            try
            {
                AppDomainSetup setup = new AppDomainSetup();
                setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
                appDummyDomains.Add(AppDomain.CreateDomain("ImageSharingp2pClient" + ID.ToString(), null, setup, new System.Security.PermissionSet(PermissionState.Unrestricted)));
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.ExtraInfo = AppDomain.CurrentDomain.ApplicationTrust.ExtraInfo;
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.DefaultGrantSet = new System.Security.Policy.PolicyStatement(new System.Security.PermissionSet(PermissionState.Unrestricted));
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.IsApplicationTrustedToRun = true;
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.Persist = true;
                objImageSharingDummies.Add(InstantiateDecimal(appDummyDomains[appDummyDomains.Count - 1], new DomainBinder(), new CultureInfo("en-US"), UserName, P2PUri));
            }
            catch (Exception exp)
            {
                VMuktiHelper.ExceptionHandler(exp, "ImageSharingP2PClient", "P2PImageSharingDummyClient.cs");
            }
        }

        static object InstantiateDecimal(AppDomain appDomain, Binder binder, CultureInfo cultureInfo, string UName, string P2PUri)
        {
            try
            {
                object instance = appDomain.CreateInstance(
                   "ImageSharing.Presentation",
                   "ImageSharing.Presentation.P2PImageSharingClient",
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
                VMuktiHelper.ExceptionHandler(exp, "InstantiateDecimal", "P2PImageSharingDummyClient.cs");
                return null;
            }
        }
    }
}
