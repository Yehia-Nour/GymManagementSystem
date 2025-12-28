using GymManagement.DAL.Entities.Enums;

namespace GymManagement.DAL.Entities
{
    public class Trainer : GymUser
    {
        public Specialties Specialties { get; set; }

        public ICollection<Session> Sessions { get; set; } = null!;
    }
}
