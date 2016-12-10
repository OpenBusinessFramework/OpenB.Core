using System;

namespace OpenB.Modeling
{
    /// <summary>
    /// Encapsulated model defintion, encapsulating an existing .NET type. 
    /// 
    /// This definitiontype can be used to create complex definitions or use exisiting classes for definitions.
    /// </summary>
    public class EncapsulatedModelDefinition : ModelDefinition
    {
        public Type EncapsulatedType { get; private set; }

        public EncapsulatedModelDefinition(string key, string name, string description, Type encapsulatedType)
            : base(key, name, description)
        {
            EncapsulatedType = encapsulatedType;
        }
    }
}