using System.Collections.Generic;

namespace Resume.Domain
{
    public static class ExtentionMethods
    {
        public static List<T> ToListOfSelf<T>(this T caller) => new List<T>() { caller };
    }
}
