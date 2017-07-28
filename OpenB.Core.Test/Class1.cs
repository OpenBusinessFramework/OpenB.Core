using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenB.Core.Integration;

namespace OpenB.Core.Test
{
    [TestFixture]
    public class Class1
    {
        [Test]
        public void ApplicationInitializationTest()
        {
            AppDomainSetup appDomainSetup = new AppDomainSetup();
            appDomainSetup.ApplicationName = "PersonalFinance";

            AppDomain appDomain = AppDomain.CreateDomain("PersonalFinance", null, appDomainSetup);
            IRepository<Person> personRepository = new XmlRepository<Person>();

            IList<IRepository> repositories = new List<IRepository> { personRepository };

            DomainContext domainContext = new DomainContext("PersonalFinance", appDomain, repositories);
            IList<DomainContext> domainContexts = new List<DomainContext> { domainContext };

            ApplicationContext applicationContext = new ApplicationContext(domainContexts);

        }

       
    }



    internal class Person : IModel
    {
        public string Description
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public bool IsActive
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public string Key
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string Name
        {
            get
            {
                throw new NotImplementedException();
            }
        }
    }
}
