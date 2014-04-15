﻿using UnityEngine;
using System.Collections;

public class Enemy_FlyingPhysics : MonoBehaviour {
	
	public float maxSpeed = 4;
	public float acceleration = 20;
	
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
		amountToMove.x = GetDirection(playerTransform.position.x, transform.position.x) * 4;
		amountToMove.y = GetDirection(playerTransform.position.y, transform.position.y) * 4;
		
		if (Mathf.Abs(playerTransform.position.x - transform.position.x) > 3)
		{
			transform.Translate (amountToMove * Time.deltaTime);
			//transform.Rotate ( transform.rotation * Vector2.zero);
		}
		
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
