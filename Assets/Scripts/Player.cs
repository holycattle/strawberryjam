using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	const float MIN_SPEED = 1f;
	const float MAX_SPEED = 4f;
	const float FAT_CONSUMPTIONRATE = 1f;
	
	public float fatness;
	
	private float rotationSpeed = 180;
	private float moveSpeed = 16f;
	private float moveSpeedMultiplier = 1f;
	
	public bool controllable = false;
	
	private Transform trans;
	private Rigidbody rigid;
	
	void Start () {
		trans = transform;
		rigid = rigidbody;
		
		fatness = 5;
	}
	
	void OnGUI() {
		if (controllable) {
			GUI.Box(new Rect(0, 0, 100, 30), "Fat: " + fatness);
		}
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
				
				// Subtract Fat
				fatness -= FAT_CONSUMPTIONRATE * Time.fixedDeltaTime;
			} else {
				rigid.AddForce(transform.forward * move * (moveSpeed * moveSpeedMultiplier));
				if(rigid.velocity.magnitude > MIN_SPEED) {
					rigid.velocity = rigid.velocity.normalized * MIN_SPEED;
				}
			}
			
			// Decrease the velocity in other directions
//			rigid.velocity = trans.forward * rigid.velocity.magnitude;
			
			// Increase slowdown rate
//			if (turn == 1 || turn == -1) {
//				// Moving	
//			} else {
//				// Shouldn't Be Moving (slowdown by a lot)
//				rigid.velocity *= 
//			}
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
		} else if (c.gameObject.tag == "Food") {
			Debug.Log("Food!");	
			fatness += c.gameObject.GetComponent<Item>().fat;
			Destroy(c.gameObject);
		}
	}
}
