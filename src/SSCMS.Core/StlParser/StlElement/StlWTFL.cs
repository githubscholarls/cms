using Datory.Utils;
using Microsoft.AspNetCore.Components;
using NPOI.SS.Formula;
using NSwag;
using Org.BouncyCastle.Asn1.X509;
using SSCMS.Core.StlParser.Attributes;
using SSCMS.Parse;
using SSCMS.Services;
using SSCMS.Utils;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using static System.Net.WebRequestMethods;

namespace SSCMS.Core.StlParser.StlElement
{
    [StlElement(Title = "获取物通网201友情链接")]
    public static class StlWTFL
    {
        public const string ElementName = "stl:wtfl";

        private const string FromPro = nameof(FromPro);
        private const string FromCity = nameof(FromCity);
        private const string FromArea = nameof(FromArea);
        private const string ToPro = nameof(ToPro);
        private const string ToCity = nameof(ToCity);
        private const string ToArea = nameof(ToArea);

        public static async Task<object> ParseAsync(IParseManager parseManager)
        {
            var attributes = new NameValueCollection();

            var fromPro = string.Empty;
            var fromCity = string.Empty;
            var fromArea = string.Empty;
            var toPro = string.Empty;
            var toCity = string.Empty;
            var toArea = string.Empty;

            foreach (var name in parseManager.ContextInfo.Attributes.AllKeys)
            {
                var value = parseManager.ContextInfo.Attributes[name];
                if (StringUtils.EqualsIgnoreCase(name, FromPro))
                {
                    fromPro = value;
                }
                else if (StringUtils.EqualsIgnoreCase(name, FromCity))
                {
                    fromCity = value;
                }
                else if (StringUtils.EqualsIgnoreCase(name, FromArea))
                {
                    fromArea = value;
                }
                else if (StringUtils.EqualsIgnoreCase(name, ToPro))
                {
                    toPro = value;
                }
                else if (StringUtils.EqualsIgnoreCase(name, ToCity))
                {
                    toCity = value;
                }
                else if (StringUtils.EqualsIgnoreCase(name, ToArea))
                {
                    toArea = value;
                }
                else
                {
                    attributes[name] = value;
                }
            }
            return await ParseAsync(parseManager, fromPro, fromCity, fromArea, toPro, toCity, toArea, attributes);
        }

        private static async Task<string> ParseAsync(IParseManager parseManager, string fromPro, string fromCity, string fromArea, string toPro, string toCity, string toArea, NameValueCollection attributes)
         {

            string href = string.Empty;
            try
            {
#if DEBUG
                Environment.SetEnvironmentVariable("wtapi", "https://webapi.chinawutong.com");
#endif

                href = await Get201Url(Environment.GetEnvironmentVariable("wtapi")?? "http://192.168.0.154", fromPro, fromCity, fromArea, toPro, toCity, toArea);
            }
            catch (Exception)
            {
                href = await Get201Url("https://webapi.chinawutong.com", fromPro, fromCity, fromArea, toPro, toCity, toArea);
            }


            var from = GetShortAddr(fromPro, fromCity, fromArea);
            var to = GetShortAddr(toPro, toCity, toArea);

            return($"<a target=\"_black\" href='https://www.chinawutong.com/201{href}' {TranslateUtils.ToAttributesString(attributes)}>{from}到{to}物流公司</a>");

        }

        private static async Task<string> Get201Url(string wtapi, string fromPro, string fromCity, string fromArea, string toPro, string toCity, string toArea)
        {
            var url = $"{wtapi}/wlpageview/rediecturl/urlturned/?listwlline=" + HttpUtility.UrlEncode($"f={fromPro}-{fromCity}-{fromArea}&t={toPro}-{toCity}-{toArea}");

            var resString = await HttpClientUtils.GetStringAsync(url);
            if (string.IsNullOrEmpty(resString))
            {
                return string.Empty;
            }
            var res = Newtonsoft.Json.JsonConvert.DeserializeObject<UrlTurnedRes>(resString);
            if (res is null || res.success != "true")
            {
                return string.Empty;
            }
            return res.data;
        }
        /// <summary>
        /// {
        ///    "success": "true",
        ///    "errcode": "",
        ///    "msg": "",
        ///    "data": "/s5923227/"
        ///}
        /// </summary>
        private class UrlTurnedRes
        {
            public string success { get; set; }
            public string errcode { get; set; }
            public string msg { get; set; }
            public string data { get; set; }
        }


        public static string GetShortAddr(string pro, string city, string area)
        {
            try
            {

                string param = GetShortAddr(area);
                if (string.IsNullOrWhiteSpace(param))
                {
                    param = GetShortAddr(city);
                    if (string.IsNullOrWhiteSpace(param))
                    {
                        param = GetShortAddr(pro);
                    }
                }
                return param;
            }
            catch
            {
            }

            return "";
        }

        public static string GetShortAddr(string param, bool ispro = false)
        {

            try
            {
                /*
             ①通用：2个汉字不处理
             ②去掉≥3字以上：“地区”以及最后1字“市”“州”
             ③去掉≥4字以上的：“盟”“自治州”“自治县”“自治旗”“直辖县”“市辖区”
             */
                if (!string.IsNullOrWhiteSpace(param) && param != "市辖区")
                {
                    if (param.Length > 4)
                    {
                        param = param.Replace("盟", "").Replace("自治州", "").Replace("自治县", "").Replace("自治旗", "").Replace("直辖县", "").Replace("市辖区", "");
                    }
                    if (param.Length >= 3)
                    {
                        if (param.Substring(param.Length - 2, 2) == "地区")
                            param = param.Substring(0, param.Length - 2);
                        else if (param.Substring(param.Length - 1, 1) == "市")
                            param = param.Substring(0, param.Length - 1);
                        else if (param.Substring(param.Length - 1, 1) == "州")
                            param = param.Substring(0, param.Length - 1);
                    }
                    if (ispro)
                    {
                        param = param.Replace("省", String.Empty).Replace("区", String.Empty).Replace("自治区", "").Replace("直辖市", "").Replace("特别行政区", "");
                    }
                    return param;
                }
            }
            catch
            {
            }
            return "";
        }
    }
}

