using System.Runtime.InteropServices.ComTypes;

namespace OpenB.Core
{
    public class UserContext
    {
        private readonly IModelFactory _modelFactory;

        public UserContext(IModelFactory modelFactory)
        {
            _modelFactory = modelFactory;
        }

        public T GetInstance<T>(string definitionKey, string modelKey) where T : class, IModel
        {
           object objInstance =  _modelFactory.ReadInstance(definitionKey, modelKey);

            return objInstance as T;
        }
    }
}