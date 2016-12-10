using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace OpenB.Modeling
{
    public class ModelDefinitionRepository
    {
        public IEnumerable<ModelDefinition> ModelDefinitions { get; }

        public ModelDefinitionRepository(IEnumerable<ModelDefinition> modelDefinitions)
        {
            if (modelDefinitions == null) throw new ArgumentNullException(nameof(modelDefinitions));
            ModelDefinitions = modelDefinitions;
        }

        public ModelDefinition GetDefinitionForType(Type type)
        {
            string modelDefinitionName = type.Name;

            IEnumerable<ModelDefinition> foundDefinitions = ModelDefinitions.Where(md => md.Name.Equals(modelDefinitionName)).ToList();

            if (!foundDefinitions.Any())
            {
                throw new NotSupportedException(string.Format("Cannot find definition for type {0}", type.FullName));
            }

            return foundDefinitions.Single();
        }

        public XmlDocument ToXml()
        {
            XmlDocument document = new XmlDocument();
            
                using (XmlWriter xmlWriter = document.CreateNavigator().AppendChild())
                {
                    xmlWriter.WriteStartElement("ModelDefinitions");

                    foreach (var modelDefinition in ModelDefinitions)
                    {
                        xmlWriter.WriteStartElement("ModelDefinition");
                        xmlWriter.WriteAttributeString("Key", modelDefinition.Key);
                        xmlWriter.WriteAttributeString("Name", modelDefinition.Name);

                        xmlWriter.WriteElementString("Description", modelDefinition.Description);

                        if (modelDefinition.Properties.Any())
                        {
                            xmlWriter.WriteStartElement("PropertyDefinitions");
                            foreach (PropertyDefinition propertyDefinition in modelDefinition.Properties)
                            {
                                xmlWriter.WriteStartElement("PropertyDefinition");
                                xmlWriter.WriteAttributeString("Name", propertyDefinition.Name);
                                xmlWriter.WriteAttributeString("Definition", propertyDefinition.ModelDefinition.Key);
                                xmlWriter.WriteAttributeString("Flags", propertyDefinition.PropertyFlags.ToString());
                                xmlWriter.WriteAttributeString("Cardinality", propertyDefinition.Cardinality.ToString());
                               
                            xmlWriter.WriteEndElement();
                            }
                             xmlWriter.WriteEndElement();
                        }    

                        xmlWriter.WriteEndElement();

                    
                    }
                    xmlWriter.WriteEndElement();

                    xmlWriter.Flush();


                }
                return document;
            }
        
    }
}