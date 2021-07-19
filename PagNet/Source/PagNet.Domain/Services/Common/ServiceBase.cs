using PagNet.Domain.Interface.Repository;
using PagNet.Domain.Interface.Services;
using System.Collections.Generic;

namespace PagNet.Domain.Services
{
    public class ServiceBase<TEntity> : IServiceBase<TEntity> where TEntity : class
    {
        protected readonly IRepositoryBase<TEntity> _repository;
        private IPagNet_InstrucaoEmailService rep;

        public ServiceBase(IRepositoryBase<TEntity> repository)
        {
            _repository = repository;
        }

        public ServiceBase(IPagNet_InstrucaoEmailService rep)
        {
            this.rep = rep;
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
