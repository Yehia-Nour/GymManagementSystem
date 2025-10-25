using AutoMapper;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.TrainerViewModels;
using GymManagement.DAL.Entities;
using GymManagement.DAL.UnitOfWork.Interfaces;
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

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            if (!trainers.Any())
                return [];

            var trainerViewModels = _mapper.Map<IEnumerable<TrainerViewModel>>(trainers);

            return trainerViewModels;
        }

        public TrainerWithDetailsViewModel? GetTrainerDetails(int id)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
            if (trainer is null)
                return null;

            var trainerViewModel = _mapper.Map<TrainerWithDetailsViewModel>(trainer);

            return trainerViewModel;
        }

        public bool CreateTrainer(CreateTrainerViewModel createTrainer)
        {
            try
            {
                var emailExists = IsEmailExists(createTrainer.Email);
                var phoneExists = IsPhoneExists(createTrainer.Phone);
                if (emailExists || phoneExists)
                    return false;

                var trainer = _mapper.Map<Trainer>(createTrainer);

                _unitOfWork.GetRepository<Trainer>().Add(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch { return false; }
        }

        public TrainerToUpdateViewModel? GetTrainerToUpdate(int id)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
            if (trainer is null)
                return null;

            var trainerToUpdate = _mapper.Map<TrainerToUpdateViewModel>(trainer);

            return trainerToUpdate;
        }

        public bool UpdateTrainer(int id, TrainerToUpdateViewModel trainerToUpdate)
        {
            try
            {
                var emailExists = _unitOfWork.GetRepository<Trainer>()
                    .GetAll(t => t.Email == trainerToUpdate.Email && t.Id != id).Any();

                var phoneExists = _unitOfWork.GetRepository<Trainer>()
                    .GetAll(t => t.Phone == trainerToUpdate.Phone && t.Id != id).Any();
                if (emailExists || phoneExists)
                    return false;

                var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
                if (trainer is null)
                    return false;

                _mapper.Map(trainerToUpdate, trainer);

                _unitOfWork.GetRepository<Trainer>().Update(trainer);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch { return false; }
        }

        public bool DeleteTrainer(int id)
        {
            try
            {
                var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
                if (trainer is null)
                    return false;

                var haveSession = _unitOfWork.GetRepository<Session>().GetAll(s => s.TrainerId == id || s.StartDate > DateTime.UtcNow).Any();
                if (haveSession)
                    return false;

                _unitOfWork.GetRepository<Trainer>().Delete(trainer);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch { return false; }
        }


        private bool IsEmailExists(string email) => _unitOfWork.GetRepository<Trainer>().GetAll(t => t.Email == email).Any();

        private bool IsPhoneExists(string phone) => _unitOfWork.GetRepository<Trainer>().GetAll(t => t.Phone == phone).Any();
    }
}
