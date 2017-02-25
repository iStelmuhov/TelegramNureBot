using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NureBot.Model
{
    public class User
    {
        public Guid DatabaseId { get; set; }
        public long   Id        { get; set; }
        public string FirstName { get; set; }
        public Role   Role      { get; set; }
        public string Group     { get; set; }

        public User(long id, string firstName,Role role, string group = "")
        {
            Id = id;
            FirstName = firstName;
            Role = role;
            Group = group;
        }

        public User(long id, string firstName)
        {
            Id = id;
            FirstName = firstName;
        }

        protected User() { }

        protected bool Equals(User other)
        {
            return Id == other.Id && string.Equals(FirstName, other.FirstName) && Role == other.Role && string.Equals(Group, other.Group);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((User) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Id.GetHashCode();
                hashCode = (hashCode * 397) ^ (FirstName != null ? FirstName.GetHashCode() : 0);
                return hashCode;
            }
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(FirstName)}: {FirstName},{nameof(Group)}: {Group}";
        }
    }
}
