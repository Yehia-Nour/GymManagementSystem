using System.ComponentModel.DataAnnotations;

namespace GymManagement.BLL.ViewModels.PlanViewModels
{
    public class UpdatePlanViewModel
    {
        public string PlanName { get; set; } = null!;

        [Required(ErrorMessage = "Description is Required")]
        [StringLength(100, MinimumLength = 5, ErrorMessage = "Description Must be Between 5 and 100 Char")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Duration Days is Required")]
        [Range(1, 365, ErrorMessage = "Duration Days Must be Between 1 and 365")]
        public int DurationDays { get; set; }

        [Required(ErrorMessage = "Price Days is Required")]
        [Range(0.1, 100000, ErrorMessage = "Price Must be Between 0.1 and 100000")]
        public decimal Price { get; set; }
    }
}
