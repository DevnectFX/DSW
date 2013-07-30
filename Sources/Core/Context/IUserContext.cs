using System;
using DSW.Models;

namespace DSW.Context
{
    public interface IUserContext
    {
        UserInfo UserInfo { get; }
    }
}

