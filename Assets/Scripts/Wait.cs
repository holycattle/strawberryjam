using UnityEngine;
using System.Collections;


public class Wait : MonoBehaviour {
//	public GUIStyle topLabelStyle;
	
	public Texture2D texBackground;
	public GUIStyle connectButton;
	public Texture2D[][] faces;
	public Rect[] rects;
	
	// Use this for initialization
	void Start () {
		// Load stuff
		faces = new Texture2D[4][];
		for (int i = 0; i < 4; i++) {
			faces[i] = new Texture2D[2];
			for (int o = 0; o < 2; o++) {
				faces[i][o] = Resources.Load("img/WaitMenu/Faces/" + i + o, typeof(Texture2D)) as Texture2D;
			}
		}
		rects = new Rect[4];
		rects[0] = new Rect(0, 0, 512, 512);
		rects[1] = new Rect(Screen.width - 512, 0, 512, 512);
		rects[2] = new Rect(0, Screen.height - 512, 512, 512);
		rects[3] = new Rect(Screen.width - 512, Screen.height - 512, 512, 512);
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
		var btnW  = 512;
		var btnH = 128;
		
		// Background
		GUI.DrawTexture(new Rect(0, 0, w, h), texBackground);
		
		if (Networking.players[0] == Network.player) {
			if (GUI.Button(new Rect(w/2 - btnW/2, 600, btnW, btnH), "Start Game!")) {
				networkView.RPC("MakeLoad", RPCMode.All);	
			}
		}
		
		for (int i = 0; i < 4; i++) {
			GUI.DrawTexture(rects[i], faces[i][Networking.nPlayers < i ? 1 : 0]);
		}
	}
	
	// Update is called once per frame
	void Update () {
			
	}
}
