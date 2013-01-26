using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	const float MIN_SPEED = 4f;
	const float MAX_SPEED = 8f;
	const float FAT_CONSUMPTIONRATE = 1f;
	const float NORMAL_HEART = 1f;
	const float HEART_SPEED_INC = 0.1f;
	
	const float MIN_FAT = 1f;
	const float MAX_FAT = 10f;
	
	private float rotationSpeed = 180;
	private float moveSpeed = 16f;
	private float moveSpeedMultiplier = 1f;
	
	public bool controllable = false;
	public bool AIcontrollable = false;
	
	public float knockbackMultiplier = 1f;
	public float knockbackResistor = 1f;
	
	public float fatness;
	private float heartbeatInterval = 1f;
	private float heartbeatTimer;
	private AudioSource heartbeat;
	private float stunRemaining;
	
//	public Player lastTouch;
//	public float sinceTouch;
	
	private Transform trans;
	private Rigidbody rigid;
	
	void Start () {
		trans = transform;
		rigid = rigidbody;
		
		audio.Play();
		heartbeatTimer = heartbeatInterval;
		fatness = 5;
	}
	
	void OnGUI() {
		if (controllable) {
			GUI.Box(new Rect(0, 0, 100, 30), "Fat: " + fatness);
		}
	}
	
	void Update() {
		if (controllable) {
			heartbeatTimer -= Time.deltaTime;
			if (heartbeatTimer <= 0) {
				audio.Play();
				heartbeatTimer += heartbeatInterval;
			}
		}
	}
	
	void FixedUpdate () {
		float turn = 0;
		float move = 0;
		if (controllable) {
			turn = Input.GetAxis("Horizontal");
			move = Input.GetAxis("Vertical");
		}
		if (AIcontrollable) {
			// AI
			turn = 0.5f;
			move = 1f;
		}
			
		if (turn != 0) {
			trans.Rotate(new Vector3(0, turn * rotationSpeed * Time.fixedDeltaTime, 0));
		}
		if (move != 0) {
			if(Input.GetKey(KeyCode.Space) && controllable || AIcontrollable) {
				rigid.AddForce(transform.forward * move * (moveSpeed * moveSpeedMultiplier));
				if(rigid.velocity.magnitude > MAX_SPEED) {
					rigid.velocity = rigid.velocity.normalized * MAX_SPEED;
				}
				
				// Subtract Fat
				fatness -= FAT_CONSUMPTIONRATE * Time.fixedDeltaTime;
				
				// Increase Heart Rate
				heartbeatInterval -= HEART_SPEED_INC * Time.fixedDeltaTime;
				// Subtract Fat if fat > 0
				if(fatness > 1f)
					fatness -= FAT_CONSUMPTIONRATE * Time.fixedDeltaTime;
			} else {
				rigid.AddForce(transform.forward * move * (moveSpeed * moveSpeedMultiplier));
				if(rigid.velocity.magnitude > MIN_SPEED) {
					rigid.velocity = rigid.velocity.normalized * MIN_SPEED;
				}
			}
		} else {
			// Rest
			heartbeatInterval += HEART_SPEED_INC * Time.fixedDeltaTime;
			if (heartbeatInterval > NORMAL_HEART) {
				heartbeatInterval = NORMAL_HEART;	
			}
		}
	}
	
	void OnCollisionEnter(Collision c) {
		if (c.gameObject.tag == "Player") {
			Debug.Log("Coll: " + gameObject.name);
			Player p = c.gameObject.GetComponent<Player>();
			c.rigidbody.velocity += rigid.velocity * knockbackMultiplier * p.knockbackResistor;
		} else if (c.gameObject.tag == "Food") {
			Debug.Log("Food!");	
			fatness += c.gameObject.GetComponent<Item>().fat;
			rigid.mass = fatness;
			Destroy(c.gameObject);
		}
	}
}
