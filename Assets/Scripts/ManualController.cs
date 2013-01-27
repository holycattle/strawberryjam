using UnityEngine;

public class ManualController : MonoBehaviour
{
	Player character;	
	public void Start()	{
		this.character = this.GetComponent<Player>();
		
		// Add the sprite thing
		
	}
	
	public void FixedUpdate(){
		if(Input.GetMouseButtonDown(0)){
			Vector3 vector = Utils.MousePosition() - character.position;
			vector.y = 0;
			vector = vector/vector.magnitude;
			networkView.RPC ("BroadcastShove", RPCMode.All, vector, Networking.myId);
		}else if(Input.GetMouseButtonDown (1)){
			Vector3 vector = Utils.MousePosition() - character.position;
			vector.y = 0;
			vector = vector/vector.magnitude;
			networkView.RPC ("BroadcastCharge", RPCMode.All, vector, Networking.myId);
			networkView.RPC ("BroadcastRotateTowards", RPCMode.All, vector, Networking.myId);
		}else if(character.status != Player.State.CHARGING) {
			Vector3 result = Vector3.zero;

			if(Input.GetKey(KeyCode.W)){
				result += Vector3.forward;
			}
			if(Input.GetKey (KeyCode.S)){
				result += Vector3.back;
			}
			if(Input.GetKey (KeyCode.A)){
				result += Vector3.left;
				//result += new Vector3(-vector.z, vector.y, vector.x);
			}
			if(Input.GetKey (KeyCode.D)){
				result += Vector3.right;
				//result += new Vector3(vector.z, vector.y, -vector.x);
			}
			
			if(result != Vector3.zero){
				result /= result.magnitude;
				networkView.RPC ("BroadcastMoveForward", RPCMode.All, result, Networking.myId);
				networkView.RPC ("BroadcastRotateTowards", RPCMode.All, result, Networking.myId);
			}
		}
		
		/*else if (move != 0 && false) { //if moving
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
			
		} 
		
		if (Input.GetKeyDown (KeyCode.R) && controllable){
			GameObject[] objects = GameObject.FindGameObjectsWithTag ("Player");
			foreach(GameObject g in objects){
				Debug.Log ("Test!");
				Player p = g.GetComponent<Player>();
				Vector3 vec = p.rigid.position - rigid.position;
				if( p != this && vec.magnitude < 50){
					p.rigid.AddForce(vec/vec.magnitude*2000);
				}
			}
		}
		*/
	}
}

