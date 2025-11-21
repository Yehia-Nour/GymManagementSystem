using AutoMapper;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.MemberSessionViewModels;
using GymManagement.BLL.ViewModels.MembershipViewModels;
using GymManagement.BLL.ViewModels.SessionViewModels;
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
    public class BookingService : IBookingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BookingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SessionViewModel>> GetAllSessionsWithTrainerAndCategoryAsync()
        {
            var sessionRepo = _unitOfWork.SessionRepository;
            var sessions = await sessionRepo.GetAllSessionsWithTrainersAndCategoriesAsync();

            if (!sessions.Any())
                return [];

            var sessionsVm = _mapper.Map<IEnumerable<SessionViewModel>>(sessions);

            foreach (var sessionVm in sessionsVm)
                sessionVm.AvailableSlots = await sessionRepo.GetCountOfBookedSlotsAsync(sessionVm.Id);

            return sessionsVm;
        }

        public async Task<IEnumerable<MemberForSessionViewModel>> GetAllMembersForUpcomingSessionAsync(int id)
        {
            var bookingRepo = _unitOfWork.BookingRepository;
            var memberForSessions = await bookingRepo.GetSessionByIdAsync(id);

            var memberForBookingVm = _mapper.Map<IEnumerable<MemberForSessionViewModel>>(memberForSessions);
            return memberForBookingVm;
        }

        public async Task<IEnumerable<MemberForSessionViewModel>> GetAllMembersForOngoingSessionAsync(int id)
        {
            var BookingRepo = _unitOfWork.BookingRepository;
            var MembersForSession = await BookingRepo.GetSessionByIdAsync(id);
            var memberForBookingVm = _mapper.Map<IEnumerable<MemberForSessionViewModel>>(MembersForSession);
            return memberForBookingVm;
        }

        public async Task<bool> CreateBookingAsync(CreateBookingViewModel createBookingViewModel)
        {
            try
            {
                var session = await _unitOfWork.SessionRepository.GetByIdAsync(createBookingViewModel.SessionId);
                if (session is null || session.StartDate <= DateTime.UtcNow)
                    return false;

                var membershipRepo = _unitOfWork.MembershipRepository;
                var activeMembership = membershipRepo.GetAllQueryable(m => m.MemberId == createBookingViewModel.MemberId && m.EndDate >= DateTime.Now).ToListAsync();

                if (activeMembership is null)
                    return false;

                var sessionRepo = _unitOfWork.SessionRepository;
                var bookedSlots = await sessionRepo.GetCountOfBookedSlotsAsync(createBookingViewModel.SessionId);

                var availableSlots = session.Capacity - bookedSlots;
                if (availableSlots == 0)
                    return false;

                var booking = _mapper.Map<MemberSession>(createBookingViewModel);

                booking.IsAttended = false;
                await _unitOfWork.BookingRepository.AddAsync(booking);


                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }

        }

        public async Task<bool> MemberAttendedAsync(MemberAttendOrCancelViewModel model)
        {
            try
            {
                var memberSession = await _unitOfWork.GetRepository<MemberSession>()
                                           .GetAllQueryable(X => X.MemberId == model.MemberId && X.SessionId == model.SessionId)
                                           .FirstOrDefaultAsync();
                if (memberSession is null) return false;

                memberSession.IsAttended = true;
                memberSession.UpdatedAt = DateTime.Now;
                _unitOfWork.GetRepository<MemberSession>().Update(memberSession);
                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> CancelBookingAsync(MemberAttendOrCancelViewModel model)
        {
            try
            {
                var session = await _unitOfWork.SessionRepository.GetByIdAsync(model.SessionId);
                if (session is null || session.StartDate <= DateTime.Now) return false;

                var Booking = await _unitOfWork.BookingRepository.GetAllQueryable(X => X.MemberId == model.MemberId && X.SessionId == model.SessionId)
                                                           .FirstOrDefaultAsync();
                if (Booking is null) return false;
                _unitOfWork.BookingRepository.Delete(Booking);
                return await _unitOfWork.SaveChangesAsync() > 0;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<MemberSelectListViewModel>> GetMembersForDropdownAsync(int id)
        {
            var bookingRepo = _unitOfWork.BookingRepository;
            var bookedMemberIds = await bookingRepo.GetAllQueryable(s => s.Id == id)
                                                      .Select(s => s.MemberId)
                                                      .ToListAsync();

            var availableMembersToBook = await  _unitOfWork.GetRepository<Member>().GetAllQueryable(m => !bookedMemberIds.Contains(m.Id)).ToListAsync();

            var memberSelectListViewModel = _mapper.Map<IEnumerable<MemberSelectListViewModel>>(availableMembersToBook);

            return memberSelectListViewModel;

        }
    }
}