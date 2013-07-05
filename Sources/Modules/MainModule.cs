using System;
using Nancy;
using Nancy.Bootstrapper;
using Nancy.Routing;



namespace DSW.Modules
{
	public class MainModule : NancyModule
	{
		public MainModule()
		{
			Post["/Main.do"] = _ => {
				new RouteBuilder(
			};
		}
	}
}

