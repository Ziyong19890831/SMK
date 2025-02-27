using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace SMK.Web.Heppers
{
    public static class HtmlHelperExtension
    {
        #region tag

        public static TagBuilder CreateInput(this IHtmlHelper @this, object attributes = null)
        {
            TagBuilder button = new TagBuilder("input");
            if (attributes == null)
            {
                button.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(new
                {
                    type = "text"
                }));
            }
            else
            {
                button.MergeAttributes(HtmlHelper.AnonymousObjectToHtmlAttributes(attributes));
            }

            return button;
        }

        public static IHtmlContent DropDownListBoolFor<TModel, TResult>(
            this IHtmlHelper<TModel> @this,
            Expression<Func<TModel, TResult>> expression,
            object attributes = null
            )
        {
            var options = new List<SelectListItem>() {
                new SelectListItem(){
                    Text = "是",
                    Value = "true"
                },
                new SelectListItem(){
                    Text = "否",
                    Value = "false"
                }
            };

            return @this.DropDownListFor(expression, options, attributes);
        }

        public static IHtmlContent DropDownListBool<TModel>(
            this IHtmlHelper<TModel> @this,
            string name,
            bool value = false,
            object attributes = null
            )
        {
            value = !value;

            var options = new List<SelectListItem>() {
                new SelectListItem(){
                    Text = "是",
                    Value = "true",
                    Selected = value
                },
                new SelectListItem(){
                    Text = "否",
                    Value = "false",
                    Selected = value
                }
            };

            return @this.DropDownList(name, options, attributes);
        }

        #endregion tag

        public static string GetControllerName<T>(this IHtmlHelper @this) where T : Controller
        {
            return nameof(T).Replace("Controller", "");
        }

        public static string GetCurrentControllerName(this IHtmlHelper @this)
        {
            return @this.ViewContext.RouteData.Values["controller"].ToString();
        }

        public static string GetCurrentActionName(this IHtmlHelper @this)
        {
            return @this.ViewContext.RouteData.Values["action"].ToString();
        }

        public static IHtmlContent MyDropDownList<TModel>(
            this IHtmlHelper @this,
            string name,
            IEnumerable<TModel> source,
            Func<TModel, SelectListItem> mapper,
            object htmlAttributes,
            bool addEmptyOption = false
            )
        {
            var list = new List<SelectListItem>();

            if (addEmptyOption)
            {
                list.Add(new SelectListItem()
                {
                    Value = "",
                    Text = "--請選擇--"
                });
            }

            foreach (var m in source)
            {
                list.Add(mapper(m));
            }

            return @this.DropDownList(name, list, htmlAttributes);
        }

        private static JsonSerializerSettings serializeOption = new JsonSerializerSettings()
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public static HtmlString ToJson(this IHtmlHelper @this, object data)
        {
            return new HtmlString(JsonConvert.SerializeObject(data, serializeOption));
        }


    }
}