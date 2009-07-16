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
using DShowNET;

namespace DirectX.Capture
{
    /// <summary>
    ///  Provides collections of devices and compression codecs
    ///  installed on the system. 
    /// </summary>
    /// <example>
    ///  Devices and compression codecs are implemented in DirectShow 
    ///  as filters, see the <see cref="Filter"/> class for more 
    ///  information. To list the available video devices:
    ///  <code><div style="background-color:whitesmoke;">
    ///   Filters filters = new Filters();
    ///   foreach ( Filter f in filters.VideoInputDevices )
    ///   {
    ///		Debug.WriteLine( f.Name );
    ///   }
    ///  </div></code>
    ///  <seealso cref="Filter"/>
    /// </example>
    public class Filters
    {
        // ------------------ Public Properties --------------------

        /// <summary> Collection of available video capture devices. </summary>
        public FilterCollection VideoInputDevices = new FilterCollection(FilterCategory.VideoInputDevice);

        /// <summary> Collection of available audio capture devices. </summary>
        //public FilterCollection AudioInputDevices = new FilterCollection( FilterCategory.AudioInputDevice ); 
        public FilterCollection AudioInputDevices = null;

        /// <summary> Collection of available video compressors. </summary>
        public FilterCollection VideoCompressors = new FilterCollection(FilterCategory.VideoCompressorCategory);

        /// <summary> Collection of available audio compressors. </summary>
        //public FilterCollection AudioCompressors = new FilterCollection( FilterCategory.AudioCompressorCategory ); 
        public FilterCollection AudioCompressors = null;

    }
}
