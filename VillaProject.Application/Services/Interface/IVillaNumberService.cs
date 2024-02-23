using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VillaProject.Domain.Entities;

namespace VillaProject.Application.Services.Interface
{
    public interface IVillaNubmberService
    {
        IEnumerable<VillaNumber> GetAllVillaNumbers();
        VillaNumber GetVillaNumberById(int id);
        void CreateVillaNumber(VillaNumber villaNumber); //можно вместо void использовать ResponseDTO который показывают всю инфу про этот метод.
        void UpdateVillaNumber(VillaNumber villaNumber);
        bool DeleteVillaNumber(int id);

    }
}
