using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.CSharp;
using OpenB.Core;
using OpenB.Core.Utils;

namespace OpenB.Modeling
{
    public class ProjectModuleCreator
    {
        IList<ModelClassCreator> classBuilders;
        readonly Project project;
        readonly FormattedStringBuilder formattedStringBuilder;

        public static ProjectModuleCreator GetInstance(Project project)
        {
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            return new ProjectModuleCreator(new FormattedStringBuilder(), project);
        }
        internal ProjectModuleCreator(FormattedStringBuilder formattedStringBuilder, Project project)
        {
            if (formattedStringBuilder == null)
                throw new ArgumentNullException(nameof(formattedStringBuilder));
            if (project == null)
                throw new ArgumentNullException(nameof(project));

            this.project = project;
            this.classBuilders = new List<ModelClassCreator>();
            this.formattedStringBuilder = formattedStringBuilder;

        }

        public ModelClassCreator AddClass(ModelDefinition modelDefinition)
        {
            ModelClassCreator creator = new ModelClassCreator(formattedStringBuilder, modelDefinition);
            classBuilders.Add(creator);

            return creator;
        }
    }

    public class ModelClassCreator : IClassPartCreator
    {
        private ModelDefinition modelDefinition;
        private IList<ModelPropertyCreator> propertyCreators;
        readonly FormattedStringBuilder formattedStringBuilder;

        public ModelClassCreator(FormattedStringBuilder formattedStringBuilder, ModelDefinition modelDefinition)
        {
            this.formattedStringBuilder = formattedStringBuilder;
            if (formattedStringBuilder == null)
                throw new ArgumentNullException(nameof(formattedStringBuilder));
            this.modelDefinition = modelDefinition;

            propertyCreators = new List<ModelPropertyCreator>();
        }

        public ModelPropertyCreator AddProperty(PropertyDefinition propertyDefinition)
        {
            ModelPropertyCreator creator = new ModelPropertyCreator(formattedStringBuilder, propertyDefinition);
            propertyCreators.Add(creator);

            return creator;
        }

        public void Create()
        {
            throw new NotImplementedException();
        }
    }

    public class ModelPropertyCreator : IClassPartCreator
    {
        private PropertyDefinition propertyDefinition;

        readonly FormattedStringBuilder formattedStringBuilder;

        internal ModelPropertyCreator(FormattedStringBuilder formattedStringBuilder, PropertyDefinition propertyDefinition)
        {
            if (formattedStringBuilder == null)
                throw new ArgumentNullException(nameof(formattedStringBuilder));
            if (propertyDefinition == null)
                throw new ArgumentNullException(nameof(propertyDefinition));

            this.propertyDefinition = propertyDefinition;
            this.formattedStringBuilder = formattedStringBuilder;
        }

        public void Create()
        {
            string quantifier = (propertyDefinition.Cardinality == Cardinality.OneToMany) ? $"IList<{propertyDefinition.ModelDefinition.Name}>" : propertyDefinition.ModelDefinition.Name;

            formattedStringBuilder.AppendLine($"private {quantifier} _{propertyDefinition.Name};");
            formattedStringBuilder.AppendLine($"public virtual {quantifier} {propertyDefinition.Name}");
            formattedStringBuilder.AppendLine("{");
            formattedStringBuilder.LevelDown();
            formattedStringBuilder.AppendLine($"get {{ return _{propertyDefinition.Name}; }}");
            formattedStringBuilder.LevelDown();
            formattedStringBuilder.AppendLine("set");
            formattedStringBuilder.AppendLine("{");
            formattedStringBuilder.LevelDown();
            formattedStringBuilder.AppendLine($"if (!_{propertyDefinition.Name}.Equals(value))");
            formattedStringBuilder.AppendLine("{");
            formattedStringBuilder.LevelDown();
            formattedStringBuilder.AppendLine($"_{propertyDefinition.Name} = value;");
            formattedStringBuilder.AppendLine("IsDirty = true;");
            formattedStringBuilder.LevelUp();
            formattedStringBuilder.AppendLine("}");
            formattedStringBuilder.LevelUp();
            formattedStringBuilder.AppendLine("}");
            formattedStringBuilder.LevelUp();
            formattedStringBuilder.LevelUp();
            formattedStringBuilder.AppendLine("}");
        }
    }

    public interface IClassPartCreator
    {
        void Create();
    }

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
            if (definition == null)
                throw new ArgumentNullException(nameof(definition));

            if (definition is EncapsulatedModelDefinition)
            {
                throw new NotSupportedException($"Cannot create class for encapsulated model {definition.Name}.");
            }

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
                string propertySignature = PropertyNameFactory.GetPropertyName(propertyDefinition.Name, propertyDefinition.ModelDefinition,
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

            throw new NotSupportedException("Cannot operate on definitionflag combination.");
        }

        public void CompileAssembly(Project project)
        {
            foreach (ModelDefinition definition in project.ModelDefinitions)
            {
                string classString = CreateClassDefinition(definition);
                string fullFilePath = Path.Combine(project.ModelFolder, string.Concat(definition.Name, ".cs"));

                File.WriteAllText(fullFilePath, classString);
            }

            var codeProvider = new CSharpCodeProvider();
            var parameters = new CompilerParameters
            {
                GenerateExecutable = false,
                GenerateInMemory = false,
                OutputAssembly = string.Join(".", project.Name, ".models")
            };

            parameters.ReferencedAssemblies.Add("OpenB.Modeling.dll");
            parameters.ReferencedAssemblies.Add("OpenB.Core.dll");

            string[] fileNames = new DirectoryInfo(project.ModelFolder).GetFiles("*.cs").Select(f => f.FullName).ToArray();

            CompilerResults compilerResults = codeProvider.CompileAssemblyFromFile(parameters, fileNames);
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
                    null, new Object[] { key, name, description }, CultureInfo.InvariantCulture, null) as IModel;
        }
    }
}