using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillaProject.Application.Common.Interfaces;
using VillaProject.Application.Services.Interface;
using VillaProject.Domain.Entities;
using VillaProject.Infrastructure.Data;

namespace VillaProject.Infrastructure.Repository
{
    public class VillaImageRepository : Repository<VillaImages>, IVillaImageRepository
    {
        private ApplicationDbContext _db;

        public VillaImageRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public void Update(VillaImages obj)
        {
            _db.VillaImages.Update(obj);
        }
    }
}
