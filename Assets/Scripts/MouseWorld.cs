using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    public static MouseWorld Instance { get; private set; }

    public Vector3 GetMouseWorldPosition()
        => transform.position;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, float.MaxValue, 1 << LayerMask.NameToLayer("MousePlane")))
        {
            transform.position = hitInfo.point;
        }
    }
}
