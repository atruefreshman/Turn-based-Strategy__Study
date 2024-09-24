using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSyatemVisualSingle : MonoBehaviour
{
    [SerializeField]MeshRenderer meshRenderer;

    public void Show(Material material) 
    {
        meshRenderer.material = material;
        meshRenderer.enabled = true;
    }

    public void Hide() 
    {
        meshRenderer.enabled=false;
    }
}
