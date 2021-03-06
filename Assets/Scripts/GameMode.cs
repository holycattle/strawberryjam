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
using System.Collections.Generic;

public class GameMode : MonoBehaviour {
	
	const float TIME_WIDTH = 0.2f;
	const float TIME_HEIGHT = 0.05f;
	const float ROUND_TIME = 90f;
	
	public Score[] scores;
	
	float timeRemaining;
	bool gameEnded = false;
	
	public static Player[] players;
	public static int nConnected = 0;
	public static bool gameStarted = false;
	public static Player mainPlayer;
	
	void Start() {
		timeRemaining = ROUND_TIME;
		gameStarted = false;
		if (Network.player != Networking.players[0]) {
			networkView.RPC ("ReportConnect", Networking.players[0]);
		} else {
			ReportConnect();
		}
	}
	
	[RPC]
	public void ReportConnect() {
		Debug.Log ("New connection reported");
		++GameMode.nConnected;
		if (GameMode.nConnected == Networking.nPlayers) {
			Debug.Log ("Starting game");
			networkView.RPC ("StartGame", RPCMode.All, Random.Range (0, 4));
		}
	}
	
	[RPC]
	void StartGame(int sIdx) {
		Debug.Log ("Seriously starting game");
		// set my network ID
		int nPlayers = Networking.nPlayers;
		for (int i = 0; i < nPlayers; ++i) {
			NetworkPlayer player = Networking.players[i];
			if (player == Network.player) {
				Networking.myId = i;
				break;
			}
		}
		
		// create the players
		players = new Player[nPlayers];
		for (int i = 0; i < nPlayers; ++i) {
			
			GameObject playerPrefab = Resources.Load ("prefabs/P" + (i+1)) as GameObject;
			GameObject player = Instantiate (playerPrefab,
				Utils.startingPosition(i),
				Quaternion.identity) as GameObject;
			if (i == Networking.myId) {
				player.AddComponent("ManualController");
				/*GameObject gPref = Resources.Load("prefabs/Selector", typeof(GameObject)) as GameObject;
				GameObject g = Instantiate(gPref, Vector3.zero, Quaternion.identity) as GameObject;
				g.transform.parent = player.transform;
				g.transform.localPosition = new Vector3(0, 0.8f, 0);*/
				
				//spriteObj.renderer.material = Resources.Load("PlayerShadow") as Material;
				mainPlayer = player.GetComponent<Player>();
				mainPlayer.transform.FindChild("Sprite").FindChild("Shadow").renderer.material = Resources.Load("PlayerShadow") as Material;
			}
			Player p = player.GetComponent<Player>();
			p.networkId = i;
			players[i] = p;
		}
		
		//get list of scores from each player
		initScores();
		
		//Set gravity to super high.
		Physics.gravity = Physics.gravity*7;
		gameStarted = true;
	}
	
	
	void OnGUI() {
		
		if (gameStarted) {
			GUI.Box(new Rect(Screen.width - Screen.width * TIME_WIDTH, 0, Screen.width * TIME_WIDTH, Screen.height * TIME_HEIGHT),
				"Time Remaining: " + Mathf.CeilToInt(timeRemaining));
			
			//int id = -1;
			
			//if (Utils.DEBUG) {
			//	GUI.Label (new Rect(100, 100, 200, 200), "Player ID is: " + id);
			//}
			
			if (Input.GetKey(KeyCode.Tab) || gameEnded) {
				Debug.Log ("the end!");
				int width = 170;
				int height = 60;
				int itr = 0;
				foreach (Player p in players) {
					GUI.Box(new Rect((Screen.width - width) / 2, (Screen.height - height * scores.Length / 2f) / 2 + height * itr,
										width, height), p.score.GetString());
					itr++;
	
				}
				
				if (gameEnded) {
					// Show Winner
					
				}
			} 
		} else {
			var dW = 200;
			var dH = 100;
			GUI.Box (new Rect(Screen.width/2 - dW/2, Screen.height/2 - dH/2, dW, dH), "Waiting for other players to finish loading...");	
		}
	}
	
	void Update() {
		
		
		timeRemaining -= Time.deltaTime;
		if (timeRemaining <= 0) {
			timeRemaining = ROUND_TIME;
			
			// End of Game
			Time.timeScale = 0;
			gameEnded = true;
		}
	}
	
	public Score[] GetWinner() {
		int max = -1;
		Score[] maxxes = new Score[scores.Length];
		
		for (int i = 0; i < scores.Length; i++) {
			
			if (scores[i].kills > max) {
				max = scores[i].kills;
				// Clear array
				for (int o = 0; o < maxxes.Length; o++) {
					maxxes[o] = null;	
				}
				maxxes[0] = scores[i];
			} else if (scores[i].kills == max) {
				int o = 1;
				while (maxxes[o] == null) {
						
				}
			}
		}
		
		return null;
	}
	
	public void initScores() {
		int i = 0;
		foreach(Player p in players) {
			scores = new Score[Networking.nPlayers];
			scores[i++] = p.score;
		}
		Debug.Log ("populating scores list...");
	}
}