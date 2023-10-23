using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private float cameraSpeed;
    [SerializeField] private float xBorder;
    [SerializeField] private float yBorder;

    private void LateUpdate()
    {
        Vector3 previousPos = transform.position;

        if (Mathf.Abs(transform.position.y - camera.transform.position.y) >= yBorder
            || Mathf.Abs(transform.position.x - camera.transform.position.x) >= xBorder)
        {
            camera.transform.position = Vector3.Lerp(camera.transform.position, transform.position, cameraSpeed * Time.deltaTime);
            camera.transform.position = new Vector3(camera.transform.position.x, camera.transform.position.y, -1f);
        }
    }

    public void AlignCameraPosition()
    {
        camera.transform.position = new Vector3(transform.position.x, transform.position.y, -1f);
    }
}
