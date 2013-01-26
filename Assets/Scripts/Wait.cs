using UnityEngine;
using System.Collections;


public class Wait : MonoBehaviour {
	public GUIStyle topLabelStyle;
	
	// Use this for initialization
	void Start () {
		
	}
	
	void OnPlayerConnected(NetworkPlayer player) {
		Debug.Log ("number of players: " + Networking.players.Count);
		foreach (NetworkPlayer p in Networking.players) {
			Debug.Log ("Sending a player to new guy");
			networkView.RPC ("PlayerConnected", player, p);	
		}
		Networking.players.Add (player);
		foreach (NetworkPlayer p in Networking.players) {
			if (p != Network.player) { 
				Debug.Log ("Sending new guy to a player");
				networkView.RPC("PlayerConnected", p, player);
			}
		}
		if (Networking.players.Count == Networking.NUM_PLAYERS) {
			Application.LoadLevel("Main");
		}
		
		Debug.Log ("number of players2: " + Networking.players.Count);
		
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
			"Number of people: " + Networking.players.Count,
			topLabelStyle);
	}
	
	// Update is called once per frame
	void Update () {
			
	}
}
