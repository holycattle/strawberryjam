using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	const float MIN_SPEED = 1f;
	const float MAX_SPEED = 4f;
	
	
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
			
			if(Input.GetKey(KeyCode.Space)) {
				rigid.AddForce(transform.forward * move * (moveSpeed * moveSpeedMultiplier));
				if(rigid.velocity.magnitude > MAX_SPEED) {
					rigid.velocity = rigid.velocity.normalized * MAX_SPEED;
				}
				Debug.Log("Accelerating: " + rigid.velocity.ToString());
			} else {
				rigid.AddForce(transform.forward * move * (moveSpeed * moveSpeedMultiplier));
				if(rigid.velocity.magnitude > MIN_SPEED) {
					rigid.velocity = rigid.velocity.normalized * MIN_SPEED;
				}
				Debug.Log(rigid.velocity.ToString());
			}
//			velocity = Vector3.forward * move * moveSpeed * moveSpeedMultiplier * Time.fixedDeltaTime;
//			trans.Translate(velocity);
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
