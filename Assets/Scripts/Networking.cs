using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Networking : MonoBehaviour {
	public static int nPlayers = 0;
	public static NetworkPlayer[] players = new NetworkPlayer[8];
	public static int myId = -1;
	public const int NUM_PLAYERS = 2;
	private int tick = 0;
	private int resyncInterval = 50;
	
	[RPC]
	public void SyncPlayers(NetworkPlayer p1, NetworkPlayer p2, NetworkPlayer p3, NetworkPlayer p4, int nPlayers) {
		players[0] = p1;
		players[1] = p2;
		players[2] = p3;
		players[3] = p4;
		Networking.nPlayers = nPlayers;
		if (Application.loadedLevel != 2 && Networking.nPlayers >= NUM_PLAYERS) {
			Application.LoadLevel("Main");
		}
	}
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (players[0] == Network.player) {
			tick = (tick + 1) % resyncInterval;
			if (tick == 0) {
				networkView.RPC ("SyncPlayers", RPCMode.Others, players[0], players[1], players[2], players[3], nPlayers);	
				if (Application.loadedLevel != 2 && Networking.nPlayers >= NUM_PLAYERS) {
					Application.LoadLevel("Main");
				}
			}
			
		}
	}
}
