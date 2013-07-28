using System;
using System.Collections.Generic;

using Nancy;
using Nancy.ModelBinding;

using DSW.Core.Context;
using DSW.Models;
using DSW.User;


namespace DSW.Modules
{
    public class UserModule : DSWModule
    {
        private UserService service;


        public UserModule()
            : base("/user")
        {
            service = new UserService();

            Get["/list"] = _ => UserList();
            Post["/list"] = _ => GetUserList();
            Get["/regist"] = _ => View["User/UserRegistForm"];
            Post["/regist"] = _ => RegistUserInfo();
        }

        private dynamic UserList()
        {
            var uList = new List<UserInfo>();
            uList.Add(new UserInfo() { Name = "ABC", UserId = "123", GroupId = "DEPT1", DelYn = "Y" });
            uList.Add(new UserInfo() { Name = "DEF", UserId = "567", GroupId = "DEPT2", DelYn = "Y" });

            this.BindTo(uList);

            return View["User/UserList"];
        }

        private dynamic GetUserList()
        {
            var uList = service.GetUserList();

            foreach (var item in uList)
            {
                Console.WriteLine(item.Name);
            }

            this.BindTo(uList);

            return Response.AsJson(uList, HttpStatusCode.OK);
        }

        private dynamic RegistUserInfo()
        {
            var form = Context.Request.Form;
            var registed = service.RegistUserInfo(new UserInfo() { UserId = form.userId, Name = form.name, DelYn = "N", GroupId = null });

            return null;
        }
    }
}

