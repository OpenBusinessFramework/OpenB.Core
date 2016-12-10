using System.Collections.Generic;

namespace OpenB.Modeling
{
    public class AuthorizableModelDefinition : ModelDefinition
    {
        public AuthorizableModelDefinition(string key, string name, string description,
            IList<PropertyDefinition> properties) : base(key, name, description, properties, DefinitionFlags.Authorizable)
        {
        }
    }
}