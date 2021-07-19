using PagNet.Infra.Data.Context;
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using PagNet.Domain.Interface.Repository;
using PagNet.Infra.Data.Repositories.Common;

namespace PagNet.Infra.Data.Repositories
{
    public abstract class RepositoryBase<TEntity> : IDisposable, IRepositoryBase<TEntity>
        where TEntity : class
    {
        protected ContextConcentrador DbConcentrador; // = new ContextConcentrador();
        protected ContextAutorizador DbAutorizador; // = new ContextConcentrador();
        protected ContextNetCard DbNetCard; // = new ContextConcentrador();
        private readonly DbSet<TEntity> _dbSet;

        public RepositoryBase(ContextConcentrador context)
        {
            DbConcentrador = context;
            Connectionstring = context.ConnectionString;
            _dbSet = DbConcentrador.Set<TEntity>();
            InitializeAutoInc();
        }
        public RepositoryBase(ContextAutorizador context)
        {
            DbAutorizador = context;
            Connectionstring = context.ConnectionString;
            _dbSet = DbAutorizador.Set<TEntity>();
            InitializeAutoInc();
        }
        public RepositoryBase(ContextNetCard context)
        {
            DbNetCard = context;
            Connectionstring = context.ConnectionString;
            _dbSet = DbNetCard.Set<TEntity>();
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

            if (DbNetCard == null) return;

            EntityAutoInc<TEntity>
                .InitialSeed(DbNetCard.GetNameBD, GetInitialSeed());
        }

        public virtual int GetMaxKey()
        {
            return EntityAutoInc<TEntity>.GetNextId(DbNetCard.GetNameBD);
        }

        public string Connectionstring { get; }

        public void Dispose()
        {
            if (DbAutorizador != null)
            {
                DbAutorizador.Dispose();
            }
            else if (DbConcentrador != null)
            {
                DbConcentrador.Dispose();
            }
            else
            {
                DbNetCard.Dispose();
            }
        }

        public void Add(TEntity obj)
        {
            if (DbAutorizador != null)
            {
                DbAutorizador.Set<TEntity>().Add(obj);
                DbAutorizador.SaveChanges();
            }
            else if (DbConcentrador != null)
            {
                DbConcentrador.Set<TEntity>().Add(obj);
                DbConcentrador.SaveChanges();
            }
            else
            {
                DbNetCard.Set<TEntity>().Add(obj);
                DbNetCard.SaveChanges();
            }
        }
        public async void AddAsync(TEntity obj)
        {

            if (DbAutorizador != null)
            {
                //DbAutorizador.Set<TEntity>().Add(obj);
                DbAutorizador.Entry(obj).State = EntityState.Added;
                await DbAutorizador.SaveChangesAsync();
            }
            else if (DbConcentrador != null)
            {
                //DbConcentrador.Set<TEntity>().Add(obj);
                DbConcentrador.Entry(obj).State = EntityState.Added;
                await DbConcentrador.SaveChangesAsync();
            }
            else
            {
                //DbNetCard.Set<TEntity>().Add(obj);
                DbNetCard.Entry(obj).State = EntityState.Added;
                await DbNetCard.SaveChangesAsync();
            }
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
            if (DbAutorizador != null)
            {
                return DbAutorizador.Set<TEntity>().ToList();
            }
            else if (DbConcentrador != null)
            {
                return DbConcentrador.Set<TEntity>().ToList();
            }
            else
            {
                return DbNetCard.Set<TEntity>().ToList();
            }
        }

        public TEntity GetById(int id)
        {
            if (DbAutorizador != null)
            {
                return DbAutorizador.Set<TEntity>().Find(id);
            }
            else if (DbConcentrador != null)
            {
                return DbConcentrador.Set<TEntity>().Find(id);
            }
            else
            {
                return DbNetCard.Set<TEntity>().Find(id);
            }
        }

        public void Remove(TEntity obj)
        {
            if (DbAutorizador != null)
            {
                DbAutorizador.Set<TEntity>().Remove(obj);
                DbAutorizador.SaveChanges();
            }
            else if (DbConcentrador != null)
            {
                DbConcentrador.Set<TEntity>().Remove(obj);
                DbConcentrador.SaveChanges();
            }
            else
            {
                DbNetCard.Set<TEntity>().Remove(obj);
                DbNetCard.SaveChanges();
            }
        }

        public void RemoveRanger(IEnumerable<TEntity> List)
        {
            if (DbAutorizador != null)
            {
                DbAutorizador.Set<TEntity>().RemoveRange(List);
            }
            else if (DbConcentrador != null)
            {
                DbConcentrador.Set<TEntity>().RemoveRange(List);
            }
            else
            {
                DbNetCard.Set<TEntity>().RemoveRange(List);
            }
        }

        public void Update(TEntity obj)
        {
            if (DbAutorizador != null)
            {
                DbAutorizador.Entry(obj).State = EntityState.Modified;
                DbAutorizador.SaveChanges();
            }
            else if (DbConcentrador != null)
            {
                DbConcentrador.Entry(obj).State = EntityState.Modified;
                DbConcentrador.SaveChanges();
            }
            else
            {
                DbNetCard.Entry(obj).State = EntityState.Modified;
                DbNetCard.SaveChanges();
            }


        }

    }
}

