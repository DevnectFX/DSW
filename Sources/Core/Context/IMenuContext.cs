using System;
using System.Collections.Generic;
using DSW.Models;
using DSW.Context;


namespace DSW.Core.Context
{
    /// <summary>
    /// Description of IMenuContext.
    /// </summary>
    public interface IMenuContext : IContext
    {
        IEnumerable<MenuInfo> TopMenuList { get; }
        MenuInfo SelectMenuOrDefault(MenuInfo topMenu);
        MenuInfo SelectMenuOrDefault(string menuId);
    }
}
