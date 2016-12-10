using System.Collections.Generic;
using System.Xml;
using NUnit.Framework;
using OpenB.Core;
using OpenB.Core.ACL;
using OpenB.Modeling.BaseDefinitions;
using Rhino.Mocks;

namespace OpenB.Modeling.Test
{
    public class TestModel : BaseModel 
    {
        public TestModel(string key, string name, string description) : base(key, name, description)
        {
        }
    }

    [TestFixture]
    public class ModelingServiceTest
    {
        [Test]
        public void ReadInstance_ModelCanBeFound()
        {
            MockRepository mockRepository = new MockRepository();

            User user = new User("TestUser", new UserGroup("myUserGroup", "My usergroup", "My usergroup description"));

            var modelDefinition = CreateModelDefinition();

            TestModel resultModel = new TestModel("Demo", "Name", "Description");
           
            IList<ModelDefinition> definitions = new List<ModelDefinition> {modelDefinition};

            ModelDefinitionRepository modelDefinitionRepository = new ModelDefinitionRepository(definitions);

            IModelRepository modelRepository = mockRepository.Stub<IModelRepository>();
            modelRepository.Stub(m => m.GetModel(modelDefinition, "Demo")).Return(resultModel);

            ModelingService service = new ModelingService(user, modelRepository, modelDefinitionRepository);
            mockRepository.ReplayAll();

            TestModel model = service.ReadInstance<TestModel>("Demo");

            Assert.That(model, Is.Not.Null);
            Assert.That(model, Is.EqualTo(resultModel));
        }

        private static ModelDefinition CreateModelDefinition()
        {
            ModelDefinition modelDefinition = new ModelDefinition("MyDefinition", "TestModel",
                "My definition description", new List<PropertyDefinition>(), DefinitionFlags.Authorizable)
            {
                IsActive = true
            };

            modelDefinition.Properties.Add(new PropertyDefinition("Count", new IntegerDefinition(), Cardinality.OneToOne, PropertyFlags.Required));
            return modelDefinition;
        }
    }
}