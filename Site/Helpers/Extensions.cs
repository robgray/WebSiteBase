using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web.Mvc;
using Domain.Entities;

namespace WebBase.Mvc.Helpers
{
    public static class Extensions
    {
        public static SelectList ToSelectList(
            this IEnumerable<string> enumerable,            
            bool includeBlank = true)
        {
            var items = enumerable.Select(f => new SelectListItem()
            {
                Text = f,
                Value = f
            }).ToList();
            if (includeBlank)
            {
                items.Insert(0, new SelectListItem()
                                    {
                                        Text = "",
                                        Value = ""
                                    });
            }
            return new SelectList(items, "Value", "Text");
        }

        public static SelectList ToSelectList(
            this IEnumerable<ILookupValue> enumerable,
            bool includeBlank = true)
        {
            var items = enumerable.Select(f => new SelectListItem()
                                                   {
                                                       Text = f.Description,
                                                       Value = f.Id.ToString()
                                                   }).ToList();
            if (includeBlank)
            {
                items.Insert(0, new SelectListItem
                                    {
                                        Text = "",
                                        Value = ""
                                    });
            }
            return new SelectList(items, "Value", "Text");
        }

        public static SelectList ToSelectList<TModel, TProp>(
            this IEnumerable<TModel> enumerable,
            Expression<Func<TModel, TProp>> textField,
            bool includeBlank = true) where TModel : IHasId
        {
            var prop = textField.Compile();

            var items = enumerable.Select(f => new SelectListItem()
                                                   {
                                                       Text = prop(f).ToString(),
                                                       Value = f.Id.ToString() 
                                                   }).ToList();
            if (includeBlank)
            {
                items.Insert(0, new SelectListItem()
                                    {
                                        Text = "",
                                        Value = "0"
                                    });
            }
            return new SelectList(items, "Value", "Text");
        }

        public static SelectList ToSelectList(
            this IEnumerable<int> enumerable,
            bool includeBlank = true)
        {
            var items = enumerable.Select(f => new SelectListItem()
            {
                Text = f.ToString(CultureInfo.InvariantCulture),
                Value = f.ToString(CultureInfo.InvariantCulture),
            }).ToList();
            if (includeBlank)
            {
                items.Insert(0, new SelectListItem()
                                    {
                                        Text = "",
                                        Value = "0"
                                    });
            }
            return new SelectList(items, "Value", "Text");
        }

        public static string ToJavascriptArray<T>(
            this IEnumerable<T> enumerable)
        {
            var builder = new StringBuilder();
            builder.Append("[");
            foreach (var item in enumerable)
            {
                if (builder.Length > 1)
                    builder.Append(",");
                builder.Append("\"" + item.ToString() + "\"");                
            }
            builder.Append("]");

            return builder.ToString();
        }
    }
}