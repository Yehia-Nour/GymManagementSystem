using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Entities
{
    public class Session : BaseEntity
    {
        public string Description { get; set; } = null!;
        public int Capacity { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        #region RelationShips

        #region Session - Category

        public int CategoryId { get; set; }
        public Category Category { get; set; } = null!;

        #endregion

        #region Session - Trainer

        public int TrainerId { get; set; }
        public Trainer Trainer { get; set; } = null!;

        #endregion

        #region Session - MemberSessions

        public ICollection<MemberSession> MemberSessions { get; set; } = null!;

        #endregion

        #endregion

    }
}
