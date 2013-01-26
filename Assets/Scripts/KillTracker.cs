using UnityEngine;
using System.Collections;

public class KillTracker : MonoBehaviour {

	void Start () {
	
	}
	
	void Update () {
	
	}
	
	public void AddToDeadList(GameObject g) {
		g.SetActive(false);
		
	}
}