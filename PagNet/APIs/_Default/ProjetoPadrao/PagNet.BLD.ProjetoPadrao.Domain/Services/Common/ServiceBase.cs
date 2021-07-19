using PagNet.BLD.ProjetoPadrao.Domain.Interface.Repository.Common;
using PagNet.BLD.ProjetoPadrao.Domain.Interface.Services.Common;
using System.Collections.Generic;

namespace PagNet.BLD.ProjetoPadrao.Domain.Services.Common
{
    public class ServiceBase<TEntity> : IServiceBase<TEntity> where TEntity : class
    {
        protected readonly IRepositoryBase<TEntity> _repository;

        public ServiceBase(IRepositoryBase<TEntity> repository)
        {
            _repository = repository;
        }

        protected IRepositoryBase<TEntity> Repository
        {
            get { return _repository; }
        }

        public void Add(TEntity obj)
        {
            _repository.Add(obj);
        }

        public TEntity GetById(int id)
        {
            return _repository.GetById(id);
        }

        public IEnumerable<TEntity> GetAll()
        {
            return _repository.GetAll();
        }

        public void Update(TEntity obj)
        {

            _repository.Update(obj);
        }

        public void Remove(TEntity obj)
        {
            _repository.Remove(obj);
        }

    }
  
}
