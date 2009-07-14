using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using System.Reflection;

namespace ImageSharing.Presentation
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
