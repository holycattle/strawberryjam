using UnityEngine;
using System.Collections;

public class KillingFloor : MonoBehaviour {
	
//	KillTracker kt;
	
	void Start() {
//		kt = GameObject.Find(" GameController").GetComponent<KillTracker>();
	}
	
	void OnCollisionEnter(Collision c) {
//		kt.AddToDeadList(c.gameObject);
		Reviver r = c.gameObject.AddComponent<Reviver>();
		r.remaining = 6f;
	}
	
}
