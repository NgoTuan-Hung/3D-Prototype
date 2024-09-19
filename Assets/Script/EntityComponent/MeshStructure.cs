using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class MeshStructure : MonoBehaviour
{
    private MeshRenderer[] meshRenderers;
    private SkinnedMeshRenderer[] skinnedMeshRenderers;
    [SerializeField] private bool meshInChildren = false;
    [SerializeField] private bool useSkinnedMesh = false;
    private void Awake() 
    {
        if (useSkinnedMesh)
        {
            if (meshInChildren)
            {
                skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            } else skinnedMeshRenderers = GetComponents<SkinnedMeshRenderer>();
        }
        else
        {
            if (meshInChildren)
            {
                meshRenderers = GetComponentsInChildren<MeshRenderer>();
            } else meshRenderers = GetComponents<MeshRenderer>();
        }
    }
}
