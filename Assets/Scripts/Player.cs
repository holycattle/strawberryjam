using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	const float MIN_SPEED = 5f;
	const float MAX_SPEED = 10f;
	const float FAT_CONSUMPTIONRATE = 0.2f;
	
	// Heart Constants
	const float NORMAL_HEART = 1f;
	float heartspeedIncrease = 0.05f;
	float heartspeedDecrease = 0.1f;
	const float HEART_SPEED_MIN = 0.4f;
	const float STUN_DURATION = 2f;
	
	const float MIN_FAT = 3f;
	const float MAX_FAT = 10f;
	
	private float rotationSpeed = 180;
	private float moveSpeed = MIN_SPEED;
	private float moveSpeedMultiplier = 1f;
	
	public int activeDirection = 0;
	
	public bool controllable = false;
	public bool AIcontrollable = false;
	
	const float KNOCKBACK_FACTOR = 0.5f;
	public float knockbackMultiplier = 5f;
	public float knockbackResistor = 1f;
	
	public GameObject stunParticle;
	public float fatness;
	private float heartbeatInterval = 1f;
	private float heartbeatTimer;
	private AudioSource heartbeat;
	private float stunRemaining;
	
	public Score score;
	
	public string name;
	
	public Player lastTouch;
	public float sinceTouch;
	
	private Transform trans;
	private Rigidbody rigid;
	
	void Start () {
		trans = transform;
		rigid = rigidbody;
		
		audio.Play();
		heartbeatTimer = heartbeatInterval;
		fatness = 5;
		rigid.mass = fatness;

		score = new Score(this.gameObject);
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
		
		if (sinceTouch > 0) {
			sinceTouch -= Time.deltaTime;
			if (sinceTouch <= 0) {
				lastTouch = null;	
			}
		}
	}
	
	void FixedUpdate () {
		float move = 0;
		if (controllable) {
			move = Input.GetAxis("Vertical");
		}
		if (AIcontrollable) {
			// AI
			move = 1f;
		}
		
		if (stunRemaining > 0) {
			stunRemaining -= Time.fixedDeltaTime;
			move = 0;
		}
		
		if (controllable) {
			Vector3 direction = Utils.mousePosition() - rigid.position;
			direction.y = 0;
			direction = direction/direction.magnitude;

			if(Input.GetKey ( KeyCode.Space ) ){
				rigid.velocity = direction * move * MAX_SPEED / rigid.mass;
				
				// Lose weight
				decreaseFat();
				
				// Increase Heart Rate
				increaseHeartbeat(false);

			} else {
				rigid.velocity = direction * move * MIN_SPEED / rigid.mass;

				increaseHeartbeat(true); //rest
			}
			Quaternion temp = Quaternion.LookRotation(direction);
			rigid.rotation = temp;
			
//			trans.Rotate(new Vector3(0, turn * rotationSpeed * Time.fixedDeltaTime, 0));
			if (trans.rotation.eulerAngles.y < 0) {
				trans.rotation = Quaternion.Euler(trans.rotation.eulerAngles.x, trans.rotation.eulerAngles.y + 360, trans.rotation.eulerAngles.z);
			}
			if (trans.rotation.eulerAngles.y > 360) {
				trans.rotation = Quaternion.Euler(trans.rotation.eulerAngles.x, trans.rotation.eulerAngles.y - 360, trans.rotation.eulerAngles.z);
			}
			
			activeDirection = ((int) (trans.rotation.eulerAngles.y + 22.5f) / 45) % 8;
		} else if (move != 0) { //if moving
			//if running
			if((Input.GetKey(KeyCode.Space) && controllable) || AIcontrollable) {
				rigid.velocity = transform.forward * move * MAX_SPEED / rigid.mass;
				
				// Lose weight
				decreaseFat();
				
				// Increase Heart Rate
				increaseHeartbeat(false);
				
			} else {
				rigid.velocity = transform.forward * move * MIN_SPEED / rigid.mass;

				increaseHeartbeat(true); //rest
			}
			
		} else {
			increaseHeartbeat(true); //rest
		}
	}
	
	private void maxHeartbeatReached() {
		Instantiate(stunParticle, transform.position + Vector3.up * 1, Quaternion.identity);
		stunRemaining = STUN_DURATION; //TODO: add fat
	}
	
	private void increaseHeartbeat(bool increase) {
		heartbeatInterval += heartspeedIncrease * Time.fixedDeltaTime * (increase ? 1 : -1);
		if (heartbeatInterval <= HEART_SPEED_MIN) {
			maxHeartbeatReached();
			heartbeatInterval = HEART_SPEED_MIN;	
		} else if (heartbeatInterval > NORMAL_HEART) {
			heartbeatInterval = NORMAL_HEART;
		}
	}
	
	private void increaseFat(float fat) {
		fatness += fat;
		rigid.mass = fatness;
		
		knockbackMultiplier = fatness * knockbackMultiplier * 0.5f;
		knockbackResistor = fatness * knockbackResistor * 0.5f;
	}
	
	private void decreaseFat() {
		// Subtract Fat if fat > 1
		if(fatness > 1f) {
			fatness -= FAT_CONSUMPTIONRATE * Time.fixedDeltaTime;
			rigid.mass = fatness;
		}
		
		if(knockbackMultiplier > 1f) {
			knockbackMultiplier -= fatness * knockbackMultiplier * KNOCKBACK_FACTOR;
		} else {
			knockbackMultiplier = 1f;
		}
		
		if(knockbackResistor > 1f) {
			knockbackResistor -= fatness * knockbackResistor * KNOCKBACK_FACTOR;
		} else {
			knockbackResistor = 1f;
		}
	}
	
	void OnCollisionEnter(Collision c) {
		if (c.gameObject.tag == "Player") {
			Debug.Log("Coll: " + gameObject.name);
			Player p = c.gameObject.GetComponent<Player>();
			//c.rigidbody.velocity += rigid.velocity * knockbackMultiplier * p.knockbackResistor;
			lastTouch = p;
			
			sinceTouch = 6f;
		} else if (c.gameObject.tag == "Food") {
			Debug.Log("Food!");
			increaseFat(c.gameObject.GetComponent<Item>().fat);
			Destroy(c.gameObject);
		}
	}
}
