using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillaProject.Domain.Entities;

namespace VillaProject.Application.Services.Interface
{
    public interface IVillaService
    {
        IEnumerable<Villa> GetAllVillas();
        Villa GetVillaById(int id);
        void CreateVilla(Villa villa); //можно вместо void использовать ResponseDTO который показывают всю инфу про этот метод.
        void UpdateVilla(Villa villa);
        bool DeleteVilla(int id);

        IEnumerable<Villa> GetVillasAvailabilityByDate(int nights, DateOnly checkInDate);
        bool IsVillaAvailableByDate(int villaId, int nights, DateOnly checkInDate);
    }
}
