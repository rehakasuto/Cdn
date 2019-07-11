using System.Configuration;
using System.Web;

namespace PointrCdn
{
    public static class ParameterUtil
    {
        public static string GetCdnPath()
        {
            return System.AppDomain.CurrentDomain.BaseDirectory + @"Upload\";
        }
        public static string GetCdnUrl()
        {
            return "http://" + HttpContext.Current.Request.Url.Authority + "/Upload/";
        }
    }
}