using GymManagement.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Repositories.Interfaces
{
    public interface IMemberRepository
    {
        IEnumerable<Member> GetAll();
        Member? GetById(int id);
        int Add(Member member);
        int Update(Member member);
        int Delete(int id);
    }
}
