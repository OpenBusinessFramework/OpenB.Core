namespace OpenB.Core
{
    /// <summary>
    /// 
    /// </summary>
    public interface IModelFactory
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="definitionKey"></param>
        /// <param name="modelKey"></param>
        /// <returns></returns>
        object ReadInstance(string definitionKey, string modelKey);
    }
}