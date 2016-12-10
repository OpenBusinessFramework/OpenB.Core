namespace OpenB.Modeling.BaseDefinitions
{
    public class BooleanDefinition : EncapsulatedModelDefinition
    {
        public BooleanDefinition() : base("SYS_BOOLEAN", "BooleanDefinition", "Represents a boolean value.", typeof(System.Boolean))
        {
        }
    }
}