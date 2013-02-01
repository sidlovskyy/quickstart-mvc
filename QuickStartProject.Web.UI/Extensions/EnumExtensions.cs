using System.ComponentModel;
using System.Reflection;

namespace QuickStartProject.Web.UI.Extensions
{
    public static class EnumExtensions
    {
        public static string GetDescription<TEnum>(this TEnum value)
        {
            FieldInfo fi = value.GetType().GetField(value.ToString());

            if (fi != null)
            {
                DescriptionAttribute[] attributes =
                    (DescriptionAttribute[]) fi.GetCustomAttributes(
                        typeof (DescriptionAttribute),
                        false);

                if (attributes.Length > 0)
                    return attributes[0].Description;
            }

            return value.ToString();
        }
    }
}