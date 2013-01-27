using UnityEngine;
using System.Collections;

public class Broadcaster : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	[RPC]
	public void BroadcastMoveForward(Vector3 unitVector, int networkId){
		foreach (Player player in GameMode.players) {
			player.MoveForward(unitVector, networkId);
		}
	}
	
	[RPC]
	public void BroadcastCharge(Vector3 unitVector, int networkId){
		foreach (Player player in GameMode.players) {
			player.Charge(unitVector, networkId);
		}
	}
	
	[RPC]
	public void BroadcastShove(Vector3 unitVector, int networkId){
		foreach (Player player in GameMode.players) {
			player.Shove(unitVector, networkId);
		}
	}
	
	[RPC]
	public void BroadcastRotateTowards(Vector3 vector, int networkId){
		foreach (Player player in GameMode.players) {
			player.RotateTowards(vector, networkId);
		}
	}
	
	[RPC]
	public void BroadcastResync(int networkId, Vector3 position, Quaternion rotation) {
		Player player = GameMode.players[networkId];
		player.transform.position = position;
		player.transform.rotation = rotation;
	}
	
	[RPC]
	public void BroadcastEat(int networkId, string foodId) {
		GameObject.Destroy (GameObject.Find(foodId));
		GameMode.players[networkId].updateFatness (2);		
	}
	
	[RPC]
	public void BroadcastDeath(int networkId, int kills, int deaths) {
		Player player = GameMode.players[networkId];
		player.score.kills = kills;
		player.score.deaths = deaths;
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
