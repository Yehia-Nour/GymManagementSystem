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
    public class MemberRepository : IMemberRepository
    {
        private readonly GymDbContext _dbContext;
        public MemberRepository(GymDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Member> GetAll()
        {
            return _dbContext.Members.ToList();
        }

        public Member? GetById(int id)
        {
            return _dbContext.Members.Find(id);
        }

        public int Add(Member member)
        {
            _dbContext.Members.Add(member);
            return _dbContext.SaveChanges();
        }

        public int Update(Member member)
        {
            _dbContext.Members.Update(member);
            return _dbContext.SaveChanges();
        }

        public int Delete(int id)
        {
            var member = GetById(id);
            if (member != null)
                _dbContext.Members.Remove(member);

            return _dbContext.SaveChanges();
        }
    }
}
