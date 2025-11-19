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
        Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync();
        Task<bool> CreateTrainerAsync(CreateTrainerViewModel createTrainer);
        Task<TrainerWithDetailsViewModel?> GetTrainerDetailsAsync(int id);
        Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int id);
        Task<bool> UpdateTrainerAsync(int id, TrainerToUpdateViewModel trainerToUpdate);
        Task<bool> DeleteTrainerAsync(int id);
    }
}
