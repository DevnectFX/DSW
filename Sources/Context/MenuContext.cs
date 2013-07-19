using System;
using DSW.Core.Context;
using DSW.Services.Common;
using System.Collections;
using System.Collections.Generic;
using DSW.Models;


namespace DSW.Context
{
    /// <summary>
    /// Description of MenuContext.
    /// </summary>
    public class MenuContext : IMenuContext
    {
        private MenuService menuService;

        private bool isCached;

        private IList<MenuInfo> menuList = new List<MenuInfo>();


        public MenuContext(MenuService menuService)
        {
            this.menuService = menuService;
        }

        public IEnumerable<DSW.Models.MenuInfo> TopMenuList
        {
            get
            {
                if (isCached == false)
                    Refresh();

                return null;
            }
        }

        public void Refresh()
        {
            menuList.Clear();
            menuService.

            isCached = true;
        }
    }
}
