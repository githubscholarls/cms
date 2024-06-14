using Microsoft.AspNetCore.Mvc;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SSCMS.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.Fonts;
using SSCMS.Utils;
using SixLabors.ImageSharp.Drawing;

namespace SSCMS.Core.StlParser.StlElement
{
    /// <summary>
    /// 礼邦分站画图
    /// </summary>
    public static class StlWTImgLBFZ
    {
        public const string ElementName = "stl:wtlbfz";
        private const string SiteName = nameof(SiteName);
        private const string Id = nameof(Id);
        private const string Title = nameof(Title);
        private const string From = nameof(From);
        private const string To = nameof(To);
        private const string Description = nameof(Description);
        private const string DeliveryArea = nameof(DeliveryArea);
        private static readonly string BGPath = System.IO.Path.Combine("WTTemplate", "Image", "立邦物流模板底图.jpg");


        public static async Task<object> ParseAsync(IParseManager parseManager)
        {
             

            var webroot = parseManager.SettingsManager.WebRootPath;
            var WTData = System.IO.Path.Combine(webroot, parseManager.ContextInfo.Site.SiteName,"WTData");
            if (!Directory.Exists(WTData))
            {
                Directory.CreateDirectory(WTData);
            }
            var ImgData = System.IO.Path.Combine(WTData, "Image", parseManager.ContextInfo.Content.AddDate?.ToString("yyyyMMdd"));
            if (!Directory.Exists(ImgData))
            {
                Directory.CreateDirectory(ImgData);
            }


            var id = string.Empty;
            var title = string.Empty;
            var from = string.Empty;
            var to = string.Empty;
            var description = string.Empty;
            var deliveryArea = string.Empty;



            foreach (var name in parseManager.ContextInfo.Attributes.AllKeys)
            {
                var value = parseManager.ContextInfo.Attributes[name];
                if (StringUtils.EqualsIgnoreCase(name, Id) && !value.StartsWith("{"))
                {
                    id = value;
                }
                else if (StringUtils.EqualsIgnoreCase(name, Id))
                {
                    id = await parseManager.ReplaceStlEntitiesForAttributeValueAsync(value);
                }
                else if (StringUtils.EqualsIgnoreCase(name, Title) && !value.StartsWith("{"))
                {
                    title = value;
                }
                else if (StringUtils.EqualsIgnoreCase(name, Title))
                {
                    title = await parseManager.ReplaceStlEntitiesForAttributeValueAsync(value);
                }
                else if (StringUtils.EqualsIgnoreCase(name, From) && !value.StartsWith("{"))
                {
                    from = value;
                }
                else if (StringUtils.EqualsIgnoreCase(name, From))
                {
                    from = await parseManager.ReplaceStlEntitiesForAttributeValueAsync(value);
                }
                else if (StringUtils.EqualsIgnoreCase(name, To) && !value.StartsWith("{"))
                {
                    to = value;
                }
                else if (StringUtils.EqualsIgnoreCase(name, To))
                {
                    to = await parseManager.ReplaceStlEntitiesForAttributeValueAsync(value);
                }
                else if (StringUtils.EqualsIgnoreCase(name, Description) && !value.StartsWith("{"))
                {
                    description = value;
                }
                else if (StringUtils.EqualsIgnoreCase(name, Description))
                {
                    description = await parseManager.ReplaceStlEntitiesForAttributeValueAsync(value);
                }
                else if (StringUtils.EqualsIgnoreCase(name, DeliveryArea) && !value.StartsWith("{"))
                {
                    deliveryArea = value;
                }
                else if (StringUtils.EqualsIgnoreCase(name, DeliveryArea))
                {
                    deliveryArea = await parseManager.ReplaceStlEntitiesForAttributeValueAsync(value);
                }
            }


            var saveImgPath = System.IO.Path.Combine(ImgData, "siteId" + parseManager.ContextInfo.Site.Id + "cId" + parseManager.ContextInfo.ContentId + "Id" +id +".jpg");

            foreach (var item in SystemFonts.Families)
            {
                await Console.Out.WriteLineAsync(item.Name);
            }

            var bg = System.IO.Path.Combine(webroot,BGPath);


            using (var img = Image.Load(bg))
            {
                var fontFamily = SystemFonts.Families.FirstOrDefault(f => f.Name == "Microsoft YaHei");

                var fontSize = 40;

                var boldFont = fontFamily.CreateFont(fontSize, FontStyle.Bold);

                if (!string.IsNullOrEmpty(title))
                {
                    PathBuilder pathBuilder = new PathBuilder();
                    pathBuilder.SetOrigin(new Point(350, 32));

                    pathBuilder.AddLine(new Point(0, 0), new Point(700, 0));
                    var path = pathBuilder.Build();

                    // Draw the text along the path wrapping at the end of the line
                    var textOptions = new TextOptions(boldFont)
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        WrappingLength = path.ComputeLength()

                    };


                    //// Let's generate the text as a set of vectors drawn along the path
                    var glyphs = TextBuilder.GenerateGlyphs(title, path, textOptions);

                    img.Mutate(ctx => ctx
                        .Fill(Color.White, glyphs));
                }


                if (!string.IsNullOrEmpty(from))
                {
                    PathBuilder fromPathBuilder = new PathBuilder();
                    fromPathBuilder.SetOrigin(new Point(150, 125));
                    fromPathBuilder.AddLine(new Point(0, 0), new Point(200, 0));

                    var fromPath = fromPathBuilder.Build();

                    var fromTextOption = new TextOptions(fontFamily.CreateFont(48, FontStyle.Bold))
                    {

                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        WrappingLength = fromPath.ComputeLength()
                    };

                    //// Let's generate the text as a set of vectors drawn along the path
                    var fromglyphs = TextBuilder.GenerateGlyphs(from, fromPath, fromTextOption);

                    img.Mutate(ctx => ctx
                        .Fill(Color.FromRgb(201, 42, 58), fromglyphs));

                }
                if (!string.IsNullOrEmpty(to))
                {
                    PathBuilder toPathBuilder = new PathBuilder();
                    toPathBuilder.SetOrigin(new Point(550, 125));
                    toPathBuilder.AddLine(new Point(0, 0), new Point(200, 0));

                    var toPath = toPathBuilder.Build();

                    var toTextOption = new TextOptions(fontFamily.CreateFont(48, FontStyle.Bold))
                    {

                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center,
                        WrappingLength = toPath.ComputeLength()
                    };

                    //// Let's generate the text as a set of vectors drawn along the path
                    var toglyphs = TextBuilder.GenerateGlyphs(to, toPath, toTextOption);

                    img.Mutate(ctx => ctx
                        .Fill(Color.FromRgb(201, 42, 58), toglyphs));
                }

                if (!string.IsNullOrEmpty(description))
                {
                    var lichengFontSize = 28;
                    PathBuilder desPathBuilder = new PathBuilder();
                    desPathBuilder.SetOrigin(new Point(350, 0));
                    desPathBuilder.AddLine(new Point(0, 175), new Point(700, 175));

                    var desPath = desPathBuilder.Build();

                    var desTextOption = new TextOptions(fontFamily.CreateFont(lichengFontSize, FontStyle.Italic))
                    {
                        HorizontalAlignment = HorizontalAlignment.Center,
                        WrappingLength = desPath.ComputeLength()
                    };

                    //// Let's generate the text as a set of vectors drawn along the path
                    var desglyphs = TextBuilder.GenerateGlyphs(description, desPath, desTextOption);

                    img.Mutate(ctx => ctx
                        .Fill(Color.FromRgb(201, 42, 58), desglyphs));
                }
                if (!string.IsNullOrEmpty(deliveryArea))
                {
                    var areaFontSize = 20;
                    PathBuilder areaPathBuilder = new PathBuilder();
                    areaPathBuilder.SetOrigin(new Point(420, 300));
                    areaPathBuilder.AddLine(new PointF(0, 0), new PointF(510, 0));

                    var areaPath = areaPathBuilder.Build();

                    var areaFont = fontFamily.CreateFont(areaFontSize, FontStyle.Regular);

                    var areaTextOptions = new TextOptions(areaFont)
                    {
                        HorizontalAlignment = HorizontalAlignment.Center, // 文本水平居中
                        VerticalAlignment = VerticalAlignment.Center,     // 文本垂直居中
                        WrappingLength = areaPath.ComputeLength(),
                        LineSpacing = 1.5f
                    };

                    //// Let's generate the text as a set of vectors drawn along the path
                    var areaglyphs = TextBuilder.GenerateGlyphs(deliveryArea, areaPath, areaTextOptions);

                    img.Mutate(ctx => ctx
                        .Fill(Color.Black, areaglyphs));
                }
                img.Save(saveImgPath);
            }
            ///huizhoufz/WTData/Image/20240524/siteId1501cId0Id.jpg
            

            var url = saveImgPath.Split("wwwroot")[1];
            var splitChar = url[0];
            var endList = new List<string>
            {
                "WTData"
            };
            endList.AddRange(url.Split("WTData")[1].Split(new char[] { '/', '\\' }).ToList());
            return splitChar + System.IO.Path.Combine(endList.ToArray());

            
        }
    }
}

