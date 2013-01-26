using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	const float MIN_SPEED = 5f;
	const float MAX_SPEED = 20f;
	const float FAT_CONSUMPTIONRATE = 0.2f;
	
	// Heart Constants
	const float NORMAL_HEART = 1f;
	float heartspeedIncrease = 0.05f;
	float heartspeedDecrease = 0.1f;
	const float HEART_SPEED_MIN = 0.4f;
	const float STUN_DURATION = 2f;
	
	const float MIN_FAT = 3f;
	const float MAX_FAT = 10f;
	
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
	
	// networking
	public int networkId;
	// end networking
	
	public Player lastTouch;
	public float sinceTouch;
	
	private bool attacked;
	public double distance;
	
	public enum State {WAITING, CHARGING, SHOVING, PUSHED};
	public State status;
	
	public Score score;
	
	public Vector3 position
	{
		get{ return this.transform.position; }
		set{ this.transform.position = position; }
	}
	public Vector3 velocity;
	
	void Start () {
		GameObject go = GameObject.FindGameObjectWithTag("GameController");
		MovementEngine engine = go.GetComponent<MovementEngine>();
		engine.Register(this);
		
		audio.Play();
		heartbeatTimer = heartbeatInterval;
		fatness = 5;
		
		knockbackMultiplier = fatness * knockbackMultiplier * KNOCKBACK_FACTOR;
		knockbackResistor = fatness * knockbackResistor * KNOCKBACK_FACTOR;
		status = State.WAITING;
		
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
			Vector3 direction = Utils.MousePosition() - rigidbody.position;
			direction.y = 0;
			direction = direction/direction.magnitude;

			if(Input.GetKey ( KeyCode.Space ) ){
				rigidbody.velocity = direction * move * MAX_SPEED / rigidbody.mass;
				
				// Lose weight
				decreaseFat();
				
				// Increase Heart Rate
				increaseHeartbeat(false);

			} else {
				rigidbody.velocity = direction * move * MIN_SPEED / rigidbody.mass;

				increaseHeartbeat(true); //rest
			}
			Quaternion temp = Quaternion.LookRotation(direction);
			rigidbody.rotation = temp;
			
//			trans.Rotate(new Vector3(0, turn * rotationSpeed * Time.fixedDeltaTime, 0));
			if (transform.rotation.eulerAngles.y < 0) {
				transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 
					transform.rotation.eulerAngles.y + 360, transform.rotation.eulerAngles.z);
			}
			if (transform.rotation.eulerAngles.y > 360) {
				transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, 
					transform.rotation.eulerAngles.y - 360, transform.rotation.eulerAngles.z);
			}
			
			activeDirection = ((int) (transform.rotation.eulerAngles.y + 22.5f) / 45) % 8;
		} else if (move != 0) { //if moving
			//if running
			if((Input.GetKey(KeyCode.Space) && controllable) || AIcontrollable) {
				rigidbody.velocity = transform.forward * move * MAX_SPEED / rigidbody.mass;
				
				// Lose weight
				decreaseFat();
				
				// Increase Heart Rate
				increaseHeartbeat(false);
				
			} else {
				rigidbody.velocity = transform.forward * move * MIN_SPEED / rigidbody.mass;

				increaseHeartbeat(true); //rest
			}
		}
		
		if(status == State.WAITING){
			if(distance < 0){
				velocity = Vector3.zero;
			}
		}else if(status == State.CHARGING){
			//For each other player, see if they should be pushed back.
			if(distance <= 0){
				status = State.WAITING;
				velocity = Vector3.zero;
			}
		}else if(status == State.SHOVING){
			//For each other player, see if they should be pushed back.
			if(true){
				status = State.WAITING;
			}
		}else if(status == State.PUSHED){
			if(distance <= 0){
				status = State.WAITING;
				velocity = Vector3.zero;
			}
		}
	}
	
	
	
	public void MoveForward(Vector3 unitVector, int networkId){
		if (this.networkId == networkId) {
			if(status == State.WAITING){
				velocity = unitVector * MIN_SPEED;
				distance = 0.01;
			}	
		}
	}
	
	public void Charge(Vector3 unitVector, int networkId){
		if (this.networkId == networkId) {
			if(status == State.WAITING){
				velocity = unitVector * MAX_SPEED;
				status = State.CHARGING;
				distance = 5;
			}
		}
	}
	
	public void Shove(Vector3 unitVector, int networkId){
		if (this.networkId == networkId) {
			if(status == State.WAITING){
				velocity = Vector3.zero;
				status = State.SHOVING;
			}
		}
	}
	
	public void RotateTowards(Vector3 vector, int networkId){
		Debug.Log ("I got an RPC for " + networkId + ", and my net ID is " + this.networkId);
		if (this.networkId == networkId) {
			transform.rotation = Quaternion.LookRotation(vector);
			activeDirection = ((int) (transform.rotation.eulerAngles.y + 22.5f) / 45) % 8;
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
		
		knockbackMultiplier = fatness * knockbackMultiplier * 0.5f;
		knockbackResistor = fatness * knockbackResistor * 0.5f;
	}
	
	private void decreaseFat() {
		// Subtract Fat if fat > 1
		if(fatness > 1f) {
			fatness -= FAT_CONSUMPTIONRATE * Time.fixedDeltaTime;
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
	
	void OnTriggerEnter(Collider c) {
		Debug.Log("hi");
		/*
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
		*/
	}
	void OnColliderEnter(Collider c) {
		Debug.Log("collider");
		/*
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
		*/
	}
	void OnCollisionEnter(Collision c) {
		Debug.Log("collision");
		/*
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
		*/
	}
}
