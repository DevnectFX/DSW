/*
 * SharpDevelop으로 작성되었습니다.
 * 사용자: spowner
 * 날짜: 2013-07-16
 * 시간: 오후 4:22
 * 
 * 이 템플리트를 변경하려면 [도구->옵션->코드 작성->표준 헤더 편집]을 이용하십시오.
 */
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
            DB.MenuInfo.FindAllBy(UseYn: "Y").OrderBy(DB.MenuInfo.
        }
    }
}
