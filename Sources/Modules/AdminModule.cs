using System;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Responses.Negotiation;
using Nancy.Security;
using DSW;
using DSW.Core.Context;
 using DSW.Services.Admin;
using Nancy.TinyIoc;

namespace DSW.Modules
{
	public class AdminModule : DSWModule
	{
		private MenuService menu;
		
				
		public AdminModule(IMenuContext menuContext, 
		                   MenuService menu)
			: base("/admin", menuContext)
		{
			this.menu = menu;
			//this.RequiresAuthentication();						// 인증을 해야 접근 가능하도록 등록
            menuContext.GetId();
			
			Get["/"]		= _ => View["Admin/Default"];
			Get["/menu"]	= _ => Menu();
		}
		
		private Negotiator Menu()
		{
			var model = "!" + menu + ", " + MenuContext + ", " + menu.MenuContext; //menu.GetTopMenuList();
			
			return View["Admin/Menu", model];
		}
	}
}

