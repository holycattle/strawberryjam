using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMode : MonoBehaviour {
	
	const float TIME_WIDTH = 0.2f;
	const float TIME_HEIGHT = 0.05f;
	const float ROUND_TIME = 10f;
	
	public Score[] scores;
	
	float timeRemaining;
	bool gameEnded = false;
	
	void Start() {
		timeRemaining = ROUND_TIME;
		
		GameObject[] g = GameObject.FindGameObjectsWithTag("Player");
		scores = new Score[g.Length];
		int i = 0;
		foreach (GameObject gg in g) {
			scores[i] = new Score(gg);
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
				
	public void SomeoneDied(Player p) {
		// Add death + kill to counter
		foreach (Score s in scores) {
			if (s.g.name == p.gameObject.name) {
				s.deaths++;
			} else if (p.lastTouch != null && s.g.name == p.lastTouch.name) {
				s.kills++;
			}
		}			
	}
}

public class Score {
	public GameObject g;
	public int kills;
	public int deaths;
	
	public Score(GameObject gg) {
		g = gg;	
	}
	
	public string GetString() {
		return g.name + " " + kills + "/" + deaths;
	}
}