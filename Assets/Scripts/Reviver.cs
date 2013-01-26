using UnityEngine;
using System.Collections;

public class Reviver : MonoBehaviour {
	public float remaining;
	
	void Start() {
		transform.position = new Vector3(0, -10, 0);	
	}
	
	void Update() {
		remaining -= Time.deltaTime;
		if (remaining <= 0) {
			// Revive
			transform.position = new Vector3(0, 1, 0);
//			gameObject.SetActive(true);
			Destroy(this);
		}
	}
}
