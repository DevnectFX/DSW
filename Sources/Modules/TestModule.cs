using System;
using Nancy;


namespace DSW.Modules
{
	public class TestModule : NancyModule
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
		}
	}
}

