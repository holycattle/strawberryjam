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
	
	void Start() {
		timeRemaining = ROUND_TIME;
		for (int i = 0; i < Networking.players.Count; ++i) {
			NetworkPlayer player = Networking.players[i];
			if (player == Network.player) {
				Networking.myId = i;
				break;
			}
		}
		
	}
	
	void OnGUI() {
		GUI.Box(new Rect(Screen.width - Screen.width * TIME_WIDTH, 0, Screen.width * TIME_WIDTH, Screen.height * TIME_HEIGHT),
			"Time Remaining: " + Mathf.CeilToInt(timeRemaining));
		
		int id = -1;
		
		if (Utils.DEBUG) {
			GUI.Label (new Rect(100, 100, 200, 200), "Player ID is: " + id);
		}
		
		if (Input.GetKey(KeyCode.Tab) || gameEnded) {
			int width = 100;
			int height = 30;
			for (int i = 0; i < scores.Length; i++) {
				GUI.Box(new Rect((Screen.width - width) / 2, (Screen.height - height * scores.Length / 2f) / 2 + height * i,
									width, height), scores[i].GetString());
			}
		}
		
		if (gameEnded) {
			// Show Winner
			
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
}