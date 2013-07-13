using System;
using Nancy;
using DSW;


namespace DSW.Modules
{
	public class AdminModule : DSWModule
	{
		public AdminModule()
			: base("/admin")
		{
			Get["/"] = _ => {
				return View["Admin/Default"];
			};

			Get["/menu"] = _ => View["Admin/Menu"];
		}
	}
}

