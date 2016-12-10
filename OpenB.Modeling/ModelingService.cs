using System;
using System.Collections;
using OpenB.Core;
using OpenB.Core.ACL;

namespace OpenB.Modeling
{
    public class ModelingService
    {
        private readonly User _user;
        private readonly IModelRepository _modelRepository;
        private readonly ModelDefinitionRepository _modelDefinitionRepository;

        public ModelingService(User user, IModelRepository modelRepository, ModelDefinitionRepository modelDefinitionRepository)
        {
            if (user == null) throw new ArgumentNullException(nameof(user));
            if (modelRepository == null) throw new ArgumentNullException(nameof(modelRepository));
            if (modelDefinitionRepository == null) throw new ArgumentNullException(nameof(modelDefinitionRepository));

            _user = user;
            _modelRepository = modelRepository;
            _modelDefinitionRepository = modelDefinitionRepository;
        }

        public T ReadInstance<T>(string key) where T :class, IModel
        {
           ModelDefinition definition = _modelDefinitionRepository.GetDefinitionForType(typeof (T));

            if (definition == null)
            {
                throw new InvalidOperationException("Cannot instantiate a type for which is no definition available");
            }

            if (!definition.IsActive)
            {
                throw new InvalidOperationException("Cannot instantiate an instance of an inactive model definition.");
            }

            object instanceRepresentation = _modelRepository.GetModel(definition, key);

            T instance = _modelRepository.GetModel(definition, key) as T;

            if (instanceRepresentation != null && instance == null)
            {
                throw new NullReferenceException(string.Format("Instance with key {0} not found for definition {1}", key, definition.Name));
            }

            return instance;
        }

        public void UpdateInstance()
        {

        }

        public void DeleteInstance()
        {
            
        }
    }

    public interface IModelRepository
    {
        object GetModel(ModelDefinition definition, string key);
    }
}