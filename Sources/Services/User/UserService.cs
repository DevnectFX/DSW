using System;
using System.Collections.Generic;

using DSW.Models;

using Simple.Data;


namespace DSW.User
{
    public class UserService : DSWService
    {
        public UserService()
        {
        }

        public IEnumerable<UserInfo> GetUserList()
        {
            SimpleQuery list = DB.UserInfo.All();

            return list.Cast<UserInfo>();
        }

        public bool RegistUserInfo(UserInfo userInfo)
        {
            var user = DB.UserInfo.Insert(userInfo);

            return true;
        }
    }
}

