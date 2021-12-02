using Data.Interfaces._BaseEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Interfaces.Repositories
{
    /// <summary>
    /// Интерфейс базового репозитория
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IBaseEntityRepository<T> where T : IBaseEntity
    {
        T Get(Guid guid);
        T Add(T model);
        IEnumerable<T> AddRange(IEnumerable<T> models);
        bool Update(T models);
        bool UpdateRange(IEnumerable<T> models);
        bool Remove(T model);
        bool Remove(Guid Guid);
        bool RemoveCascade(T model);
        bool RemoveRange(IEnumerable<T> models);
        bool RemoveRange(IEnumerable<Guid> guids);
        bool Delete(T model);
        IQueryable<T> GetListQuery();
        IQueryable<T> GetListQueryWithDeleted();
        List<T> GetList();
        IEnumerable<T> GetListWithDeleted();
        bool Any(Expression<Func<T, bool>> func);
        T FirstOrDefault(Expression<Func<T, bool>> func);
    }
}