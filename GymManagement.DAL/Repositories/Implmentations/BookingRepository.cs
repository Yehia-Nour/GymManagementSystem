using GymManagement.DAL.Data.Context;
using GymManagement.DAL.Entities;
using GymManagement.DAL.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Implmentations
{
    public class BookingRepository : GenericRepository<MemberSession>, IBookingRepository
    {
        public BookingRepository(GymDbContext dbContext) : base(dbContext) { }

        public async Task<IEnumerable<MemberSession>> GetSessionByIdAsync(int id)
        {
            return await _dbContext.MembersSessions
                .Where(ms => ms.SessionId == id)
                .Include(ms => ms.Member)
                .ToListAsync();
        }
    }
}
