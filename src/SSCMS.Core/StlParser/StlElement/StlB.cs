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

namespace SSCMS.Core.StlParser.StlElement
{
    [StlElement(Title = "获取链接")]
    public static class StlB
    {
        public const string ElementName = "stl:b";

        [StlAttribute(Title = "栏目索引")]
        private const string Width = nameof(Width);

        [StlAttribute(Title = "链接目标")]
        private const string Target = nameof(Target);

        [StlAttribute(Title = "链接参数")]
        private const string ShowContent = nameof(ShowContent);

        public static async Task<object> ParseAsync(IParseManager parseManager)
        {
            var attributes = new NameValueCollection();
            var width = string.Empty;
            var showContent = string.Empty;
            var target = string.Empty;

            foreach (var name in parseManager.ContextInfo.Attributes.AllKeys)
            {
                var value = parseManager.ContextInfo.Attributes[name];
                if (StringUtils.EqualsIgnoreCase(name, Width))
                {
                    width = await parseManager.ReplaceStlEntitiesForAttributeValueAsync(value);
                    if (!string.IsNullOrEmpty(Width))
                    {
                        parseManager.ContextInfo.ContextType = ParseType.Channel;
                    }
                }
                else if (StringUtils.EqualsIgnoreCase(name, ShowContent))
                {
                    showContent = value;
                }
                else if (StringUtils.EqualsIgnoreCase(name, Target))
                {
                    target = value;
                }
                else
                {
                    attributes[name] = value;
                }

            }
            return await ParseAsync(parseManager, width, showContent, target, attributes);
        }

        private static async Task<string> ParseAsync(IParseManager parseManager, string Width, string ShowContent, string Target, NameValueCollection attributes)
        {
            return $@"<a style=""width: {Width}px; "" href=""{Target}"">{ShowContent}</a>";
            //return $@"<a>测试</a>";
        }
    }
}
