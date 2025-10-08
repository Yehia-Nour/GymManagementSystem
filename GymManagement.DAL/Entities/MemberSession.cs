using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Entities
{
    public class MemberSession : BaseEntity
    {
        public bool IsAttended { get; set; }

        public int MemberId { get; set; }
        public Member Member { get; set; } = null!;

        public int SessionId { get; set; }
        public Session Session { get; set; } = null!;
    }
}
