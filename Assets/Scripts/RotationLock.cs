using UnityEngine;
using System.Collections;

public class RotationLock : MonoBehaviour {
	
	private Quaternion lockRot;
	void Start () {
		lockRot = transform.rotation;
	}
	
	void LateUpdate () {
		transform.rotation = lockRot;
	}
}
