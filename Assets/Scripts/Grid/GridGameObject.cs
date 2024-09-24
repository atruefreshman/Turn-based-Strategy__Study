using TMPro;
using UnityEngine;

public class GridGameObject : MonoBehaviour
{
    [SerializeField] private TextMeshPro textMeshPro;

    private GridObject gridObject;

    public void SetGridObject(GridObject gridObject)
    {
        this.gridObject = gridObject;
    }

    private void Update()
    {
        textMeshPro.SetText(gridObject.ToString());
    }
}
