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
        private IDictionary<string, MenuInfo> menuMap = new Dictionary<string, MenuInfo>();
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
                var newMenuMap = new Dictionary<string, MenuInfo>();
                var result = menuService.getMenuList();
                foreach (var menu in result)
                {
                    MenuInfo m = menu;
                    m.ParentMenu = FindMenu(newTopMenuList, m.ParentMenuId);
                    // 최상위 메뉴면 최상위메뉴목록에 등록한다.
                    if (m.ParentMenu == null)
                        newTopMenuList.Add(m);

                    newMenuMap[m.MenuId] = m;
                }

                isCached = true;
                topMenuList = newTopMenuList;
                menuMap = newMenuMap;
            }
        }

        private static MenuInfo FindMenu(IEnumerable<MenuInfo> list, string menuId)
        {
            // 차후 LINQ를 이용해 탐색하는 코드로 변경해보자.
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

        public MenuInfo FindMenu(string menuId)
        {
            MenuInfo menu;
            var result = menuMap.TryGetValue(menuId, out menu);
            if (result == false)
                return null;

            return menu;
        }

        /// <summary>
        /// 상위 메뉴를 선택했을 때, 메뉴경로가 없으면 하위 메뉴 중 기본 메뉴를 선택해 반환한다.
        /// </summary>
        /// <returns></returns>
        /// <param name="topMenu">상위메뉴</param>
        public MenuInfo SelectMenuOrDefault(MenuInfo topMenu)
        {
            var selMenu = topMenu;
            if (string.IsNullOrWhiteSpace(selMenu.MenuPath) == false)
            {
                return selMenu;
            }
            foreach (var menu in topMenu.ChildMenuList)
            {
                selMenu = SelectMenuOrDefault(menu);
                if (selMenu != menu)
                    return selMenu;
            }

            // 아무것도 선택할 수 없으므로 상위메뉴를 그대로 반환한다.
            return topMenu;
        }

        public MenuInfo SelectMenuOrDefault(string menuId)
        {
            var menu = FindMenu(menuId);
            if (menu == null)
                return null;
            return SelectMenuOrDefault(menu);
        }

        public void Refresh()
        {
            lock (topMenuListLock)
                isCached = false;
        }
    }
}
