using System;

namespace OpenB.Core.Integration
{
    public class XmlRepository<T> : RepositoryBase, IRepository<T> where T : IModel
    {
        public T Create()
        {
            throw new NotImplementedException();
        }

        public void DeleteByKey(string key)
        {
            throw new NotImplementedException();
        }

        public T GetByKey(string key)
        {
            throw new NotImplementedException();
        }

        public void Update(T model)
        {
            throw new NotImplementedException();
        }
    }
}
