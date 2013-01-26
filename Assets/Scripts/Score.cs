using UnityEngine;
using System.Collections;

public class Score {
	public GameObject g;
	public int kills;
	public int deaths;
	
	public Score(GameObject gg) {
		g = gg;
	}
	
	public string GetString() {
		return kills + "/" + deaths;
	}
}