using GymManagement.DAL.Entities.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Entities
{
    public class Trainer : GymUser
    {
        public Specialties Specialties { get; set; }
    }
}
