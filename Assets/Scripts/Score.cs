using UnityEngine;
using System.Collections;

public class Score {
	public GameObject g;
	public int kills;
	public int deaths;
	
	public Score(GameObject gg) {
		g = gg;
	}
	
	string[] names = {"Sloppy Joe (P1)", "Sumo Sushi (P2)", "Gourmet Piere (P3)", "Juan dela Cruz (P4)"};  
	
	public string GetString() {
		return g.transform.gameObject.GetComponent<Player>().networkId + ": " + kills.ToString() + "/" + deaths.ToString ();
	}
}