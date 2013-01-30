using UnityEngine;
using System.Collections;

public class Broadcaster : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	[RPC]
	public void BroadcastMoveForward(Vector3 unitVector, int networkId){
		Player player = GameMode.players[networkId];
		player.MoveForward(unitVector);
	}
	
	[RPC]
	public void BroadcastCharge(Vector3 unitVector, int networkId){
		Player player = GameMode.players[networkId];
		player.Charge(unitVector);
	}
	
	[RPC]
	public void BroadcastShove(Vector3 unitVector, int networkId){
		Player player = GameMode.players[networkId];
		player.Shove(unitVector);
	}
	
	[RPC]
	public void BroadcastRotateTowards(Vector3 vector, int networkId){
		Player player = GameMode.players[networkId];
		player.RotateTowards(vector);
	}
	
	[RPC]
	public void BroadcastRotateAndMove(Vector3 vector, int networkId){
		Player player = GameMode.players[networkId];
		player.MoveForward (vector);
		player.RotateTowards(vector);
	}
	
	[RPC]
	public void BroadcastResync(int networkId, Vector3 position, Quaternion rotation) {
		Player player = GameMode.players[networkId];
		player.rigidbody.position = position;
		player.rigidbody.rotation = rotation;
	}
	
	[RPC]
	public void BroadcastEat(int networkId, string foodId) {
		GameObject.Destroy (GameObject.Find(foodId));
		ItemSpawner.numFood--;
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
