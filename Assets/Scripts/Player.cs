using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	private float rotationSpeed = 180;
	private float moveSpeed = 16f;
	private float moveSpeedMultiplier = 1f;
	
	public bool controllable = false;
	
	private Vector3 velocity;
	
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
			
//			velocity = Vector3.forward * move * moveSpeed * moveSpeedMultiplier * Time.fixedDeltaTime;
//			trans.Translate(velocity);
		}
	}
	
	void OnCollisionEnter(Collision c) {
//		rigid.AddForce(-rigid.velocity.normalized * 100);
//		rigid.velocity = -rigid.velocity;
		Debug.Log("Coll: " + gameObject.name);
	}
}
