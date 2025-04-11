using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestApi.Infrastructure.Extensions
{
    public static class StringExtension
    {
        public static bool IsEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        public static bool IsValidJson(this string str)
        {
            if (str.IsEmpty())
                return false;

            str = str.Trim();

            if ((str.StartsWith("{") && str.EndsWith("}")) ||
                (str.StartsWith("[") && str.EndsWith("]")))
            {
                try
                {
                    var obj = JToken.Parse(str);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    return false;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
                return false;
        }
    }
}
