using System;
using Nancy;
using DSW.Core.Context;
using System.Collections.Generic;

namespace DSW
{
    public abstract class DSWModule : NancyModule
    {
        public IMenuContext MenuContext { get; set; }


        public DSWModule(string modulePath, IMenuContext menuContext)
            : this(modulePath)
        {
            MenuContext = menuContext;
        }

        public DSWModule(string modulePath)
            : base(modulePath)
        {
        }
    }
}

