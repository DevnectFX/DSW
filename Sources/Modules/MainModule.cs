using System;
using Nancy.Responses.Negotiation;
using Nancy.Routing;
using Nancy.Authentication.Forms;
using Nancy.ModelBinding;
using System.Collections.Generic;
using Nancy;
using DSW.Core.Context;

namespace DSW.Modules
{
    public class MainModule : DSWModule
    {
        public MainModule(ILoginContext userContext)
            : base("/")
        {
            Post["/"]        = _ => MainForm();
            Get["/login"]    = _ => LoginForm();
            Post["/login"]    = _ => LoginProcess(userContext);
            Get["/logout"]    = _ => LogoutProcess(userContext);
        }

        private dynamic MainForm()
        {
            return View["MainForm"];
        }

        private dynamic LoginForm()
        {
            var form = Context.Request.Form;
            return View["Login/LoginForm", form];
        }

        private dynamic LoginProcess(ILoginContext userContext)
        {
            var form = Context.Request.Form;
            var id = form.Id;
            var passwd = form.Passwd;
            var isRemember = form.IsRemember.HasValue;

            Guid? guid = userContext.Login(id, passwd);
            if (guid == null)
            {
                form.hasError = true;
                return View["Login/LoginForm", form];
            }

            DateTime? expiry = null;
            if (isRemember = true)
                expiry = DateTime.Now.AddDays(7);

            return this.LoginAndRedirect(guid.Value, expiry);
        }

        private dynamic LogoutProcess(ILoginContext userContext)
        {
            userContext.Logout(Context);
            return this.LogoutAndRedirect("/");
        }
    }
}

