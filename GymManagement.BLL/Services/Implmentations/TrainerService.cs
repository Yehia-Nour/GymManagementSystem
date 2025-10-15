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

        public TrainerService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<TrainerViewModel> GetAllTrainers()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();
            if (!trainers.Any())
                return [];

            var trainerViewModels = trainers.Select(t => new TrainerViewModel
            {
                Name = t.Name,
                Email = t.Email,
                Phone = t.Phone,
                Specialties = t.Specialties.ToString()
            });

            return trainerViewModels;
        }

        public TrainerWithDetailsViewModel? GetTrainerDetails(int id)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
            if (trainer is null)
                return null;

            var trainerViewModel = new TrainerWithDetailsViewModel
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                Specialties = trainer.Specialties.ToString(),
                DateOfBirth = trainer.DateOfBirth.ToShortDateString(),
                Address = $"{trainer.Address.BuildingNumber} - {trainer.Address.Street} - {trainer.Address.City}"
            };

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

                var trainer = new Trainer
                {
                    Name = createTrainer.Name,
                    Email = createTrainer.Email,
                    Phone = createTrainer.Phone,
                    DateOfBirth = createTrainer.DateOfBirth,
                    Gender = createTrainer.Gender,
                    Address = new Address
                    {
                        BuildingNumber = createTrainer.BuildingNumber,
                        Street = createTrainer.Street,
                        City = createTrainer.City,
                    },
                    Specialties = createTrainer.Specialties
                };

                _unitOfWork.GetRepository<Trainer>().Add(trainer);
                return _unitOfWork.SaveChanges() > 0;
            }
            catch { return false; }
        }

        public TrainerToUpdaterViewModel? GetMemberToUpdate(int id)
        {
            var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
            if (trainer is null)
                return null;

            var trainerToUpdate = new TrainerToUpdaterViewModel
            {
                Name = trainer.Name,
                Email = trainer.Email,
                Phone = trainer.Phone,
                BuildingNumber = trainer.Address.BuildingNumber,
                Street = trainer.Address.Street,
                City = trainer.Address.City,
                Specialties = trainer.Specialties
            };

            return trainerToUpdate;
        }

        public bool UpdateTrainer(int id, TrainerToUpdaterViewModel trainerToUpdate)
        {
            try
            {
                var emailExists = IsEmailExists(trainerToUpdate.Email);
                var phoneExists = IsPhoneExists(trainerToUpdate.Phone);
                if (emailExists || phoneExists)
                    return false;

                var trainer = _unitOfWork.GetRepository<Trainer>().GetById(id);
                if (trainer is null)
                    return false;

                trainer.Email = trainerToUpdate.Email;
                trainer.Phone = trainerToUpdate.Phone;
                trainer.Address.BuildingNumber = trainerToUpdate.BuildingNumber;
                trainer.Address.City = trainerToUpdate.City;
                trainer.Address.Street = trainerToUpdate.Street;
                trainer.Specialties = trainerToUpdate.Specialties;

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
