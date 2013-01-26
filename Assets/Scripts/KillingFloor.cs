using UnityEngine;
using System.Collections;

public class KillingFloor : MonoBehaviour {
	
	void Start() {
	}
	
	void OnCollisionEnter(Collision c) {
		Player p = c.gameObject.GetComponent<Player>();
		Reviver r = c.gameObject.AddComponent<Reviver>();
		r.remaining = 6f;
		
		// Add death + kill to counter
		foreach (Score s in scores) {
			if (s.g.name == c.gameObject.name) {
				s.deaths++;
			} else if (p.lastTouch != null && s.g.name == p.lastTouch.name) {
				s.kills++;
			}
		}
	}
}