using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Northwind.WebApp.Htmlhelper
{
    public static class HtmlExtension
    {
        
        public static MvcHtmlString Image(this HtmlHelper helper, byte[] image)
        {
            return Image(helper, image, null);

        }

        public static MvcHtmlString Image(this HtmlHelper helper, byte[] image, string atternate)
        {
            return Image(helper,new Guid().ToString(), image, atternate,null);

        }


        public static MvcHtmlString Image(this HtmlHelper helper, string id, byte[] image, string alternateText, object htmlAttributes)
        
       {

            if (helper == null)
            {
                //throw new ArgumentNullException("helper");
                return new MvcHtmlString("");
            }
            if (image == null)
            {
                //throw new ArgumentNullException("image");
                return new MvcHtmlString("");
            }

            string img = GetImage(image);

            if (img != string.Empty)
            {
                // Create tag builder
                var builder = new TagBuilder("img");

                // Create valid id
                //builder.GenerateId(id);



                // Add attributes
                builder.MergeAttribute("src", img);
                builder.MergeAttribute("alt", alternateText);
                builder.MergeAttribute("id", id);
                builder.MergeAttribute("name", id);
                builder.MergeAttribute("data-val", "true");
                builder.MergeAttribute("type", "file");
                builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
                //builder.GenerateId(helper.ViewContext.HttpContext.Items[MyHelpers.IdKey]);

                // Render tag
                return new MvcHtmlString(builder.ToString(TagRenderMode.SelfClosing));
            }
            return new MvcHtmlString(string.Empty);
       }

        //todo unit test
        // reference : http://www.nullskull.com/a/10450951/aspnet-mvc-display-images-directly-from-the-viewmodel-into-your-views.aspx
        public static string GetImage(byte[] rawImage)
        {
            if (rawImage != null)
            {
                MemoryStream ms = new MemoryStream();

                byte[] image = OleImageUnwrap.GetImageBytesFromOLEField(rawImage);
                if (image != null)
                {
                    ms.Write(image, 0, image.Length - 1);
                    //ms.Write(image, 78, image.Length - 78);
                    // strip out 78 byte OLE header (don't need to do this for normal images)
                    string imageBase64 = Convert.ToBase64String(ms.ToArray());

                    return string.Format("data:image/gif;base64,{0}", imageBase64);
                } 
            }  
            return string.Empty;
        }


        // reference :http://www.asp.net/mvc/tutorials/older-versions/views/using-the-tagbuilder-class-to-build-html-helpers-cs
        public static string Image(this HtmlHelper helper, string id, string url, string alternateText, object htmlAttributes)
        {
            // Create tag builder
            var builder = new TagBuilder("img");

            // Create valid id
            builder.GenerateId(id);

            // Add attributes
            builder.MergeAttribute("src", url);
            builder.MergeAttribute("alt", alternateText);
            builder.MergeAttributes(new RouteValueDictionary(htmlAttributes));

            // Render tag
            return builder.ToString(TagRenderMode.SelfClosing);
        }
    }
}