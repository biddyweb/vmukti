/* VMukti 2.0 -- An Open Source Video Communications Suite
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
using NetFwTypeLib;

namespace VMukti.Presentation
{
    class WinXPSP2FireWall
    {
        public enum FW_ERROR_CODE
        {
            FW_NOERROR = 0,
            FW_ERR_INITIALIZED,					// Already initialized or doesn't call Initialize()
            FW_ERR_CREATE_SETTING_MANAGER,		// Can't create an instance of the firewall settings manager
            FW_ERR_LOCAL_POLICY,				// Can't get local firewall policy
            FW_ERR_PROFILE,						// Can't get the firewall profile
            FW_ERR_FIREWALL_IS_ENABLED,			// Can't get the firewall enable information
            FW_ERR_FIREWALL_ENABLED,			// Can't set the firewall enable option
            FW_ERR_INVALID_ARG,					// Invalid Arguments
            FW_ERR_AUTH_APPLICATIONS,			// Failed to get authorized application list
            FW_ERR_APP_ENABLED,					// Failed to get the application is enabled or not
            FW_ERR_CREATE_APP_INSTANCE,			// Failed to create an instance of an authorized application
            FW_ERR_SYS_ALLOC_STRING,			// Failed to alloc a memory for BSTR
            FW_ERR_PUT_PROCESS_IMAGE_NAME,		// Failed to put Process Image File Name to Authorized Application
            FW_ERR_PUT_REGISTER_NAME,			// Failed to put a registered name
            FW_ERR_ADD_TO_COLLECTION,			// Failed to add to the Firewall collection
            FW_ERR_REMOVE_FROM_COLLECTION,		// Failed to remove from the Firewall collection
            FW_ERR_GLOBAL_OPEN_PORTS,			// Failed to retrieve the globally open ports
            FW_ERR_PORT_IS_ENABLED,				// Can't get the firewall port enable information
            FW_ERR_PORT_ENABLED,				// Can't set the firewall port enable option
            FW_ERR_CREATE_PORT_INSTANCE,		// Failed to create an instance of an authorized port
            FW_ERR_SET_PORT_NUMBER,				// Failed to set port number
            FW_ERR_SET_IP_PROTOCOL,				// Failed to set IP Protocol
            FW_ERR_EXCEPTION_NOT_ALLOWED,		// Failed to get or put the exception not allowed
            FW_ERR_NOTIFICATION_DISABLED,		// Failed to get or put the notification disabled
            FW_ERR_UNICAST_MULTICAST,			// Failed to get or put the UnicastResponses To MulticastBroadcast Disabled Property 
            FW_ERR_APPLICATION_ITEM,            // Failed to returns the specified application if it is in the collection.
            FW_ERR_SAME_PORT_EXIST,             // The port which you try to add is already existed.
            FW_ERR_UNKNOWN,                     // Unknown Error or Exception occured
        };

        INetFwProfile m_FirewallProfile = null;

        public FW_ERROR_CODE Initialize()
        {
            if (m_FirewallProfile != null)
                return FW_ERROR_CODE.FW_ERR_INITIALIZED;

            Type typFwMgr = null;
            INetFwMgr fwMgr = null;

            typFwMgr = Type.GetTypeFromCLSID(new Guid("{304CE942-6E39-40D8-943A-B913C40C9CD4}"));
            fwMgr = (INetFwMgr)Activator.CreateInstance(typFwMgr);
            if (fwMgr == null)
                return FW_ERROR_CODE.FW_ERR_CREATE_SETTING_MANAGER;
            INetFwPolicy fwPolicy = fwMgr.LocalPolicy;
            if (fwPolicy == null)
                return FW_ERROR_CODE.FW_ERR_LOCAL_POLICY;

            try
            {
                m_FirewallProfile = fwPolicy.GetProfileByType(fwMgr.CurrentProfileType);
            }
            catch
            {
                return FW_ERROR_CODE.FW_ERR_PROFILE;
            }

            return FW_ERROR_CODE.FW_NOERROR;
        }

        public FW_ERROR_CODE IsWindowsFirewallOn(ref bool bOn)
        {
            bOn = false;

            if (m_FirewallProfile == null)
                return FW_ERROR_CODE.FW_ERR_INITIALIZED;

            bOn = m_FirewallProfile.FirewallEnabled;

            return FW_ERROR_CODE.FW_NOERROR;
        }

    }
}
