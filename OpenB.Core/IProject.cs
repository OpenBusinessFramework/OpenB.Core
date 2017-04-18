using System;
using System.Collections.Generic;

namespace OpenB.Core
{
    public interface IProject
    {
        string Name { get; }
        string BaseFolder { get; }
    }
    
}