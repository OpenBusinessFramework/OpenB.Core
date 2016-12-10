using System;
using System.Collections.Generic;
using OpenB.Core;
using OpenB.Core.ACL;

namespace OpenB.Modeling
{
    [Flags]
    public enum DefinitionFlags
    {
        None = 0 ,
        Authorizable =  1,
        Versioned = 2
    }

    /// <summary>
    ///     Contains a definition for this model.
    /// </summary>
    public class ModelDefinition
    {
        public DefinitionFlags DefinitionFlags { get; private set; }

        public ModelDefinition(string key, string name, string description, IList<PropertyDefinition> properties, DefinitionFlags definitionFlags)
            : this(key, name, description)
        {
            DefinitionFlags = definitionFlags;
            Properties = properties;
        }

        protected ModelDefinition(string key, string name, string description)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(key));

            if (name.Contains(" "))
            {
                throw new NotSupportedException("Model definition name cannot contain whitespaces");
            }

            Description = description;
            Name = name;
            Key = key;

            Created = DateTime.Now;
            Modified = DateTime.Now;
            IsActive = false;
        }

        public bool IsActive { get; set; }

        public DateTime Modified { get; set; }

        public DateTime Created { get; set; }

        public string Key { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public IList<PropertyDefinition> Properties { get; private set; }
    }
}