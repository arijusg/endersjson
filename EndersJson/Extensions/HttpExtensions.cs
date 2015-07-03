using System.Linq;
using System.Web;

namespace EndersJson.Extensions
{
    public static class HttpExtensions
    {
        public static string ToQueryString(this object instance)
        {
            var properties = instance.GetType().GetProperties()
                .OrderBy(x => x.Name)
                .Where(p => p.GetValue(instance, null) != null)
                .Select(p => p.Name + "=" + HttpUtility.UrlEncode(p.GetValue(instance, null).ToString()));
            return string.Join("&", properties.ToArray());
        }
    }
}