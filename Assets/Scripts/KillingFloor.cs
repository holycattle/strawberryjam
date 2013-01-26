using UnityEngine;
using System.Collections;

public class KillingFloor : MonoBehaviour {
	
	void Start() {
	}
	
	void OnCollisionEnter(Collision c) {
		Player p = c.gameObject.GetComponent<Player>();
		p.lastTouch.score.kills++;
		p.score.deaths++;
		Debug.Log("kills: " + p.lastTouch.score.kills.ToString());
		Reviver r = c.gameObject.AddComponent<Reviver>();
		r.remaining = 6f;
	}
}