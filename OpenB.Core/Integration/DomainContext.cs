using System;
using System.Collections.Generic;

namespace OpenB.Core.Integration
{
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
}
