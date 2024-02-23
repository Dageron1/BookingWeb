using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VillaProject.Application.Common.Interfaces;
using VillaProject.Domain.Entities;
using VillaProject.Infrastructure.Data;

namespace VillaProject.Infrastructure.Repository
{
    //получает все методы от Repository и выполняет интерфейс так же
    public class VillaRepository : Repository<Villa>, IVillaRepository
    {
        private readonly ApplicationDbContext _db;
        //base отправит на выполнение в Repository 
        //base Надо потому что мы не реализуем dbSet тут.

        //2)VillaRepository получает _db от UnitOfWork.
        //3)И далее передаем его в базовую репозиторию base(db)
        public VillaRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        //public void Add(Villa entity)
        //{
        //    //даже если мы не указывам на пряму в какую таблицу добавить наподобии _db.VillaNumbers.Add(). EF и так поймет куда добавить и без указания таблицы.
        //    _db.Add(entity);
        //}

        public void Update(Villa entity)
        {
            _db.Villas.Update(entity);
        }
    }
}
