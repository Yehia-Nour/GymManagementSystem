using GymManagement.BLL.ViewModels.TrainerViewModels;

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
