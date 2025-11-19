using AutoMapper.Execution;
using GymManagement.BLL.Services.Interfaces;
using GymManagement.BLL.ViewModels.AnalyticsViewModels;
using GymManagement.DAL.Entities;
using GymManagement.DAL.UnitOfWork.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Member = GymManagement.DAL.Entities.Member;

namespace GymManagement.BLL.Services.Implmentations
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public AnalyticsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<AnalyticsViewModel> GetAnalyticsDataAsync()
        {
            var sessionRepo = _unitOfWork.SessionRepository.GetAllQueryable();
            return new AnalyticsViewModel
            {
                ActiveMembers = await _unitOfWork.GetRepository<MemberShip>().GetAllQueryable(m => m.Status == "Active").CountAsync(),
                TotalMembers = await _unitOfWork.GetRepository<Member>().GetAllQueryable().CountAsync(),
                TotalTrainers = await _unitOfWork.GetRepository<Trainer>().GetAllQueryable().CountAsync(),
                UpcomingSessions = await sessionRepo.CountAsync(s => s.StartDate > DateTime.Now),
                OngoingSessions = await sessionRepo.CountAsync(s => s.StartDate <= DateTime.Now && s.EndDate > DateTime.Now),
                CompletedSessions = await sessionRepo.CountAsync(s => s.EndDate < DateTime.Now)
            };
        }
    }
}
