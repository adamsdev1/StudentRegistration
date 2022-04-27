using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRegistration.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbContext _dbContext { get; set; }

        public void Create(TEntity model)
        {
            _dbContext.Set<TEntity>().Add(model);
        }

        public void Delete(TEntity model)
        {
            _dbContext.Set<TEntity>().Remove(model);
        }

        public void DeleteById(object Id)
        {
            TEntity entity = _dbContext.Set<TEntity>().Find(Id);
            this.Delete(entity);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _dbContext.Set<TEntity>().ToList();
        }

        public TEntity GetById(object Id)
        {
            return _dbContext.Set<TEntity>().Find(Id);
        }

        public void Edit(TEntity model)
        {
            _dbContext.Entry<TEntity>(model).State = EntityState.Modified;
        }

    }
}
