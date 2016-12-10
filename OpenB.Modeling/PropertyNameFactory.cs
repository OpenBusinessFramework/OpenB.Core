namespace OpenB.Modeling
{
    public static class PropertyNameFactory
    {
        public static string GetPropertyName(string propertyName, ModelDefinition modelDefinition, bool isRequired)
        {
            EncapsulatedModelDefinition encapsulatedModelDefinition =
                modelDefinition as EncapsulatedModelDefinition;

            if (encapsulatedModelDefinition != null && encapsulatedModelDefinition.EncapsulatedType.IsValueType)
            {
                var name = encapsulatedModelDefinition.EncapsulatedType.Name;

                return !isRequired ? string.Format("{0}?", name) : name;
            }
            else
            {
                return propertyName;
            }
        }
    }
}