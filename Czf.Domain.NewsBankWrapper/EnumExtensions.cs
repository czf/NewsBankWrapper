
using System.ComponentModel;
using System.Reflection;


namespace Czf.Domain.NewsBankWrapper.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription(this System.Enum value)
        {
            string description = value.ToString();
            FieldInfo fi = value.GetType().GetField(value.ToString());
            var attribute = (DescriptionAttribute)fi.GetCustomAttribute(typeof(DescriptionAttribute));
            return attribute.Description;

        }
    }
}
