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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using VMuktiAPI;
using System.Windows.Controls;
using System.Text;

namespace VMukti.Presentation.Controls
{
    public partial class CtlMExpanderItem : System.Windows.Controls.UserControl, IDisposable
    {
        public static StringBuilder sb1 = new StringBuilder();
        SolidColorBrush objMouseover = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFAEE7EE"));
        SolidColorBrush objMouseoverborder = new System.Windows.Media.SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF00C8C7"));
        SolidColorBrush objpreviousBg = new SolidColorBrush();
        SolidColorBrush objpreviousBb = new SolidColorBrush();
        string strImgPath = "";

        public string Caption
        {
            get
            {
                return ((TextBlock)(((Grid)(((Border)(this.Content)).Child)).Children[1])).Text;
            }
            set
            {
                ((TextBlock)(((Grid)(((Border)(this.Content)).Child)).Children[1])).Text = value;
            }
        }

        public string Image
        {
            get
            {
                return strImgPath;
            }
            set
            {
                strImgPath = value;
                if (value != "")
                {
                    ((Grid)(((Border)(this.Content)).Child)).ColumnDefinitions[0].Width = new GridLength(20);
                    ((Image)(((Grid)(((Border)(this.Content)).Child)).Children[0])).Source = new BitmapImage(new Uri(strImgPath, UriKind.RelativeOrAbsolute));
                }
                else
                {
                    ((Grid)(((Border)(this.Content)).Child)).ColumnDefinitions[0].Width = new GridLength(0);
                    ((Image)(((Grid)(((Border)(this.Content)).Child)).Children[0])).Source = null;
                
                }
            }
        }

        public CtlMExpanderItem()
        {
            try
            {
               
                this.InitializeComponent();
                this.MouseEnter += new System.Windows.Input.MouseEventHandler(CtlExpanderItem_MouseEnter);
                this.MouseLeave += new System.Windows.Input.MouseEventHandler(CtlExpanderItem_MouseLeave);
                objpreviousBg = (SolidColorBrush)((Border)(this.Content)).Background;
                objpreviousBb = (SolidColorBrush)((Border)(this.Content)).BorderBrush;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlMExpanderItem()", "Controls\\ModuleExplorer\\CtlMExpanderItem.xaml.cs");
            }
        }

        void CtlExpanderItem_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                  ((Border)(this.Content)).BorderBrush = objpreviousBb;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlExpanderItem_MouseLeave()", "Controls\\ModuleExplorer\\CtlMExpanderItem.xaml.cs");
            }
        }

        void CtlExpanderItem_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            try
            {
                ((Border)(this.Content)).BorderBrush = objMouseoverborder;
                
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "CtlExpanderItem_MouseEnter()", "Controls\\ModuleExplorer\\CtlMExpanderItem.xaml.cs");
            }
        }

        public void Setimage(BitmapImage objImage)
        {
            try
            {
                ((Image)(((Grid)(((Border)(this.Content)).Child)).Children[0])).Source = objImage;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Setimage()", "Controls\\ModuleExplorer\\CtlMExpanderItem.xaml.cs");
            }
        }
        ~CtlMExpanderItem()
        {
            Dispose();
        }

        #region IDisposable Members

        public void Dispose()
        {
            if (strImgPath != null)
            {
                strImgPath = null;
            }
            
        }

        #endregion
    }
}