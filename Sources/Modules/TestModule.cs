using System;
using Nancy;


namespace DSW.Modules
{
	public class TestModule : NancyModule
	{
		public TestModule()
		{
			Get["/"] = _ => {
				return View["Test"];
			};
		}
	}
}

