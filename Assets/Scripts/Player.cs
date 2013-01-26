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
		engine.RegisterPlayer(this);
		
		audio.Play();
		heartbeatTimer = heartbeatInterval;
		fatness = 5;
		
		status = State.WAITING;
		
		score = new Score(this.gameObject);
		
	}
	
	void OnDestroy(){
		GameObject go = GameObject.FindGameObjectWithTag("GameController");
		MovementEngine engine = go.GetComponent<MovementEngine>();
		engine.DestroyPlayer(this);
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
	
	void updateFatness(float amount){
		fatness += amount;
		if(fatness < 1) fatness = 1;
		if(fatness > 10) fatness = 10;
	}
	
	void FixedUpdate () {
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
		if (this.networkId == networkId) {
			transform.rotation = Quaternion.LookRotation(vector);
			activeDirection = ((int) (transform.rotation.eulerAngles.y + 22.5f) / 45) % 8;
		}
	}
	
	public void Attacked(Vector3 knockbackVector, float knockbackDistance, float fatLoss){
		if(status != State.CHARGING){
			//Charging is immune to knockback??
			velocity = knockbackVector * MAX_SPEED;
			distance = knockbackDistance;
			updateFatness (-fatLoss);
			status = State.PUSHED;
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
	
	void onCollide(SimpleCollider other){
		string tag = other.gameObject.tag;
		if(tag == "Player"){
			Player enemy = other.gameObject.GetComponent<Player>();
			if(this.status == State.CHARGING && enemy.status != State.CHARGING){
				Vector3 v = other.position - position;
				v /= v.magnitude;
				enemy.Attacked(v, 3 + 0.5f*fatness, 1f);
			}
		}else if(tag == "Food"){
			GameObject.Destroy (other.gameObject);
			updateFatness (2);
		}
	}
}
