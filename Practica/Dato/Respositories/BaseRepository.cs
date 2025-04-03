using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dato.Respositories
{
    public abstract class BaseRepository<TEntity, TKey> where TEntity : class
    {
        protected readonly ApplicationDbContext _ctx;
        public BaseRepository(ApplicationDbContext context)
        {
            _ctx = context;
        }

        /// <summary>
        /// 
        /// </summary>
        public DbSet<TEntity> Datasource => _ctx.Set<TEntity>();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> Query() => Datasource.AsNoTracking().AsQueryable();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IQueryable<TEntity> All()
        {
            return Datasource.AsQueryable();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public TEntity Get(TKey id)
        {
            return Datasource.Find(id);
        }


        public async Task<TEntity> GetAsync(TKey id)
        {
            return await Datasource.FindAsync(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public void Add(TEntity entity)
        {
            Datasource.Add(entity);
        }

        public async Task AddRangeAsync(List<TEntity> errors)
        {
            await _ctx.AddRangeAsync(errors);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity"></param>
        public async Task AddAsync(TEntity entity)
        {
            await Datasource.AddAsync(entity);
        }

        public void Update(TEntity entity)
        {
            Datasource.Update(entity);
        }

        public void Remove(TEntity entities)
        {
            Datasource.Remove(entities);
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            Datasource.RemoveRange(entities);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public int SaveChanges()
        {
            return _ctx.SaveChanges();
        }

    }
}
