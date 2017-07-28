namespace OpenB.Core.Integration
{
    public interface IRepository<T> : IRepository where T : IModel
    {   
        /// <summary>
        /// Returns an new instance of the model.
        /// </summary>
        /// <returns></returns>
        T Create();
        void Update(T model);
        T GetByKey(string key);
        void DeleteByKey(string key);      
    }
}
