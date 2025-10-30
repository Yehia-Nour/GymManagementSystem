using GymManagement.BLL.ViewModels.MemberViewModels;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using GymManagement.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Interfaces
{
    public interface ITrainerService
    {
        IEnumerable<TrainerViewModel> GetAllTrainers();
        bool CreateTrainer(CreateTrainerViewModel createTrainer);
        TrainerWithDetailsViewModel? GetTrainerDetails(int id);
        TrainerToUpdateViewModel? GetTrainerToUpdate(int id);
        bool UpdateTrainer(int id, TrainerToUpdateViewModel trainerToUpdate);
        bool DeleteTrainer(int id);
    }
}
