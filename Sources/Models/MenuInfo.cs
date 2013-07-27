using System;
using System.Collections.Generic;
using DSW.Core;

namespace DSW.Models
{
    public class MenuInfo : Model
    {
        /// <summary>
        /// 메뉴아이디
        /// </summary>
        /// <value>The menu identifier.</value>
        public string MenuId { get; set; }
        /// <summary>
        /// 상위메뉴아이디
        /// </summary>
        /// <value>The parent menu identifier.</value>
        public string ParentMenuId { get; set; }
        /// <summary>
        /// 메뉴명
        /// </summary>
        /// <value>The menu text.</value>
        public string MenuTxt { get; set; }
        /// <summary>
        /// 메뉴 설명
        /// </summary>
        /// <value>The menu desc text.</value>
        public string MenuDescTxt { get; set; }
        /// <summary>
        /// 정렬순서
        /// </summary>
        /// <value>The sort order.</value>
        public int SortOrder { get; set; }
        /// <summary>
        /// 깊이
        /// </summary>
        /// <value>The depths.</value>
        public int Depths { get; set; }
        /// <summary>
        /// 메뉴경로
        /// </summary>
        /// <value>The menu path.</value>
        public string MenuPath { get; set; }
        /// <summary>
        /// 추가정보
        /// </summary>
        /// <value>The extra info.</value>
        public string ExtraInfo { get; set; }
        /// <summary>
        /// 사용유무
        /// </summary>
        /// <value>The use yn.</value>
        public string UseYn { get; set; }
        /// <summary>
        /// 마지막메뉴인지의 유무
        /// </summary>
        /// <value><c>true</c> if this instance is leaf; otherwise, <c>false</c>.</value>
        public bool IsLeaf
        {
            get
            {
                return childMenuList.Count == 0 ? true : false;
            }
        }

        private MenuInfo parentMenu;
        /// <summary>
        /// 상위메뉴
        /// </summary>
        /// <value>The parent menu.</value>
        public MenuInfo ParentMenu
        {
            get
            {
                return parentMenu;
            }

            set
            {
                // 이미 부모가 같으면 무시
                if (parentMenu == value)
                    return;

                // null값이면 부모였던 메뉴에서 제거
                if (value == null)
                {
                    parentMenu.childMenuList.Remove(this);
                    parentMenu = null;
                    return;
                }

                // 이전 부모 메뉴와의 관계 정리
                if (parentMenu != null)
                    ParentMenu = null;

                parentMenu = value;
                parentMenu.childMenuList.Add(this);
            }
        }

        private IList<MenuInfo> childMenuList = new List<MenuInfo>();
        /// <summary>
        /// 자식메뉴목록
        /// </summary>
        /// <value>The child menu.</value>
        public IEnumerable<MenuInfo> ChildMenuList
        {
            get
            {
                return childMenuList;
            }
        }
    }
}

