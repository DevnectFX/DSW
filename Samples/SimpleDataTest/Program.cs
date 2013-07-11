using System;
using System.Collections.Generic;

namespace SimpleDataTest
{
    class UserInfo
    {
        public string UserId { get; set; }
        public string Name { get; set; }
    }


	class MainClass
	{
		public static void Main (string[] args)
		{
			var db = Simple.Data.Database.Open ();

            IEnumerable<UserInfo> result = db.UserInfo.All();
            foreach (var row in result)
            {
                Console.WriteLine(row.UserId + ": " + row.Name);
            }

            var result2 = db.UserDetailInfo.All();
            foreach (var row in result2)
            {
                Console.WriteLine(row.UserId + ": " + row.BirthDt);
            }


            var result3 = db.UserInfo.Select(db.UserInfo.UserDetailInfo.BirthDt);
            foreach (var row in result3)
            {
                Console.WriteLine(row.UserId + ": " + row.BirthDt);
            }


            //db.UserInfo.Insert(UserId: "spowner", Name:"¡§ºº¿œ");
            //db.UserDetailInfo.Insert(UserId: "spowner", BirthDt: "1978-08-23");
		}
	}
}
