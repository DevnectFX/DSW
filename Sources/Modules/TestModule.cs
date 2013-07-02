using System;
using Nancy;


namespace DSW.Modules
{
	public class TestModule : NancyModule
	{
		public TestModule()
			: base("/test")
		{
			Get["/"] = _ => {
				return View["Test"];
			};
		}
	}
}

