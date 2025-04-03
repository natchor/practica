using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Dato.Interfaces.Repositories
{
    public interface IRepository<T, TKey> where T : class
    {
        /// <summary>
        /// Las consultas de AsNoTracking() son útiles cuando los resultados se usan en un escenario de solo lectura. Su ejecución es más rápida porque no es necesario configurar información de seguimiento de cambios.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> Query();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        T Get(TKey id);

        Task<T> GetAsync(TKey id);

        void Add(T entity);
        Task AddAsync(T entity);

        Task AddRangeAsync(List<T> errors);
        void Remove(T entity);

        void Update(T entity);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IQueryable<T> All();

        void RemoveRange(IEnumerable<T> entities);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        int SaveChanges();
    }
}
