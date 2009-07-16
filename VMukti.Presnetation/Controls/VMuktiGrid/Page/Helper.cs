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
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Markup;
using System.Xml;
using System.IO;
using System.ComponentModel;
using System.Windows.Controls;
using VMuktiAPI;

namespace VMuktiGrid.ctlPage
{
    class Helper: IDisposable
    {

        private bool disposed = false;

        /// <summary>
        /// Find a specific parent object type in the visual tree
        /// </summary>
        public static T FindParentControl<T>(DependencyObject outerDepObj) where T : DependencyObject
        {
            try
            {
                DependencyObject dObj = VisualTreeHelper.GetParent(outerDepObj);
                if (dObj == null)
                    return null;

                if (dObj is T)
                    return dObj as T;

                while ((dObj = VisualTreeHelper.GetParent(dObj)) != null)
                {
                    if (dObj is T)
                        return dObj as T;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FindParentControl()", "Controls\\VMuktiGrid\\Page\\Helper.cs");
            }
            return null;
        }

        /// <summary>
        /// Find the Panel for the TabControl
        /// </summary>
        public static VirtualizingTabPanel FindVirtualizingTabPanel(Visual visual)
        {
            try
            {
                if (visual == null)
                    return null;

                for (int i = 0; i < VisualTreeHelper.GetChildrenCount(visual); i++)
                {
                    Visual child = VisualTreeHelper.GetChild(visual, i) as Visual;

                    if (child != null)
                    {
                        if (child is VirtualizingTabPanel)
                        {
                            object temp = child;
                            return (VirtualizingTabPanel)temp;
                        }

                        VirtualizingTabPanel panel = FindVirtualizingTabPanel(child);
                        if (panel != null)
                        {
                            object temp = panel;
                            return (VirtualizingTabPanel)temp; // return the panel up the call stack
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "FindVirtualization()", "Controls\\VMuktiGrid\\Page\\Helper.cs");
            }
            return null;
        }

        /// <summary>
        /// Clone an element
        /// </summary>
        /// <param name="elementToClone"></param>
        /// <returns></returns>
        public static object CloneElement(object elementToClone)
        {
            try
            {
                string xaml = XamlWriter.Save(elementToClone);
                return XamlReader.Load(new XmlTextReader(new StringReader(xaml)));
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CloneElement()", "Controls\\VMuktiGrid\\Page\\Helper.cs");                
                return null;
            }
        }


        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public void Dispose(bool disposing)
        {
            if (!this.disposed)
			{
				if (disposing)
				{
					try
					{
                       
					}
                    catch (Exception ex)
                    {
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose()", "Controls\\VMuktiGrid\\Page\\Helper.cs");
                    }
				}
				disposed = true;
			}
			GC.SuppressFinalize(this);
        }

        ~Helper()
        {
            Dispose(false);
        }

        #endregion
    }
}
