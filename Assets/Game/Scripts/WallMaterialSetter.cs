using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallMaterialSetter : MonoBehaviour
{
    public void SetMaterials(Material mainMaterial, Material edgeMaterial, Material darkMaterial)
    {
        MeshRenderer[] meshRenderers = GetComponentsInChildren<MeshRenderer>();
        for (int i = 0; i < meshRenderers.Length; i++)
        {
            Material[] materials = meshRenderers[i].sharedMaterials;
            materials[0] = meshRenderers[i].CompareTag("Dark Brick") ? darkMaterial : mainMaterial;
            materials[1] = edgeMaterial;
            meshRenderers[i].sharedMaterials = materials;
        }
    }
}
