using UnityEngine;
using System.Collections;

public class EnemyPhysics : MonoBehaviour {

	public float maxSpeed = 4;
	public float acceleration = 20;

	private float currentSpeed;
	private float targetSpeed;
	private Vector2 amountToMove;


	private GameObject player;
	private Transform playerTransform;

	private bool isFollowing = false;

	private float atkTimer;
	private const float atkTime = 3;




	// Use this for initialization
	void Start () {
		player = GameObject.Find("Player");
		playerTransform = player.transform;
		atkTimer = atkTime;
	}
	
	// Update is called once per frame
	void Update () {
		

		atkTimer -= Time.deltaTime;
		//Debug.Log (atkTimer);

		if (atkTimer <= 0) {
			renderer.material.color = Color.blue;
			atkTimer = atkTime;
		} else {
			renderer.material.color = Color.white;
		}


		if (!isFollowing) 
		{
			if (Mathf.Abs (playerTransform.position.x - transform.position.x) > 3 && playerTransform.position.x - transform.position.x < 7) 
			{
					isFollowing = true;
			}
		} else {
			if (Mathf.Abs (playerTransform.position.x - transform.position.x) > 2)
			{
				amountToMove.x = GetDirection(playerTransform.position.x, transform.position.x) * 4;
			}
			if(Mathf.Abs (playerTransform.position.x - transform.position.x) < 2)
			{
				amountToMove.x = 0;
			}
		}

		transform.Translate (amountToMove * Time.deltaTime);
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
