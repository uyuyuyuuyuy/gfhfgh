using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    public float dragSpeed = 2f;
    public Vector2 minBounds = new Vector2(-20, -20);
    public Vector2 maxBounds = new Vector2(20, 20);

    public float zoomSpeed = 5f;
    public float minZoom = 5f;
    public float maxZoom = 20f;

    private Vector3 dragOrigin;
    private Camera cam;

    void Start()
    {
        cam = GetComponent<Camera>();  // Исправлено
    }

    void Update()
    {
        HandleDrag();
        HandleZoom();
    }

    private void HandleDrag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = Input.mousePosition;
            return;
        }

        if (!Input.GetMouseButton(0)) return;

        Vector3 difference = cam.ScreenToViewportPoint(dragOrigin - Input.mousePosition);
        Vector3 move = new Vector3(difference.x * dragSpeed, 0, difference.y * dragSpeed);

        transform.position += move;

        transform.position = new Vector3(
            Mathf.Clamp(transform.position.x, minBounds.x, maxBounds.x),
            transform.position.y,
            Mathf.Clamp(transform.position.z, minBounds.y, maxBounds.y)
        );

        dragOrigin = Input.mousePosition;
    }

    private void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll == 0f)
            return;

        if (cam.orthographic)
        {
            cam.orthographicSize -= scroll * zoomSpeed;
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize, minZoom, maxZoom);
        }
        else
        {
            Vector3 direction = cam.transform.forward;
            Vector3 newPosition = cam.transform.position + direction * scroll * zoomSpeed;

            float newY = Mathf.Clamp(newPosition.y, minZoom, maxZoom);
            newPosition = new Vector3(newPosition.x, newY, newPosition.z);
            cam.transform.position = newPosition;
        }
    }
}
