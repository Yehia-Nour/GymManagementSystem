using AutoMapper;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.SessionViewModels;
using GymManagement.DAL.Entities;
using GymManagement.DAL.UnitOfWork.Interfaces;
using Microsoft.Data.SqlClient.DataClassification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.BLL.Services.Implmentations
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public SessionService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public IEnumerable<SessionViewModel> GetAllSessions()
        {
            var sessions = _unitOfWork.SessionRepository.GetAllSessionsWithTrainersAndCategories();
            if (!sessions.Any())
                return [];

            var sessionViewModels = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);

            foreach (var session in sessionViewModels)
                session.AvailableSlots = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id);

            return sessionViewModels;
        }

        public SessionViewModel? GetSessionDetails(int id)
        {
            var session = _unitOfWork.SessionRepository.GetSessionByIdWithTrainerandCategory(id);
            if (session is null)
                return null;

            var sessionViewModel = _mapper.Map<SessionViewModel>(session);
            sessionViewModel.AvailableSlots = session.Capacity - _unitOfWork.SessionRepository.GetCountOfBookedSlots(id);

            return sessionViewModel;
        }

        public bool CreateSession(CreateSessionViewModel createSession)
        {
            try
            {
                var trainerExists = IsTrainerExists(createSession.TrainerId);
                var categpruExists = IsCategpruExists(createSession.TrainerId);
                var vaildDate = IsVaildDateRange(createSession.StartDate, createSession.EndDate);

                if (!trainerExists || !categpruExists || !vaildDate)
                    return false;

                var session = _mapper.Map<Session>(createSession);
                _unitOfWork.GetRepository<Session>().Add(session);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch { return false; }
        }

        public UpdateSessionViewModel? GetSessionToUpdate(int id)
        {
            var session = _unitOfWork.SessionRepository.GetById(id);
            if (!IsSessionAvailableForUpdate(session))
                return null;

            var sessionViewModel = _mapper.Map<UpdateSessionViewModel>(session);

            return sessionViewModel;
        }

        public bool UpdateSession(UpdateSessionViewModel updateSession, int id)
        {
            try
            {
                var session = _unitOfWork.SessionRepository.GetById(id);
                if (!IsSessionAvailableForUpdate(session))
                    return false;

                var trainerExists = IsTrainerExists(updateSession.TrainerId);
                var vaildDate = IsVaildDateRange(updateSession.StartDate, updateSession.EndDate);
                if (!trainerExists || !vaildDate)
                    return false;

                _mapper.Map(updateSession, session);
                session.UpdatedAt = DateTime.Now;

                return _unitOfWork.SaveChanges() > 0;
            }
            catch { return false; }
        }

        public bool DeleteSession(int id)
        {
            try
            {
                var session = _unitOfWork.SessionRepository.GetById(id);
                if (!IsSessionAvailableForRemoving(session))
                    return false;

                _unitOfWork.SessionRepository.Delete(session!);

                return _unitOfWork.SaveChanges() > 0;
            }
            catch { return false; }
        }

        public IEnumerable<TrainerSelectViewModel> GetAllTrainersForDropDown()
        {
            var trainers = _unitOfWork.GetRepository<Trainer>().GetAll();

            var trainerViewModels = _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);

            return trainerViewModels;
        }

        public IEnumerable<CategorySelectViewModel> GetAllCategoriesForDropDown()
        {
            var categories = _unitOfWork.GetRepository<Category>().GetAll();

            var categoryViewModels = _mapper.Map<IEnumerable<CategorySelectViewModel>>(categories);

            return categoryViewModels;
        }

        private bool IsTrainerExists(int id) => _unitOfWork.GetRepository<Trainer>().GetById(id) is not null;

        private bool IsCategpruExists(int id) => _unitOfWork.GetRepository<Category>().GetById(id) is not null;

        private bool IsVaildDateRange(DateTime startDate, DateTime endDate) => startDate < endDate && startDate > DateTime.Now;

        private bool IsSessionAvailableForUpdate(Session? session)
        {
            if (session is null)
                return false;
            if (session.EndDate < DateTime.Now)
                return false;
            if (session.StartDate <= DateTime.Now)
                return false;

            var HasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if (HasActiveBooking)
                return false;

            return true;
        }

        private bool IsSessionAvailableForRemoving(Session? session)
        {
            if (session is null)
                return false;
            if (session.StartDate > DateTime.Now)
                return false;
            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now)
                return false;

            var HasActiveBooking = _unitOfWork.SessionRepository.GetCountOfBookedSlots(session.Id) > 0;
            if (HasActiveBooking)
                return false;

            return true;
        }
    }
}
