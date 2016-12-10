using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Castle.DynamicProxy;
using NUnit.Framework;
using OpenB.Core;

namespace OpenB.Modeling.Test
{
    [TestFixture]
    public class ProxyTest
    {
        public class DemoModel : IModel
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

        [Test]
        public void DoSomething()
        {
           
        }
    }

    
}
