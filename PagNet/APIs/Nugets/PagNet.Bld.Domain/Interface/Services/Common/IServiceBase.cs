using System.Collections.Generic;

namespace PagNet.Bld.Domain.Interface.Services.Common
{
    public interface IServiceBase<TEntity> where TEntity : class
    {
        void Add(TEntity obj);
        TEntity GetById(int id);
        TEntity GetById(string id);
        IEnumerable<TEntity> GetAll();
        void Update(TEntity obj);
        void UpdateRange(IEnumerable<TEntity> List);
        void Remove(TEntity obj);
    }
}
