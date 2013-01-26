using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Networking : MonoBehaviour {
	public static bool isServer = false;
	public static List<NetworkPlayer> players = new List<NetworkPlayer>();
	
	/*void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info) {
		stream.Serialize (ref nPlayers);
		Debug.Log ("Serialize!");
	}*/
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
