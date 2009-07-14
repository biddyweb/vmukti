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
using System.Windows;
using System.Windows.Controls;
using VMuktiAPI;
using System.Text;

namespace VMukti.Presentation.Controls
{
    /// <summary>
    /// Interaction logic for CtlPBX.xaml
    /// </summary>
    public partial class CtlPBX : UserControl,IDisposable
    {

        VMukti.Bussiness.PBXConfiguration.PBXConfiguration PBXConfig = null;
        public CtlPBX()
        {
            try
            {
                InitializeComponent();
                PBXConfig = new VMukti.Bussiness.PBXConfiguration.PBXConfiguration();
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "CtlPBX", "Controls\\CtlPBX.xaml.cs");
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtPassword_M.Password == txtConfirmPassword_M.Password)
                {
                    if (txtUserName_M.Text != "" && txtPassword_M.Password != "" && txtDirname_M.Text != "")
                    {
                        PBXConfig.FncPBXBusinessInsertCredential(txtUserName_M.Text, txtPassword_M.Password, txtDirname_M.Text);
                        txtUserName_M.Text = "";
                        txtPassword_M.Password = "";
                        txtConfirmPassword_M.Password = "";
                        txtDirname_M.Text = "";
                        MessageBox.Show("Saved successfully");
                    }
                    else
                    {
                        MessageBox.Show("Fill Up All Information");
                    }
                }
                else
                {
                    MessageBox.Show("Password can't confirm");
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSubmit_click()", "Controls\\CtlPBX.xaml.cs");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                txtUserName_M.Text = "";
                txtPassword_M.Password = "";
                txtConfirmPassword_M.Password = "";
                txtDirname_M.Text = "";
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnCancel_Click()", "Controls\\CtlPBX.xaml.cs");
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            try
            {
            if (PBXConfig != null)
            {
                PBXConfig = null;
            }
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "Dispose()", "Controls\\CtlPBX.xaml.cs");
            }
        }

        #endregion

        ~CtlPBX()
        {
            try
            {
            Dispose();
        }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "~CtlPBX()", "Controls\\CtlPBX.xaml.cs");
            }
        }

    }
}
