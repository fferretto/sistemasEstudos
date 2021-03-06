using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace PagNet.Bld.Domain.Interface.Repository.Common
{
    public interface IRepositoryBase<TEntity> where TEntity : class
    {
        IEnumerable<TEntity> ExecStoreProcedure(string procedure, params object[] parametros);

        void Add(TEntity obj);
        void AddAsync(TEntity obj);

        TEntity GetById(int id);
        TEntity GetById(string id);

        IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter = null,
            string includeProperties = "");
        
        IEnumerable<TEntity> GetAll();

        string Connectionstring { get; }

        void Update(TEntity obj);
        void UpdateRange(IEnumerable<TEntity> List);

        void Remove(TEntity obj);

        void RemoveRanger(IEnumerable<TEntity> List);

        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        void Dispose();
    }   

}
