using System;
namespace WTNK.Common
{
    public static class StringExtensions
    {
        public static string[] Split(this string self, char seprator, int count)
        {
            if(self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            return self.Split(new[] { seprator }, count);
        }

        public static bool IsNullOrEmpty(this string self)
        {
            return string.IsNullOrEmpty(self);
        }
    }
}