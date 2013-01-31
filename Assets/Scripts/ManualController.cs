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
			if (Networking.myId == 0) {
				gameObject.GetComponent<Broadcaster>().BroadcastRotateTowards(vector, Networking.myId);
			} else {
				networkView.RPC ("BroadcastRotateTowards", Networking.players[0], vector, Networking.myId);
			}
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
				if (Networking.myId == 0) {
					gameObject.GetComponent<Broadcaster>().BroadcastRotateAndMove(result, Networking.myId);
				} else {
					networkView.RPC ("BroadcastRotateAndMove", Networking.players[0], result, Networking.myId);
				}
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

