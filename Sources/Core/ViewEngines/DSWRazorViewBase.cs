using System;
using Nancy.ViewEngines.Razor;
using Nancy.ViewEngines;
using System.Linq;
using DSW;
using DSW.Extention;
using System.Collections.Generic;
using Nancy;
using DSW.Context;
using DSW.Models;
using DSW.Core.Context;
using Nancy.TinyIoc;


namespace DSW.ViewEngines.Razor
{
    public abstract class DSWRazorViewBase<TModel> : NancyRazorViewBase<TModel>
    {
        private NancyContext context;
        private IMenuContext menu;

        public DSWRazorViewBase()
        {
        }

        public override void Initialize(RazorViewEngine engine, Nancy.ViewEngines.IRenderContext renderContext, object model)
        {
            try
            {
                menu = TinyIoCContainer.Current.Resolve<IMenuContext>();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
            }

            base.Initialize(engine, renderContext, model);

            context = renderContext.Context;
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
                result += "PopupLayout.cshtml";
            else
                result += "Layout.cshtml";

            return result;
        }

        public new void ExecuteView(string body, IDictionary<string, string> sectionContents)
        {
            base.ExecuteView(body, sectionContents);

            if (string.IsNullOrEmpty(body) == true)
                Layout = GetLayout(RenderContext);
        }

        public UserInfo User
        {
            get {
                var userIndentity = context.CurrentUser as UserIdentity;
                if (userIndentity == null)
                    return null;
                return userIndentity.UserInfo;
            }
        }

        public IMenuContext Menu
        {
            get {
                return menu;
            }
        }
    }

    public abstract class DSWRazorViewBase : DSWRazorViewBase<object>
    {
    }
}

