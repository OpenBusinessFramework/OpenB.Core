using System;
using System.Collections.Generic;
using System.IO;
using OpenB.Core;
using OpenB.Modeling;

public class Project : IProject
{
    public string Name { get; private set; }
    public IList<ModelDefinition> ModelDefinitions { get; private set; }
    private string modelFolder;
    public string ModelFolder
    {
        get
        {
            if (modelFolder == null)
            {
                modelFolder = Path.Combine(BaseFolder, "Models", Name);
            }
            return modelFolder;
        }
    }
    public string BaseFolder { get; private set; }

    public Project(string name, IList<ModelDefinition> modelDefinitions, string baseFolder)
    {
        if (name == null)
            throw new ArgumentNullException(nameof(name));
        if (modelDefinitions == null)
            throw new ArgumentNullException(nameof(modelDefinitions));

        Name = name;
        ModelDefinitions = modelDefinitions;
        BaseFolder = baseFolder;
    }

    public static Project Create(string name, string baseFolder)
    {
        // Create Modeldefinitions from xml.
        throw new NotImplementedException();

    }

  


}

