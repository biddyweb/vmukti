using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using VMuktiAPI;
using System.Globalization;

namespace Desktop_Sharing.Presentation
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
            try
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
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "BindToMethod", "DomainBinder.cs");

                state = null;
                return null;
            }
        }

        public override object ChangeType(
           object value,
           Type type,
           CultureInfo culture
        )
        {
            try
            {
                return Convert.ChangeType(value, type);
            }

            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "ChangeType", "DomainBinder.cs");

                return null;
            }
        }

        public override FieldInfo BindToField(
           BindingFlags bindingAttr,
           FieldInfo[] match,
           object value,
           CultureInfo culture
        )
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {

                VMuktiHelper.ExceptionHandler(ex, "BindToField", "DomainBinder.cs");
                return null;

            }
        }

        public override MethodBase SelectMethod(
           BindingFlags bindingAttr,
           MethodBase[] match,
           Type[] types,
           ParameterModifier[] modifiers
        )
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {


                VMuktiHelper.ExceptionHandler(ex, "SelectMethod", "DomainBinder.cs");

                return null;
            }
        }

        public override PropertyInfo SelectProperty(
           BindingFlags bindingAttr,
           PropertyInfo[] match,
           Type returnType,
           Type[] indexes,
           ParameterModifier[] modifiers
        )
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {

                VMuktiHelper.ExceptionHandler(ex, "SelectProperty", "DomainBinder.cs");

                return null;
            }
        }

        public override void ReorderArgumentArray(ref object[] args,
           object state
        )
        {
            try
            {
                throw new NotImplementedException();
            }
            catch (Exception ex)
            {
                VMuktiHelper.ExceptionHandler(ex, "RecorderArgumentArray", "DomainBinder.cs");
            }
        }
    }
    
}
