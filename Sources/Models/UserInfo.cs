using System;
using System.Collections.Generic;
using Nancy.Security;
using DSW.Core;

namespace DSW.Models
{
    /// <summary>
    /// 사용자 정보
    /// </summary>
    public class UserInfo : Model
    {
        /// <summary>
        /// 사용자 아이디
        /// </summary>
        /// <value>사용자 아이디</value>
        public string UserId { get; set; }
        /// <summary>
        /// 사용자 명
        /// </summary>
        /// <value>사용자 명</value>
        public string Name { get; set; }
        /// <summary>
        /// 삭제 유무
        /// </summary>
        /// <value>삭제 유무</value>
        public string DelYn { get; set; }
        /// <summary>
        /// 그룹 아이디
        /// </summary>
        /// <value>그룹 아이디</value>
        public string GroupId { get; set; }

        /// <summary>
        /// 선택된 메뉴의 메뉴아이디
        /// </summary>
        /// <value>The selected menu identifier.</value>
        public string SelectedMenuId { get; set; }
    }
}

