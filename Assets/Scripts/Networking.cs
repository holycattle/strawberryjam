using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Networking : MonoBehaviour {
	public static bool isServer = false;
	public static NetworkPlayer server;
	public static List<NetworkPlayer> players = new List<NetworkPlayer>();
	
	/*void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
		stream.Serialize (ref nPlayers);
		Debug.Log ("Serialize!");
	}*/
		[RPC]
	void PlayerConnected(NetworkPlayer player) {
		Networking.players.Add (player);
		Debug.Log ("Got RPC for PlayerConnected!");
	}
	
	[RPC]
	void SetServer(NetworkPlayer player) {
		Networking.server = player;
		Debug.Log("Got RPC for SetServer");
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
