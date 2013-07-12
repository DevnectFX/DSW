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
		}
	}
}
