using System;
using UnityEngine;

public class Entity : MonoBehaviour 
{
    private UtilObject utilObject = new UtilObject();
    [SerializeField] private string entityName;

    private void Start() 
    {
        EntityData entityData = utilObject.LoadEntityData(entityName);
        AddRequiredComponent(entityData);
    }

    public void AddRequiredComponent(EntityData entityData)
    {
        entityData.entityComponents.ForEach(entityComponent => 
        {
            gameObject.AddComponent(Type.GetType(entityComponent));
        });
    }
}