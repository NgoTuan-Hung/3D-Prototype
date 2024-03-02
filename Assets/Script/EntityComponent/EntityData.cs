using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EntityData
{
    public string entityName;
    public List<String> entityComponents;
    public EntityData(string entityName, List<string> entityComponents)
    {
        this.entityName = entityName;
        this.entityComponents = entityComponents;
    }
}
