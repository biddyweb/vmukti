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
using System.Windows.Data;
using VMuktiAPI;
using System.Text;

namespace VMukti.Presentation.Converters
{
    /// <summary>
    /// All Converters for ctlBuddyList
    /// </summary>
    public class ClstBuddiesHeight : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value == null)
                {
                    return 0.0;
                }
                if (double.Parse(value.ToString()) - 95.0 > 0)
                {
                    return double.Parse(value.ToString()) - 95.0;
                }
                else
                {
                    return 0.0;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Convert()", "Converters\\ClsBuddyListConverters.cs");                               
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ConvertBack()", "Converters\\ClsBuddyListConverters.cs");                
                return null;
            }
        }
    }

    public class ClstBuddiesWidth : IValueConverter
    {
        
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value == null)
                {
                    return 0.0;
                }
                if (double.Parse(value.ToString()) - 10.0 > 0)
                {
                    return double.Parse(value.ToString()) - 10.0;
                }
                else
                {
                    return 0.0;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Convert()", "Converters\\ClsBuddyListConverters.cs");               
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ConvertBack()", "Converters\\ClsBuddyListConverters.cs");               
                return null;
            }
        }
    }

    public class CtxtNameWidth : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value == null)
                {
                    return 0.0;
                }
                if (double.Parse(value.ToString()) - 50.0 > 0)
                {
                    return double.Parse(value.ToString()) - 50.0;
                }
                else
                {
                    return 0.0;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Convert()--CtxtNameWidth", "Converters\\ClsBuddyListConverters.cs");                
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ConvertBack()--CtxtNameWidth", "Converters\\ClsBuddyListConverters.cs");                                
                return null;
            }
        }
    }

    public class ClstBuddiesTop : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                if (value == null)
                {
                    return 40.0;
                }
                if (double.Parse(value.ToString()) + 40.0 > 0)
                {
                    return double.Parse(value.ToString()) + 40.0;
                }
                else
                {
                    return 40.0;
                }
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "Convert()--ClstBuddiesTop", "Converters\\ClsBuddyListConverters.cs");                 
                return null;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                return null;
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ConvertBack()--ClstBuddiesTop", "Converters\\ClsBuddyListConverters.cs");                                 
                return null;
            }
        }
    }
}
