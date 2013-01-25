using UnityEngine;
using System.Collections;

public class KillingFloor : MonoBehaviour {
	
	void OnCollisionEnter(Collision c) {
		Destroy(c.gameObject);
	}
	
}
