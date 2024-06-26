﻿using Microsoft.AspNetCore.Components;
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

namespace SSCMS.Core.StlParser.StlElementWT
{
    [StlElement(Title = "获取物通网201友情链接")]
    public static class StlFriendLink
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
        [StlAttribute(Title = "到达省")]
        private const string ToPro = nameof(ToPro);

        [StlAttribute(Title = "默认 ： https://www.chinawutong.com/201")]
        private const string StartHref = nameof(StartHref);
        [StlAttribute(Title = "页面类型  首页  详情页")]
        private const string PageType = nameof(PageType);

        #region 静态数据

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



        private static string[] zhixiaquxian = ["北京市,北京市,东城区",
"北京市,北京市,西城区",
"北京市,北京市,朝阳区",
"北京市,北京市,丰台区",
"北京市,北京市,石景山区",
"北京市,北京市,海淀区",
"北京市,北京市,市辖区",
"北京市,北京市,房山区",
"北京市,北京市,通州区",
"北京市,北京市,顺义区",
"北京市,北京市,昌平区",
"北京市,北京市,大兴区",
"北京市,北京市,怀柔区",
"北京市,北京市,平谷区",
"北京市,北京市,门头沟区",
"北京市,北京市,密云区",
"北京市,北京市,延庆区",
"上海市,上海市,黄浦区",
"上海市,上海市,徐汇区",
"上海市,上海市,长宁区",
"上海市,上海市,静安区",
"上海市,上海市,普陀区",
"上海市,上海市,虹口区",
"上海市,上海市,杨浦区",
"上海市,上海市,闵行区",
"上海市,上海市,宝山区",
"上海市,上海市,市辖区",
"上海市,上海市,浦东新区",
"上海市,上海市,金山区",
"上海市,上海市,松江区",
"上海市,上海市,青浦区",
"上海市,上海市,南汇区",
"上海市,上海市,奉贤区",
"上海市,上海市,嘉定区",
"上海市,上海市,崇明区",
"天津市,天津市,和平区",
"天津市,天津市,河东区",
"天津市,天津市,河西区",
"天津市,天津市,南开区",
"天津市,天津市,河北区",
"天津市,天津市,红桥区",
"天津市,天津市,市辖区",
"天津市,天津市,汉沽区",
"天津市,天津市,滨海新区",
"天津市,天津市,东丽区",
"天津市,天津市,西青区",
"天津市,天津市,津南区",
"天津市,天津市,北辰区",
"天津市,天津市,武清区",
"天津市,天津市,宝坻区",
"天津市,天津市,塘沽区",
"天津市,天津市,宁河区",
"天津市,天津市,静海区",
"天津市,天津市,蓟州区",
"重庆市,重庆市,市辖区",
"重庆市,重庆市,江津区",
"重庆市,重庆市,合川区",
"重庆市,重庆市,永川区",
"重庆市,重庆市,南川区",
"重庆市,重庆市,涪陵区",
"重庆市,重庆市,渝中区",
"重庆市,重庆市,大渡口区",
"重庆市,重庆市,江北区",
"重庆市,重庆市,沙坪坝区",
"重庆市,重庆市,九龙坡区",
"重庆市,重庆市,南岸区",
"重庆市,重庆市,北碚区",
"重庆市,重庆市,万盛区",
"重庆市,重庆市,渝北区",
"重庆市,重庆市,巴南区",
"重庆市,重庆市,黔江区",
"重庆市,重庆市,长寿区",
"重庆市,重庆市,万州区",
"重庆市,重庆市,綦江区",
"重庆市,重庆市,潼南区",
"重庆市,重庆市,铜梁区",
"重庆市,重庆市,大足区",
"重庆市,重庆市,荣昌区",
"重庆市,重庆市,璧山区",
"重庆市,重庆市,梁平区",
"重庆市,重庆市,城口县",
"重庆市,重庆市,丰都县",
"重庆市,重庆市,垫江县",
"重庆市,重庆市,武隆区",
"重庆市,重庆市,忠县",
"重庆市,重庆市,开州区",
"重庆市,重庆市,云阳县",
"重庆市,重庆市,奉节县",
"重庆市,重庆市,巫山县",
"重庆市,重庆市,巫溪县",
"重庆市,重庆市,石柱县",
"重庆市,重庆市,秀山县",
"重庆市,重庆市,酉阳县",
"重庆市,重庆市,彭水县"];


        private static string[] shengshi = ["安徽省,合肥市,市辖区",
"安徽省,芜湖市,市辖区",
"安徽省,蚌埠市,市辖区",
"安徽省,淮南市,市辖区",
"安徽省,马鞍山市,市辖区",
"安徽省,淮北市,市辖区",
"安徽省,铜陵市,市辖区",
"安徽省,安庆市,市辖区",
"安徽省,黄山市,市辖区",
"安徽省,滁州市,市辖区",
"安徽省,阜阳市,市辖区",
"安徽省,宿州市,市辖区",
"安徽省,巢湖市,市辖区",
"安徽省,六安市,市辖区",
"安徽省,亳州市,市辖区",
"安徽省,池州市,市辖区",
"安徽省,宣城市,市辖区",
"福建省,福州市,市辖区",
"福建省,厦门市,市辖区",
"福建省,莆田市,市辖区",
"福建省,三明市,市辖区",
"福建省,泉州市,市辖区",
"福建省,漳州市,市辖区",
"福建省,南平市,市辖区",
"福建省,龙岩市,市辖区",
"福建省,宁德市,市辖区",
"甘肃省,兰州市,市辖区",
"甘肃省,嘉峪关市,市辖区",
"甘肃省,金昌市,市辖区",
"甘肃省,白银市,市辖区",
"甘肃省,天水市,市辖区",
"甘肃省,武威市,市辖区",
"甘肃省,张掖市,市辖区",
"甘肃省,平凉市,市辖区",
"甘肃省,酒泉市,市辖区",
"甘肃省,庆阳市,市辖区",
"甘肃省,定西市,市辖区",
"甘肃省,陇南市,市辖区",
"甘肃省,甘南自治州,市辖区",
"甘肃省,临夏自治州,市辖区",
"广东省,广州市,市辖区",
"广东省,韶关市,市辖区",
"广东省,深圳市,市辖区",
"广东省,珠海市,市辖区",
"广东省,汕头市,市辖区",
"广东省,佛山市,市辖区",
"广东省,江门市,市辖区",
"广东省,湛江市,市辖区",
"广东省,茂名市,市辖区",
"广东省,肇庆市,市辖区",
"广东省,惠州市,市辖区",
"广东省,梅州市,市辖区",
"广东省,汕尾市,市辖区",
"广东省,河源市,市辖区",
"广东省,阳江市,市辖区",
"广东省,清远市,市辖区",
"广东省,东莞市,市辖区",
"广东省,中山市,市辖区",
"广东省,潮州市,市辖区",
"广东省,揭阳市,市辖区",
"广东省,云浮市,市辖区",
"广西区,南宁市,市辖区",
"广西区,柳州市,市辖区",
"广西区,桂林市,市辖区",
"广西区,梧州市,市辖区",
"广西区,北海市,市辖区",
"广西区,防城港市,市辖区",
"广西区,钦州市,市辖区",
"广西区,贵港市,市辖区",
"广西区,玉林市,市辖区",
"广西区,百色市,市辖区",
"广西区,贺州市,市辖区",
"广西区,河池市,市辖区",
"广西区,来宾市,市辖区",
"广西区,崇左市,市辖区",
"贵州省,贵阳市,市辖区",
"贵州省,六盘水市,市辖区",
"贵州省,遵义市,市辖区",
"贵州省,安顺市,市辖区",
"贵州省,毕节地区,市辖区",
"贵州省,黔东南自治州,市辖区",
"贵州省,黔南自治州,市辖区",
"贵州省,黔西南自治州,市辖区",
"贵州省,铜仁地区,市辖区",
"海南省,海口市,市辖区",
"海南省,三亚市,市辖区",
"海南省,海南直辖市,市辖区",
"海南省,海南直辖县,市辖区",
"河北省,石家庄市,市辖区",
"河北省,唐山市,市辖区",
"河北省,秦皇岛市,市辖区",
"河北省,邯郸市,市辖区",
"河北省,邢台市,市辖区",
"河北省,保定市,市辖区",
"河北省,张家口市,市辖区",
"河北省,承德市,市辖区",
"河北省,沧州市,市辖区",
"河北省,廊坊市,市辖区",
"河北省,衡水市,市辖区",
"河南省,郑州市,市辖区",
"河南省,开封市,市辖区",
"河南省,洛阳市,市辖区",
"河南省,平顶山市,市辖区",
"河南省,安阳市,市辖区",
"河南省,鹤壁市,市辖区",
"河南省,新乡市,市辖区",
"河南省,焦作市,市辖区",
"河南省,济源市,市辖区",
"河南省,濮阳市,市辖区",
"河南省,许昌市,市辖区",
"河南省,漯河市,市辖区",
"河南省,三门峡市,市辖区",
"河南省,南阳市,市辖区",
"河南省,商丘市,市辖区",
"河南省,信阳市,市辖区",
"河南省,周口市,市辖区",
"河南省,驻马店市,市辖区",
"黑龙江省,哈尔滨市,市辖区",
"黑龙江省,齐齐哈尔市,市辖区",
"黑龙江省,鸡西市,市辖区",
"黑龙江省,鹤岗市,市辖区",
"黑龙江省,双鸭山市,市辖区",
"黑龙江省,大庆市,市辖区",
"黑龙江省,伊春市,市辖区",
"黑龙江省,佳木斯市,市辖区",
"黑龙江省,七台河市,市辖区",
"黑龙江省,牡丹江市,市辖区",
"黑龙江省,黑河市,市辖区",
"黑龙江省,绥化市,市辖区",
"黑龙江省,大兴安岭地区,市辖区",
"湖北省,武汉市,市辖区",
"湖北省,黄石市,市辖区",
"湖北省,十堰市,市辖区",
"湖北省,宜昌市,市辖区",
"湖北省,襄阳市,市辖区",
"湖北省,鄂州市,市辖区",
"湖北省,荆门市,市辖区",
"湖北省,孝感市,市辖区",
"湖北省,荆州市,市辖区",
"湖北省,黄冈市,市辖区",
"湖北省,咸宁市,市辖区",
"湖北省,随州市,市辖区",
"湖北省,仙桃市,市辖区",
"湖北省,潜江市,市辖区",
"湖北省,天门市,市辖区",
"湖北省,神农架,市辖区",
"湖北省,恩施自治州,市辖区",
"湖南省,长沙市,市辖区",
"湖南省,株洲市,市辖区",
"湖南省,湘潭市,市辖区",
"湖南省,衡阳市,市辖区",
"湖南省,邵阳市,市辖区",
"湖南省,岳阳市,市辖区",
"湖南省,常德市,市辖区",
"湖南省,张家界市,市辖区",
"湖南省,益阳市,市辖区",
"湖南省,郴州市,市辖区",
"湖南省,永州市,市辖区",
"湖南省,怀化市,市辖区",
"湖南省,娄底市,市辖区",
"湖南省,湘西自治州,市辖区",
"吉林省,长春市,市辖区",
"吉林省,吉林市,市辖区",
"吉林省,四平市,市辖区",
"吉林省,辽源市,市辖区",
"吉林省,通化市,市辖区",
"吉林省,白山市,市辖区",
"吉林省,松原市,市辖区",
"吉林省,白城市,市辖区",
"吉林省,延边自治州,市辖区",
"江苏省,南京市,市辖区",
"江苏省,无锡市,市辖区",
"江苏省,徐州市,市辖区",
"江苏省,常州市,市辖区",
"江苏省,苏州市,市辖区",
"江苏省,南通市,市辖区",
"江苏省,连云港市,市辖区",
"江苏省,淮安市,市辖区",
"江苏省,盐城市,市辖区",
"江苏省,扬州市,市辖区",
"江苏省,镇江市,市辖区",
"江苏省,泰州市,市辖区",
"江苏省,宿迁市,市辖区",
"江西省,南昌市,市辖区",
"江西省,景德镇市,市辖区",
"江西省,萍乡市,市辖区",
"江西省,九江市,市辖区",
"江西省,新余市,市辖区",
"江西省,鹰潭市,市辖区",
"江西省,赣州市,市辖区",
"江西省,吉安市,市辖区",
"江西省,宜春市,市辖区",
"江西省,抚州市,市辖区",
"江西省,上饶市,市辖区",
"辽宁省,沈阳市,市辖区",
"辽宁省,大连市,市辖区",
"辽宁省,鞍山市,市辖区",
"辽宁省,抚顺市,市辖区",
"辽宁省,本溪市,市辖区",
"辽宁省,丹东市,市辖区",
"辽宁省,锦州市,市辖区",
"辽宁省,营口市,市辖区",
"辽宁省,阜新市,市辖区",
"辽宁省,辽阳市,市辖区",
"辽宁省,盘锦市,市辖区",
"辽宁省,铁岭市,市辖区",
"辽宁省,朝阳市,市辖区",
"辽宁省,葫芦岛市,市辖区",
"内蒙古区,呼和浩特市,市辖区",
"内蒙古区,包头市,市辖区",
"内蒙古区,乌海市,市辖区",
"内蒙古区,赤峰市,市辖区",
"内蒙古区,通辽市,市辖区",
"内蒙古区,鄂尔多斯市,市辖区",
"内蒙古区,呼伦贝尔市,市辖区",
"内蒙古区,巴彦淖尔市,市辖区",
"内蒙古区,乌兰察布市,市辖区",
"内蒙古区,阿拉善盟,市辖区",
"内蒙古区,锡林郭勒盟,市辖区",
"内蒙古区,兴安盟,市辖区",
"宁夏区,银川市,市辖区",
"宁夏区,石嘴山市,市辖区",
"宁夏区,吴忠市,市辖区",
"宁夏区,固原市,市辖区",
"宁夏区,中卫市,市辖区",
"青海省,西宁市,市辖区",
"青海省,果洛自治州,市辖区",
"青海省,海北自治州,市辖区",
"青海省,海东地区,市辖区",
"青海省,海南自治州,市辖区",
"青海省,海西自治州,市辖区",
"青海省,黄南自治州,市辖区",
"青海省,玉树自治州,市辖区",
"山东省,济南市,市辖区",
"山东省,青岛市,市辖区",
"山东省,淄博市,市辖区",
"山东省,枣庄市,市辖区",
"山东省,东营市,市辖区",
"山东省,烟台市,市辖区",
"山东省,潍坊市,市辖区",
"山东省,济宁市,市辖区",
"山东省,泰安市,市辖区",
"山东省,威海市,市辖区",
"山东省,日照市,市辖区",
"山东省,莱芜市,市辖区",
"山东省,临沂市,市辖区",
"山东省,德州市,市辖区",
"山东省,聊城市,市辖区",
"山东省,滨州市,市辖区",
"山东省,菏泽市,市辖区",
"山西省,太原市,市辖区",
"山西省,大同市,市辖区",
"山西省,阳泉市,市辖区",
"山西省,长治市,市辖区",
"山西省,晋城市,市辖区",
"山西省,朔州市,市辖区",
"山西省,晋中市,市辖区",
"山西省,运城市,市辖区",
"山西省,忻州市,市辖区",
"山西省,临汾市,市辖区",
"山西省,吕梁市,市辖区",
"陕西省,西安市,市辖区",
"陕西省,铜川市,市辖区",
"陕西省,宝鸡市,市辖区",
"陕西省,咸阳市,市辖区",
"陕西省,渭南市,市辖区",
"陕西省,延安市,市辖区",
"陕西省,汉中市,市辖区",
"陕西省,榆林市,市辖区",
"陕西省,安康市,市辖区",
"陕西省,商洛市,市辖区",
"四川省,成都市,市辖区",
"四川省,自贡市,市辖区",
"四川省,攀枝花市,市辖区",
"四川省,泸州市,市辖区",
"四川省,德阳市,市辖区",
"四川省,绵阳市,市辖区",
"四川省,广元市,市辖区",
"四川省,遂宁市,市辖区",
"四川省,内江市,市辖区",
"四川省,乐山市,市辖区",
"四川省,南充市,市辖区",
"四川省,眉山市,市辖区",
"四川省,宜宾市,市辖区",
"四川省,广安市,市辖区",
"四川省,达州市,市辖区",
"四川省,雅安市,市辖区",
"四川省,巴中市,市辖区",
"四川省,资阳市,市辖区",
"四川省,阿坝自治州,市辖区",
"四川省,甘孜自治州,市辖区",
"四川省,凉山自治州,市辖区",
"西藏区,拉萨市,市辖区",
"西藏区,阿里地区,市辖区",
"西藏区,昌都地区,市辖区",
"西藏区,林芝地区,市辖区",
"西藏区,那曲地区,市辖区",
"西藏区,日喀则地区,市辖区",
"西藏区,山南地区,市辖区",
"新疆区,乌鲁木齐市,市辖区",
"新疆区,克拉玛依市,市辖区",
"新疆区,石河子市,市辖区",
"新疆区,阿拉尔市,市辖区",
"新疆区,图木舒克市,市辖区",
"新疆区,五家渠市,市辖区",
"新疆区,昌吉自治州,市辖区",
"新疆区,阿克苏地区,市辖区",
"新疆区,阿勒泰地区,市辖区",
"新疆区,巴音郭楞州,市辖区",
"新疆区,博尔塔拉州,市辖区",
"新疆区,哈密地区,市辖区",
"新疆区,和田地区,市辖区",
"新疆区,喀什地区,市辖区",
"新疆区,克孜勒苏州,市辖区",
"新疆区,塔城地区,市辖区",
"新疆区,吐鲁番地区,市辖区",
"新疆区,伊犁自治州,市辖区",
"新疆区,北屯市,市辖区",
"云南省,昆明市,市辖区",
"云南省,曲靖市,市辖区",
"云南省,玉溪市,市辖区",
"云南省,保山市,市辖区",
"云南省,昭通市,市辖区",
"云南省,丽江市,市辖区",
"云南省,普洱市,市辖区",
"云南省,临沧市,市辖区",
"云南省,楚雄自治州,市辖区",
"云南省,红河自治州,市辖区",
"云南省,文山自治州,市辖区",
"云南省,西双版纳州,市辖区",
"云南省,大理自治州,市辖区",
"云南省,德宏自治州,市辖区",
"云南省,怒江自治州,市辖区",
"云南省,迪庆自治州,市辖区",
"浙江省,杭州市,市辖区",
"浙江省,宁波市,市辖区",
"浙江省,温州市,市辖区",
"浙江省,嘉兴市,市辖区",
"浙江省,湖州市,市辖区",
"浙江省,绍兴市,市辖区",
"浙江省,金华市,市辖区",
"浙江省,衢州市,市辖区",
"浙江省,舟山市,市辖区",
"浙江省,台州市,市辖区",
"浙江省,丽水市,市辖区"];


        #endregion


        public static async Task<object> ParseAsync(IParseManager parseManager)
        {
            var attributes = new NameValueCollection();

            var ulClass = string.Empty;
            var fromPro = string.Empty;
            var fromCity = string.Empty;
            var fromArea = string.Empty;
            var startHref = string.Empty;
            var pageType = string.Empty;
            var toPro = string.Empty;


            foreach (var name in parseManager.ContextInfo.Attributes.AllKeys)
            {
                var value = parseManager.ContextInfo.Attributes[name];
                if (StringUtils.EqualsIgnoreCase(name, UlClass))
                {
                    ulClass = value;
                }
                else if (StringUtils.EqualsIgnoreCase(name, FromPro))
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
                else if (StringUtils.EqualsIgnoreCase(name, StartHref))
                {
                    startHref = value;
                }
                else if (StringUtils.EqualsIgnoreCase(name, PageType))
                {
                    pageType = value;
                }
                else
                {
                    attributes[name] = value;
                }
            }
            return await ParseAsync(parseManager, ulClass, fromPro, fromCity, fromArea, toPro, pageType, string.IsNullOrEmpty(startHref) ? "https://www.chinawutong.com/201" : startHref, attributes);
        }

        private static async Task<string> ParseAsync(IParseManager parseManager, string ulClass, string fromPro, string fromCity, string fromArea, string toPro, string pageType, string StartHref, NameValueCollection attributes)
        {
            var builder = new StringBuilder();
            List<string> toCity = new();
            if (pageType == "首页")
            {
                toCity.AddRange(shenghui.ToArray());
                toCity.AddRange(zhixiashi.ToArray());
            }
            else if (pageType == "详情页")
            {
                toCity.AddRange(shengshi.Where(s => s.StartsWith(toPro)));
                toCity.AddRange(zhixiaquxian.Where(s => s.StartsWith(toPro)));
            }

            builder.Append($"<ul class='{ulClass}'>");
            foreach (var item in toCity)
            {
                var tos = item.Split(',');
                var top = tos[0];
                var toc = tos[1];
                var toa = tos[2];
                var href = await Get201Url(fromPro, fromCity, fromArea, top, toc, toa);

                var from = GetShortAddr(fromPro, fromCity, fromArea);
                var to = GetShortAddr(top, toc, toa);

                builder.Append($"<li><a href='{StartHref}{href}'>{from}到{to}物流公司</a></li>");
            }
            builder.Append($"</ul>");
            return builder.ToString();
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
                        param = param.Replace("省", string.Empty).Replace("区", string.Empty).Replace("自治区", "").Replace("直辖市", "").Replace("特别行政区", "");
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

