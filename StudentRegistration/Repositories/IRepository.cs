using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRegistration.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        void Create(TEntity model);
        IEnumerable<TEntity> GetAll();
        TEntity GetById(object Id);
        void Edit(TEntity model);
        void Delete(TEntity model);
        void DeleteById(object Id);
    }
}
