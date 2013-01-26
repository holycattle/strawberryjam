using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Networking : MonoBehaviour {
	public static int nPlayers = 0;
	public static NetworkPlayer[] players = new NetworkPlayer[8];
	public static int myId = -1;
	public const int NUM_PLAYERS = 2;
	
	/*void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
		stream.Serialize (ref nPlayers);
		Debug.Log ("Serialize!");
	}*/
	[RPC]
	public void AddPlayer(NetworkPlayer player, int id) {
		Networking.players[id] = player;
		Networking.nPlayers++;
		if (Networking.nPlayers == NUM_PLAYERS) {
			Application.LoadLevel("Main");
		}
	}
	
	
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
