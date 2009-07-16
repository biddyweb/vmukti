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
using System.Diagnostics;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace VMukti.Presentation
{
	/// <summary>
	/// Trace listener that writes formatted messages to the Visual Studio debugger output.
	/// </summary>
	[ConfigurationElementType(typeof(CustomTraceListenerData))]
	public class DebugTraceListener : CustomTraceListener
    {
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if (data is LogEntry && this.Formatter != null) 
            {
                this.WriteLine(this.Formatter.Format(data as LogEntry));
            }
            else
            {
                this.WriteLine(data.ToString());
            }
        }
               

        /// <summary>
        /// Writes a message to the debug window 
        /// </summary>
        /// <param name="message">The string to write to the debug window</param>
        public override void Write(string message)
        {
            Debug.Write(message);
        }

        /// <summary>
        /// Writes a message to the debug window 
        /// </summary>
        /// <param name="message">The string to write to the debug window</param>
        public override void WriteLine(string message)
        {
            Debug.WriteLine(message);
        }
 
	}
}
