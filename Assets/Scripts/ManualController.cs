using UnityEngine;

public class ManualController : MonoBehaviour
{
	Player character;	
	public void Start()
	{
		this.character = this.GetComponent<Player>();
	}
	
	public void FixedUpdate(){
		Vector3 vector = Utils.MousePosition() - character.position;
		vector.y = 1;
		vector = vector/vector.magnitude;
		character.RotateTowards(vector);
		
		if(Input.GetKeyDown(KeyCode.Space)){
			character.Charge(vector);
		}else if(Input.GetKeyDown(KeyCode.R)){
			character.Shove(vector);
		}else{
			Vector3 result = Vector3.zero;
			if(Input.GetKey(KeyCode.W)){
				result += vector;
			}
			if(Input.GetKey (KeyCode.S)){
				result += -vector;
			}
			if(Input.GetKey (KeyCode.A)){
				result += new Vector3(-vector.z, vector.y, vector.x);
			}
			if(Input.GetKey (KeyCode.D)){
				result += new Vector3(vector.z, vector.y, -vector.x);
			}
			if(result != Vector3.zero){
				result /= result.magnitude;
				character.MoveForward (result);
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

