using System;
using OpenB.Core;

namespace OpenB.Modeling
{
    public abstract class BaseModel : IModel
    {
        public string Key { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        public bool IsActive { get; set; }

        protected BaseModel(string key, string name, string description)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (name == null) throw new ArgumentNullException(nameof(name));
            if (description == null) throw new ArgumentNullException(nameof(description));

            Key = key;
            Name = name;
            Description = description;
            IsActive = false;
        }
    }
}