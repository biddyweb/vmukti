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

namespace VMukti.Presentation.Controls
{
    /// <summary>
    /// Interaction logic for CtlThemItem.xaml
    /// </summary>
    public partial class CtlThemPopUp : UserControl
    {
        public delegate void delChangeTheme(string tag);
        public event delChangeTheme entChangeTheme;
        //public static StringBuilder sb1;

        public CtlThemPopUp()
        {
            try
            {
                InitializeComponent();
                btnTheme.Click += new RoutedEventHandler(btnTheme_Click);
                btnMain.Click += new RoutedEventHandler(btnMain_Click);
                System.Windows.Media.Imaging.BitmapImage objb = new System.Windows.Media.Imaging.BitmapImage(new Uri(@"\Skins\Images1\Themes.png",UriKind.RelativeOrAbsolute));
                          
                img.Source = objb;
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "CtlThemPopUp()", "Controls\\CtlThemPopUp.xaml.cs");
            }
        }

        void btnMain_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                popup.IsOpen = (!popup.IsOpen);
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "btnMain_Click()", "Controls\\CtlThemPopUp.xaml.cs");
            }
        }

        void btnTheme_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                entChangeTheme("Meeting");
            }
            catch (Exception exp)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(exp, "btnTheme_Click()", "Controls\\CtlThemPopUp.xaml.cs");
            }
        }
    }
}
