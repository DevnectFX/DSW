using System;

using Nancy;

using DSW.Core.Context;


namespace DSW.Modules
{
    public class UserModule : DSWModule
    {
        public UserModule()
            : base("/")
        {
            Get["/user/regist"] = _ => View["/User/UserRegistForm"];
        }
    }
}

