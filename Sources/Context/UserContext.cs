using System;
using DSW.Services.Common;
using DSW.Models;

namespace DSW.Context
{
    public class UserContext : IUserContext
    {
        private UserService userService;
        private UserInfo userInfo;

        public UserContext(UserService userService, UserInfo userInfo)
        {
            this.userService = userService;
            this.userInfo = userInfo;
        }

        public DSW.Models.UserInfo UserInfo
        {
            get
            {
                return userInfo;
            }
        }
    }
}

