using System;
using System.Collections.Generic;

namespace OpenB.Core.Integration
{
    public class ApplicationContext
    {        
        public IList<DomainContext> DomainContexts { get; private set; }

        public ApplicationContext() : this(new List<DomainContext>())
        { }

        public ApplicationContext(IList<DomainContext> domainContexts)
        {
            if (domainContexts == null)
                throw new ArgumentNullException(nameof(domainContexts));

            this.DomainContexts = domainContexts;
        }
    }

    public class DomainContext
    {
        public string Name { get; private set; }
        public AppDomain ApplicationDomain { get; private set; }

        public IList<IRepository> Repositories { get; private set; }

        public DomainContext(string name, AppDomain applicationDomain, IList<IRepository> repositories)
        {
            if (repositories == null)
                throw new ArgumentNullException(nameof(repositories));
            if (applicationDomain == null)
                throw new ArgumentNullException(nameof(applicationDomain));
            if (name == null)
                throw new ArgumentNullException(nameof(name));

            Name = name;
            ApplicationDomain = applicationDomain;
            Repositories = repositories;
        }       
    }

    public interface IRepository
    {
    }

    public interface IRepository<T> : IRepository where T : IModel
    {   
        /// <summary>
        /// Returns an new instance of the model.
        /// </summary>
        /// <returns></returns>
        T Create();
        void Update(T model);
        T GetByKey(string key);
        void DeleteByKey(string key);      
    }

    public abstract class RepositoryBase
    {
       
    } 

    

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

    public class MongoDBRepository<T> : RepositoryBase, IRepository<T> where T : IModel
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

    public class SQLiteRepository<T> : RepositoryBase, IRepository<T> where T : IModel
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
