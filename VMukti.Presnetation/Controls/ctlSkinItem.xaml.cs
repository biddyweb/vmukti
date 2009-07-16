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

namespace VMukti.Presentation.Controls
{
    /// <summary>
    /// Interaction logic for ctlSkinItem.xaml
    /// </summary>
    public partial class ctlSkinItem
    {

        public ctlSkinItem()
        {
            try
            {
                this.InitializeComponent();

                this.MouseDown += new MouseButtonEventHandler(ctlSkinItem_MouseDown);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlSkinItem()", "Controls\\CtlSkinItem.xaml.cs");
            }
        }

        void ctlSkinItem_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                ResourceDictionary obj = new ResourceDictionary();
                obj.Source = new Uri(this.Tag.ToString(), UriKind.RelativeOrAbsolute);
                Application.Current.Resources.MergedDictionaries.Add(obj);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "ctlSkinItem_MouseDown()", "Controls\\CtlSlinItem.xaml.cs");
            }
        }


    }
}
