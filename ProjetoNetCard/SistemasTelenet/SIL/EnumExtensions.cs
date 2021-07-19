using System;
using System.ComponentModel;
using System.Linq;

namespace SIL
{
    public static class EnumExtensions
    {
        public static string GetDescription<TEnum>(this TEnum @enum) where TEnum : struct
        {
            var type = typeof(TEnum);

            if (!type.IsEnum)
            {
                throw new ArgumentException(string.Format("{0} must be an enumerated type", type.FullName));
            }

            var field = type.GetMember(@enum.ToString());
            var attribute = field.First().GetCustomAttributes(typeof(DescriptionAttribute), false).Cast<DescriptionAttribute>().FirstOrDefault();
            return attribute == null ? Enum.GetName(type, @enum) : attribute.Description;
        }
    }
}
