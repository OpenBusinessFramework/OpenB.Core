using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using NUnit.Framework;
using OpenB.Core;
using OpenB.Modeling.BaseDefinitions;

namespace OpenB.Modeling.Test
{
    [TestFixture]
    public class CompilerTest
    {
        [Test]
        public void ReadModelConfigurationAndParse()
        {
            ModelConfigurationReader configurationReader = new ModelConfigurationReader();
            string folderPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "xml");
            var models = configurationReader.ReadFolder(folderPath);
        }

        [Test]
        public void Compile_SimpleModel_KeyNameDescription_AreFilled()
        {
            PropertyDefinition propertyDefinition = new PropertyDefinition("MyFirstModelProperty",
                new IntegerDefinition());

            ModelDefinition definition = new ModelDefinition("MYFIRSTDEFININTION", "MyFirstDefinition",
                "My first definition", new List<PropertyDefinition>() { propertyDefinition }, DefinitionFlags.None);

            

            ModelFactory factory = new ModelFactory(new Project("MyFirstProject", null, null));
            IModel model = (IModel)factory.CreateInstance(definition, "KEY", "NAME", "DESCRIPTION");

            Assert.That(model.GetType().Assembly.GetName().Name, Is.EqualTo("MyFirstProject"));
            Assert.That(model.GetType().Name, Is.EqualTo("MyFirstDefinition"));

            Assert.That(model.Key, Is.EqualTo("KEY"));
            Assert.That(model.Name, Is.EqualTo("NAME"));
            Assert.That(model.Description, Is.EqualTo("DESCRIPTION"));
        }
    }

    internal class ModelConfigurationReader
    {
        IDictionary<string, ModelDefinition> readDefinitions;

        public ModelConfigurationReader()
        {
            readDefinitions = new Dictionary<string, ModelDefinition>();
        }

        internal PropertyDefinition ReadProperty(XmlNode propertyNode)
        {
            if (propertyNode == null)
                throw new ArgumentNullException(nameof(propertyNode));

            string name = propertyNode.Attributes["name"].Value;
            XmlAttribute cardinalityAttribute = propertyNode.Attributes["cardinality"];
            Cardinality cardinality = Cardinality.OneToOne;

            if (cardinalityAttribute != null)
            {
                cardinality = (Cardinality)Enum.Parse(typeof(Cardinality), cardinalityAttribute.Value);
            }

            string modelDefinition = propertyNode.Attributes["definition"].Value;
            ModelDefinition definition = readDefinitions[modelDefinition];

            return new PropertyDefinition(name, definition, cardinality);
        }

        internal IEnumerable<ModelDefinition> ReadFolder(string folderPath)
        {
            IList<ModelDefinition> resultList = new List<ModelDefinition>();

            DirectoryInfo folderInfo = new DirectoryInfo(folderPath);

            if (!folderInfo.Exists)
            {
                throw new NotSupportedException("$Folder {folderPath} not found.");
            }

            foreach (FileInfo fileInfo in folderInfo.GetFiles("*.ModelDefinition.xml"))
            {
                resultList.Add(ReadFile(fileInfo.FullName));
            }

            return resultList;
        }

        private ModelDefinition CreateDefinition(XmlNode rootNode, XmlNamespaceManager xmlNamespaceManager)
        {
            switch (rootNode.LocalName)
            {
                case "modeldefinition":
                    return CreateModelDefinition(rootNode, xmlNamespaceManager);
               case "encapsulatedmodeldefinition":
                    return CreatEncapulatedDefinition(rootNode, xmlNamespaceManager);
                
                default:
                    throw new NotSupportedException($"Cannot read definition of type {rootNode.LocalName}.");
            }
        }

        private ModelDefinition CreatEncapulatedDefinition(XmlNode rootNode, XmlNamespaceManager xmlNamespaceManager)
        {
            string key = rootNode["key"].InnerText;
            string description = rootNode["description"].InnerText;
            string name = rootNode["name"].InnerText;
            string encapsulatedType = rootNode["encapsulatedType"].InnerText;
            

            return new EncapsulatedModelDefinition(key, name, description, Type.GetType(encapsulatedType));
        }

        public ModelDefinition ReadFile(string filePath)
        {
            using (FileStream fileStream = File.OpenRead(filePath))
            {
                XmlDocument xmlDocument = new XmlDocument();
                XmlNamespaceManager xmlNameSpaceManager = new XmlNamespaceManager(xmlDocument.NameTable);
                xmlNameSpaceManager.AddNamespace("openb", "http://schemas.openb.org/modeldefinition");

                xmlDocument.Load(fileStream);

                ModelDefinition definition = CreateDefinition(xmlDocument.DocumentElement, xmlNameSpaceManager);
                readDefinitions.Add(definition.Key, definition);

                return definition;
            }

        }

        private ModelDefinition CreateModelDefinition(XmlNode definitionRootNode, XmlNamespaceManager xmlNameSpaceManager)
        {
            string key = definitionRootNode["key"].InnerText;
            string description = definitionRootNode["description"].InnerText;
            string name = definitionRootNode["name"].InnerText;

            IList<PropertyDefinition> propertyDefinitions = new List<PropertyDefinition>();

            XmlNodeList properties = definitionRootNode.SelectNodes("properties/property", xmlNameSpaceManager);
            if (properties != null)
            {
                foreach (XmlNode propertyNode in properties)
                {
                    propertyDefinitions.Add(ReadProperty(propertyNode));
                }
            }

            return new ModelDefinition(key, name, description, propertyDefinitions, DefinitionFlags.None);
        }
    }
}