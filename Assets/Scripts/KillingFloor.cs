using UnityEngine;
using System.Collections;

public class KillingFloor : MonoBehaviour {
	
	void Start() {
	}
	
	void OnCollisionEnter(Collision c) {
		Player p = c.gameObject.GetComponent<Player>();
		Reviver r = c.gameObject.AddComponent<Reviver>();
		r.remaining = 6f;
	}
}