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
using System.Collections;
using System.Runtime.InteropServices;
using DShowNET;
using DShowNET.Device;
using System.Collections.Generic;

namespace DirectX.Capture
{
    /// <summary>
    ///	 A collection of Filter objects (DirectShow filters).
    ///	 This is used by the <see cref="Capture"/> class to provide
    ///	 lists of capture devices and compression filters. This class
    ///	 cannot be created directly.
    /// </summary>
    public class FilterCollection : CollectionBase
    {
        /// <summary> Populate the collection with a list of filters from a particular category. </summary>
        /// 
        List<string> lstVideoInput = null;

        internal FilterCollection(Guid category)
        {
            lstVideoInput = new List<string>();
            getFilters(category);
        }

        /// <summary> Populate the InnerList with a list of filters from a particular category </summary>
        protected void getFilters(Guid category)
        {
            int hr;
            object comObj = null;
            ICreateDevEnum enumDev = null;
            UCOMIEnumMoniker enumMon = null;
            UCOMIMoniker[] mon = new UCOMIMoniker[1];

            try
            {
                // Get the system device enumerator
                Type srvType = Type.GetTypeFromCLSID(Clsid.SystemDeviceEnum);
                if (srvType == null)
                    throw new NotImplementedException("System Device Enumerator");
                comObj = Activator.CreateInstance(srvType);
                enumDev = (ICreateDevEnum)comObj;

                // Create an enumerator to find filters in category
                hr = enumDev.CreateClassEnumerator(ref category, out enumMon, 0);
                if (hr != 0)
                    throw new NotSupportedException("No devices of the category");

                // Loop through the enumerator
                int f;
                do
                {
                    // Next filter
                    hr = enumMon.Next(1, mon, out f);
                    if ((hr != 0) || (mon[0] == null))
                        break;

                    // Add the filter
                    Filter filter = new Filter(mon[0]);
                    InnerList.Add(filter);
                    lstVideoInput.Add(filter.Name);
                    // Release resources
                    Marshal.ReleaseComObject(mon[0]);
                    mon[0] = null;
                }
                while (true);

                // Sort
                InnerList.Sort();
            }
            finally
            {
                enumDev = null;
                if (mon[0] != null)
                    Marshal.ReleaseComObject(mon[0]); mon[0] = null;
                if (enumMon != null)
                    Marshal.ReleaseComObject(enumMon); enumMon = null;
                if (comObj != null)
                    Marshal.ReleaseComObject(comObj); comObj = null;
            }
        }

        /// <summary> Get the filter at the specified index. </summary>
        public Filter this[int index]
        {
            get { return ((Filter)InnerList[index]); }
        }
        
        public List<string> GetVideoInputDevices()
        {
            lstVideoInput.Sort();
            return lstVideoInput;
        }
    }
}
