using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.ViewModels.TrainerViewModels
{
    public class TrainerWithDetailsViewModel : TrainerViewModel
    {
        public string DateOfBirth { get; set; } = null!;
        public string Address { get; set; } = null!;
    }
}
