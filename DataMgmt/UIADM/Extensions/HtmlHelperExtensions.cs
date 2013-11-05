using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using UIADM.ViewModels;

namespace UIADM.Extensions
{
    public static class HtmlHelperExtensions
    {
        public static MvcHtmlString PageLinks(this HtmlHelper html, PagingInfo pagingInfo, Func<int, string> pageUrlAccessor)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= pagingInfo.PageCount; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrlAccessor(i));
                tag.InnerHtml = i.ToString();
                if (i == pagingInfo.PageIndex)
                {
                    tag.AddCssClass("selected");
                }
                result.Append(tag.ToString());
            }
            return MvcHtmlString.Create(result.ToString());
        }
    }
}