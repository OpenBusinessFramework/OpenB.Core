namespace OpenB.Core
{
    public interface IModel
    {
        string Key { get; }
        string Name { get; }
        string Description { get; }
        
        bool IsActive { get; set; }
    }
}