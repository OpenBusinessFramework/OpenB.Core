using System;
using System.CodeDom.Compiler;
using System.Globalization;
using System.Reflection;
using System.Runtime.Remoting.Channels;
using System.Text;
using Microsoft.CSharp;
using OpenB.Core;
using OpenB.Core.Utils;

namespace OpenB.Modeling
{
    public class ModelCreationService
    {
        private readonly string _defaultNamespace;
        private readonly PropertyCreationService _propertyCreationService;
        private FormattedStringBuilder _classStringBuilder;

        public ModelCreationService(string defaultNamespace)
        {
            _classStringBuilder = new FormattedStringBuilder();
            _defaultNamespace = defaultNamespace;
            _propertyCreationService = new PropertyCreationService(new PropertySignatureFactory());
        }

        public string CreateClassDefinition(ModelDefinition definition)
        {
            string baseClass = GetBaseClass(definition.DefinitionFlags);
            
            _classStringBuilder = new FormattedStringBuilder();
            _classStringBuilder.AppendLine("using System;");
            _classStringBuilder.AppendLine("using OpenB.Core;");
            _classStringBuilder.AppendLine("using OpenB.Modeling;");
            _classStringBuilder.AppendLine();
            _classStringBuilder.AppendLine(string.Format("namespace {0}", _defaultNamespace));
            _classStringBuilder.AppendLine("{");
            _classStringBuilder.LevelDown();
            _classStringBuilder.AppendLine(string.Format("public class {0} : {1}", definition.Name, baseClass));
            _classStringBuilder.AppendLine("{");
            _classStringBuilder.LevelDown();
            _classStringBuilder.AppendLine("public bool IsDirty { get; private set; }");

            foreach (PropertyDefinition propertyDefinition in definition.Properties)
            {
               var propertySignature = PropertyNameFactory.GetPropertyName(propertyDefinition.Name, propertyDefinition.ModelDefinition,
                    (propertyDefinition.PropertyFlags & PropertyFlags.Required) == PropertyFlags.Required);

                _propertyCreationService.CreatePropertyDefinition(propertyDefinition, propertySignature,
                    _classStringBuilder);
            }

            _classStringBuilder.AppendLine(
                string.Format("public {0}(string key, string name, string description) : base(key, name, description)",
                    definition.Name));
            _classStringBuilder.AppendLine("{}");

            _classStringBuilder.LevelUp();
            _classStringBuilder.AppendLine("}");
            _classStringBuilder.LevelUp();
            _classStringBuilder.AppendLine("}");

            return _classStringBuilder.ToString();
        }

        private string GetBaseClass(DefinitionFlags definitionFlags)
        {
            if ((definitionFlags & DefinitionFlags.None) == DefinitionFlags.None)
           {
               return "BaseModel";
           }

            throw new NotSupportedException("Cannot operate on definitionflag combination");
        }

        public IModel InstantiateModel(ModelDefinition definition, string key, string name, string description)
        {
            string classString =
                CreateClassDefinition(definition);

            var codeProvider = new CSharpCodeProvider();
            var parameters = new CompilerParameters
            {
                GenerateExecutable = false,
                GenerateInMemory = true,
                OutputAssembly = _defaultNamespace
            };
            parameters.ReferencedAssemblies.Add("OpenB.Modeling.dll");
            parameters.ReferencedAssemblies.Add("OpenB.Core.dll");

            CompilerResults compilerResults = codeProvider.CompileAssemblyFromSource(parameters, classString);

            if (compilerResults.Errors.HasErrors)
            {
                var errorStringBuilder = new StringBuilder("There were errors compiling type:");
                foreach (object error in compilerResults.Errors)
                {
                    errorStringBuilder.AppendLine(error.ToString());
                }

                throw new Exception(errorStringBuilder.ToString());
            }
            return
                compilerResults.CompiledAssembly.CreateInstance(
                    string.Format("{0}.{1}", _defaultNamespace, definition.Name), false, BindingFlags.CreateInstance,
                    null, new Object[] {key, name, description}, CultureInfo.InvariantCulture, null) as IModel;
        }
    }
}