// Retirado de https://wiki.unity3d.com/index.php/RigidbodyFPSWalker
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]

public class CharacterControls : MonoBehaviour
{

	public float speed = 10.0f;
	public float turnSpeed = 180f;
	public float startRotX, startRotY, startRotZ;
	private float movementInput;
	private float turnInput;

    private new Rigidbody rigidbody;

    void Awake()
	{
        rigidbody = GetComponent<Rigidbody>();
        rigidbody.freezeRotation = true;
		rigidbody.useGravity = false;
        Vector3 playerPos = new Vector3(0, GameManager.Instance.height + 0.5f, 0f);
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
		Vector3 front = new Vector3(transform.forward.x, 0, transform.forward.z);
		Vector3 movement =  front * movementInput * speed * Time.deltaTime;
		rigidbody.MovePosition(rigidbody.position + movement);
    }

	private void Turn()
    {
		float turn = turnInput * turnSpeed * Time.deltaTime;

		Vector3 turnRotation = new Vector3(0f, turn, 0f);
		transform.Rotate( turnRotation, Space.World);
    }
}