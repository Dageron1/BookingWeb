using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillaProject.Application.Common.Interfaces;
using VillaProject.Application.Services.Interface;
using VillaProject.Infrastructure.Data;

namespace VillaProject.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;
        public IVillaRepository Villa {get; private set;}
        public IVillaNumberRepository VillaNumber { get; private set;}
        public IAmenityRepository Amenity { get; private set;}
        public IBookingRepository Booking { get; private set;}
        public IApplicationUserRepository User { get; private set;}
        public IVillaImageRepository VillaImage { get; private set;}
        public IFacilityRepository Facility { get; private set;}
        public ICategoryRepository Category { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            //1)передаем _db в конструктор VillaRepository
            Villa = new VillaRepository(_db); //после всех действий это будет равно Villa == dbSet == db.Set<T>(где T это Villa)
            //и когда происходит обращение к _unitOfWork.Villa по сути мы обращаемся к dbSet, а более конкретнее db.Set<T> == db.Villas, Villas - название в ApplicationDbContext у таблицы
            VillaNumber = new VillaNumberRepository(_db);
            Amenity = new AmenityRepository(_db);
            Booking = new BookingRepository(_db);
            User = new ApplicationUserRepository(_db);
            VillaImage = new VillaImageRepository(_db);
            Facility = new FacilityRepository(_db);
            Category = new CategoryRepository(_db);
        }
        public void Save()
        {
            _db.SaveChanges();
        }
    }
}
