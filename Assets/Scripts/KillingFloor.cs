using UnityEngine;
using System.Collections;

public class KillingFloor : MonoBehaviour {
	
	private float respawnTimer;
	
	void Start() {
		respawnTimer = 0.50f;
	}
	void Update(){
		respawnTimer -= Time.deltaTime;
		if(respawnTimer <= 0){
			respawnTimer = 0.50f;
			GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
			foreach(GameObject go in players){
				Vector3 position = go.rigidbody.position;
				if(position.y < -20 && go.GetComponent<Reviver>() == null ){
					Player p = go.GetComponent<Player>();
					if (p.lastTouch != null) {
						p.lastTouch.score.kills++;
					}
					p.score.deaths++;
					p.transform.position = new Vector3(0, -1000, 0);
					Reviver r = go.AddComponent<Reviver>();
					r.remaining = 2f;
				}
			}
		}
	}
	
	void OnCollisionEnter(Collision c) {
		Player p = c.gameObject.GetComponent<Player>();
		if (p.lastTouch != null) {
			p.lastTouch.score.kills++;
		}
		p.score.deaths++;
		p.transform.position = new Vector3(0, -1000, 0);
//		Debug.Log("kills: " + p.lastTouch.score.kills.ToString());
		Debug.Log ("Attaching reviver");
		Reviver r = c.gameObject.AddComponent<Reviver>();
		r.remaining = 2f;
	}
}