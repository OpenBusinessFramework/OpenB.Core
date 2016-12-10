using System;

namespace OpenB.Core
{
    public class Project
    {
        public string Name { get; private set; }

        public Project(string name)
        {
            Name = name;
        }

        public static Project GetInstance(string name)
        {
            throw new NotImplementedException();
        }
    }
}