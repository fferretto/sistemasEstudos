using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using PagNet.Bld.AntecipPGTO.Domain.Interface.Repository.Common;
using PagNet.Bld.AntecipPGTO.Infra.Data.ContextDados;

namespace PagNet.Bld.AntecipPGTO.Infra.Data.Repositories.Common
{
    public abstract class RepositoryBase<TEntity> : IDisposable, IRepositoryBase<TEntity>
        where TEntity : class
    {
        protected ContextPagNet DbPagNet; // = new ContextConcentrador();
        private readonly DbSet<TEntity> _dbSet;

        public RepositoryBase(ContextPagNet context)
        {
            DbPagNet = context;
            Connectionstring = context.ConnectionString;
            _dbSet = DbPagNet.Set<TEntity>();
            InitializeAutoInc();
        }

        protected DbSet<TEntity> DbSet
        {
            get { return _dbSet; }
        }

        protected virtual int GetInitialSeed()
        {
            return 0;
        }

        protected virtual void InitializeAutoInc()
        {
            EntityAutoInc<TEntity>
                .InitialSeed(GetInitialSeed());
        }

        public virtual int GetMaxKey()
        {
            return EntityAutoInc<TEntity>.GetNextId();
        }

        public string Connectionstring { get; }

        public void Dispose()
        {
            DbPagNet.Dispose();
        }

        public void Add(TEntity obj)
        {
            DbPagNet.Set<TEntity>().Add(obj);
            DbPagNet.SaveChanges();
        }
        public async void AddAsync(TEntity obj)
        {
            //DbPagNet.Set<TEntity>().Add(obj);
            DbPagNet.Entry(obj).State = EntityState.Added;
            await DbPagNet.SaveChangesAsync();
        }
        public IEnumerable<TEntity> ExecStoreProcedure(string procedure, params object[] parametros)
        {
            var dados = DbSet.FromSql(procedure, parametros);

            return dados;
        }


        public IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        public IEnumerable<TEntity> Get(Expression<Func<TEntity, bool>> filter = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = DbSet;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            foreach (var includeProperty in includeProperties.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty.Trim());
            }
            return query.ToList();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return DbPagNet.Set<TEntity>().ToList();
        }

        public TEntity GetById(int id)
        {
            return DbPagNet.Set<TEntity>().Find(id);
        }

        public void Remove(TEntity obj)
        {
            DbPagNet.Set<TEntity>().Remove(obj);
            DbPagNet.SaveChanges();
        }

        public void RemoveRanger(IEnumerable<TEntity> List)
        {
            DbPagNet.Set<TEntity>().RemoveRange(List);
        }

        public void Update(TEntity obj)
        {
            DbPagNet.Entry(obj).State = EntityState.Modified;
            DbPagNet.SaveChanges();

        }

    }
}

