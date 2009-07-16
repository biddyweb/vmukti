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
using System.Globalization;
using System.Reflection;
using System.Security.Permissions;
using VMuktiAPI;
using System.Text;

namespace Audio.Presentation
{
    [Serializable]
    public class DummyClient
    {
        public string UserName;
        public int MyId;
        List<AppDomain> appDummyDomains = new List<AppDomain>();
        public static List<object> objAudioDummies = new List<object>();

        public DummyClient(string uname)
        {
            try
            {
                this.UserName = uname;
                //MyId = int.Parse(UserName.Split('~')[1]);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "DummyClinet()", "Audio\\DummyClient");
            }
        }

        public string AudioClient(int ID, string netP2pUri, string strAudioSNodeIp)
        {
            try
            {
                string httpUri = "http://" + strAudioSNodeIp + ":80/VMukti/Audio" + (objAudioDummies.Count + 1).ToString();
                AppDomainSetup setup = new AppDomainSetup();
                setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
                //appDummyDomains.Add(AppDomain.CreateDomain("DummyAudio" + ID.ToString(), null, setup));
                //objAudioDummies.Add(InstantiateAudio(appDummyDomains[appDummyDomains.Count - 1], new DomainBinder(), new CultureInfo("en-US"), UserName, "", ID, netP2pUri, httpUri));

                appDummyDomains.Add(AppDomain.CreateDomain("DummyAudio" + ID.ToString(), null, setup, new System.Security.PermissionSet(PermissionState.Unrestricted)));
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.ExtraInfo = AppDomain.CurrentDomain.ApplicationTrust.ExtraInfo;
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.DefaultGrantSet = new System.Security.Policy.PolicyStatement(new System.Security.PermissionSet(PermissionState.Unrestricted));
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.IsApplicationTrustedToRun = true;
                appDummyDomains[appDummyDomains.Count - 1].ApplicationTrust.Persist = true;
                objAudioDummies.Add(InstantiateAudio(appDummyDomains[appDummyDomains.Count - 1], new DomainBinder(), new CultureInfo("en-US"), UserName, "", ID, netP2pUri, httpUri));

                return httpUri;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AudioClient()", "Audio\\DummyClient");                
                return null;
                
            }
        }
        

        public string AudioClientWithoutDummy(int ID, string netP2pUri, string strAudioSNodeIp)
        {
            try
            {
                string httpUri = "http://" + strAudioSNodeIp + ":80/VMukti/Audio" + (objAudioDummies.Count + 1).ToString();
                objAudioDummies.Add(new AudioDummy(UserName, "", ID, netP2pUri, httpUri));
                return httpUri;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AudioClientWithOutDummy()", "Audio\\DummyClient");                
                return null;
            }
        }

        static object InstantiateAudio(AppDomain appDomain, Binder binder, CultureInfo cultureInfo, string MyName, string UName, int Id, string netP2pUri, string httpUri)
        {
            try
            {

                object instance = appDomain.CreateInstance(
                   "Audio.Presentation",
                   "Audio.Presentation.AudioDummy",
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
                VMuktiHelper.ExceptionHandler(ex, "InstantiateAudio()", "Audio\\DummyClient");                
                return null;
            }
        }
    }
}
