using System;
using DSW.Core.Context;
using DSW.Context;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Conventions;
using Nancy.Session;

namespace DSW.Utils
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
                UserMapper = container.Resolve<ILoginContext>(),
            };
            FormsAuthentication.Enable(pipelines, formsAuthConfiguration);
        }
        
        protected override void ConfigureApplicationContainer(Nancy.TinyIoc.TinyIoCContainer container)
        {
            base.ConfigureApplicationContainer(container);
            
            container.Register<ILoginContext, LoginContext>();            // 세션 Context
        }

        /// <summary>
        /// TinyIoC 컨테이너를 다른곳에서도 이용하기 위해 GetApplicationContainer 메소드를 대치한다.
        /// </summary>
        /// <returns>The application container.</returns>
        protected override Nancy.TinyIoc.TinyIoCContainer GetApplicationContainer()
        {
            return Nancy.TinyIoc.TinyIoCContainer.Current;
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