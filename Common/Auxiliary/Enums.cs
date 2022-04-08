using System;
using System.ComponentModel;

namespace crmweb.Common.Auxiliary
{


    public static class ReflectionHelpers
    {
        public static string GetCustomDescription(object ObjEnum)
        {
            var vField = ObjEnum.GetType().GetField(ObjEnum.ToString() ?? string.Empty);
            if (vField is { })
            {
                var vAttributes = (DescriptionAttribute[])vField.GetCustomAttributes(typeof(DescriptionAttribute), false);
                return vAttributes.Length > 0 ? vAttributes[0].Description : ObjEnum.ToString();
            }

            return "";
        }

        public static string Definition(this Enum Value)
        {
            return GetCustomDescription(Value);
        }
    }
}
