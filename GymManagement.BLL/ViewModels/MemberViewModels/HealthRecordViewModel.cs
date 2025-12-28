using System.ComponentModel.DataAnnotations;

namespace GymManagement.BLL.ViewModels.MemberViewModels
{
    public class HealthRecordViewModel
    {
        [Required(ErrorMessage = "Height is Required")]
        [Range(0.1, 300, ErrorMessage = "Height Must be Between 0.1 and 300")]
        public decimal Height { get; set; }

        [Required(ErrorMessage = "Weight is Required")]
        [Range(0.1, 500, ErrorMessage = "Weight Must be Between 0.1 and 500")]
        public decimal Weight { get; set; }

        [Required(ErrorMessage = "BloodType is Required")]
        [StringLength(3, ErrorMessage = "BloodType Must be Between 3 Char or Less ")]
        public string BloodType { get; set; } = null!;

        public string? Note { get; set; } = null!;
    }
}
