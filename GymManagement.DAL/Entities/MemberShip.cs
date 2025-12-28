namespace GymManagement.DAL.Entities
{
    public class MemberShip : BaseEntity
    {
        public DateTime EndDate { get; set; }

        public string Status
        {
            get
            {
                if (EndDate < DateTime.Now)
                    return "Expired";
                else
                    return "Active";
            }
        }

        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        public int PlanId { get; set; }
        public Plan Plan { get; set; } = null!;
    }
}
