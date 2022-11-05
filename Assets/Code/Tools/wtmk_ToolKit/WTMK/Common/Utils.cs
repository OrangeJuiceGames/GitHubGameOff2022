using System;
using System.IO;
namespace WTNK.Common
{
    public static class Utility
    {
        public static T Min<T>(T itemI, T itemII) where T : IComparable<T>
        {
            return (itemI.CompareTo(itemII) < 0) ? itemI : itemII;
        }

        public static string ReadAllText(string path)
        {
            if(File.Exists(path))
            {
                return File.ReadAllText(path);
            }

            return "";
        }

        public static void WriteAllText(string path, string text)
        {
            File.WriteAllText(path, text);
        }

        public static string PathBuilder(string[] paths)
        {
            return Path.Combine(paths);
        }
    }
}