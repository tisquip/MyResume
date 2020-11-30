using System;
using System.Collections.Generic;
using System.Text;

namespace Resume.Domain
{
    public static class ExtentionMethods
    {
        public static List<T> ToListOfSelf<T>(this T caller) => new List<T>() { caller };
    }
}
