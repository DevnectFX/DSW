using System;
using System.Collections.Generic;

namespace SimpleDataTest
{
    public class UserInfo
    {
        public string UserId { get; set; }
        public string Name { get; set; }
        public string DelYn { get; set; }
        public IList<UserDetailInfo> UserDetailInfo { get; set; }
    }

    public class UserDetailInfo
    {
        public string UserId { get; set; }
        public DateTime BirthDt { get; set; }
    }

    public class UserExtraInfo
    {
        public string UserId { get; set; }
    }

	public class MenuInfo
	{
		public string MenuId { get; set; }
		public string ParentMenuId { get; set; }
		public string MenuTxt { get; set; }
		public string MenuDescTxt { get; set; }
		public int SortOrder { get; set; }
		public string MenuPath { get; set; }
		public string ExtraInfo { get; set; }
		public string UseYn { get; set; }
		public string ChngId { get; set; }
		public DateTime ChngDt { get; set; }
		public string CreateId { get; set; }
		public DateTime CreateDt { get; set; }

		public IList<MenuInfo> ParentMenuInfo { get; set; }
	}

	class MainClass
	{
		public static void Main (string[] args)
		{
			var db = Simple.Data.Database.Open ();

            IEnumerable<UserInfo> result = db.UserInfo.All();
            foreach (var row in result)
            {
                Console.WriteLine(row.UserId + ": " + row.Name + ", " + row.DelYn);
            }

            var result2 = db.UserDetailInfo.All();
            foreach (var row in result2)
            {
                Console.WriteLine(row.UserId + ": " + row.BirthDt);
            }


            var result3 = db.UserInfo.Select(db.UserInfo.UserId, db.UserInfo.UserDetailInfo.BirthDt);
            foreach (var row in result3)
            {
                Console.WriteLine(row.UserId + ": " + row.BirthDt);
            }

            IEnumerable<UserInfo> userInfo = db.UserInfo.WithUserDetailInfo().WithUserExtraInfo();
            foreach (var row in userInfo)
            {
                Console.WriteLine(row.UserId + ": " + row.Name);
				if (row.UserDetailInfo != null)
				{
					foreach (var row2 in row.UserDetailInfo)
					{
						Console.WriteLine ("!" + row2.BirthDt + "!");
					}
				}
            }

            var result4 = db.UserInfo.WithUserDetailInfo();
            foreach (var row in result4)
            {
                Console.WriteLine(row.UserId + ": " + row.Name);
				if (row.UserDetailInfo != null)
				{
					foreach (var row2 in row.UserDetailInfo)
					{
						Console.WriteLine ("!" + row2.BirthDt + "!");
					}
				}
            }


            //db.UserInfo.Insert(UserId: "superdev", Name:"김상린");
            //db.UserDetailInfo.Insert(UserId: "superdev", BirthDt: "1972-01-01");
            //db.UserExtraInfo.Insert(UserId: "spowner");
//			db.userinfo.insert(userid: "test2", name:"테스트2");
//			UserInfo info = db.UserInfo.Get("spowner");
//            info.DelYn = "N";
//            db.UserInfo.Update(info);

//			var menu = new MenuInfo {
//				MenuId = "M1000",
//				MenuTxt = "테스트 대메뉴",
//				UseYn = "Y",
//				CreateId = "spowner",
//				CreateDt = DateTime.Now
//			};
//			var menu = new MenuInfo {
//				MenuId = "M1100",
//				ParentMenuId = "M1000",
//				MenuTxt = "테스트 중메뉴",
//				UseYn = "Y",
//				CreateId = "spowner",
//				CreateDt = DateTime.Now
//			};
//			var menu = new MenuInfo {
//				MenuId = "M1110",
//				ParentMenuId = "M1100",
//				MenuTxt = "테스트 소메뉴",
//				UseYn = "Y",
//				CreateId = "spowner",
//				CreateDt = DateTime.Now
//			};
//			db.MenuInfo.Insert (menu);

			dynamic parentMenuInfo;
			//IEnumerable<MenuInfo> menu = db.MenuInfo.All ().Join (db.MenuInfo.As ("ParentMenuInfo"), out parentMenuInfo).On (db.MenuInfo.ParentMenuId == parentMenuInfo.MenuId).WithOne (parentMenuInfo);
			var menu = db.MenuInfo.All ().Join (db.MenuInfo.As ("ParentMenuInfo"), out parentMenuInfo).On (db.MenuInfo.ParentMenuId == parentMenuInfo.MenuId).WithOne (parentMenuInfo);

			Console.WriteLine (parentMenuInfo);
			foreach (var row in menu) {
				Console.WriteLine (row.MenuId + ": " + row.MenuTxt + ",   " + row.ParentMenuId + ",    " + row.ParentMenuInfo);
			}
		}
	}
}
