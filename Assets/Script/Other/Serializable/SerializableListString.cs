using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SerializableListString
{
    public List<string> list = new List<string>();

    public SerializableListString(List<string> list)
    {
        this.list = list;
    }
}
