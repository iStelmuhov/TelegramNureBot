using System;
using System.Collections.Generic;
using System.Linq;
using Model;
using Repository;
using Microsoft.Practices.Unity;
namespace Service.Impl
{
    public class UserService : IUserService
    {
        [Dependency]
        protected IUserRepository UserRepository { get; set; }

        public T View<T>(long id) where T : User
        {
            return ResolveUser<User>(id) as T;
        }

        public IList<long> ViewAll()
        {
            return UserRepository.SelectAllDomainIds().ToList();
        }

        public User CreateUser(long id, string fName, Role role)
        {
            if (UserRepository.FindByDomainId(id) != null)
                throw new ArgumentException($"User {id} are already exist");

            User user = new User(id, fName, role);
            UserRepository.Add(user);
            return user;
        }

        public User CreateTeacher(long id, string fName, Role role)
        {
            if (UserRepository.FindByDomainId(id) != null)
                throw new ArgumentException($"User {id} are already exist");


            Teacher user = new Teacher(id, fName, role);
            UserRepository.Add(user);
            return user;
        }

        public User CreateStudent(long id, string fName, Role role)
        {
            if (UserRepository.FindByDomainId(id) != null)
                throw new ArgumentException($"User {id} are already exist");

            Student user = new Student(id, fName, role);
            UserRepository.Add(user);
            return user;
        }

        public void ChangeFirstName(long id, string newName)
        {
            User a = ResolveUser<User>(id);
            a.FirstName = newName;
        }

        public void ChangeRole(long id, Role newRole)
        {
            User a = ResolveUser<User>(id);

            UserRepository.Delete(a);

            UserRepository.Commit();

            UserRepository.StartTransaction();
            switch (newRole)
            {
                case Role.Student:
                    CreateStudent(a.Id, a.FirstName, newRole);
                    break;
                case Role.Teacher:
                    CreateTeacher(a.Id, a.FirstName, newRole);
                    break;
                default:
                    CreateUser(a.Id, a.FirstName, newRole);
                    break;
            }
        }
        public void ChangeGroup(long id, int groupId)
        {
            Student a = ResolveUser<Student>(id);
            a.GroupId = groupId;
        }
        public void ChangeTeacherId(long id, int teacherId)
        {
            Teacher a = ResolveUser<Teacher>(id);
            a.TeacherId = teacherId;
        }

        private T ResolveUser<T>(long id) where T : class
        {
            return ServiceUtils.ResolveEntity(UserRepository, id) as T;
        }

        public void RemoveUser(long id)
        {
            User a = ResolveUser<User>(id);
            UserRepository.Delete(a);
        }
    }
}