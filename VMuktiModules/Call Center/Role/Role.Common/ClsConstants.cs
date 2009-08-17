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

namespace Role.Common
{
    ////////////////////////////////////////////////////////////////////////////
    /// <summary>
    ///   Contains a listing of constants used throughout the application
    /// </summary>
    ////////////////////////////////////////////////////////////////////////////
    public sealed class ClsConstants
    {
        /// <summary>
        /// The value used to represent a null DateTime value
        /// </summary>
        public static DateTime NullDateTime = DateTime.MinValue;

        /// <summary>
        /// The value used to represent a null decimal value
        /// </summary>
        public static decimal NullDecimal = decimal.MinValue;

        /// <summary>
        /// The value used to represent a null double value
        /// </summary>
        public static double NullDouble = double.MinValue;

        /// <summary>
        /// The value used to represent a null Guid value
        /// </summary>
        public static Guid NullGuid = Guid.Empty;

        /// <summary>
        /// The value used to represent a null int value
        /// </summary>
        public static int NullInt = int.MinValue;

        /// <summary>
        /// The value used to represent a null long value
        /// </summary>
        public static long NullLong = long.MinValue;

        /// <summary>
        /// The value used to represent a null float value
        /// </summary>
        public static float NullFloat = float.MinValue;

        /// <summary>
        /// The value used to represent a null string value
        /// </summary>
        public static string NullString = string.Empty;

        /// <summary>
        /// Maximum DateTime value allowed by SQL Server
        /// </summary>
        public static DateTime SqlMaxDate = new DateTime(9999, 1, 3, 23, 59, 59);

        /// <summary>
        /// Minimum DateTime value allowed by SQL Server
        /// </summary>
        public static DateTime SqlMinDate = new DateTime(1753, 1, 1, 00, 00, 00);
    }
}