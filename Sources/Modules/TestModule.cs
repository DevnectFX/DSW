using System;
using Nancy;
using DSW;
using DSW.Core.Context;


namespace DSW.Modules
{
    public class TestModule : DSWModule
    {
        private IMenuContext menuContext;


        public TestModule(IMenuContext menuContext)
            : base("/test")
        {
            this.menuContext = menuContext;

            Get["/"] = _ => {
                return View["Test2"];
            };

            Get["/test/test"] = _ => {
                return View["Test/Test"];
            };

            Get["/abc"] = _ => {
                return View["Test2"];
            };

            Get["/ua"] = _ => {
                Console.WriteLine(Request.Headers.UserAgent);
                return Request.Headers.UserAgent; };
        }
    }
}

