namespace OpenB.Core.Data
{
    public interface IDataStoreService
    {
        /// <summary>
        /// Gets a model from the repository.
        /// </summary>
        /// <typeparam name="T">Type of the model to retrieve.</typeparam>
        /// <param name="key">Unique key referencing the model.</param>
        /// <returns>The requested model.</returns>
        T GetModel<T>(string key) where T : IModel;
    }
}