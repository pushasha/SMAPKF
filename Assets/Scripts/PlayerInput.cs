using UnityEngine;
using System.Collections;

[RequireComponent(typeof(PlayerPhysics))]

public class PlayerInput : MonoBehaviour {

	public float speed = 8;
	public float acceleration = 20;
	//public float jumpHeight = 5;

	public Vector2 jumpForce;

	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;
	private bool isJumping = false;

	private PlayerPhysics playerPhysics;


	// Use this for initialization
	void Start () {
		playerPhysics = GetComponent<PlayerPhysics> ();
		jumpForce = new Vector2 (0, 50);
	}
	
	// Update is called once per frame
	void Update () {


		if (Input.GetButtonDown ("Fire1")) {
			transform.renderer.material.color = Color.red;
		} else if (Input.GetButtonDown ("Fire2")) {
			transform.renderer.material.color = Color.yellow;
		} else if (Input.GetButtonDown ("Fire3")) {
			transform.renderer.material.color = Color.green;
		} else {
			transform.renderer.material.color = Color.white;
		}

		targetSpeed = Input.GetAxisRaw ("Horizontal") * speed;
		currentSpeed = IncrementTowards(currentSpeed, targetSpeed, acceleration);

		amountToMove.x = currentSpeed;
		if (Input.GetButtonDown ("Jump") && !isJumping) {
			isJumping = true;
			rigidbody2D.AddForce(jumpForce);
		} 

		//if(collider2D     GameObject.Find("Floor").collider2D.
		//isJumping = false;

		playerPhysics.Move(amountToMove * Time.deltaTime);

	}


	private void OnTriggerEnter(BoxCollider2D other)
	{
			isJumping = false;
	}

	private float IncrementTowards(float n, float target, float a)
	{
		if (n == target) 
		{
			return n;
		} 
		else 
		{
			float dir = Mathf.Sign(target - n);
			n += a * Time.deltaTime * dir;
			return(dir == Mathf.Sign(target - n))? n: target;
				
		}
	}
}
