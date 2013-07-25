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
        // 싱글톤 인스턴스이기 때문에 한번만 Resolve한 후 재사용하도록 한다.
        private static IMenuContext menu = TinyIoCContainer.Current.Resolve<IMenuContext>();

        public DSWRazorViewBase()
        {
        }

        public override void Initialize(RazorViewEngine engine, Nancy.ViewEngines.IRenderContext renderContext, object model)
        {
            base.Initialize(engine, renderContext, model);
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
                var userIndentity = RenderContext.Context.CurrentUser as UserIdentity;
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

