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
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using VMuktiAPI;

namespace VMuktiGrid.CustomMenu
{
    /// <summary>
    /// Interaction logic for addBuddy.xaml
    /// </summary>
    public partial class ctlMenuItem : UserControl, IDisposable
    {
        private bool disposed = false;

        #region Public Properties

        public string Title
        {
            set
            {
                txtTitle.Text = value;
            }
        }

        public string strLeftImage
        {
            set 
            {
                if (value == "")
                {
                    imgLeftIcon.Source = null;
                }
                else
                {
                    imgLeftIcon.Source = new BitmapImage(new Uri(value, UriKind.RelativeOrAbsolute));
                }
            }
        }

        public string strRightImage
        {
            set 
            {
                if (value == "")
                {
                    imgRightIcon.Source = null;
                }
                else
                {
                    imgRightIcon.Source = new BitmapImage(new Uri(value, UriKind.RelativeOrAbsolute));
                }
            }
        }

        #endregion

        public ctlMenuItem()
        {
            try
            {
                InitializeComponent();
                txtTitle.MouseDown += new System.Windows.Input.MouseButtonEventHandler(txtTitle_MouseDown);
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ctlMenuItem()", "Controls\\VMuktiGrid\\Menu\\ctlMenuItem.xaml.cs");
            }
        }
       
        #region Private Members

        private void txtTitle_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {

        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "txtTitle_MouseDown()", "Controls\\VMuktiGrid\\Menu\\ctlMenuItem.xaml.cs");
            }
        }
        #endregion

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
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose()", "Controls\\VMuktiGrid\\Menu\\ctlMenuItem.xaml.cs");
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
                        VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose(bool disposing)", "Controls\\VMuktiGrid\\Menu\\ctlMenuItem.xaml.cs");
                    }
				}
				disposed = true;
			}
			GC.SuppressFinalize(this);
        }

        ~ctlMenuItem()
        {
            Dispose(false);
        }
        #endregion
    }
}
