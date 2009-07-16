/* VMukti 1.0 -- An Open Source Unified Communications Engine
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
using System.Globalization;
using System.Reflection;

namespace QA.Presentation
{
    [Serializable]
    public class DomainBinder : Binder
    {

        public override MethodBase BindToMethod(
           BindingFlags bindingAttr,
           MethodBase[] match,
           ref object[] args,
           ParameterModifier[] modifiers,
           CultureInfo culture,
           string[] names,
           out object state
        )
        {
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] is string)
                {
                    args[i] = ChangeType(args[i], typeof(string), culture);
                }
                if (args[i] is int)
                {
                    args[i] = ChangeType(args[i], typeof(int), culture);
                }
                if (args[i] is bool)
                {
                    args[i] = ChangeType(args[i], typeof(bool), culture);
                }
            }
            return Type.DefaultBinder.BindToMethod(
               bindingAttr,
               match,
               ref args,
               modifiers,
               culture,
               names,
               out state
            );
        }

        public override object ChangeType(
           object value,
           Type type,
           CultureInfo culture
        )
        {
            return Convert.ChangeType(value, type);
        }

        public override FieldInfo BindToField(
           BindingFlags bindingAttr,
           FieldInfo[] match,
           object value,
           CultureInfo culture
        )
        {
            throw new NotImplementedException();
        }

        public override MethodBase SelectMethod(
           BindingFlags bindingAttr,
           MethodBase[] match,
           Type[] types,
           ParameterModifier[] modifiers
        )
        {
            throw new NotImplementedException();
        }

        public override PropertyInfo SelectProperty(
           BindingFlags bindingAttr,
           PropertyInfo[] match,
           Type returnType,
           Type[] indexes,
           ParameterModifier[] modifiers
        )
        {
            throw new NotImplementedException();
        }

        public override void ReorderArgumentArray(ref object[] args,
           object state
        )
        {
            throw new NotImplementedException();
        }
    }
}
