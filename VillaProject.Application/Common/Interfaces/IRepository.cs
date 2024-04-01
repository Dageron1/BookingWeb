using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using VillaProject.Domain.Entities;

namespace VillaProject.Application.Common.Interfaces
{
    //we dont know what that class will be right now
    //that is how generics work
    public interface IRepository<T> where T : class
    {
        //может быть nullable выражение в скобках.
        //IQuerable  _villaService.GetAll().FirstOrDefault(x => x.Id == villadId); использование снаруижи
        IEnumerable<T> GetAll(Expression<Func<T, bool>>? filter = null, string? includeProperties = null, bool tracked = false);
        //include properties, if we want to include navigational properties
        T Get(Expression<Func<T, bool>> filter, string? includeProperties = null, bool tracked = false);

        //i typically do not like the generic repository because many times you do not want to update all the fields
        //maybe you only want some fields and that can be different based on the individual model.
        void Add(T entity);
        bool Any(Expression<Func<T, bool>> filter); //когда используем Any, фильтер не обязателен
        void Remove(T entity);
    }
}
