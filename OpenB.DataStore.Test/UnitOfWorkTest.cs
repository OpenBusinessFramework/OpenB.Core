using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Castle.DynamicProxy;
using Moq;
using NUnit.Framework;
using OpenB.Core;

namespace OpenB.DataStore.Test
{
    public class Person : IModel
    {
        public virtual string Description
        {
            get; set;
        }

        public virtual bool IsActive
        {
            get; set;
        }

        public string Key
        {
            get; set;
        }

        public virtual string Name
        {
            get; set;
        }

        public virtual bool IsMarried { get; set; }
    }

    [TestFixture]
    public class UnitOfWorkTest
    {
        [Test]
        public void CreateNewModelAndCommit()
        {
            Mock<IModel> modelMock = new Mock<IModel>();
            IUnitOfWork unitOfWork = new UnitOfWork(new RepositoryService());

            unitOfWork.Create(modelMock.Object);
            unitOfWork.Commit();
        }

        [Test]
        public void ModelWithoutKeyResultsInCreation()
        {
            UnitOfWork unitOfWork = UnitOfWork.GetInstance();
            ProxyGenerator generator = new ProxyGenerator();
            Person person = generator.CreateClassProxy<Person>(new ChangeInterceptor(unitOfWork));

            person.IsMarried = false;

            Assert.That(unitOfWork.actionQueue.Dequeue() is CreateAction);
        }

        [Test]
        public void ModelWithKeyResultsInUpdating()
        {
            UnitOfWork unitOfWork = UnitOfWork.GetInstance();
            ProxyGenerator generator = new ProxyGenerator();
            Person person = generator.CreateClassProxy<Person>(new ChangeInterceptor(unitOfWork));
            
            person.Key = "MYFirstObject";
            person.IsMarried = false;
            person.IsActive = true;


            Assert.That(unitOfWork.actionQueue.Dequeue() is UpdateAction);
        }

        [Test]
        public void ModelUpdate_ChangesAreMade()
        {
            UnitOfWork unitOfWork = UnitOfWork.GetInstance();
            RepositoryService repositoryService = new RepositoryService();

            IRepository<Person> personRepository = repositoryService.GetRepository<Person>();
            Person person = personRepository.Create();





            person.IsMarried = true;
            Assert.That(person.IsMarried, Is.True);
        }

        [Test]
        public void ModelWithKeyResultsInNothing()
        {
            UnitOfWork unitOfWork = UnitOfWork.GetInstance();
            ProxyGenerator generator = new ProxyGenerator();
            Person person = generator.CreateClassProxy<Person>(new ChangeInterceptor(unitOfWork));

            person.Key = "MYFirstObject";
            person.IsMarried = false;


            Assert.That(unitOfWork.actionQueue.Count == 0);
        }

        [Test]
        public void UnitOfWork_DuplicatePropertyChangeResultsInOneAction()
        {
            UnitOfWork unitOfWork = UnitOfWork.GetInstance();
            ProxyGenerator generator = new ProxyGenerator();
            Person person = generator.CreateClassProxy<Person>(new ChangeInterceptor(unitOfWork));

            person.Key = "MYFirstObject";
            person.IsMarried = true;
            person.IsMarried = false;

            Assert.That(unitOfWork.actionQueue.Count == 1);

        }
    }



    internal class ChangeInterceptor : IInterceptor
    {
        readonly IUnitOfWork unitOfWork;

        public ChangeInterceptor(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
            if (unitOfWork == null)
                throw new ArgumentNullException(nameof(unitOfWork));
        }

        public void Intercept(IInvocation invocation)
        {
            if (invocation == null)
                throw new ArgumentNullException(nameof(invocation));

            MethodInfo method = invocation.Method;

            IModel target = invocation.InvocationTarget as IModel;

            if (target == null)
            {
                return;
            }

            if (method.Name.StartsWith("set_", StringComparison.Ordinal))
            {
                string propertyName = method.Name.Substring(4, method.Name.Length - 4);
                object currentValue = target.GetType().GetProperty(propertyName).GetValue(target);
                object newValue = invocation.Arguments[0];

                if (!newValue.Equals(currentValue))
                {

                    if (string.IsNullOrEmpty(target.Key))
                    {
                        // New model.
                        unitOfWork.Create(target);
                    }
                    else
                    {
                        // Existing model.
                        unitOfWork.Update(target);
                    }
                }

            }
            invocation.Proceed();










        }
    }

    internal class CreateAction : BaseUnitOfWorkAction
    {
        readonly IRepositoryService repositoryService;

        public CreateAction(IModel model, IRepositoryService repositoryService) : base(model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (repositoryService == null)
                throw new ArgumentNullException(nameof(repositoryService));


            this.repositoryService = repositoryService;
        }

        public override void Execute()
        {
            IRepository<IModel> modelRepository = repositoryService.GetRepository<IModel>();
            modelRepository.Create();
        }
    }

    internal class DummyDataStore<TModel> : IDataStore<TModel> where TModel : IModel
    {
        public void Insert(TModel model)
        {
            throw new NotImplementedException();
        }

        public TModel Select(Expression<Func<TModel, bool>> whereConditions)
        {
            throw new NotImplementedException();
        }

        public void Update(TModel model)
        {
            throw new NotImplementedException();
        }
    }

    internal class RepositoryService : IRepositoryService
    {
        public IRepository<T> GetRepository<T>() where T : IModel
        {
            //return new Repository<T>(new DummyDataStore<T>());
            return null;
        }
    }

    internal class DeleteAction : BaseUnitOfWorkAction
    {
        public IModel Model { get; private set; }
        readonly IRepositoryService repositoryService;

        public DeleteAction(IModel model, IRepositoryService repositoryService) : base(model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (repositoryService == null)
                throw new ArgumentNullException(nameof(repositoryService));


            this.repositoryService = repositoryService;
        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }

    internal class UpdateAction : BaseUnitOfWorkAction
    {

        readonly IRepositoryService repositoryService;

        public UpdateAction(IModel model, IRepositoryService repositoryService) : base(model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));
            if (repositoryService == null)
                throw new ArgumentNullException(nameof(repositoryService));

            this.repositoryService = repositoryService;

        }

        public override void Execute()
        {
            throw new NotImplementedException();
        }
    }

    internal abstract class BaseUnitOfWorkAction : IUnitOfWorkAction
    {
        public IModel Model { get; private set; }

        public BaseUnitOfWorkAction(IModel model)
        {
            Model = model;
        }

        public abstract void Execute();
    }

    internal interface IUnitOfWork
    {
        void Create<T>(T model) where T : IModel;
        void Update<T>(T model) where T : IModel;
        void Delete<T>(T model) where T : IModel;

        void Commit();
        void Rollback();
    }

    public interface IUnitOfWorkAction
    {
        IModel Model { get; }
        void Execute();
    }

    internal class UnitOfWork : IUnitOfWork
    {
        readonly IRepositoryService repositoryService;

        public UnitOfWork(IRepositoryService repositoryService)
        {

            if (repositoryService == null)
                throw new ArgumentNullException(nameof(repositoryService));

            actionQueue = new Queue<IUnitOfWorkAction>();
            this.repositoryService = repositoryService;
        }

        public void Create<T>(T model) where T : IModel
        {
            if (!actionQueue.Any(a => a is CreateAction && a.Model.Equals(model)))
            {
                actionQueue.Enqueue(new CreateAction(model, repositoryService));
            }
        }


        public void Delete<T>(T model) where T : IModel
        {
            if (!actionQueue.Any(a => a is DeleteAction && a.Model.Equals(model)))
            {
                actionQueue.Enqueue(new DeleteAction(model, repositoryService));
            }
        }

        public void Update<T>(T model) where T : IModel
        {
            if (!actionQueue.Any(a => a is UpdateAction && a.Model.Equals(model)))
            {
                actionQueue.Enqueue(new UpdateAction(model, repositoryService));
            }
        }

        private object lockObject = new object();
        internal Queue<IUnitOfWorkAction> actionQueue;

        public void AddWork(IUnitOfWorkAction action)
        {
            if (!actionQueue.Contains(action))
            {
                actionQueue.Enqueue(action);
            }
        }

        public void Commit()
        {
            while (actionQueue.Count > 0)
            {
                lock (lockObject)
                {
                    IUnitOfWorkAction action = actionQueue.Dequeue();
                    action.Execute();
                }
            }
        }

        public void Rollback()
        {
            actionQueue.Clear();
        }

        internal static UnitOfWork GetInstance()
        {
            return new UnitOfWork(new RepositoryService());
        }
    }

    public interface IRepositoryService
    {
        IRepository<T> GetRepository<T>() where T : IModel;
    }
}
