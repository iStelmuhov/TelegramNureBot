using System;
using System.Collections.Generic;
using System.Linq;
using TelegramNureBot.Model;
using TelegramNureBot.Repository;

namespace TelegramNureBot.Service.Impl
{
    public class UserService : IUserService
    {
        protected IUserRepository UserRepository { get; set; }

        public UserService(IUserRepository userRepository)
        {
            UserRepository = userRepository;
        }

        public User View(long id)
        {
            return ResolveUser(id);
        }

        public IList<long> ViewAll()
        {
            return UserRepository.SelectAllDomainIds().ToList();
        }

        public User CreateUser(long id, string fName, Role role, string group = "")
        {
            if (UserRepository.FindByDomainId(id) != null)
                throw new ArgumentException($"User {id} are already exist");

            UserRepository.StartTransaction();

            User user = new User(id, fName, role, group);
            UserRepository.Add(user);
            UserRepository.Commit();
            return user;
        }


        public void ChangeFirstName(long id, string newName)
        {
            UserRepository.StartTransaction();
            User a = ResolveUser(id);
            a.FirstName = newName;
            UserRepository.Commit();
        }

        public void ChangeRole(long id, Role newRole)
        {
            UserRepository.StartTransaction();
            User a = ResolveUser(id);
            a.Role = newRole;
            UserRepository.Commit();
        }
        public void ChangeGroup(long id, string groupName)
        {
            UserRepository.StartTransaction();
            User a = ResolveUser(id);
            a.Group = groupName;
            UserRepository.Commit();
        }

        private User ResolveUser(long id)
        {
            return ServiceUtils.ResolveEntity(UserRepository, id);
        }


    }
}