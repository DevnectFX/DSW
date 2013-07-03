using System;
using Nancy.ViewEngines.Razor;
using Nancy.ViewEngines;
using System.Linq;
using DSW.Model;
using DSW.Core;


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
			var path = renderContext.Context.Request.Path;
			Console.WriteLine(path);
			var header = path.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);
			Console.WriteLine(header.Length);
			string result = "/";
			if (header.Length > 0)
				result = "/" + header[1] + "/" + header[1].ToFirstUpper();
			result += "Layout.cshtml";
			Console.WriteLine(result);
			return result;
		}
	}

	public abstract class DSWRazorViewBase : DSWRazorViewBase<object>
	{
	}
}

