namespace GymManagement.BLL.ViewModels.MembershipViewModels
{
    public class MembershipViewModel
    {
        public int Id { get; set; }
        public string MemberName { get; set; } = null!;
        public string PlanName { get; set; } = null!;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
