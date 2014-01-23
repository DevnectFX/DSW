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
    /// Description of LoginContext.
    /// </summary>
    public class LoginContext : ILoginContext
    {
        private IMenuContext menuContext;
        private UserService userService;

        private Dictionary<Guid, UserIdentity> map = new Dictionary<Guid, UserIdentity>();
        private ReaderWriterLockSlim mapLock = new ReaderWriterLockSlim();


        public LoginContext(IMenuContext menuContext, UserService userService)
        {
            this.menuContext = menuContext;
            this.userService = userService;
        }
        
        public IUserIdentity GetUserFromIdentifier(Guid identifier, Nancy.NancyContext context)
        {
            mapLock.EnterReadLock();
            try
            {
                var result = map.ContainsKey(identifier);
                if (result == false)
                    return null;

                return map[identifier];
            }
            finally
            {
                mapLock.ExitReadLock();
            }
        }

        #region IUserContext implementation

        public Guid? Login(string id, string passwd)
        {
            // RentalFarm 개발로 인해 임시로 막아둠.
            //var userInfo = userService.GetUserInfo(id, passwd);
            UserInfo userInfo = null;
            if (id == "admin" && passwd == "impacsys")
                userInfo = GetAdminInfo();

            if (userInfo == null)
                return null;

            var guid = Guid.NewGuid();
            var newUserService = new UserService(); // TODO: UserService의 생성인자가 변경되면 수정하여야 함
            var userIdentity = new UserIdentity(guid, menuContext, newUserService, userInfo);

            mapLock.EnterWriteLock();
            try
            {
                map[guid] = userIdentity;
            }
            finally
            {
                mapLock.ExitWriteLock();
            }

            return guid;
        }

        public void Logout(NancyContext context)
        {
            var userIdentity = context.CurrentUser as UserIdentity;
            if (userIdentity == null)
                return;

            var guid = userIdentity.Identifier;

            mapLock.EnterWriteLock();
            try
            {
                map.Remove(guid);
            }
            finally
            {
                mapLock.ExitWriteLock();
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
        public Guid Identifier { get; private set; }
        public string UserName
        {
            get
            {
                return userName;
            }
        }
        public UserContext UserContext { get; set; }

        public UserIdentity(Guid identifier, IMenuContext menuContext, UserService userService, UserInfo userInfo)
        {
            this.Identifier = identifier;
            this.UserContext = new UserContext(menuContext, userService, userInfo);
            this.userName = userInfo.UserId;
        }
    }
}
