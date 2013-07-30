using System;
using DSW.Services.Common;
using DSW.Models;
using DSW.Core.Context;
using System.Collections.Generic;

namespace DSW.Context
{
    public class UserContext : IUserContext
    {
        private IMenuContext menuContext;
        private UserService userService;
        private UserInfo userInfo;

        public UserContext(IMenuContext menuContext, UserService userService, UserInfo userInfo)
        {
            this.menuContext = new UserMenuContext(menuContext);
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

        public IMenuContext Menu
        {
            get
            {
                return menuContext;
            }
        }
    }

    public class UserMenuContext : IMenuContext
    {
        private IMenuContext menuContext;

        public UserMenuContext(IMenuContext menuContext)
        {
            this.menuContext = menuContext;
        }

        public IEnumerable<MenuInfo> TopMenuList
        {
            get
            {
                return menuContext.TopMenuList;
            }
        }

        public MenuInfo SelectMenuOrDefault(MenuInfo topMenu)
        {
            return menuContext.SelectMenuOrDefault(topMenu);
        }

        public MenuInfo SelectMenuOrDefault(string menuId)
        {
            return menuContext.SelectMenuOrDefault(menuId);
        }
    }
}

