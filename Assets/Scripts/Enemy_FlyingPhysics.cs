using UnityEngine;
using System.Collections;

public class Enemy_FlyingPhysics : MonoBehaviour {
	
	public float maxSpeed = 4;
	public float acceleration = 20;
	public float health = 5;
	
	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;
	
	
	private GameObject player;
	private Transform playerTransform;
	
	
	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		playerTransform = player.transform;
	}
	
	// Update is called once per frame
	void Update () {
		
			
		//currentSpeed = IncrementTowards(currentSpeed, targetSpeed, acceleration);

		
		if (Mathf.Abs (playerTransform.position.x - transform.position.x) > 3) 
		{
			amountToMove.x = GetDirection(playerTransform.position.x, transform.position.x) * 4;
			amountToMove.y = (GetDirection(playerTransform.position.y, transform.position.y) * 4) + 3;


			//transform.Rotate ( transform.rotation * Vector2.zero);
		} else if(Mathf.Abs (playerTransform.position.x - transform.position.x) < 0.5 || Mathf.Abs (playerTransform.position.y - transform.position.y) < 0.5 )
		{
			amountToMove.x = GetDirection(playerTransform.position.x, transform.position.x) * -4;
			amountToMove.y = (GetDirection(playerTransform.position.y, transform.position.y) * -4);

		}

		else {
			/*
			 * this was the part that makes the flying enemy jitter
			amountToMove.x = Random.Range(-5.0F, 5.0F);
			amountToMove.y = Random.Range(-5.0F, 5.0F);
			*/

		}

		transform.Translate(amountToMove * Time.deltaTime);
		
	}

	private void OnCollisionEnter2D(Collision2D col)
	{
		if (col.gameObject.tag == "Player")
		{
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
	}
	
	private float GetDirection(float p, float e)
	{
		if (p - e > 1) 
		{
			return 1;
		}
		else
		{
			return -1;
		}
	}
}

