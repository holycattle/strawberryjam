using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	private float rotationSpeed = 180;
	private float moveSpeed = 16f;
	private float moveSpeedMultiplier = 1f;
	
	public bool controllable = false;
	
	private Transform trans;
	private Rigidbody rigid;
	
	void Start () {
		trans = transform;
		rigid = rigidbody;
	}
	
	void FixedUpdate () {
		if (controllable) {
			float turn = Input.GetAxis("Horizontal");
			float move = Input.GetAxis("Vertical");
			
			trans.Rotate(new Vector3(0, turn * rotationSpeed * Time.fixedDeltaTime, 0));
			
			rigid.AddForce(transform.forward * move * moveSpeed * moveSpeedMultiplier);
		}
	}
	
	void OnCollisionEnter(Collision c) {
		if (c.gameObject.tag == "Player") {
			Debug.Log("Coll: " + gameObject.name);
//			if (controllable)
				Debug.Log("RBV: " + rigid.velocity.x + ", " + rigid.velocity.y + ", " + rigid.velocity.z);
			c.rigidbody.velocity += rigid.velocity;
//			rigid.AddForce(-rigid.velocity * 256);
	//		rigid.velocity = -rigid.velocity;
		}
	}
}
