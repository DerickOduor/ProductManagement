using Entities;
using System;
using System.Collections.Generic;

namespace Repository
{
    public class MyRepo
    {
        public interface IRepository<T> where T : BaseEntity
        {
            IEnumerable<T> GetAll();
            //IEnumerable<T> GetLike();
            T GetById(Guid id);
            T Insert(T entity);
            T Update(T entity);
            bool Delete(Guid id);
        }
    }
}
