namespace Model
{
    public class Student:User
    {
        public int GroupId { get; set; }

        public Student()
        {
            
        }
        public Student(int groupId)
        {
            GroupId = groupId;
        }

        public Student(long id, string firstName, Role role) : base(id, firstName, role)
        {
        }

        public Student(long id, string firstName, Role role, int groupId) 
            : base(id, firstName, role)
        {
            GroupId = groupId;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(GroupId)}: {GroupId}";
        }

        protected bool Equals(Student other)
        {
            return base.Equals(other) && GroupId == other.GroupId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Student) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ GroupId;
            }
        }
    }
}