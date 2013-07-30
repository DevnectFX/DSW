using System;
using System.Linq;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Responses.Negotiation;
using Nancy.Security;
using DSW;
using DSW.Core.Context;
using Nancy.TinyIoc;
using DSW.Services.Common;
using DSW.Models;
using System.Globalization;

namespace DSW.Modules
{
    public class AdminModule : DSWModule
    {
        public AdminModule()
            : base("/admin")
        {
            this.RequiresAuthentication();                        // 인증을 해야 접근 가능하도록 등록
            
            Get["/"] = _ => GoMenu();
        }

        private dynamic GoMenu()
        {
            var menuId = Request.Form["MenuId"];
            MenuInfo menu;
            if (string.IsNullOrEmpty(menuId) == true)
            {
                menu = MenuContext.TopMenuList.FirstOrDefault();
                if (menu == null)
                    return null;
                menuId = menu.MenuId;
            }

            menu = MenuContext.SelectMenuOrDefault(menuId);
            Console.WriteLine(menu);
            string viewName = menu.MenuPath.Substring(1);

            return View[viewName];
        }
        
        private dynamic Menu()
        {
            var model = "!" + MenuContext + ", " ; //menu.GetTopMenuList();
            
            return View["Admin/Menu", model];
        }
    }
}

