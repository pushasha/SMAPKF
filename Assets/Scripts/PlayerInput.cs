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
	private Animator animator;


	// Use this for initialization
	void Start () {
		gm = GameObject.Find ("GameManager").GetComponent<GameManager>();
		animator = GetComponentInChildren<Animator>();
		isJumping = false;
		jumpForce = 1000;
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetButtonDown ("Fire1")) {
			//spriteRenderer.sprite = gm.spriteDict["debug_playerKickLight"];
			animator.SetTrigger("ActivateKick1");
		} else if (Input.GetButtonDown ("Fire2")) {
			animator.SetTrigger("ActivateKick2");
			//spriteRenderer.sprite = gm.spriteDict["debug_playerPunchLight"]; 
		} else if (Input.GetButtonDown ("Fire3")) {
			animator.SetTrigger("ActivatePunch1");
			//spriteRenderer.sprite = gm.spriteDict["debug_playerPunchHeavy"]; 
		} 
		else if(Input.GetButtonDown("Fire4")){
			animator.SetTrigger("ActivatePunch2");
		}
		else {
			//spriteRenderer.sprite = gm.spriteDict["debug_playerIdle"]; 
		}

		// calculate horizontal movement
		targetSpeed = Input.GetAxisRaw ("Horizontal") * speed;
		if(targetSpeed != 0 && !animator.GetBool("IsRunning") && !isJumping){
			animator.SetBool("IsRunning", true);
		}
		else if (targetSpeed == 0 && animator.GetBool("IsRunning")){
			animator.SetBool("IsRunning", false);
		}
		//currentSpeed = IncrementTowards(currentSpeed, targetSpeed, acceleration);
		amountToMove.x = targetSpeed;

		// check if player just landed
		if (transform.position.y <= oldY + 0.1f && isJumping) {
			isJumping = false;
		}
		// handle jump
		if (Input.GetButtonDown ("Jump") && !isJumping) {
			isJumping = true;
			animator.SetBool("IsJumping", true);
			animator.SetTrigger("ActivateJump");
			oldY = transform.position.y; // save old Y
			rigidbody2D.AddForce(Vector2.up * jumpForce); // apply jump vector
		}

		if (isJumping){
			animator.SetBool("IsRunning", false);
		}

		// handle horizontal movement
		transform.Translate (amountToMove * Time.deltaTime);
	}// end Update()

	private void OnCollisionEnter2D(Collision2D col){
		Debug.Log ("Blooop");
		if (col.gameObject.tag == "Ground") {

			isJumping = false;
			animator.SetBool("IsJumping", false);

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
