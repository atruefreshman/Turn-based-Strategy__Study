using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    CinemachineTransposer transposer;

    int x = 0;
    int z = 0;
    private float moveSpeed = 5f;
    private float rotateSpeed = 200f;

    private void Awake()
    {
        transposer = virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
    }

    private void Update()
    {
        HandleMove();
        HandleRotation();
        HandleZoom();
    }

    private void HandleZoom()
    {
        Vector3 followOffset = transposer.m_FollowOffset;
        followOffset.y += Input.mouseScrollDelta.y;
        followOffset.y = Mathf.Clamp(followOffset.y, 2f, 12f);
        transposer.m_FollowOffset = Vector3.Lerp(transposer.m_FollowOffset, followOffset, Time.deltaTime * 5f);
    }

    private void HandleRotation()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            transform.Rotate(Vector3.up, -1 * rotateSpeed * Time.deltaTime);
        }
        else if (Input.GetKey(KeyCode.E))
        {
            transform.Rotate(Vector3.up, 1 * rotateSpeed * Time.deltaTime);
        }
    }

    private void HandleMove()
    {
        x = 0;
        z = 0;
        if (Input.GetKey(KeyCode.W))
            z = 1;
        else if (Input.GetKey(KeyCode.S))
            z = -1;
        if (Input.GetKey(KeyCode.A))
            x = -1;
        else if (Input.GetKey(KeyCode.D))
            x = 1;

        transform.position += (transform.forward * z + transform.right * x).normalized * moveSpeed * Time.deltaTime;
    }
}
