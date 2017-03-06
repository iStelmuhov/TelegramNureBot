using System;

namespace Model
{
    public class User
    {
        public Guid DatabaseId { get; set; }
        public long   Id        { get; set; }
        public string FirstName { get; set; }
        public Role   Role      { get; set; }

        public User()
        {
        }

        public User(long id, string firstName, Role role)
        {
            Id = id;
            FirstName = firstName;
            Role = role;
        }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(FirstName)}: {FirstName}, {nameof(Role)}: {Role}";
        }

        protected bool Equals(User other)
        {
            return Id == other.Id && string.Equals(FirstName, other.FirstName) && Role == other.Role;
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
                hashCode = (hashCode * 397) ^ (int) Role;
                return hashCode;
            }
        }
    }
}
