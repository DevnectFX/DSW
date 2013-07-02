using System;
using Nancy.ViewEngines.Razor;


namespace DSW.ViewEngines.Razor
{
	public abstract class DSWRazorViewBase<TModel> : NancyRazorViewBase<TModel>
	{
		public DSWRazorViewBase()
		{
			Console.WriteLine("!!!");
		}

		public override void Initialize(RazorViewEngine engine, Nancy.ViewEngines.IRenderContext renderContext, object model)
		{
			base.Initialize(engine, renderContext, model);
//			this.RenderContext = renderContext;
//			this.Html = new HtmlHelpers<TModel> (engine, renderContext, (TModel)((object)model));
//			this.Model = (TModel)((object)model);
//			this.Url = new UrlHelpers<TModel> (engine, renderContext);
//			this.ViewBag = renderContext.get_Context ().get_ViewBag ();
			Console.WriteLine(Html);
			Console.WriteLine(renderContext.ParsePath("/"));
			//Console.WriteLine(this.ViewBag);
			Console.WriteLine(this.Path);
		}
	}

	public abstract class DSWRazorViewBase : DSWRazorViewBase<object>
	{
	}
}

