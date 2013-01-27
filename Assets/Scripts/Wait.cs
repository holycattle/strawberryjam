using UnityEngine;
using System.Collections;


public class Wait : MonoBehaviour {
	public GUIStyle topLabelStyle;
	
	// Use this for initialization
	void Start () {
		
	}
	
	void OnPlayerConnected(NetworkPlayer player) {
		/*for (int i = 0; i < Networking.nPlayers; ++i) {
			Debug.Log ("Sending a player to new guy");
			networkView.RPC ("AddPlayer", player, Networking.players[i], i);	
		}*/
		int newId = Networking.nPlayers;
		Networking.players[Networking.nPlayers++] = player;
		/*for (int i = 1; i < Networking.nPlayers; ++i) {
			networkView.RPC ("AddPlayer", player, Networking.players[newId], newId);	
		}
		if (Networking.nPlayers == Networking.NUM_PLAYERS) {
			Application.LoadLevel("Main");
		}*/
		
		Debug.Log ("Guy connected!");
	}
	

	
	void OnGUI() {
		var w = 1280; //Screen.width;
		var h = 800; //Screen.height;
		
		if (Networking.players[0] == Network.player) {
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
