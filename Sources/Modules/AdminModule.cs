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
				return View["admin/Default"];
			};
			Get["/popup"] = _ => View["admin/DefaultPopup"];
		}
	}
}

