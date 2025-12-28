using AutoMapper;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.SessionViewModels;
using GymManagement.DAL.Entities;
using GymManagement.DAL.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsAsync()
        {
            var sessions = await _unitOfWork.SessionRepository.GetAllSessionsWithTrainersAndCategoriesAsync();
            if (!sessions.Any())
                return [];

            var sessionViewModels = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);

            foreach (var session in sessionViewModels)
                session.AvailableSlots = session.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id);

            return sessionViewModels;
        }

        public async Task<SessionViewModel?> GetSessionDetailsAsync(int id)
        {
            var session = await _unitOfWork.SessionRepository.GetSessionByIdWithTrainerandCategoryAsync(id);
            if (session is null)
                return null;

            var sessionViewModel = _mapper.Map<SessionViewModel>(session);
            sessionViewModel.AvailableSlots = session.Capacity - await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(id);

            return sessionViewModel;
        }

        public async Task<bool> CreateSessionAsync(CreateSessionViewModel createSession)
        {
            try
            {
                var trainerExists = await IsTrainerExistsAsync(createSession.TrainerId);
                var categpruExists = await IsCategpruExistsAsync(createSession.CategoryId);
                var vaildDate = IsVaildDateRange(createSession.StartDate, createSession.EndDate);

                if (!trainerExists || !categpruExists || !vaildDate)
                    return false;

                var session = _mapper.Map<Session>(createSession);
                await _unitOfWork.GetRepository<Session>().AddAsync(session);

                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch { return false; }
        }

        public async Task<UpdateSessionViewModel?> GetSessionToUpdateAsync(int id)
        {
            var session = await _unitOfWork.SessionRepository.GetByIdAsync(id);
            if (!await IsSessionAvailableForUpdateAsync(session))
                return null;

            var sessionViewModel = _mapper.Map<UpdateSessionViewModel>(session);

            return sessionViewModel;
        }

        public async Task<bool> UpdateSessionAsync(UpdateSessionViewModel updateSession, int id)
        {
            try
            {
                var session = await _unitOfWork.SessionRepository.GetByIdAsync(id);
                if (!await IsSessionAvailableForUpdateAsync(session))
                    return false;

                var trainerExists = await IsTrainerExistsAsync(updateSession.TrainerId);
                var vaildDate = IsVaildDateRange(updateSession.StartDate, updateSession.EndDate);
                if (!trainerExists || !vaildDate)
                    return false;

                _mapper.Map(updateSession, session);
                session.UpdatedAt = DateTime.Now;

                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch { return false; }
        }

        public async Task<bool> DeleteSessionAsync(int id)
        {
            try
            {
                var session = await _unitOfWork.SessionRepository.GetByIdAsync(id);
                if (!await IsSessionAvailableForRemovingAsync(session))
                    return false;

                _unitOfWork.SessionRepository.Delete(session!);

                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch { return false; }
        }

        public async Task<IEnumerable<TrainerSelectViewModel>> GetAllTrainersForDropDownAsync()
        {
            var trainers = await _unitOfWork.GetRepository<Trainer>().GetAllQueryable().ToListAsync();

            var trainerViewModels = _mapper.Map<IEnumerable<TrainerSelectViewModel>>(trainers);

            return trainerViewModels;
        }

        public async Task<IEnumerable<CategorySelectViewModel>> GetAllCategoriesForDropDownAsync()
        {
            var categories = await _unitOfWork.GetRepository<Category>().GetAllQueryable().ToListAsync();

            var categoryViewModels = _mapper.Map<IEnumerable<CategorySelectViewModel>>(categories);

            return categoryViewModels;
        }

        private async Task<bool> IsTrainerExistsAsync(int id) => await _unitOfWork.GetRepository<Trainer>().GetByIdAsync(id) is not null;

        private async Task<bool> IsCategpruExistsAsync(int id) => await _unitOfWork.GetRepository<Category>().GetByIdAsync(id) is not null;

        private bool IsVaildDateRange(DateTime startDate, DateTime endDate) => startDate < endDate && startDate > DateTime.Now;

        private async Task<bool> IsSessionAvailableForUpdateAsync(Session? session)
        {
            if (session is null)
                return false;
            if (session.EndDate < DateTime.Now)
                return false;
            if (session.StartDate <= DateTime.Now)
                return false;

            var HasActiveBooking = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id) > 0;
            if (HasActiveBooking)
                return false;

            return true;
        }

        private async Task<bool> IsSessionAvailableForRemovingAsync(Session? session)
        {
            if (session is null)
                return false;
            if (session.StartDate > DateTime.Now)
                return false;
            if (session.StartDate <= DateTime.Now && session.EndDate > DateTime.Now)
                return false;

            var HasActiveBooking = await _unitOfWork.SessionRepository.GetCountOfBookedSlotsAsync(session.Id) > 0;
            if (HasActiveBooking)
                return false;

            return true;
        }
    }
}
