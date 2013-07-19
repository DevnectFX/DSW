using System;
using DSW.Models;

namespace DSW.Services.Common
{
    public class UserService : DSWService
    {
        public UserService()
        {
        }

        public UserInfo GetUserInfo(string id, string passwd)
        {
            return DB.UserInfo.Find(DB.UserInfo.UserCert.UserId == id && DB.UserCert.Passwd == passwd);
        }
    }
}

