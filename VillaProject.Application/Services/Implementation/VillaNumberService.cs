using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillaProject.Application.Common.Interfaces;
using VillaProject.Application.Services.Interface;
using VillaProject.Domain.Entities;


namespace VillaNumberProject.Application.Services.Implementation
{
    public class VillaNumberService : IVillaNubmberService
    {
        //private readonly ApplicationDbContext _db;
        //private readonly IVillaNumberRepository _villaNumberRepo; больше не используем т к нет связанное реализации интерфейса и класса в program.cs
        //используем теперь pattern UnitOfWork
        private readonly IUnitOfWork _unitOfWork;

        public VillaNumberService(/*ApplicationDbContext db*/ /*IVillaNumberRepository villaNumberRepo,*/ IUnitOfWork unitOfWork)
        {
            //_villaNumberRepo = villaNumberRepo;
            _unitOfWork = unitOfWork;
        }

        public void CreateVillaNumber(VillaNumber villaNumber)
        {           
            _unitOfWork.VillaNumber.Add(villaNumber);
            _unitOfWork.Save();
        }

        public bool DeleteVillaNumber(int id)
        {
            try
            {
                VillaNumber? objFromDb = _unitOfWork.VillaNumber.Get(u => u.Villa_Number == id);
                if (objFromDb is not null)
                {
                    _unitOfWork.VillaNumber.Remove(objFromDb);
                    _unitOfWork.Save();
                    return true;
                } 
                return false;
            }
            catch(Exception)
            {
                return false;
            } 
        }

        public IEnumerable<VillaNumber> GetAllVillaNumbers()
        {
            return _unitOfWork.VillaNumber.GetAll(includeProperties: "Villa");
        }

        public VillaNumber GetVillaNumberById(int id)
        {
            return _unitOfWork.VillaNumber.Get(u =>u.Villa_Number == id, includeProperties: "Villa");
        }

        public void UpdateVillaNumber(VillaNumber villaNumber)
        {
            _unitOfWork.VillaNumber.Update(villaNumber);
            _unitOfWork.Save();
        }
    }
}
