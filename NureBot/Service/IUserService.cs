using System;
using System.Collections.Generic;
using NureBot.Model;

namespace NureBot.Service
{
    public interface IUserService
    {
        User View(long id);
        IList<long> ViewAll();
        User CreateUser(long id,string fName,Role role,string group="");
        void ChangeFirstName(long id, string newName);
        void ChangeRole(long id, Role newRole);
        void ChangeGroup(long id, string groupName);

    }
}