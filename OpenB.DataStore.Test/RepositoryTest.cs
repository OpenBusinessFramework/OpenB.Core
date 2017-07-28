using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Castle.DynamicProxy;
using NUnit.Framework;
using OpenB.Core;

namespace OpenB.DataStore.Test
{
    [TestFixture]
    public class RepositoryTest
    {
        [Test]
        public void DoSomething()
        {
            DummyDataStore<Person> dummy = new DummyDataStore<Person>();
            IRepository<Person> personRepository = new Repository<Person>(dummy, new GuidKeyGenerator(), new ProxyGenerator());

            personRepository.RetrieveByKey("E8B23BCB-A950-42FB-BD1C-EDF6DD5B9997");
        }


    }

    public interface IDataStore<TModel> where TModel : IModel
    {
        void Insert(TModel model);
        TModel Select(Expression<Func<TModel, bool>> whereConditions);
        void Update(TModel model);
    }

    public class Repository<TModel> : IRepository<TModel> where TModel : IModel
    {
        public static Repository<TModel> GetInstance()
        {
            // TODO: Datastore configuration and IoC.
            return new Repository<TModel>(new DummyDataStore<TModel>(), new GuidKeyGenerator(), new ProxyGenerator());
        }

        readonly ProxyGenerator proxyGenerator;
        readonly IDataStore<TModel> dataStore;
        readonly IKeyGenerator keyGenerator;
        
        public Repository(IDataStore<TModel> dataStore, IKeyGenerator keyGenerator , ProxyGenerator proxyGenerator)
        {    
            if (proxyGenerator == null)
                throw new ArgumentNullException(nameof(proxyGenerator));
            if (keyGenerator == null)
                throw new ArgumentNullException(nameof(keyGenerator));
            if (dataStore == null)
                throw new ArgumentNullException(nameof(dataStore));

            this.dataStore = dataStore;
            this.keyGenerator = keyGenerator;
            this.proxyGenerator = proxyGenerator;

        }

        public TModel Create()
        {
            //IList<IInterceptor> interceptors = new List<IInterceptor>();
            //interceptors.Add(new ChangeInterceptor());

            //if (typeof(IAuditableModel).IsAssignableFrom(typeof(TModel))
            //{
            //    interceptors.Add(new AuditInterceptor());
            //}


            //proxyGenerator.CreateClassProxy<TModel>();
            throw new NotImplementedException();
        }

        public IList<TModel> Retrieve(IList<ICondition> conditions)
        {
            throw new NotImplementedException();
        }

        public TModel RetrieveByKey(string key)
        {
            return dataStore.Select(m => m.Key == key);
        }

        public void Update(TModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            dataStore.Update(model);
        }      
    }

    internal class AuditInterceptor : IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            throw new NotImplementedException();
        }
    }

    public interface IKeyGenerator
    {
        string GenerateKey();
    }

    internal class GuidKeyGenerator : IKeyGenerator
    {
        public string GenerateKey()
        {
            Guid guid = Guid.NewGuid();
            return guid.ToString();
        }
    }

    public interface IRepository<TModel> : IRepository where TModel : IModel
    {
        TModel Create();
        void Update(TModel model);
        TModel RetrieveByKey(string key);
        IList<TModel> Retrieve(IList<ICondition> conditions);
    }

    public interface ICondition
    {
    }

    public interface IRepository
    {

    }

    
}