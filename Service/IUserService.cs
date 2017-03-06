using System.Collections.Generic;
using Model;

namespace Service
{
    public interface IUserService
    {
        T View<T>(long id) where T : User;
        IList<long> ViewAll();
        User CreateUser(long id, string fName, Role role);
        User CreateTeacher(long id, string fName, Role role);
        User CreateStudent(long id, string fName, Role role);
        void ChangeFirstName(long id, string newName);
        void ChangeRole(long id, Role newRole);
        void ChangeGroup(long id, int groupId);
        void ChangeTeacherId(long id, int teacherId);

    }
}