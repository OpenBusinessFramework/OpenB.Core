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
}
