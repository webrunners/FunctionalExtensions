using System;
using System.Linq;
using System.Reflection;
using EnumExtensions.Attributes;

namespace EnumExtensions.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum enumType)
        {
            var type = enumType.GetType();
            var field = type.GetField(enumType.ToString());
            if (field == null) // Value not found in enum.
                return String.Empty;

            var displayNameAttribute = field
                .GetCustomAttributes(typeof(EnumDisplayNameAttribute), false)
                .FirstOrDefault() as EnumDisplayNameAttribute;

            return displayNameAttribute != null ? displayNameAttribute.DisplayName : Enum.GetName(enumType.GetType(), enumType);
        }
    }
}