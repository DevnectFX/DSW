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
                if (UserIdentity == null)
                    return null;
                return UserIdentity.UserContext.UserInfo;
            }
        }

        public IMenuContext Menu
        {
            get {
                if (UserIdentity == null)
                    return null;
                return UserIdentity.UserContext.Menu;
            }
        }

        public UserIdentity UserIdentity
        {
            get
            {
                return RenderContext.Context.CurrentUser as UserIdentity;
            }
        }
    }

    public abstract class DSWRazorViewBase : DSWRazorViewBase<object>
    {
    }
}

