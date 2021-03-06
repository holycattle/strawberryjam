/*
Copyright (c) 2013 Joshua Castaneda, Payton Yao, Tim Dumol, Joniel Ibasco, Anton Chua, Joan Magno, Victoria Tadiar, and Bernadette Guiamoy

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files
(the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge,
publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR
ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using UnityEngine;
using System.Collections;


public class Wait : MonoBehaviour {
//	public GUIStyle topLabelStyle;
	
	public Texture2D texBackground;
	public GUIStyle connectButton;
	public Texture2D[][] faces;
	public Rect[] rects;
	public GUIStyle IPAddress;
	public GUISkin ski;
	
	public Texture2D aa;
	public Texture2D bb;
	
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
		rects[2] = new Rect(Screen.width - 512, Screen.height - 512, 512, 512);
		rects[3] = new Rect(0, Screen.height - 512, 512, 512);
		
		ski = Resources.Load("img/WaitMenu/CRAP", typeof(GUISkin)) as GUISkin;
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
		
		GUI.skin = ski;
		
		// Background
		GUI.DrawTexture(new Rect(0, 0, w, h), texBackground);
		
		if (Networking.players[0] == Network.player) {
			GUI.Label(new Rect(w/2 - btnW/2, 380, btnW, btnH), Network.player.ipAddress, IPAddress);
			
			if (GUI.Button(new Rect(w/2 - btnW/2, 500, btnW, btnH), "", connectButton)) {
				networkView.RPC("MakeLoad", RPCMode.All);
			}
		} else {
			GUI.Label(new Rect(w/2 - btnW/2, 380, btnW, btnH), Networking.serverAddress, IPAddress);
		}
		
		for (int i = 0; i < 4; i++) {
			GUI.DrawTexture(rects[i], faces[i][i < Networking.nPlayers ? 1 : 0]);
		}
		
		GUI.DrawTexture(new Rect(0, 144, 256, 256), aa);
		GUI.DrawTexture(new Rect(Screen.width - 256, 144, 256, 256), bb);
		
		GUI.skin = null;
	}
	
	// Update is called once per frame
	void Update () {
			
	}
}
