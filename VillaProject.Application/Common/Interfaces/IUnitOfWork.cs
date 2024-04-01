using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillaProject.Application.Services.Interface;

namespace VillaProject.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IVillaRepository Villa {  get; }
        IVillaNumberRepository VillaNumber { get; }
        IAmenityRepository Amenity { get; }
        IBookingRepository Booking { get; }
        IApplicationUserRepository User { get; }
        IVillaImageRepository VillaImage { get; }
        IFacilityRepository Facility { get; }
        ICategoryRepository Category { get; }
        void Save();
    }
}
