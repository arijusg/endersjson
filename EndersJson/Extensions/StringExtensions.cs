namespace EndersJson.Extensions
{
    public static class StringExtensions
    {
        public static string SafeToString(this object s)
        {
            if (s == null)
                return string.Empty;
            return s.ToString();
        }
    }
}