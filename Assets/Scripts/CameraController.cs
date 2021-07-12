using UnityEngine;

public class CameraController : MonoBehaviour
{

    Vector3 offset;

    float verticalLookRotation;
    float horizontalLookRotation;

    public GameObject player;
    public float offsetX, offsetY, offsetZ;

    private bool mousePressed;

    Quaternion cameraRotation;
    Quaternion freedCameraRotation;

    // Start is called before the first frame update
    void Start()
    {
        freedCameraRotation = transform.rotation;
        cameraRotation = transform.rotation;
        mousePressed = false;
        offset = new Vector3(offsetX, offsetY, offsetZ);
    }

    // Update is called once per frame
    void Update()
    {

        verticalLookRotation += Input.GetAxis("Mouse Y");
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -60, 60);

        horizontalLookRotation += Input.GetAxis("Mouse X");
        horizontalLookRotation = Mathf.Clamp(horizontalLookRotation, -60, 60);


        transform.rotation = Quaternion.Euler(-verticalLookRotation, horizontalLookRotation, 0);

        //transform.rotation = Quaternion.Euler(verticalLookRotation, horizontalLookRotation, 0);
        transform.position = player.transform.position + offset;
        //transform.LookAt(player.transform);

    }
}
