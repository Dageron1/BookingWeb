using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VillaProject.Application.Common.Interfaces;
using VillaProject.Application.Common.Utility;
using VillaProject.Domain.Entities;
using VillaProject.Infrastructure.Data;

namespace VillaProject.Infrastructure.Repository
{
    public class FacilityRepository : Repository<Facility>, IFacilityRepository
    {
        private readonly ApplicationDbContext _db;

        public FacilityRepository(ApplicationDbContext db) : base(db) 
        {
            _db = db;
        }

        public void Update(Facility entity)
        {
            _db.Facilities.Update(entity);
        }
       
    }
}
