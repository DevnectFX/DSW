using System;
using DSW.Core.Context;

namespace DSW.Services.Common
{
    /// <summary>
    /// Description of MenuService.
    /// </summary>
    public class MenuService : DSWService
    {
        public MenuService()
        {
        }

        public dynamic getMenuList()
        {
            var result = DB.MenuInfo.FindAllByUseYn("Y")
                    .OrderByDepths()
                    .ThenBySortOrder();
            return result;
        }
    }
}
