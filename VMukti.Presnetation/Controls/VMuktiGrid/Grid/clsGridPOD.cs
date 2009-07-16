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

//this was to pass/call functions of pods (ie. inter pod communication), it may or many not be used just now... can be depricated after safe testing.

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using VMuktiAPI;

namespace VMuktiGrid.CustomGrid
{
    public static partial class VMuktiHelper 
    {
        public static bool IsDraggingPOD = false;
        public static double RectSuggestHeight = 25.0;
        public static bool IsRectSuggestAdded = false;
        public static ctlPOD objPOD = null;

        public static List<VMuktiEvents> VMEvents = new List<VMuktiEvents>();

        public static void CallEvent(string eventName, object sender, VMuktiEventArgs e)
        {
            try
            {
                foreach (VMuktiEvents ve in VMEvents)
                {
                    if (ve.EventName.Equals(eventName))
                    {
                        ve.FireVMuktiEvent(sender, e);
                       
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CallEvent()", "Controls\\VMuktiGrid\\Grid\\clsGridPod.cs");
            }
        }

        public static VMuktiEvents RegisterEvent(string eventName)
        {
            try
            {
                VMEvents.Add(new VMuktiEvents(eventName));
                return VMEvents[VMEvents.Count - 1];
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "RegisterEvent()", "Controls\\VMuktiGrid\\Grid\\clsGridPod.cs");                
                return null;
            }
        }

        public static void UnRegisterEvent(string eventName)
        {
            try
            {
                for (int i = 0; i < VMEvents.Count; i++)
                {
                    if (VMEvents[i].EventName == eventName)
                    {
                        VMEvents.RemoveAt(i);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "UnRegisterEvent()", "Controls\\VMuktiGrid\\Grid\\clsGridPod.cs");
            }
        }
    }

    #region Add Events

    public class VMuktiEvents : IDisposable
    {      
        private bool disposed = false;
        public string EventName = "";
        public delegate void VMuktiEventHandler(object sender, VMuktiEventArgs e);
        public event VMuktiEventHandler VMuktiEvent;

        public VMuktiEvents(string eventName)
        {
            try
            {
                EventName = eventName;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "VmuktiEvents", "Controls\\VMuktiGrid\\Grid\\clsGridPod.cs");
            }
        }
        public void FireVMuktiEvent(object sender, VMuktiEventArgs e)
        {
            try
            {
                if (VMuktiEvent != null)
                {
                    VMuktiEvent(sender, e);
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FireVMuktiEvnt()", "Controls\\VMuktiGrid\\Grid\\clsGridPod.cs");
            }
        }
        #region IDisposable Members

        public void Dispose()
        {
            try
            {
                Dispose(true);
                GC.SuppressFinalize(this);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose()", "Controls\\VMuktiGrid\\Grid\\clsGridPod.cs");
            }
        }
        public void Dispose(bool disposing)
        {
            try
            {
                if (!this.disposed)
                {
                    if (disposing)
                    {
                        try
                        {

                        }
                        catch { }
                    }
                    disposed = true;
                }
                GC.SuppressFinalize(this);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose(bool disposing)", "Controls\\VMuktiGrid\\Grid\\clsGridPod.cs");
            }
        }
        ~VMuktiEvents()
        {
            try
            {
                Dispose(false);
            }
        #endregion

            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "VmuktiEvents(bool disposing)", "Controls\\VMuktiGrid\\Grid\\clsGridPod.cs");
            }
        }
    #endregion
    }
    # region VMukti Event Args

    public class VMuktiEventArgs : EventArgs
    {
        private bool disposed = false;
        public List<object> _args = new List<object>();
        public VMuktiEventArgs(params object[] parameters)
        {
            _args.AddRange(parameters);
        }

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose()", "Controls\\VMuktiGrid\\Grid\\clsGridPod.cs");
            }
        }
        public void Dispose(bool disposing)
        {
            
            if (!this.disposed)
			{
				if (disposing)
				{
					try
					{
                        _args.Clear();
					}
					catch (Exception ex)
					{
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Controls--VmuktiEventArgs", "Controls\\VMuktiGrid\\Grid\\clsGridPod.cs");
					}
				}
				disposed = true;
			}
			GC.SuppressFinalize(this);
        }
           
        ~VMuktiEventArgs()
        {
            Dispose(false);
        }

        #endregion
    }


    #endregion

    class ModuleServer
    {
        ModuleServer(string UniqueStr)
        {
        }
    }
}
