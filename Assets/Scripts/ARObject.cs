using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARObject : MonoBehaviour
{
    private bool IsSelected;    // 객체의 선택 여부
    private List<MeshRenderer> childMeshRenderers = new List<MeshRenderer>();
    private List<Material> originalMaterials = new List<Material>();

    // UnlitBlack material
    public Material unlitBlackMaterial;

    public bool Selected
    {
        get
        {
            return this.IsSelected;
        }
        set
        {
            IsSelected = value;
            UpdateMaterialColor();
        }
    }

    private void Awake()
    {
        foreach (MeshRenderer childMeshRenderer in GetComponentsInChildren<MeshRenderer>())
        {
            childMeshRenderers.Add(childMeshRenderer);
            originalMaterials.Add(childMeshRenderer.material);
        }

        // Load the UnlitBlack material from the specified path
        unlitBlackMaterial = Resources.Load<Material>("Packages/com.unity.xr.arfoundation/Assets/Materials/UnlitBlack");
        if (unlitBlackMaterial == null)
        {
            Debug.LogError("UnlitBlack material not found at the specified path.");
        }
    }

    private void UpdateMaterialColor()
    {
        if (IsSelected)
        {
            foreach (MeshRenderer childMeshRenderer in childMeshRenderers)
            {
                childMeshRenderer.material = unlitBlackMaterial;
            }
        }
        else
        {
            for (int i = 0; i < childMeshRenderers.Count; i++)
            {
                childMeshRenderers[i].material = originalMaterials[i];
            }
        }
    }
}
