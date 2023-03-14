namespace WorkHive.Common.Extensions
{
    public static class StringExtensions
    {
        public static string Standardize(this string value)
        {
            return value.Trim().ToLower();
        }
    }
}
