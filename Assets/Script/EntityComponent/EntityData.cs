using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityData
{
    public string entityName;
    public List<String> entityComponents;
    public EntityData(string entityName, List<String> entityComponents)
    {
        this.entityName = entityName;
        this.entityComponents = entityComponents;
    }

    public EntityData() {}
}
