namespace Model
{
    public class Teacher:User
    {
        public Teacher(long id, string firstName, Role role) : base(id, firstName, role)
        {
        }

        public int TeacherId { get; set; }

        public Teacher() { }
        public Teacher(int teacherId)
        {
            TeacherId = teacherId;
        }

        public Teacher(long id, string firstName, Role role, int teacherId) 
            : base(id, firstName, role)
        {
            TeacherId = teacherId;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(TeacherId)}: {TeacherId}";
        }

        protected bool Equals(Teacher other)
        {
            return base.Equals(other) && TeacherId == other.TeacherId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Teacher) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (base.GetHashCode() * 397) ^ TeacherId;
            }
        }
    }
}