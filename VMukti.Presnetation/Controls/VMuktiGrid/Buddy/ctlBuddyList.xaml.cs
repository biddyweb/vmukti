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

///   Description: This is buddylist for page this class will be loaded as part of ctlMenu.xaml.

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
    public partial class ctlBuddyList : UserControl, IDisposable
    {
        public static StringBuilder sb1 = new StringBuilder();
        private int intBuddyCount = 0;
        private bool disposed = false;

        public ctlBuddyList()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlBuddyList()", "Controls\\VMuktiGrid\\Buddy\\ctlBuddyList.xaml.cs");
            }
        }

        public bool AddBuddy(string strBuddyName)
        {
            try
            {
                int i = 0, tempBuddyCount = 0;
                for (i = 0; i < stBuddyPanel.Children.Count; i++)
                {
                    if (stBuddyPanel.Children[i].GetType() == typeof(ctlBuddy))
                    {
                        if (((ctlBuddy)stBuddyPanel.Children[i]).Title == strBuddyName)
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
                    stBuddyPanel.Children.Add(objBuddy);
                    intBuddyCount++;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "AddBuddy()", "Controls\\VMuktiGrid\\Buddy\\ctlBuddyList.xaml.cs");                
                return false;
            }
        }

        public bool RemoveBuddy(string strBuddyName)
        {
            try
            {
                int i = 0;
                bool blnIsAdded = false;
                for (i = 0; i < stBuddyPanel.Children.Count; i++)
                {
                    if (stBuddyPanel.Children[i].GetType() == typeof(ctlBuddy))
                    {
                        if (((ctlBuddy)stBuddyPanel.Children[i]).Title == strBuddyName)
                        {
                            blnIsAdded = true;
                            break;
                        }
                    }
                }
                if (blnIsAdded)
                {
                    stBuddyPanel.Children.RemoveAt(i);
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

        //when buddy dropped at that time this function will be called by tab or page with buddy name and it will set maxcounter value to that buddy object.
        public void SetMaxCounter(int intMaxCounter, string strBuddyName)
        {
            try
            {
                for (int i = 0; i < stBuddyPanel.Children.Count; i++)
                {
                    if (stBuddyPanel.Children[i].GetType() == typeof(ctlBuddy))
                    {
                        if (((ctlBuddy)stBuddyPanel.Children[i]).Title == strBuddyName)
                        {
                            ((ctlBuddy)stBuddyPanel.Children[i]).MaxCounter = intMaxCounter;
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                VMuktiHelper.ExceptionHandler(ex, "setMaxCounter()", "Controls\\VMuktiGrid\\Buddy\\ctlBuddyList.xaml.cs"); ex.Data.Add("My Key", "SetMaxCounter()--:--ctlExpMenuItem.xaml.cs--:--" + ex.Message + " :--:--");
            }
        }

        //if the buddy is already exists than it will not add the same
        public bool CheckBuddy(string strBuddyName)
        {
            try
            {
                int i = 0, tempBuddyCount = 0;
                for (i = 0; i < stBuddyPanel.Children.Count; i++)
                {
                    if (stBuddyPanel.Children[i].GetType() == typeof(ctlBuddy))
                    {
                        if (((ctlBuddy)stBuddyPanel.Children[i]).Title == strBuddyName)
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
                VMuktiHelper.ExceptionHandler(ex, "CheckBuddy()", "Controls\\VMuktiGrid\\Buddy\\ctlBuddyList.xaml.cs");                
                return false;
            }
        }

        //showing buddy in the list once the module are completely loaded on that buddy screen- not used right now / partialy implemented
        public void ShowBuddy(string strBuddyName)
        {
            try
            {
                int i = 0;
                for (i = 0; i < stBuddyPanel.Children.Count; i++)
                {
                    if (stBuddyPanel.Children[i].GetType() == typeof(ctlBuddy))
                    {
                        if (((ctlBuddy)stBuddyPanel.Children[i]).Title == strBuddyName)
                        {
                            ((ctlBuddy)stBuddyPanel.Children[i]).Show();
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ShowBuddy()", "Controls\\VMuktiGrid\\Buddy\\ctlBuddyList.xaml.cs");
            }
        }

        public List<string> GetBuddies()
        {
            List<string> buddyname = new List<string>();

            for (int i = 0; i < stBuddyPanel.Children.Count; i++)
            {
                if (stBuddyPanel.Children[i].GetType() == typeof(ctlBuddy))
                {
                    if(((ctlBuddy)stBuddyPanel.Children[i]).Title !=VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName)
                    {
                        buddyname.Add(((ctlBuddy)stBuddyPanel.Children[i]).Title);
                    }
                }
            }
            return buddyname;
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
                VMuktiHelper.ExceptionHandler(ex, "Dispose()", "Controls\\VMuktiGrid\\Buddy\\ctlBuddyList.xaml.cs");
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
                        stBuddyPanel.Children.Clear();
					}
					catch (Exception ex)
					{
                        VMuktiHelper.ExceptionHandler(ex, "Dispose(bool dispose)", "Controls\\VMuktiGrid\\Buddy\\ctlBuddyList.xaml.cs");
					}
				}
				disposed = true;
			}
			GC.SuppressFinalize(this);
        }

        ~ctlBuddyList()
        {
            try
            {
            Dispose(false);
        }

        #endregion
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlBuddyList()", "Controls\\VMuktiGrid\\Buddy\\ctlBuddyList.xaml.cs");
            }
        }
    }
}
