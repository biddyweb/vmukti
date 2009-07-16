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

//This is for pod height setting... in which we are passing two values and based on first value are returning the second value with some manipulation.
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Data;
using System.Windows;
using System.Windows.Controls;
using System.Globalization;
using VMuktiAPI;

namespace VMuktiGrid.CustomGrid
{
    

    public class CPODHeight : IMultiValueConverter
    {
        public static StringBuilder sb1=new StringBuilder();

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (values == null)
                {
                    return 0.0;
                }

                if (((Visibility)values[0]) == Visibility.Collapsed)
                {
                    return 25.0;
                }
                else if (((Visibility)values[0]) == Visibility.Visible)
                {
                    if (values[1].ToString() == "NaN")
                    {
                        return 25.0;
                    }
                    else
                    {
                        return double.Parse(values[1].ToString()) + 25.0;
                    }
                }
                else
                {
                    return 0.0;
                }
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "clsConveryers()", "Controls\\VMuktiGrid\\Grid\\clsConverter.cs");
                return new object();
            }
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            try
            {
                string[] splitValues = ((string)value).Split(' ');
                return splitValues;
            }
            catch (Exception ex)
            {
                VMuktiAPI.VMuktiHelper.ExceptionHandler(ex, "ConvertBack()", "Controls\\VMuktiGrid\\Grid\\clsConverter.cs");                
                return new List<object>().ToArray();
            }
        }
    }
}
