using System;
using Nancy;
using DSW;


namespace DSW.Modules
{
	public class TestModule : DSWModule
	{
		public TestModule()
			: base("/")
		{
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

