/*
Copyright (c) 2013 Joshua Castaneda, Payton Yao, Tim Dumol, Joniel Ibasco, Anton Chua, Joan Magno, Victoria Tadiar, and Bernadette Guiamoy

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files
(the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge,
publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR
ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using UnityEngine;

public class Player : MonoBehaviour {
	const float MIN_SPEED = 3f; //Movement speed is MIN_SPEED + FAT_SPEED/fatness
	const float FAT_SPEED = 5f;
	
	const float CHARGE_SPEED = 20f; //This is speed of charging.
	
	const float MIN_SHOVE = 1f; //Shove distance is MIN_SHOVE + FatDifference*FAT_SHOVE. Minimum of MIN_SHOVE.
	const float FAT_SHOVE = 0.5f;
	
	const float KNOCKBACK_SPEED = 20f; //This is knockback speed.
	
	const float WALK_CONSUMPTION = 0.2f; //Cost per second of walking.
	const float CHARGE_CONSUMPTION = 0.5f; //Cost per charge.
	const float SHOVE_CONSUMPTION = 0.1f; //Cost per shove.
	
	const float PROJECTILE_SPEED = 15f;
	
	const double CHARGE_COOLDOWN = 0.75;
	const double SHOVE_COOLDOWN = 0.40;
	
	// Heart Constants
	const float NORMAL_HEART = 1f;
	float heartspeedIncrease = 0.05f;
	float heartspeedDecrease = 0.1f;
	const float HEART_SPEED_MIN = 0.4f;
	const float STUN_DURATION = 2f;
	
	const float MIN_FAT = 3f;
	const float MAX_FAT = 10f;
	
	public int activeDirection = 0;
	
	public GameObject stunParticle;
	public float fatness;
	
	public float heartbeatInterval;
	private float heartbeatTimer;
	private AudioSource heartbeat;
	private float stunRemaining;
	
	private AudioClip eatFoodSound;
	private AudioClip bounceSound;
	
	// networking
	public int networkId;
	// end networking
	
	// desync-related stuff
	private int fixedTicks;
	private const int RESYNC_RATE = 1;
	// end desync-related
	
	public Player lastTouch;
	public float sinceTouch;
	
	public double distance;
	private double timer;
	
	public enum State {WAITING, CHARGING, SHOVING, PUSHED};
	public State status;
	
	public Score score;	
	public ParticleEmitter parEmit;
	
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
	
	public double chargeCooldown; //These will be <= 0 if usable.
	public double shoveCooldown;
	
	
	void Start () {
		/*
		GameObject go = GameObject.FindGameObjectWithTag("GameController");
		MovementEngine engine = go.GetComponent<MovementEngine>();
		engine.RegisterPlayer(this);
		*/
		audio.Play();
		heartbeatInterval = 1f;
		heartbeatTimer = heartbeatInterval;
		fatness = 5;
		
		status = State.WAITING;
		
		score = new Score(this.gameObject);
		fixedTicks = 0;
		
		eatFoodSound = Resources.Load("sfx/eatfood", typeof(AudioClip)) as AudioClip;
		bounceSound = Resources.Load("sfx/bounce", typeof(AudioClip)) as AudioClip;
		
		parEmit = GetComponentInChildren<ParticleEmitter>();
		parEmit.emit = false;
	}
	
	void OnDestroy(){
		/*
		GameObject go = GameObject.FindGameObjectWithTag("GameController");
		MovementEngine engine = go.GetComponent<MovementEngine>();
		engine.DestroyPlayer(this);
		*/
	}
	
	void OnGUI() {
		if (Networking.myId == networkId) {
//			GUI.Box(new Rect(0, 0, 100, 30), "Fat: " + fatness);
			/*if (GetComponent<ManualController>() != null) {
				GUI.Label (new Rect(000, 200, 200, 400), "Rotation is: " + transform.rotation.eulerAngles);
			}*/
		}
	}
	
	void Update() {
		if (Networking.myId == networkId) {
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
		
		if(status == State.SHOVING){
			timer -= Time.deltaTime;
			if(timer <= 0){
				status = State.WAITING;
			}
		}
		
		shoveCooldown -= Time.deltaTime;
		chargeCooldown -= Time.deltaTime;
		activeDirection = ((int) (transform.rotation.eulerAngles.y + 22.5f) / 45) % 8;
		
	}
	
	public void updateFatness(float amount){
		fatness += amount;
		if(fatness < 1) fatness = 1;
		if(fatness > 10) fatness = 10;
		
		this.gameObject.transform.localScale = new Vector3(1+0.15f*fatness, 1+0.15f*fatness, 1+0.15f*fatness);
		
		
	}
	
	void FixedUpdate () {
		if(old_position == null){
			old_position = position;
		}
		
		parEmit.emit = false;
		if(status == State.WAITING){
			float traveled = (position-old_position).magnitude;
			old_position = position;
			if(traveled > 0.001f){
				distance -= traveled;
				if(distance <= 0){
					rigidbody.velocity = Vector3.zero;
					//We finished walking for this frame.
				}
				updateFatness(-WALK_CONSUMPTION*0.02f);
			}
		}else if(status == State.CHARGING){
			parEmit.emit = true;

			float traveled = (position-old_position).magnitude;
			old_position = position;
			if(traveled > 0.001f){
				distance -= traveled;
				if(distance <= 0){
					rigidbody.velocity = Vector3.zero;
					status = State.WAITING;
					
					//Charging ended here.
				}
			}
		}else if(status == State.SHOVING){
			//Nothing here yet.
		}else if(status == State.PUSHED){
			float traveled = (position-old_position).magnitude;
			old_position = position;
			if(traveled > 0.001f){
				distance -= traveled;
				if(distance <= 0){
					rigidbody.velocity = Vector3.zero;
					status = State.WAITING;
					
					//Shoving ended here.
				}
			}
		}
		// resync
		fixedTicks = (fixedTicks + 1) % RESYNC_RATE;
		if (Networking.myId == 0  && fixedTicks == 0) {
			// resync this guy's position
			networkView.RPC ("BroadcastResync", RPCMode.Others, this.networkId, transform.position, transform.rotation);
		}
	}
	
	public void MoveForward(Vector3 unitVector){
			if(status == State.WAITING){
				rigidbody.velocity = unitVector * (MIN_SPEED + FAT_SPEED/fatness);
				distance = 0.10;
			}
	}
	
	public void Charge(Vector3 unitVector){
			if(status == State.WAITING && chargeCooldown <= 0){
				rigidbody.velocity = unitVector * CHARGE_SPEED;
				status = State.CHARGING;
				distance = 5;
				updateFatness (-CHARGE_CONSUMPTION);
				chargeCooldown = CHARGE_COOLDOWN;
			}
	}
	
	
	public void Shove(Vector3 unitVector){
		//I'm really not happy with how this turned out.
		
			if(status == State.WAITING && shoveCooldown <= 0){
				updateFatness (-SHOVE_CONSUMPTION);
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
							float fatnessDifference = fatness-player.fatness;
							
							player.Attacked (unit, Mathf.Max(MIN_SHOVE+FAT_SHOVE*fatnessDifference, 1), MIN_SHOVE);
						}
					}	
				}
				
				shoveCooldown = SHOVE_COOLDOWN;
			}
	}
	/*
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
	*/
	public void RotateTowards(Vector3 vector){
			var rot = Quaternion.LookRotation(vector);
			transform.rotation = Quaternion.Euler(0, rot.eulerAngles[1], 0);
			rigidbody.rotation = Quaternion.Euler(0, rot.eulerAngles[1], 0);
			
	}
	
	public void Attacked(Vector3 knockbackVector, float knockbackDistance, float fatLoss){
		if(status != State.CHARGING){
			//Charging is immune to knockback??
			rigidbody.velocity = knockbackVector * KNOCKBACK_SPEED;

			distance = knockbackDistance;
			updateFatness (-fatLoss);
			status = State.PUSHED;
			
			// [Sound] Bounce
			audio.PlayOneShot(bounceSound);
		} else if(status != State.SHOVING) {
			audio.PlayOneShot(bounceSound);
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
			this.lastTouch = enemy;
			enemy.lastTouch = this;
			if(this.status == State.CHARGING && enemy.status != State.CHARGING){
				Vector3 v = enemy.position - position;
				v /= v.magnitude;
				enemy.Attacked(v, 3 + 0.5f*fatness, 1f);
				this.chargeCollided = true;
				this.chargeVector = this.velocity;
			}
		} else if(tag == "Food") {
			if (Networking.myId == 0) {
				networkView.RPC ("BroadcastEat", RPCMode.All, this.networkId, collision.gameObject.name);
				
				// [Sound] Food
				audio.PlayOneShot(eatFoodSound);
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
