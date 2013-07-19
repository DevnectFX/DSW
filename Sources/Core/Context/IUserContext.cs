using System;
using Nancy.Authentication.Forms;
using DSW.Models;
using Nancy;


namespace DSW.Core.Context
{
    /// <summary>
    /// Description of ISessionContext.
    /// </summary>
    public interface IUserContext : IUserMapper
    {
        Guid? Login(string id, string passwd);
        void Logout(NancyContext context);
    }
}
