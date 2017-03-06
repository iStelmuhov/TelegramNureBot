using Newtonsoft.Json;

namespace CistNureApi.Model.Api
{
    public class Teacher
    {
        public int Id { get; set; }
        [JsonProperty("full_name")]
        public string FullName { get; set; }
        [JsonProperty("short_name")]
        public string ShortName { get; set; }

        protected bool Equals(Teacher other)
        {
            return Id == other.Id && string.Equals(FullName, other.FullName) && string.Equals(ShortName, other.ShortName);
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
                var hashCode = Id;
                hashCode = (hashCode * 397) ^ (FullName != null ? FullName.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (ShortName != null ? ShortName.GetHashCode() : 0);
                return hashCode;
            }
        }

    }
}