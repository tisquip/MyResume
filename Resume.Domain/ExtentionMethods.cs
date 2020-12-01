using System.Collections.Generic;

namespace Resume.Domain
{
    public static class ExtentionMethods
    {
        public static List<T> ToListOfSelf<T>(this T caller) => new List<T>() { caller };
        public static string SerializeToJson<T>(this T caller)
        {
            string vtr = null;
            try
            {
                vtr = Newtonsoft.Json.JsonConvert.SerializeObject(caller);
            }
            catch (System.Exception)
            {
            }
            return vtr;
        }
        public static T DeserializeFromJson<T>(this string caller)
        {
            T vtr = default;
            try
            {
                vtr = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(caller);
            }
            catch (System.Exception)
            {
            }
            return vtr;
        }
        public static string AddToJsonStringList<T>(this string caller, T itemToAdd)
        {
            string vtr = caller;
            try
            {
                List<T> list = caller.DeserializeFromJson<List<T>>() ?? new List<T>();
                list.Add(itemToAdd);
                vtr = list.SerializeToJson();
            }
            catch (System.Exception)
            {
            }
            return vtr;
        }
    }
}
