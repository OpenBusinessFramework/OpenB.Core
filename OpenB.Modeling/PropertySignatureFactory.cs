using System;

namespace OpenB.Modeling
{
    public class PropertySignatureFactory : IPropertySignatureFactory
    {
        public string GetSignature(string modelName, Cardinality cardinality)
        {
            switch (cardinality)
            {
                case Cardinality.OneToOne:
                    return modelName;
                 
                case Cardinality.OneToMany:
                    return string.Format("IList<{0}>", modelName);

                default:
                    throw new NotImplementedException();

            }
        }
    }
}