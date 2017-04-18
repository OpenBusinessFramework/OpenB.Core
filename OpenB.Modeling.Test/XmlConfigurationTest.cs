using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Xml;
using NUnit.Framework;
using OpenB.Core;
using OpenB.Core.ACL;
using OpenB.DataStore.MongoDB;
using OpenB.Modeling.BaseDefinitions;
using Rhino.Mocks;

namespace OpenB.Modeling.Test
{
    [TestFixture]
    public class XmlConfigurationTest
    {
        [Test]
        public void ReadInstance_ModelCanBeFound()
        {
            MockRepository mockRepository = new MockRepository();

            User user = new User("TestUser", new UserGroup("myUserGroup", "My usergroup", "My usergroup description"));

            ModelDefinitionRepository repository = CreateRepository();
            
            IModelRepository modelRepository = mockRepository.Stub<IModelRepository>();
            mockRepository.ReplayAll();
            
            ModelFactory factory = new ModelFactory(new Project("MyFirstProject", null, null));
            IModel model = (IModel)factory.CreateInstance(repository.ModelDefinitions.First(), "KEY", "NAME", "DESCRIPTION");

            var dataStoreService = new MongoDataStoreService("localhost", "mydb");
            dataStoreService.CreateModel(model);

         //   IModel result = dataStoreService.GetModel("");


        }

        private static ModelDefinitionRepository CreateRepository()
        {
            ModelDefinition scrumProjectDefinition = new ModelDefinition("MY_AGILE_SCRUM_PROJECT", "ScrumProject",
                "Definition for an agile scrum project", new List<PropertyDefinition>(), DefinitionFlags.Authorizable)
            {
                IsActive = true
            };

            ModelDefinition userStoryDefinition = new ModelDefinition("MY_SCRUM_USERSTORY", "UserStory",
                "Definition for a scrum user story", new List<PropertyDefinition>(), DefinitionFlags.Authorizable)
            {
                IsActive = true
            };

            ModelDefinition scrumTaskDefinition = new ModelDefinition("MY_SCRUM_TASK_DEFINITION", "ScrumTask",
                "Definition for a task", new List<PropertyDefinition>(), DefinitionFlags.Authorizable);

            scrumTaskDefinition.Properties.Add(new PropertyDefinition("IsDone", new BooleanDefinition()));

            ModelDefinition scrumSprintDefinition = new ModelDefinition("MY_SCRUM_SPRINT_DEFINITION",
                "ScrumSprint", "Definition for an agile scrum sprint", new List<PropertyDefinition>(),
                DefinitionFlags.Authorizable);

            scrumSprintDefinition.Properties.Add(new PropertyDefinition("StartDate", new DateTimeDefinition()));
            scrumSprintDefinition.Properties.Add(new PropertyDefinition("EndDate", new DateTimeDefinition()));
            scrumSprintDefinition.Properties.Add(new PropertyDefinition("Userstories", userStoryDefinition,
                Cardinality.OneToMany));
          
            scrumProjectDefinition.Properties.Add(new PropertyDefinition("Backlog", userStoryDefinition, Cardinality.OneToMany));
            scrumProjectDefinition.Properties.Add(new PropertyDefinition("Sprints", scrumSprintDefinition,
                Cardinality.OneToMany));

            userStoryDefinition.Properties.Add(new PropertyDefinition("StoryPoints", new IntegerDefinition()));
            userStoryDefinition.Properties.Add(new PropertyDefinition("Tasks", scrumTaskDefinition, Cardinality.OneToMany));

            IList<ModelDefinition> definitions = new List<ModelDefinition>();
            definitions.Add(scrumTaskDefinition);
            definitions.Add(scrumProjectDefinition);
            definitions.Add(scrumTaskDefinition);
            definitions.Add(scrumSprintDefinition);

            ModelDefinitionRepository modelDefinitionRepository = new ModelDefinitionRepository(definitions);

            return modelDefinitionRepository;
        }
    }
}