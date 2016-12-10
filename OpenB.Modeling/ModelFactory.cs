using System;
using OpenB.Core;

namespace OpenB.Modeling
{
    public class ModelFactory : IModelFactory 
    {
        private readonly ModelCreationService _modelCreationService;

        public ModelFactory(Project project)
        {           
            _modelCreationService = new ModelCreationService(project.Name);            
        }

        public object CreateInstance(ModelDefinition definition, string key, string name, string description)
        {
            return _modelCreationService.InstantiateModel(definition, key, name, description);
        }

        public object ReadInstance(string definitionKey, string modelKey)
        {
            if (string.IsNullOrEmpty(definitionKey))
            {
                throw new ArgumentNullException(nameof(definitionKey));
            }

            if (string.IsNullOrEmpty(modelKey))
            {
                throw new ArgumentNullException(nameof(modelKey));
            }
                      
            throw new NotImplementedException();
        }
    }
}