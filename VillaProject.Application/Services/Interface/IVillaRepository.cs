using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillaProject.Application.Common.Interfaces;
using VillaProject.Domain.Entities;

namespace VillaProject.Application.Services.Interface
{
    public interface IVillaImageRepository : IRepository<VillaImages>
    {
        void Update(VillaImages obj);
    }
}
