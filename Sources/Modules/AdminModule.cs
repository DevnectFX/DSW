using System;
using Nancy;
using Nancy.Authentication.Forms;
using Nancy.Responses.Negotiation;
using Nancy.Security;
using DSW;
using DSW.Core.Context;
using Nancy.TinyIoc;
using DSW.Services.Common;

namespace DSW.Modules
{
    public class AdminModule : DSWModule
    {    
        public AdminModule()
            : base("/admin")
        {
            this.RequiresAuthentication();                        // 인증을 해야 접근 가능하도록 등록
            
            Get["/"]        = _ => View["Admin/Default"];
            Get["/menu"]    = _ => Menu();
        }
        
        private dynamic Menu()
        {
            var model = "!" + MenuContext + ", " ; //menu.GetTopMenuList();
            
            return View["Admin/Menu", model];
        }
    }
}

