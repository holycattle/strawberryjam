using UnityEngine;
using System.Collections;


public class Wait : MonoBehaviour {
	public GUIStyle topLabelStyle;
	
	// Use this for initialization
	void Start () {
		
	}
	
	void OnPlayerConnected(NetworkPlayer player) {
		foreach (NetworkPlayer p in Networking.players) {
			networkView.RPC ("PlayerConnected", player, p);	
		}
		foreach (NetworkPlayer p in Networking.players) {
			networkView.RPC("PlayerConnected", p, player);	
		}
		
		Debug.Log ("Guy connected!");
	}
	
	[RPC]
	void PlayerConnected(NetworkPlayer player) {
		Networking.players.Add (player);
		Debug.Log ("Got RPC!");
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
			"Number of people: " + Networking.players.Count,
			topLabelStyle);
	}
	
	// Update is called once per frame
	void Update () {
			
	}
}
