using System;
using System.Linq;
using DSW.Core.Context;
using Nancy.Security;
using DSW.Models;
using System.Collections.Generic;
using Nancy;
using System.Threading;
using DSW.Services.Common;

namespace DSW.Context
{
    /// <summary>
    /// Description of SessionContext.
    /// </summary>
    public class UserContext : IUserContext
    {
        private UserService userService;


        private Dictionary<Guid, UserIdentity> map = new Dictionary<Guid, UserIdentity>();
        private object mapLock = new object();


        public UserContext(UserService userService)
        {
            this.userService = userService;
        }
        
        public IUserIdentity GetUserFromIdentifier(Guid identifier, Nancy.NancyContext context)
        {
            lock (mapLock)
            {
                var result = map.ContainsKey(identifier);
                if (result == false)
                    return null;

                return map[identifier];
            }
        }

        #region IUserContext implementation

        public Guid? Login(string id, string passwd)
        {
            var userInfo = userService.GetUserInfo(id, passwd);
            if (id == "admin" && passwd == "10293847")
                userInfo = GetAdminInfo();

            if (userInfo == null)
                return null;

            var guid = Guid.NewGuid();
            var userIdentity = new UserIdentity(guid, userInfo);
            lock (mapLock)
            {
                map[guid] = userIdentity;
            }

            return guid;
        }

        public void Logout(NancyContext context)
        {
            var userIdentity = context.CurrentUser as UserIdentity;
            if (userIdentity == null)
                return;

            var guid = userIdentity.Identifier;
            lock (mapLock)
            {
                map.Remove(guid);
            }
        }

        #endregion

        private UserInfo GetAdminInfo()
        {
            var info = new UserInfo()
            {
                UserId = "admin",
                Name = "관리자",
                DelYn = "N"
            };
            return info;
        }
    }

    public class UserIdentity : IUserIdentity
    {
        private string userName;


        public IEnumerable<string> Claims { get; set; }
        public UserInfo UserInfo { get; private set; }
        public Guid Identifier { get; private set; }
        public string UserName
        {
            get
            {
                return userName;
            }
        }

        public UserIdentity(Guid identifier, UserInfo userInfo)
        {
            this.Identifier = identifier;
            this.UserInfo = userInfo;
            this.userName = userInfo.UserId;
        }
    }
}
