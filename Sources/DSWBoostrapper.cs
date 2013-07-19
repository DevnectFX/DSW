using System;
using DSW.Core.Context;
using DSW.Context;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Conventions;
using Nancy.Session;

namespace DSW.Sources
{
    public class DSWBoostrapper : DefaultNancyBootstrapper
    {
        protected override void ConfigureConventions(Nancy.Conventions.NancyConventions nancyConventions)
        {
            base.ConfigureConventions(nancyConventions);

            // 정적 컨텐츠에 접근 가능하도록 함
            Conventions.StaticContentsConventions.Add(
                StaticContentConventionBuilder.AddDirectory("", "WebContents", new string[] { "css", "js", "png", "gif", "jpg" })
            );
        }
        
        protected override void ApplicationStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines)
        {
            base.ApplicationStartup(container, pipelines);
            
            CookieBasedSessions.Enable(pipelines);

            var formsAuthConfiguration =
                new FormsAuthenticationConfiguration()
            {
                RedirectUrl = "~/login",
                UserMapper = container.Resolve<IUserContext>(),
            };
            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
        }
        
        protected override void ConfigureApplicationContainer(Nancy.TinyIoc.TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            
            container.Register<IUserContext, UserContext>();            // 세션 Context
        }
        
        protected override void ConfigureRequestContainer(Nancy.TinyIoc.TinyIoCContainer container, NancyContext context)
        {
            base.ConfigureRequestContainer(container, context);
        }
        
        protected override Nancy.Diagnostics.DiagnosticsConfiguration DiagnosticsConfiguration {
            get { return new Nancy.Diagnostics.DiagnosticsConfiguration { Password = @"10293847" }; }
        }
        
        protected override void RequestStartup(Nancy.TinyIoc.TinyIoCContainer container, Nancy.Bootstrapper.IPipelines pipelines, NancyContext context)
        {
            base.RequestStartup(container, pipelines, context);
        }
    }
}