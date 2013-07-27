using System;
using DSW.Core.Context;
using DSW.Services.Common;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

        private IList<MenuInfo> topMenuList = new List<MenuInfo>();
        private object topMenuListLock = new object();


        public MenuContext(MenuService menuService)
        {
            this.menuService = menuService;
        }

        public IEnumerable<MenuInfo> TopMenuList
        {
            get
            {
                DoCache();
                return topMenuList;
            }
        }

        public void DoCache()
        {
            if (isCached == true)
                return;

            lock (topMenuListLock)
            {
                if (isCached == true)
                    return;

                var newTopMenuList = new List<MenuInfo>();
                var result = menuService.getMenuList();
                foreach (var menu in result)
                {
                    MenuInfo m = menu;
                    m.ParentMenu = FindMenu(newTopMenuList, m.ParentMenuId);
                    // 최상위 메뉴면 최상위메뉴목록에 등록한다.
                    if (m.ParentMenu == null)
                        newTopMenuList.Add(m);
                }

                isCached = true;
                topMenuList = newTopMenuList;
            }
        }

        private static MenuInfo FindMenu(IEnumerable<MenuInfo> list, string menuId)
        {
            // 차후 LINQ를 이용해 탐색하는 코드로 변경해보자.
            //return topMenuList.FirstOrDefault(
            //    m => m.MenuId == menuId
            //    );
            foreach (var menu in list)
            {
                if (menu.MenuId == menuId)
                    return menu;

                var result = FindMenu(menu.ChildMenuList, menuId);
                if (result != null)
                    return result;
            }

            return null;
        }

        public void Refresh()
        {
            lock (topMenuListLock)
                isCached = false;
        }
    }
}
