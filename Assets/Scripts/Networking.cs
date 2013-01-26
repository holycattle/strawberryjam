using UnityEngine;
using System.Collections;

public class Networking : MonoBehaviour {
	public static bool isServer = false;
	public static int nPlayers = 1;
	
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
