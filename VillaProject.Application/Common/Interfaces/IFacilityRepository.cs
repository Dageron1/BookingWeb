using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillaProject.Domain.Entities;

namespace VillaProject.Application.Common.Interfaces
{
    public interface IFacilityRepository : IRepository<Facility>
    {
        void Update(Facility entity);
    }
}
