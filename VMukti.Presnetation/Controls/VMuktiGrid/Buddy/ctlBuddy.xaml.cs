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

///   Description: This file contains codeing portion of displaying buddy in buddy list.

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
    /// Interaction logic for addBuddy.xaml
    /// </summary>
    public partial class ctlBuddy : UserControl, IDisposable
    {
        public static StringBuilder sb1=new StringBuilder();

         private int _Counter = 0;
        private int _MaxCounter = 0;

        public int MaxCounter
        {
            get
            {
                return _MaxCounter;
            }
            set 
            {
                _MaxCounter = value;
                if (_Counter == _MaxCounter)
                {
                    this.Visibility = Visibility.Visible;
                }
            }
        }

        private bool disposed = false;

        public ctlBuddy()
        {
            try
            {
                InitializeComponent();

                //no use could be depricated in future after safe testing.
                txtTitle.MouseDown += new System.Windows.Input.MouseButtonEventHandler(txtTitle_MouseDown);
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ctlBuddy()", "Controls\\VMuktiGrid\\Buddy\\CtlBuddy.xaml.cs");
            }
        }
       
        #region Public Properties
        //buddy name
        public string Title
        {
            set
            {
                if (VMuktiAPI.VMuktiInfo.CurrentPeer.DisplayName == value)
                {
                    LayoutRoot.Visibility = Visibility.Collapsed;
                }
                txtTitle.Text = value;
            }
            get
            {
                return txtTitle.Text;
            }
        }

        #endregion

        #region Private Members

        //no use could be depricated in future after safe testing.
        private void txtTitle_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {

        }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "txtTitle_MouseDown()", "Controls\\VMuktiGrid\\Buddy\\CtlBuddy.xaml.cs");
            }
        }
        #endregion
        //to remove current buddy from the buddy list, involked on delete image near to buddy name in buddy list.
        //private void imgDelete_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    try
        //    {
        //        ((StackPanel)this.Parent).Children.Remove(this);
        //    }
        //    catch (Exception ex)
        //    {
        //        VMuktiHelper.ExceptionHandler(ex, "imgDelete_PreviewMouseDown()", "Controls\\VMuktiGrid\\Buddy\\CtlBuddy.xaml.cs");
        //    }
        //}

        //Step 1 -> When buddy will be droped then buddy will be added to perticular buddylist and maxcounter value will be set as follow:
                // Suppose we are having 1 page, 4 tabs in it and 10 pods in each tabs.
                // and when we drop a buddy on to page at that time...
                // in each pod's buddylist's buddy's maxcounter value will be 1 always.
                // in each tab's buddylist's buddy's maxcounter value will be sum of all pods in perticular tab, in our case 10 in each tab.
                // in each page's buddylist's buddy's maxcounter value will be sum of all pods in all tabs, in our case 40.
        //Step 2 -> when we get message from dropped buddy that module has been loaded there then this "Show" function will be called of respected buddy.
        //Step 3 -> when "Show" function will be called it will increase value of counter and when this counter value will be maxcounter value at that time buddy's visibility will be visible.
        public void Show()
        {
            try
            {
            _Counter++;
            if (_Counter == _MaxCounter)
            {
                this.Visibility = Visibility.Visible;
            }
        }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Show()", "Controls\\VMuktiGrid\\Buddy\\CtlBuddy.xaml.cs");
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
                VMuktiHelper.ExceptionHandler(ex, "Dispose()", "Controls\\VMuktiGrid\\Buddy\\CtlBuddy.xaml.cs");
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
                       
					}
					catch (Exception ex)
					{
                        VMuktiHelper.ExceptionHandler(ex, "Dispose(Bool disposing)", "Controls\\VMuktiGrid\\Buddy\\CtlBuddy.xaml.cs");
					}
				}
				disposed = true;
			}
			GC.SuppressFinalize(this);
        }

        ~ctlBuddy()
        {
            try
            {
            Dispose(false);
        }

        
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "~ctlBuddy()", "Controls\\VMuktiGrid\\Buddy\\CtlBuddy.xaml.cs");
            }
        }

        #endregion
    }
}
