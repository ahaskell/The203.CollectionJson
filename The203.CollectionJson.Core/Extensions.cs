using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace The203.CollectionJson.Core
{
    public static class Extensions
    {
        // TODO:  Should we cache this?  Eventually?
        public static bool ExposedToClient(this PropertyInfo propInfo)
        {
            if (propInfo.GetCustomAttributes(typeof(HideFromClientAttribute), true).Any())
                return false;

            Type type = propInfo.PropertyType;
            return type.IsScalar();
        }

        public static bool IsScalar(this Type type)
        {

            if (type.IsPrimitive)
                return true;
            if (type == typeof(string))
                return true;
            if (type == typeof(Guid))
                return true;
            if (type == typeof(Decimal))
                return true;
            if (type == typeof(DateTime))
                return true;
            if (type.IsEnum)
                return true;
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                Type nullableArg = type.GetGenericArguments().First();
                return nullableArg.IsScalar();
            }
            return false;
        }
    }
}
