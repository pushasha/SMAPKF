using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

	public Dictionary<string, Sprite> spriteDict;

	// Use this for initialization
	void Start () {
		spriteDict = new Dictionary<string, Sprite>();
		loadSprites();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	//TODO: get rid of this?
	private void loadSprites(){
		// debug moves
		spriteDict["debug_playerKickLight"] = Resources.Load<Sprite>("Textures/debug_playerKickLight");
		spriteDict["debug_playerKickHeavy"] = Resources.Load<Sprite>("Textures/debug_playerKickHeavy");
		spriteDict["debug_playerPunchLight"] = Resources.Load<Sprite>("Textures/debug_playerPunchLight");
		spriteDict["debug_playerPunchHeavy"] = Resources.Load<Sprite>("Textures/debug_playerPunchHeavy");
		spriteDict["debug_playerIdle"] = Resources.Load<Sprite>("Textures/debug_playerIdle");

		// debug environment stuff
		spriteDict["debug_platform"] = Resources.Load<Sprite>("Textures/debug_platform");
	} // end loadSprites

} // end class
