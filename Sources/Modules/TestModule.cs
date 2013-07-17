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
			: base("/")
		{
            this.menuContext = menuContext;
            Console.WriteLine(menuContext);
            menuContext.GetId();

			Get["/"] = _ => {
				return View["Test2"];
			};

			Get["/test/test"] = _ => {
				return View["Test/Test"];
			};

			Get["/abc"] = _ => {
				return View["Test2"];
			};
		}
	}
}

