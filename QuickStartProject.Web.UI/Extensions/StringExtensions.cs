namespace QuickStartProject.Web.UI.Extensions
{
    public static class StringExtensions
    {
        public static string TruncateToLength(this string str, int length)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }

            if (str.Length <= length)
            {
                return str;
            }

            str = str.Substring(0, length);
            str += "...";

            return str;
        }
    }
}