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
    public static class StlFirendLink
    {
        public const string ElementName = "stl:fl";


        [StlAttribute(Title = "外层ul")]
        private const string UlClass = nameof(UlClass);
        [StlAttribute(Title = "出发省")]
        private const string FromPro = nameof(FromPro);
        [StlAttribute(Title = "出发市")]
        private const string FromCity = nameof(FromCity);
        [StlAttribute(Title = "出发县")]
        private const string FromArea = nameof(FromArea);
        [StlAttribute(Title = "默认 ： https://www.chinawutong.com/201")]
        private const string StartHref = nameof(StartHref);
        [StlAttribute(Title = "到达地的线路类型   省会  直辖  省会直辖")]
        private const string ToCityType = nameof(ToCityType);

        private static string[] shenghui = ["河北省,石家庄市,市辖区",
"山西省,太原市,市辖区",
"内蒙古区,呼和浩特市,市辖区",
"辽宁省,沈阳市,市辖区",
"吉林省,长春市,市辖区",
"黑龙江省,哈尔滨市,市辖区",
"江苏省,南京市,市辖区",
"浙江省,杭州市,市辖区",
"安徽省,合肥市,市辖区",
"福建省,福州市,市辖区",
"江西省,南昌市,市辖区",
"山东省,济南市,市辖区",
"河南省,郑州市,市辖区",
"湖北省,武汉市,市辖区",
"湖南省,长沙市,市辖区",
"广东省,广州市,市辖区",
"广西区,南宁市,市辖区",
"海南省,海口市,市辖区",
"四川省,成都市,市辖区",
"贵州省,贵阳市,市辖区",
"云南省,昆明市,市辖区",
"西藏区,拉萨市,市辖区",
"陕西省,西安市,市辖区",
"甘肃省,兰州市,市辖区",
"青海省,西宁市,市辖区",
"宁夏区,银川市,市辖区",
"新疆区,乌鲁木齐市,市辖区"];

        private static string[] zhixiashi = ["北京市,北京市,市辖区",
"天津市,天津市,市辖区",
"上海市,上海市,市辖区",
"重庆市,重庆市,市辖区"];



        public static async Task<object> ParseAsync(IParseManager parseManager)
        {
            var attributes = new NameValueCollection();

            var ulClass = string.Empty;
            var fromPro = string.Empty;
            var fromCity = string.Empty;
            var fromArea = string.Empty;
            var startHref = string.Empty;
            var toCityType = string.Empty;


            foreach (var name in parseManager.ContextInfo.Attributes.AllKeys)
            {
                var value = parseManager.ContextInfo.Attributes[name];
                if (StringUtils.EqualsIgnoreCase(name, UlClass))
                {
                    ulClass = value;
                }
                else if (StringUtils.EqualsIgnoreCase(name,FromPro))
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
                else if (StringUtils.EqualsIgnoreCase(name, StartHref))
                {
                    startHref = value;
                }
                else if (StringUtils.EqualsIgnoreCase(name, ToCityType))
                {
                    toCityType = value;
                }
                else
                {
                    attributes[name] = value;
                }
            }
            return await ParseAsync(parseManager, ulClass, fromPro,fromCity,fromArea, toCityType, string.IsNullOrEmpty(startHref) ? "https://www.chinawutong.com/201" : startHref, attributes);
        }

        private static async Task<string> ParseAsync(IParseManager parseManager,string ulClass, string fromPro, string fromCity, string fromArea, string ToCityType, string StartHref, NameValueCollection attributes)
        {
            var builder = new StringBuilder();
            List<string> toCity = new();
            if (ToCityType == "省会")
            {
                toCity.AddRange(shenghui.ToArray());
            }
            else if (ToCityType == "直辖")
            {
                toCity.AddRange(zhixiashi.ToArray());
            }
            else if (ToCityType == "省会直辖")
            {
                toCity.AddRange(shenghui.ToArray());
                toCity.AddRange(zhixiashi.ToArray());
            }

            builder.Append($"<ul class='{ulClass}'>");
            foreach (var item in toCity)
            {
                var to = item.Split(',');
                var top = to[0];
                var toc = to[1];
                var toa = to[2];
                var href = await Get201Url(fromPro, fromCity, fromArea, top, toc, toa);
                builder.Append($"<li><a href='{StartHref}{href}'>{fromCity}到{toc}</a></li>");
            }
            builder.Append($"</ul>");
            return builder.ToString();
            //return $@"<a>测试</a>";
        }

        private static async Task<string> Get201Url(string fromPro, string fromCity, string fromArea, string toPro, string toCity, string toArea)
        {
            var url = "https://webapi.chinawutong.com/wlpageview/rediecturl/urlturned/?listwlline=" + HttpUtility.UrlEncode($"f={fromPro}-{fromCity}-{fromArea}&t={toPro}-{toCity}-{toArea}");

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
    }
}
