using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

using Nancy;

using Simple.Data;

using IBatisNet.Common;
using IBatisNet.DataMapper;


namespace SimpleDataTest.Module
{
    public class TestSimpleData : NancyModule
    {
        public TestSimpleData()
        {
            Get["/All/"] = context =>
            {
                try
                {
                    string retValue = "";
                    var fileDb = Database.OpenFile(@"Database/test.db");
                    var users = fileDb.USER.All();

                    int no = 0;
                    foreach (var user in users)
                    {
                        retValue += string.Format("{0} | ID : {1} <br />", ++no, user.ID);
                    }

                    return retValue;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }

                return "Fuck";
            };

            Get["/Insert/"] = context =>
            {
                string retValue = "";
                var fileDb = Database.OpenFile(@"Database/test.db");

                for (int i = 0; i < 10; i++)
                {
                    fileDb.USER.Insert(new User { ID = DateTime.Now.Ticks.ToString(), PASSWORD = "1234" });
                    Thread.Sleep(1000);
                }

                var users = fileDb.USER.All();

                int no = 0;
                foreach (var user in users)
                {
                    retValue += string.Format("{0} | ID : {1} <br />", ++no, user.ID);
                }

                return retValue;
            };

            Get["/FindAll/"] = context =>
            {
                try
                {
                    string retValue = "";
                    var fileDb = Database.OpenFile(@"Database/test.db");
                    var users = fileDb.USER.FindAll(fileDb.USER.ID == "superdev");

                    int no = 0;
                    foreach (var user in users)
                    {
                        retValue += string.Format("{0} | ID : {1} <br />", ++no, user.ID);
                    }

                    return retValue;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }

                return "Fuck";
            };

            Get["/FindAllBy/"] = context =>
            {
                try
                {
                    string retValue = "";
                    var fileDb = Database.OpenFile(@"Database/test.db");
                    var users = fileDb.USER.FindAllBy(ID:"superdev");

                    int no = 0;
                    foreach (var user in users)
                    {
                        retValue += string.Format("{0} | ID : {1} <br />", ++no, user.ID);
                    }

                    return retValue;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }

                return "Fuck";
            };

            Get["/Find/"] = context =>
            {
                try
                {
                    var fileDb = Database.OpenFile(@"Database/test.db");
                    var user = fileDb.USER.Find(fileDb.USER.ID == "superdev");

                    return string.Format("ID : {0} <br />", user.ID);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }

                return "Fuck";
            };

            Get["/Get/"] = context =>
            {
                try
                {
                    var fileDb = Database.OpenFile(@"Database/test.db");
                    var user = fileDb.USER.Get("superdev");

                    return string.Format("ID : {0} <br />", user.ID);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.StackTrace);
                }

                return "Fuck";
            };

            Get["/mybatis/"] = context =>
            {
                string retValue = "";
                try
                {
                    IList<User> list = Mapper.Instance().QueryForList<User>("SelectUser", null);

                    foreach (var user in list)
                    {
                        retValue += string.Format("ID : {0} <br />", user.ID);
                    }
                }
                catch (Exception ex)
                {
                    return ex.StackTrace;
                }

                return retValue;
            };     

            Get["/"] = context =>
            {
                var USER = new User() { ID = "test", PASSWORD = "1234" };
                return View["View/TestView", USER];
            };            
        }
    }
}
