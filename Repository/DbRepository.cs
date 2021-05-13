using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using static Repository.MyRepo;

namespace Repository
{
    public class DbRepository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDatabaseContext context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;
        public DbRepository(ApplicationDatabaseContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }
        public IEnumerable<T> GetAll()
        {
            return entities.AsEnumerable();
        }
        public T GetById(Guid id)
        {
            return entities.SingleOrDefault(s => s.Id == id);
        }
        public T Insert(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");

            entities.Add(entity);
            context.SaveChanges();

            return entity;
        }
        public T Update(T entity)
        {
            if (entity == null) throw new ArgumentNullException("entity");
            context.SaveChanges();

            return entity;
        }
        public bool Delete(Guid id)
        {
            if (id == null) return false;//throw new ArgumentNullException("entity");

            try
            {
                T entity = entities.SingleOrDefault(s => s.Id == id);
                entities.Remove(entity);
                context.SaveChanges();

                return true;
            }
            catch (Exception ex) {
                return false;
            }
        }

        //public IEnumerable<T> GetLike(string Key)
        //{
        //    return entities.Contains(k=>k)
        //}
    }
}
