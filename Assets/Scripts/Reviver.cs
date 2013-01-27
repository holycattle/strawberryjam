using UnityEngine;
using System.Collections;

public class Reviver : MonoBehaviour {
	public float remaining;
	
	void Start() {
		Debug.Log ("KILLING");
		transform.position = new Vector3(0, -100, 0);
		if (Networking.myId == 0) {
			for (int i = 0; i < Networking.nPlayers; ++i) {
				networkView.RPC ("BroadcastDeaths", i, GameMode.players[i].score.kills,
					GameMode.players[i].score.deaths);
			}
		}
		
	}
	
	void Update() {
		remaining -= Time.deltaTime;
		if (remaining <= 0) {
			Debug.Log ("STARTING");
			// Revive
			transform.position = Utils.startingPosition(gameObject.GetComponent<Player>().networkId);
//			gameObject.SetActive(true);
			Destroy(this);
		}
	}
}
