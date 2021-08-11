using UnityEngine;

public class CameraController : MonoBehaviour
{

    Vector3 offset;

    float verticalLookRotation;
    float horizontalLookRotation;

    public GameObject player;
    public float offsetX, offsetY, offsetZ;
    public float damping = 1f;

    // Start is called before the first frame update
    void Start()
    {
        offset = new Vector3(offsetX, offsetY, offsetZ);
    }


    private void LateUpdate()
    {
        float currentAngle = transform.eulerAngles.y;
        float desiredAngle = player.transform.eulerAngles.y;
        float angle = Mathf.LerpAngle(currentAngle, desiredAngle, Time.deltaTime * damping);

        Quaternion rotation = Quaternion.Euler(0, angle, 0);

        transform.position = player.transform.position + (rotation * offset);
        transform.LookAt(player.transform);
    }
}
