namespace NOV.ES.TAT.Job.Infrastructure.Helper
{
    public static class StringExtensionMethods
    {
        public static string LikeHelper(this string value)
        {
            if (!string.IsNullOrEmpty(value) && !value.Contains('%'))
            {
                return $"%{value}%";
            }
            return value;
        }
    }
}
