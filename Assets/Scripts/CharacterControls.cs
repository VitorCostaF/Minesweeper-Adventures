// Retirado de https://wiki.unity3d.com/index.php/RigidbodyFPSWalker
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class CharacterControls : MonoBehaviour
{

	public float speed = 10.0f;
	public float turnSpeed = 180f;
	public float startRotX, startRotY, startRotZ;
	public float gravity = 10f;
	public float jumpSpeed = 10f;
	private float movementInput;
	private float turnInput;
	private float upMov;

    private new Rigidbody rigidbody;

    void Awake()
	{
		upMov = 0;
		rigidbody = GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true;
		//rigidbody.useGravity = false;
        Vector3 playerPos = new Vector3(0, GameManager.Instance.height + 3f, 0f);
		transform.rotation = Quaternion.Euler(startRotX, startRotY, startRotZ);

        transform.position = playerPos;
		transform.gameObject.SetActive(true);
    }

	void FixedUpdate()
	{

		movementInput = Input.GetAxis("Vertical");
		turnInput = Input.GetAxis("Horizontal");

		Move();
        Turn();

    }


	private void Move()
    {
		Vector3 front;
		Vector3 movement;


		if (Input.GetButton("Jump"))
        {
			upMov = jumpSpeed;
        }
		else
        {
			upMov = Mathf.Clamp(upMov - gravity*Time.deltaTime, 0, 10f);
		}


        front = new Vector3(transform.forward.x  * movementInput * speed, upMov, transform.forward.z * movementInput * speed);
		movement = front * Time.deltaTime;

		rigidbody.MovePosition(rigidbody.position + movement);


    }

    private void Turn()
    {
		float turn = turnInput * turnSpeed * Time.deltaTime;

		Vector3 turnRotation = new Vector3(0f, turn, 0f);
		transform.Rotate( turnRotation, Space.World);
    }
}