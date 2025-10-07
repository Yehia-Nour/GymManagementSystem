using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManagement.DAL.Entities
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; } = null!;
    }
}
