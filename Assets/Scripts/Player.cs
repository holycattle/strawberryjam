using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	const float MIN_SPEED = 5f;
	const float MAX_SPEED = 20f;
	const float PROJECTILE_SPEED = 15f;
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
	
	// desync-related stuff
	private int fixedTicks;
	private const int RESYNC_RATE = 5;
	// end desync-related
	
	public Player lastTouch;
	public float sinceTouch;
	
	private bool attacked;
	public double distance;
	
	public enum State {WAITING, CHARGING, SHOVING, PUSHED};
	public State status;
	
	public Score score;
	
	private Vector3 old_position;
	public Vector3 position
	{
		get{ return this.transform.position; }
		set{ this.transform.position = position; }
	}
	public Vector3 velocity{
		get { return this.rigidbody.velocity; }
		set { this.rigidbody.velocity = velocity; }
	}
	
	void Start () {
		/*
		GameObject go = GameObject.FindGameObjectWithTag("GameController");
		MovementEngine engine = go.GetComponent<MovementEngine>();
		engine.RegisterPlayer(this);
		*/
		audio.Play();
		heartbeatTimer = heartbeatInterval;
		fatness = 5;
		
		status = State.WAITING;
		
		score = new Score(this.gameObject);
		fixedTicks = 0;
	}
	
	void OnDestroy(){
		/*
		GameObject go = GameObject.FindGameObjectWithTag("GameController");
		MovementEngine engine = go.GetComponent<MovementEngine>();
		engine.DestroyPlayer(this);
		*/
	}
	
	void OnGUI() {
		if (controllable) {
			GUI.Box(new Rect(0, 0, 100, 30), "Fat: " + fatness);
			if (GetComponent<ManualController>() != null) {
				GUI.Label (new Rect(000, 200, 200, 400), "Rotation is: " + transform.rotation.eulerAngles);
			}
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
		// resync
		if (Networking.myId == 0) {
			// resync this guy's position
			networkView.RPC ("BroadcastResync", RPCMode.Others, this.networkId, transform.position, transform.rotation);
		}
	}
	
	public void updateFatness(float amount){
		fatness += amount;
		if(fatness < 1) fatness = 1;
		if(fatness > 10) fatness = 10;
	}
	
	void FixedUpdate () {
		if(old_position == null){
			old_position = position;
		}
		distance -= (position-old_position).magnitude;
		old_position = position;
		if(distance <= 0){
			rigidbody.velocity = Vector3.zero;
			status = State.WAITING;
		}
		
		if(status == State.WAITING){
			
		}else if(status == State.CHARGING){
		}else if(status == State.SHOVING){
		}else if(status == State.PUSHED){
		}
	}
	
	


	public void MoveForward(Vector3 unitVector, int networkId){
		if (this.networkId == networkId) {
			if(status == State.WAITING){
				rigidbody.velocity = unitVector * MIN_SPEED;
				distance = 0.01;
			}
		}
	}
	
	public void Charge(Vector3 unitVector, int networkId){
		if (this.networkId == networkId) {
			if(status == State.WAITING){
				rigidbody.velocity = unitVector * MAX_SPEED;
				status = State.CHARGING;
				distance = 5;
			}
		}
	}
	
	/*
	public void Shove(Vector3 unitVector, int networkId){
		//I'm really not happy with how this turned out.
		
		if (this.networkId == networkId) {
			if(status == State.WAITING){
				status = State.SHOVING;
				
				Quaternion rotation = transform.rotation;
				float angle = rotation.eulerAngles.y/180*Mathf.PI;
				float sin = Mathf.Sin (Mathf.PI/2 - angle);
				float cos = Mathf.Cos (Mathf.PI/2 - angle);
				Vector3 vector = new Vector3(cos,0,sin);
				Vector3 ortho = new Vector3(vector.z, 0, vector.x);
				Vector3 ortho2 = new Vector3(-vector.z, 0, vector.x);
				
				Vector3 point = position + 2*vector + ortho;
				Vector3 point2 = position + 2*vector + ortho2;
				Vector3 point3 = position + ortho;
				Vector3 point4 = position + ortho2;
				
				Instantiate(stunParticle, point, Quaternion.identity);
				Instantiate(stunParticle, point2, Quaternion.identity);
				Instantiate(stunParticle, point3, Quaternion.identity);
				Instantiate(stunParticle, point4, Quaternion.identity);
				
				foreach(GameObject go in GameObject.FindGameObjectsWithTag ("Player")){
					Player player = go.GetComponent<Player>();
					if(player != this){
						Vector3 full = player.position-this.position;
						float dotA = full.x*vector.x + full.y*vector.y + full.z*vector.z;
						float dotB = full.x*ortho.x + full.y*ortho.y + full.z*ortho.z;
						if( 0 <= dotA && dotA <= 2.5 && Mathf.Abs(dotB) <= 1.5){
							Vector3 unit = full/full.magnitude;
							
							player.Attacked (unit, Mathf.Max(2+(fatness-player.fatness)/2, 1), 2);
						}
					}
				}
			}
		}
		
	}*/
	
	public void Shove(Vector3 unitVector, int networkId){
		if(status == State.WAITING){
			GameObject AttackSphere = Resources.Load ("prefabs/AttackSphere") as GameObject;
			Quaternion rotation = transform.rotation;
			float angle = rotation.eulerAngles.y/180*Mathf.PI;
			float sin = Mathf.Sin (Mathf.PI/2 - angle);
			float cos = Mathf.Cos (Mathf.PI/2 - angle);
			Vector3 vector = new Vector3(cos,0,sin);
			
			GameObject instance = Instantiate(AttackSphere, position + 1.5f*vector, rotation) as GameObject;
			instance.rigidbody.velocity = vector * PROJECTILE_SPEED;
		}
	}
	
	public void RotateTowards(Vector3 vector, int networkId){
		if (this.networkId == networkId) {
			var rot = Quaternion.LookRotation(vector);
			transform.rotation = Quaternion.Euler(0, rot.eulerAngles[1], 0);
			rigidbody.rotation = Quaternion.Euler(0, rot.eulerAngles[1], 0);
			activeDirection = ((int) (transform.rotation.eulerAngles.y + 22.5f) / 45) % 8;
		}
	}
	
	public void Attacked(Vector3 knockbackVector, float knockbackDistance, float fatLoss){
		if(status != State.CHARGING){
			//Charging is immune to knockback??
			rigidbody.velocity = knockbackVector * MAX_SPEED;
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
	
	private bool chargeCollided;
	private Vector3 chargeVector;
	
	void OnCollisionEnter(Collision collision){
		if(collision.rigidbody == null) return; //This is just the plate. Ignore.
		
		string tag = collision.gameObject.tag;
		if(tag == "Player"){
			Player enemy = collision.gameObject.GetComponent<Player>();
			if(this.status == State.CHARGING && enemy.status != State.CHARGING){
				Vector3 v = enemy.position - position;
				v /= v.magnitude;
				enemy.Attacked(v, 3 + 0.5f*fatness, 1f);
				this.chargeCollided = true;
				this.chargeVector = this.velocity;
			}
		}else if(tag == "Food"){
			if (Networking.myId == 0) {
				networkView.RPC ("BroadcastEat", RPCMode.All, this.networkId, collision.gameObject.name);
			}
		}else if(tag == "AttackSphere"){
			Vector3 v = position - collision.gameObject.rigidbody.position;
			v /= v.magnitude;
			Attacked(v, 1.5f, 1f);

			GameObject.Destroy (collision.gameObject);
		}
	}
	
	void OnCollisionExit(Collision collision){
		if(chargeCollided){
			velocity = chargeVector;
			chargeCollided = false;
		}
	}
}
