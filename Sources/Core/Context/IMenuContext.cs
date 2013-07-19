using System;
using System.Collections.Generic;
using DSW.Models;


namespace DSW.Core.Context
{
    /// <summary>
    /// Description of IMenuContext.
    /// </summary>
    public interface IMenuContext
    {
        IEnumerable<MenuInfo> TopMenuList { get; }
    }
}
