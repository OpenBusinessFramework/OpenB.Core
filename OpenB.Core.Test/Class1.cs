using System;
using System.Collections.Generic;
using NUnit.Framework;
using OpenB.Core.Integration;
using OpenB.Modeling;

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

        [Test]
        public void BuildAssemblyFromDefinitions()
        {
            EncapsulatedModelDefinition encapsulatedDateModelDefinition = new EncapsulatedModelDefinition("date", "date", "date", typeof(DateTime));
            EncapsulatedModelDefinition encapsulateStringModellDefinition = new EncapsulatedModelDefinition("string", "string", "string", typeof(string));
            EncapsulatedModelDefinition encapsulateIntegerModellDefinition = new EncapsulatedModelDefinition("integer", "integer", "integer", typeof(int));

            PropertyDefinition dateOfBirthDefinition = new PropertyDefinition("DateOfBirth", encapsulatedDateModelDefinition, Cardinality.OneToOne);
            PropertyDefinition cityDefinition = new PropertyDefinition("City", encapsulateStringModellDefinition, Cardinality.OneToOne, PropertyFlags.None);

            IList<PropertyDefinition> personProperties = new List<PropertyDefinition> { { dateOfBirthDefinition }, { cityDefinition } };
            ModelDefinition personDefinition = new ModelDefinition("Person", "Person", "Person", personProperties, DefinitionFlags.None);

            PropertyDefinition streetName = new PropertyDefinition("Street", encapsulateStringModellDefinition);
            PropertyDefinition houseNumber = new PropertyDefinition("HouseNumber", encapsulateIntegerModellDefinition);
            PropertyDefinition postalCode = new PropertyDefinition("PostalCode", encapsulateStringModellDefinition);

            IList<PropertyDefinition> addressProperties = new List<PropertyDefinition> { { streetName }, { houseNumber }, { postalCode } };
            ModelDefinition addressModelDefinition = new ModelDefinition("Address", "Address", "Address", addressProperties, DefinitionFlags.None);

            PropertyDefinition famliyAddress = new PropertyDefinition("Address", addressModelDefinition);
            PropertyDefinition familyMembersDefinition = new PropertyDefinition("Members", personDefinition, Cardinality.OneToMany);

            IList<PropertyDefinition> familyProperties = new List<PropertyDefinition> { { famliyAddress }, { familyMembersDefinition } };
            ModelDefinition familyModelDefinition = new ModelDefinition("Familiy", "Familiy", "Familiy", familyProperties, DefinitionFlags.None);

            IList<ModelDefinition> models = new List<ModelDefinition> { { personDefinition }, { addressModelDefinition }, { familyModelDefinition } };

            var baseFolder = AppDomain.CurrentDomain.BaseDirectory;
            Project project = new Project("Administration", models, baseFolder);

            ModelCreationService modelCreationService = new ModelCreationService("blaat");
            modelCreationService.CompileAssembly(project);
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
