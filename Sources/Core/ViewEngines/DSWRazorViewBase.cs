using System;
using Nancy.ViewEngines.Razor;
using Nancy.ViewEngines;
using System.Linq;
using DSW;
using DSW.Extention;


namespace DSW.ViewEngines.Razor
{
	public abstract class DSWRazorViewBase<TModel> : NancyRazorViewBase<TModel>
	{
		public DSWRazorViewBase()
		{
		}

		public override void Initialize(RazorViewEngine engine, Nancy.ViewEngines.IRenderContext renderContext, object model)
		{
			base.Initialize(engine, renderContext, model);

			Layout = GetLayout(renderContext);
		}

		private static string GetLayout(IRenderContext renderContext)
		{
            var viewName = renderContext.Context.NegotiationContext.ViewName;
			var isPopup = viewName.EndsWith("Popup");
			var header = viewName.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
            string result = "/";
            if (header.Length >= 2)
                result = "/" + header[0] + "/" + header[0].ToFirstUpper();
			if (isPopup == true)
				result += "LayoutPopup.cshtml";
			else
				result += "Layout.cshtml";

			return result;
		}
	}

	public abstract class DSWRazorViewBase : DSWRazorViewBase<object>
	{
	}
}

