using UnityEngine;
using System.Collections;

public class Wait : MonoBehaviour {
	public GUIStyle topLabelStyle;
	
	// Use this for initialization
	void Start () {
		
	}
	
	void OnPlayerConnected() {
		++Networking.nPlayers;
		Debug.Log ("Guy connected!");
	}
	
	void OnGUI() {
		var w = 1280; //Screen.width;
		var h = 800; //Screen.height;
		
		if (Networking.isServer) {
			GUI.Label(new Rect(0, 0, w, 40),
				"Please tell people to connect to: " + Network.player.ipAddress,
				topLabelStyle);
		}
		
		GUI.Label(new Rect(0, 50, w, 40), 
			"Number of people: " + Networking.nPlayers,
			topLabelStyle);
	}
	
	// Update is called once per frame
	void Update () {
			
	}
}
