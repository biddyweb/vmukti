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
#region Version Header

/// <remarks>

///   Description: This is buddylist for pod this class will be loaded as part of ctlPOD.xaml.

/// </remarks>

#endregion

using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VMuktiAPI;

namespace VMuktiGrid.Buddy
{
    /// <summary>
    /// Interaction logic for comboBox.xaml
    /// </summary>
    public partial class ctlPodBuddyList : UserControl, IDisposable
    {
        public static StringBuilder sb1 = new StringBuilder();
        private bool disposed = false;
        private int intBuddyCount = 0;

        public ctlPodBuddyList()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlPodBuddyList()", "Controls\\VMuktiGrid\\Buddy\\ctlPodBuddyList.xaml.cs");
            }
        }

        private void btnMenu_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                menuPopUP.IsOpen = (!menuPopUP.IsOpen);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnMenu_Click()", "Controls\\VMuktiGrid\\Buddy\\ctlPodBuddyList.xaml.cs");
            }
        }

        private void btnClosePopUp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                menuPopUP.IsOpen = (!menuPopUP.IsOpen);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "btnClosePopUp_Click()", "Controls\\VMuktiGrid\\Buddy\\ctlPodBuddyList.xaml.cs");
            }
        }

        public bool AddBuddy(string strBuddyName)
        {
            try
            {
                int i = 0, tempBuddyCount=0;
                for (i = 0; i < stPanel.Children.Count; i++)
                {
                    if (stPanel.Children[i].GetType() == typeof(ctlBuddy) )
                    {
                        if (((ctlBuddy)stPanel.Children[i]).Title == strBuddyName)
                        {
                            break;
                        }
                        tempBuddyCount++;
                    }
                }
                if (tempBuddyCount == intBuddyCount)
                {
                    ctlBuddy objBuddy = new ctlBuddy();
                    objBuddy.Title = strBuddyName;
                    stPanel.Children.Add(objBuddy);
                    intBuddyCount++;

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AddBuddy()", "Controls\\VMuktiGrid\\Buddy\\ctlPodBuddyList.xaml.cs");                
                return false;
            }
        }
        
        public bool RemoveBuddy(string strBuddyName)
        {
            try
            {
                int i = 0;
                bool blnIsAdded = false;
                for (i = 0; i < stPanel.Children.Count; i++)
                {
                    if (stPanel.Children[i].GetType() == typeof(ctlBuddy))
                    {
                        if (((ctlBuddy)stPanel.Children[i]).Title == strBuddyName)
                        {
                            blnIsAdded = true;
                            break;
                        }                        
                    }
                }
                if (blnIsAdded)
                {
                    stPanel.Children.RemoveAt(i);
                    intBuddyCount--;
                    return true;
                }                
                return false;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "RemoveBuddy()", "Controls\\VMuktiGrid\\Buddy\\ctlPodBuddyList.xaml.cs");
                return false;
            }
        }

        //showing buddy in the list once the module are completely loaded on that buddy screen- not used right now / partialy implemented
        public bool AddBuddy(string strBuddyName, int intMaxCounter)
        {
            try
            {
                int i = 0, tempBuddyCount = 0;
                for (i = 0; i < stPanel.Children.Count; i++)
                {
                    if (stPanel.Children[i].GetType() == typeof(ctlBuddy))
                    {
                        if (((ctlBuddy)stPanel.Children[i]).Title == strBuddyName)
                        {
                            break;
                        }
                        tempBuddyCount++;
                    }
                }
                if (tempBuddyCount == intBuddyCount)
                {
                    ctlBuddy objBuddy = new ctlBuddy();
                    objBuddy.MaxCounter = intMaxCounter;
                    objBuddy.Title = strBuddyName;
                    stPanel.Children.Add(objBuddy);
                    intBuddyCount++;

                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AddBuddy()", "Controls\\VMuktiGrid\\Buddy\\ctlPodBuddyList.xaml.cs");                
                return false;
            }
        }

        //showing buddy in the list once the module are completely loaded on that buddy screen- not used right now / partialy implemented
        public void SetMaxCounter(int intMaxCounter, string strBuddyName)
        {
            try
            {
                for (int i = 0; i < stPanel.Children.Count; i++)
                {
                    if (stPanel.Children[i].GetType() == typeof(ctlBuddy))
                    {
                        if (((ctlBuddy)stPanel.Children[i]).Title == strBuddyName)
                        {
                            ((ctlBuddy)stPanel.Children[i]).MaxCounter = intMaxCounter;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "SetMaxCounter()", "Controls\\VMuktiGrid\\Buddy\\ctlPodBuddyList.xaml.cs");
            }
        }

        public bool CheckBuddy(string strBuddyName)
        {
            try
            {
                int i = 0, tempBuddyCount = 0;
                for (i = 0; i < stPanel.Children.Count; i++)
                {
                    if (stPanel.Children[i].GetType() == typeof(ctlBuddy))
                    {
                        if (((ctlBuddy)stPanel.Children[i]).Title == strBuddyName)
                        {
                            break;
                        }
                        tempBuddyCount++;
                    }
                }
                if (tempBuddyCount == intBuddyCount)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CheckBuddy()", "Controls\\VMuktiGrid\\Buddy\\ctlPodBuddyList.xaml.cs");                
                return false;
            }
        }

        //showing buddy in the list once the module are completely loaded on that buddy screen- not used right now / partialy implemented
        public void ShowBuddy(string strBuddyName)
        {
            try
            {
                int i = 0;
                for (i = 0; i < stPanel.Children.Count; i++)
                {
                    if (stPanel.Children[i].GetType() == typeof(ctlBuddy))
                    {
                        if (((ctlBuddy)stPanel.Children[i]).Title == strBuddyName)
                        {
                            ((ctlBuddy)stPanel.Children[i]).Show();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ShowBuddy()", "Controls\\VMuktiGrid\\Buddy\\ctlPodBuddyList.xaml.cs");
            }
        }

        //To send the list of all added buddies to newly entering buddy called from buddy drop event from page, tab and pod... get sent along with unique URI
        public string[] GetBuddies()
        {
            try
            {
                int i = 0;
                List<string> lstBuddies = new List<string>();

                for (i = 0; i < stPanel.Children.Count; i++)
                {
                    if (stPanel.Children[i].GetType() == typeof(ctlBuddy))
                    {
                        lstBuddies.Add(((ctlBuddy)stPanel.Children[i]).Title);
                    }
                }
                return lstBuddies.ToArray();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "GetBuddies()", "Controls\\VMuktiGrid\\Buddy\\ctlPodBuddyList.xaml.cs");                
                return null;
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
                VMuktiHelper.ExceptionHandler(ex, "Dispose()", "Controls\\VMuktiGrid\\Buddy\\ctlPodBuddyList.xaml.cs");
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
                        stPanel.Children.Clear();
                    }
					catch (Exception ex)
					{
                        VMuktiHelper.ExceptionHandler(ex, "Dispose(Bool disposing)", "Controls\\VMuktiGrid\\Buddy\\ctlPodBuddyList.xaml.cs");
					}
				}
				disposed = true;
			}
			GC.SuppressFinalize(this);
        }

        ~ctlPodBuddyList()
        {
            try
            {
            Dispose(false);
        }

        #endregion
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "~ctlPodBuddyList()", "Controls\\VMuktiGrid\\Buddy\\ctlPodBuddyList.xaml.cs");
            }
        }
    }
}
