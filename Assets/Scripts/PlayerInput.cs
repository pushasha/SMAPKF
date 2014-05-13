using UnityEngine;
using System.Collections;



public class PlayerInput : MonoBehaviour {


	public float speed = 4;
	public float acceleration = 20;
	public float health = 10;
	//public float jumpHeight = 5;

	public float jumpForce = 1000;

	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;
	public bool isJumping;
	private float oldY;


	private GameManager gm;
	private SpriteRenderer spriteRenderer;


	// Use this for initialization
	void Start () {
		gm = GameObject.Find ("GameManager").GetComponent<GameManager>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		isJumping = false;
		jumpForce = 1000;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetButtonDown ("Fire1")) {
			spriteRenderer.sprite = gm.spriteDict["debug_playerKickLight"]; 
		} else if (Input.GetButtonDown ("Fire2")) {
			spriteRenderer.sprite = gm.spriteDict["debug_playerPunchLight"]; 
		} else if (Input.GetButtonDown ("Fire3")) {
			spriteRenderer.sprite = gm.spriteDict["debug_playerPunchHeavy"]; 
		} else {
			spriteRenderer.sprite = gm.spriteDict["debug_playerIdle"]; 
		}

		// calculate horizontal movement
		targetSpeed = Input.GetAxisRaw ("Horizontal") * speed;
		//currentSpeed = IncrementTowards(currentSpeed, targetSpeed, acceleration);
		amountToMove.x = targetSpeed;

		// check if player just landed
		if (transform.position.y <= oldY + 0.1f && isJumping) {
			isJumping = false;
		}
		// handle jump
		if (Input.GetButtonDown ("Jump") && !isJumping) {
			isJumping = true;
			oldY = transform.position.y; // save old Y
			rigidbody2D.AddForce(Vector2.up * jumpForce); // apply jump vector
		} 

		// handle horizontal movement
		transform.Translate (amountToMove * Time.deltaTime);
	}// end Update()

	private void OnCollisionEnter2D(Collision2D col){
		if (col.gameObject.tag == "Ground") {
			isJumping = false;
		}

		if (col.gameObject.tag == "Door"){
			gm.rm.nextRoom();
			Destroy(col.gameObject);
		}

		if (col.gameObject.tag == "Enemy"){
			health -= 1;
		}
		
		
	}//end OnCollisionEnter

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
	}// end incrementTowards



}// end class
