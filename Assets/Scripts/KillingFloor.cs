using UnityEngine;
using System.Collections;

public class KillingFloor : MonoBehaviour {
	
	public Score[] scores;
	
	void Start() {
		GameObject[] g = GameObject.FindGameObjectsWithTag("Player");
		scores = new Score[g.Length];
		int i = 0;
		foreach (GameObject gg in g) {
			scores[i] = new Score(gg);
			i++;
		}
	}
	
	void OnGUI() {
		if (Input.GetKey(KeyCode.Tab)) {
			int width = 100;
			int height = 30;
			for (int i = 0; i < scores.Length; i++) {
				GUI.Box(new Rect((Screen.width - width) / 2, (Screen.height - height * scores.Length / 2f) / 2 + height * i,
									width, height), scores[i].GetString());
			}
		}
	}
	
	void OnCollisionEnter(Collision c) {
		Player p = c.gameObject.GetComponent<Player>();
		Reviver r = c.gameObject.AddComponent<Reviver>();
		r.remaining = 6f;
		
		Debug.Log("Someone Died: " + gameObject.name);
		
		// Add death + kill to counter
		foreach (Score s in scores) {
			Debug.Log(">>" + s.g.name);
			if (s.g.name == c.gameObject.name) {
				s.deaths++;
			} else if (p.lastTouch != null && s.g.name == p.lastTouch.name) {
				Debug.Log("Killered");
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