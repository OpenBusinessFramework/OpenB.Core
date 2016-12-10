using System;

namespace OpenB.Modeling
{
    public class PropertyDefinition
    {
        public PropertyFlags PropertyFlags { get; private set; }
        public string Name { get; private set; }
        public ModelDefinition ModelDefinition { get; private set; }
        public Cardinality Cardinality { get; set; }

        public PropertyDefinition(string name, ModelDefinition modelDefinition) : this(name, modelDefinition, Cardinality.OneToOne)
        {
            
        }

        public PropertyDefinition(string name, ModelDefinition modelDefinition, Cardinality cardinality)
        {
            if (modelDefinition == null)
                throw new ArgumentNullException(nameof(modelDefinition));

            if (string.IsNullOrEmpty(name))
            {
                throw new NotSupportedException("Name cannot be empty");
            }

            if (name.Contains(" "))
            {
                throw new NotSupportedException("Property name cannot contain whitespaces");
            }

            Name = name;
            ModelDefinition = modelDefinition;
            Cardinality = cardinality;
        }

        public PropertyDefinition(string name, ModelDefinition modelDefinition, Cardinality cardinality, PropertyFlags propertyFlags) : this(name, modelDefinition, cardinality)
        {
            PropertyFlags = propertyFlags;
        }
    }

    public enum Cardinality
    {
        OneToOne,
        OneToMany,
        ManyToOne,
        ManyToMany
    }

    [Flags]
    public enum PropertyFlags
    {
        None,
        Required
    }
}