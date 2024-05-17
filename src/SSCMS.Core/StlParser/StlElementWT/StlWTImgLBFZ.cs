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
using Senparc.Weixin.Annotations;
using System.Security.Policy;
using System.Security.Cryptography;
using Senparc.NeuChar.App.AppStore;
using MathNet.Numerics;
using DocumentFormat.OpenXml.Bibliography;
using SSCMS.Utils;

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
        private const string LogoUp = nameof(LogoUp);
        private const string LogoDown = nameof(LogoDown);
        private const string From = nameof(From);
        private const string To = nameof(To);
        private const string Description = nameof(Description);
        private const string DeliveryArea = nameof(DeliveryArea);


        public static async Task<object> ParseAsync(IParseManager parseManager)
        {


            var webroot = parseManager.SettingsManager.WebRootPath;
            var WTData = Path.Combine(webroot, "WTData");
            if (!Directory.Exists(WTData))
            {
                Directory.CreateDirectory(WTData);
            }
            var ImgData = Path.Combine(WTData, "Image");
            if (!Directory.Exists(ImgData))
            {
                Directory.CreateDirectory(ImgData);
            }


            var id = string.Empty;
            var title = string.Empty;
            var logoUp = string.Empty;
            var logoDown = string.Empty;
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
                else if (StringUtils.EqualsIgnoreCase(name, LogoUp)&&!value.StartsWith("{"))
                {
                    logoUp = value;
                }
                else if (StringUtils.EqualsIgnoreCase(name, LogoUp))
                {
                    logoUp = await parseManager.ReplaceStlEntitiesForAttributeValueAsync(value);
                }
                else if (StringUtils.EqualsIgnoreCase(name, LogoDown) && !value.StartsWith("{"))
                {
                    logoDown = value;
                }
                else if (StringUtils.EqualsIgnoreCase(name, LogoDown))
                {
                    logoDown = await parseManager.ReplaceStlEntitiesForAttributeValueAsync(value);
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


            var saveImgPath = Path.Combine(ImgData, "siteId" + parseManager.ContextInfo.Site.Id + "cId" + parseManager.ContextInfo.ContentId + "Id" +id +".jpg");

            foreach (var item in SystemFonts.Families)
            {
                await Console.Out.WriteLineAsync(item.Name);
            }

            using (var image = new Image<Rgba32>(225, 200, Color.White))
            {
                var fontFamily = SystemFonts.Families.FirstOrDefault(f => f.Name == "Microsoft YaHei");
                var boldFont = fontFamily.CreateFont(10, FontStyle.Bold);

                image.Mutate(ctx => ctx.Fill(Color.Red));

                // Draw the yellow rectangle at the top
                var yellowRectTop = new Rectangle(0, 15, 225, 2);
                image.Mutate(ctx => ctx.Fill(Color.Yellow, yellowRectTop));


                // Draw the yellow rectangle inside the red one (bottom)
                var yellowRectBottom = new Rectangle(0, 145, 225, 30);
                image.Mutate(ctx => ctx.Fill(Color.Yellow, yellowRectBottom));



                // Top yellow text
                image.Mutate(ctx => ctx.DrawText(title, boldFont, Color.Yellow, new PointF(5, 2)));

                image.Mutate(ctx => ctx.DrawText(logoUp, boldFont, Color.White, new PointF(85, 45)));

                image.Mutate(ctx => ctx.DrawText(from, boldFont, Color.White, new PointF(15, 60)));
                image.Mutate(ctx => ctx.DrawText("<—", boldFont, Color.White, new PointF(95, 57)));
                image.Mutate(ctx => ctx.DrawText("—>", boldFont, Color.White, new PointF(95, 62)));
                image.Mutate(ctx => ctx.DrawText(to, boldFont, Color.White, new PointF(135, 60)));
                image.Mutate(ctx => ctx.DrawText(logoDown, boldFont, Color.White, new PointF(85, 80)));

                image.Mutate(ctx => ctx.DrawText(description, boldFont, Color.Red, new PointF(15, 150)));

                image.Mutate(ctx => ctx.DrawText(deliveryArea, boldFont, Color.White, new PointF(10, 186)));


                // Save the image
                image.Save(saveImgPath);
            }
            return saveImgPath.Split("wwwroot")[1];
        }
    }
}

