using UnityEngine;
using System.Collections;

public class KillingFloor : MonoBehaviour {
	
	void Start() {
	}
	
	void OnCollisionEnter(Collision c) {
		Player p = c.gameObject.GetComponent<Player>();
		if (p.lastTouch != null) {
			p.lastTouch.score.kills++;
		}
		p.score.deaths++;
		p.transform.position = new Vector3(0, -1000, 0);
//		Debug.Log("kills: " + p.lastTouch.score.kills.ToString());
		Debug.Log ("Attaching reviver");
		Reviver r = c.gameObject.AddComponent<Reviver>();
		r.remaining = 2f;
	}
}