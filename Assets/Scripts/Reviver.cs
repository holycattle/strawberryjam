using UnityEngine;
using System.Collections;

public class Reviver : MonoBehaviour {
	public float remaining;
	
	void Start() {
		Debug.Log ("KILLING");
		transform.position = new Vector3(0, -100, 0);	
	}
	
	void Update() {
		remaining -= Time.deltaTime;
		if (remaining <= 0) {
			Debug.Log ("STARTING");
			// Revive
			transform.position = new Vector3(0, 1, 0);
//			gameObject.SetActive(true);
			Destroy(this);
		}
	}
}
