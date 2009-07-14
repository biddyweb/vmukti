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
using System.Linq;
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

namespace VMukti.Presentation.Controls
{
    /// <summary>
    /// Interaction logic for CtlVMuktiVersion.xaml
    /// </summary>
    public partial class CtlVMuktiVersion : UserControl
    {
        bool blIsChecked;
        public CtlVMuktiVersion()
        {
            InitializeComponent();
            if (VMuktiAPI.VMuktiInfo.VMuktiVersion == "1.1")
            {
                chVMuktiCallCenter.IsChecked = true;
            }
        }

       
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (chVMuktiCallCenter.IsChecked == true)
                {
                    App.chHttpBootStrapService.svcUpdateVMuktiVersion(true, true);
                }
                else
                {
                    App.chHttpBootStrapService.svcUpdateVMuktiVersion(true, false);
                }
                MessageBox.Show("Saved Successfuly");
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnSave_Click()", "Controls\\CtlVMuktiVersion.xaml.cs");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                chVMuktiCallCenter.IsChecked = false;
                chVMuktiMeetingPlace.IsChecked = false;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "btnCancel_Click()", "Controls\\CtlVMuktiVersion.xaml.cs");
            }
        }

    }
}
