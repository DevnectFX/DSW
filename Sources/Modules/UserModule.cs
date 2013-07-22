using System;

using Nancy;

using DSW.Core.Context;


namespace DSW.Modules
{
    public class UserModule : DSWModule
    {
        public UserModule()
            : base("/user")
        {
            Get["/list"] = _ => UserList();
            Get["/regist"] = _ => View["User/UserRegistForm"];
        }

        private dynamic UserList()
        {
            return View["User/UserList"];
        }
    }
}

