using System;
using System.Collections.Generic;

using Nancy;
using Nancy.ModelBinding;

using DSW.Core.Context;
using DSW.Context;


namespace DSW
{
    public abstract class DSWModule : NancyModule
    {
        private IMenuContext menuContext;

        protected DSWModule(string modulePath)
            : base(modulePath)
        {
        }

        private void InjectContext<TContext>(ref TContext context)
            where TContext : class, IContext
        {
            if (context == null)
            {
                lock (typeof(TContext))
                {
                    if (context == null)
                        context = Nancy.TinyIoc.TinyIoCContainer.Current.Resolve<TContext>();
                }
            }
         }

        public IMenuContext MenuContext
        {
            get
            {
                InjectContext<IMenuContext>(ref menuContext);
                return menuContext;
            }
        }
    }
}

