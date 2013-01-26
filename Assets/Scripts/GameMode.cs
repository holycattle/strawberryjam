using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMode : MonoBehaviour {
	
	const float TIME_WIDTH = 0.2f;
	const float TIME_HEIGHT = 0.05f;
	const float ROUND_TIME = 30f;
	
	public Score[] scores;
	
	float timeRemaining;
	bool gameEnded = false;
	
	void Start() {
		timeRemaining = ROUND_TIME;
		
		GameObject[] g = GameObject.FindGameObjectsWithTag("Player");
		scores = new Score[g.Length];
		int i = 0;
		foreach (GameObject gg in g) {
			scores[i] = gg.GetComponent<Player>().score;
			i++;
		}
	}
	
	void OnGUI() {
		GUI.Box(new Rect(Screen.width - Screen.width * TIME_WIDTH, 0, Screen.width * TIME_WIDTH, Screen.height * TIME_HEIGHT),
			"Time Remaining: " + Mathf.CeilToInt(timeRemaining));
		
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
//			if (scores[i].
		}
		
		return null;
	}
}