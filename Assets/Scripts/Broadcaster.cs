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
	
	// Update is called once per frame
	void Update () {
	
	}
}
