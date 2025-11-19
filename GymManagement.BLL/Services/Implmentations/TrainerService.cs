using AutoMapper;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using GymManagement.DAL.Entities;
using GymManagement.DAL.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Implmentations
{
    public class TrainerService : ITrainerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public TrainerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TrainerViewModel>> GetAllTrainersAsync()
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllQueryable().ToListAsync();
            if (!trainers.Any())
                return [];

            var trainerViewModels = _mapper.Map<IEnumerable<TrainerViewModel>>(trainers);

            return trainerViewModels;
        }

        public async Task<TrainerWithDetailsViewModel?> GetTrainerDetailsAsync(int id)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id);
            if (trainer is null)
                return null;

            var trainerViewModel = _mapper.Map<TrainerWithDetailsViewModel>(trainer);

            return trainerViewModel;
        }

        public async Task<bool> CreateTrainerAsync(CreateTrainerViewModel createTrainer)
        {
            try
            {
                var emailExists = await IsEmailExistsAsync(createTrainer.Email);
                var phoneExists = await IsPhoneExistsAsync(createTrainer.Phone);
                if (emailExists || phoneExists)
                    return false;

                var trainer = _mapper.Map<Trainer>(createTrainer);

                await _unitOfWork.GetRepository<Trainer>().AddAsync(trainer);
                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch { return false; }
        }

        public async Task<TrainerToUpdateViewModel?> GetTrainerToUpdateAsync(int id)
        {
            var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id);
            if (trainer is null)
                return null;

            var trainerToUpdate = _mapper.Map<TrainerToUpdateViewModel>(trainer);

            return trainerToUpdate;
        }

        public async Task<bool> UpdateTrainerAsync(int id, TrainerToUpdateViewModel trainerToUpdate)
        {
            try
            {
                var emailExists = await _unitOfWork.GetRepository<Trainer>()
                    .GetAllQueryable(t => t.Email == trainerToUpdate.Email && t.Id != id).AnyAsync();

                var phoneExists = await _unitOfWork.GetRepository<Trainer>()
                    .GetAllQueryable(t => t.Phone == trainerToUpdate.Phone && t.Id != id).AnyAsync();
                if (emailExists || phoneExists)
                    return false;

                var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id);
                if (trainer is null)
                    return false;

                _mapper.Map(trainerToUpdate, trainer);

                _unitOfWork.GetRepository<Trainer>().Update(trainer);

                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteTrainerAsync(int id)
        {
            try
            {
                var trainer = await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id);
                if (trainer is null)
                    return false;

                var haveSession = await _unitOfWork.GetRepository<Session>().GetAllQueryable(s => s.TrainerId == id || s.StartDate > DateTime.UtcNow).AnyAsync();
                if (haveSession)
                    return false;

                _unitOfWork.GetRepository<Trainer>().Delete(trainer);

                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch { return false; }
        }


        private async Task<bool> IsEmailExistsAsync(string email) => await _unitOfWork.GetRepository<Trainer>().GetAllQueryable(t => t.Email == email).AnyAsync();

        private async Task<bool> IsPhoneExistsAsync(string phone) => await _unitOfWork.GetRepository<Trainer>().GetAllQueryable(t => t.Phone == phone).AnyAsync();
    }
}
