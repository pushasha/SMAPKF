using UnityEngine;
using System.Collections;

public class PlayerPhysics : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Move(Vector2 moveAmount)
	{
		transform.Translate (moveAmount);
	}
}
