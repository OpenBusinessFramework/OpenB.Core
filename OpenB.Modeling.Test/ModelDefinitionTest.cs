using System.Collections.Generic;
using NUnit.Framework;

namespace OpenB.Modeling.Test
{
    [TestFixture]
    public class ModelDefinitionTest
    {
        [Test]
        public void CreateSimpleModelWithEncapsulatedModelDefinition()
        {
            EncapsulatedModelDefinition encapsulatedModelDefinition = new EncapsulatedModelDefinition("STRING", "string", "string", typeof(string));
            PropertyDefinition propertyDefinition = new PropertyDefinition("MYFIRSTMODELPROPERTY",
                encapsulatedModelDefinition);

            ModelDefinition definition = new ModelDefinition("MYFIRSTDEFININTION", "MyFirstDefinition",
                "My first definition", new List<PropertyDefinition>() {propertyDefinition}, DefinitionFlags.None);

            Assert.That(definition.Properties.Count, Is.EqualTo(1));
        }


    }
}