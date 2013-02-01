using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Logfox.Web.UI.Extensions
{
	public static class DropDownLisExtensions
	{
		public static MvcHtmlString EnumDropDownListFor<TModel, TEnum>(
            this HtmlHelper<TModel> htmlHelper, 
            Expression<Func<TModel, TEnum>> expression,
            object htmlAttributes = null,
            bool ignoreNull = false, 
            bool addDefault = false,
            string defaultText = null)
		{
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);
			Type enumType = GetNonNullableModelType(metadata);
			IEnumerable<TEnum> values = Enum.GetValues(enumType).Cast<TEnum>();

			TypeConverter converter = TypeDescriptor.GetConverter(enumType);

			IEnumerable<SelectListItem> items =
				from value in values
				select new SelectListItem
				{
					Text = value.GetDescription() ?? converter.ConvertToString(value),
					Value = value.ToString(),
					Selected = value.Equals(metadata.Model)
				};		    

			if (metadata.IsNullableValueType && !ignoreNull)
			{
				SelectListItem[] emptyItem = new[] { new SelectListItem { Text = "", Value = "" } };
				items = emptyItem.Concat(items);
			}

            List<SelectListItem> listItems = items.ToList();

            if (addDefault)
            {
                listItems.Insert(0, new SelectListItem {Text = defaultText ?? "Select...", Value = string.Empty});
            }

            return htmlHelper.DropDownListFor(expression, listItems, htmlAttributes);
		}		

		public static MvcHtmlString DropDownListFor<TModel, TProperty>(
			this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, IEnumerable<TProperty>>> expression,
			Expression<Func<TProperty, object>> valueExp,
			Expression<Func<TProperty, object>> textExp,
			object htmlAttributes = null,
            bool addDefault = false,
            string defaultText = null)
		{
			return DropDownListFor<TModel, TProperty, object>(htmlHelper, expression, valueExp, textExp, null, htmlAttributes, addDefault, defaultText);
		}

		public static MvcHtmlString DropDownListFor<TModel, TProperty, TSelected>(
			this HtmlHelper<TModel> htmlHelper,
			Expression<Func<TModel, IEnumerable<TProperty>>> expression,
			Expression<Func<TProperty, object>> valueExp,
			Expression<Func<TProperty, object>> textExp,
			Expression<Func<TModel, TSelected>> selectedExp,
			object htmlAttributes = null,
            bool addDefault = false,
            string defaultText = null)
		{
			Func<TProperty, object> textFunc = textExp.Compile();
			Func<TProperty, object> valueFunc = valueExp.Compile();
			ModelMetadata metadata = ModelMetadata.FromLambdaExpression(expression, htmlHelper.ViewData);

			ModelMetadata model = ModelMetadata.FromLambdaExpression(x => x, htmlHelper.ViewData);
			Func<TModel, TSelected> selectedFunc = null;
			if (selectedExp != null)
			{
				selectedFunc = selectedExp.Compile();
			}

			object value = null;
			if (selectedFunc != null)
			{
				value = selectedFunc((TModel)model.Model);
			}

			IEnumerable<TProperty> objectList = metadata.Model as IEnumerable<TProperty>;

			List<SelectListItem> items = new List<SelectListItem>();
			if (objectList != null)
			{
				foreach (var o in objectList)
				{
                    if(o == null)
                    {
                        continue;
                    }

				    var listItem = new SelectListItem
					{
						Text = textFunc(o).ToString(),
						Value = valueFunc(o).ToString()
					};

					if (value != null && string.Equals(value.ToString(), listItem.Value))
					{
						listItem.Selected = true;
					}

					items.Add(listItem);
				}

                if (addDefault)
                {
                    items.Insert(0, new SelectListItem {Text = defaultText ?? "Select...", Value = string.Empty});
                }
			}

			var name = selectedExp != null 
				? ExpressionHelper.GetExpressionText(selectedExp) 
				: ExpressionHelper.GetExpressionText(expression);

			var attributes = HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes);
			name = (string)(attributes["name"] ?? name);
			return htmlHelper.DropDownList(name, items, null, attributes);
		}

        private static Type GetNonNullableModelType(ModelMetadata modelMetadata)
        {
            Type realModelType = modelMetadata.ModelType;

            Type underlyingType = Nullable.GetUnderlyingType(realModelType);
            if (underlyingType != null)
            {
                realModelType = underlyingType;
            }
            return realModelType;
        }
	}
}