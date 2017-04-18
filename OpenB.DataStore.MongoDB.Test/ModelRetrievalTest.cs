using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using NUnit.Framework;
using OpenB.Core;
using OpenB.Core.ACL;
using OpenB.Modeling;
using OpenB.Modeling.BaseDefinitions;
using DateTime = System.DateTime;

namespace OpenB.DataStore.MongoDB.Test
{
    [TestFixture]
    public class ModelRetrievalTest
    {
        private class AuditableModel : IAuditableModel
        {
            [BsonRepresentation(BsonType.ObjectId)]
            public string Id { get; set; }

            public string Key { get; set; }

            public string Name { get; set; }
            public string Description { get; set; }
            public bool IsActive { get; set; }


            public AuditRegistration Created { get; set; }
            public AuditRegistration Modified { get; set; }
            public AuditRegistration LastAccessed { get; set; }
        }

        [Test]
        public void GetAuditableModel_ReturnsModel()
        {
            var userGroup = new UserGroup("MY_USERGROUP", "MyUserGroup", "My usergroup");
            var user = new User("MyUser", userGroup);

            IAuditableModel model = new AuditableModel
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Key = "MYFIRSTMODEL",
                Name = "MyFirstModel",
                IsActive = true,
                Description = "My first model",
                Created = new AuditRegistration(user, DateTime.Now),
                Modified = new AuditRegistration(user, DateTime.Now),
                LastAccessed = new AuditRegistration(user, DateTime.Now),
            };

            var dataStoreService = new MongoDataStoreService("localhost", "mydb");
            dataStoreService.CreateModel(model);
            IAuditableModel result = dataStoreService.GetModel<AuditableModel>("MYFIRSTMODEL");

            Assert.That(result, Is.Not.Null);
        }

        [Test]
        public void GetComplexModel_ReturnsModel()
        {
            PropertyDefinition propertyDefinition = new PropertyDefinition("MyFirstModelProperty",
            new IntegerDefinition());

            ModelDefinition definition = new ModelDefinition("MYFIRSTDEFININTION", "MyFirstDefinition",
                "My first definition", new List<PropertyDefinition>() { propertyDefinition }, DefinitionFlags.None);

            ModelFactory factory = new ModelFactory(new Project("MyFirstProject", null,null));
            IModel model = (IModel)factory.CreateInstance(definition, "KEY", "NAME", "DESCRIPTION");

            var userGroup = new UserGroup("MY_USERGROUP", "MyUserGroup", "My usergroup");
            var user = new User("MyUser", userGroup);

            

            var dataStoreService = new MongoDataStoreService("localhost", "mydb");
            dataStoreService.CreateModel(model);
            IAuditableModel result = dataStoreService.GetModel<AuditableModel>("MYFIRSTMODEL");

            Assert.That(result, Is.Not.Null);
        }
    }
}