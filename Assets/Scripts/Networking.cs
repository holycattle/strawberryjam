/*
Copyright (c) 2013 Joshua Castaneda, Payton Yao, Tim Dumol, Joniel Ibasco, Anton Chua, Joan Magno, Victoria Tadiar, and Bernadette Guiamoy

Permission is hereby granted, free of charge, to any person obtaining a copy of this software and associated documentation files
(the "Software"), to deal in the Software without restriction, including without limitation the rights to use, copy, modify, merge,
publish, distribute, sublicense, and/or sell copies of the Software, and to permit persons to whom the Software is furnished to do so,
subject to the following conditions:

The above copyright notice and this permission notice shall be included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR
ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Networking : MonoBehaviour {
	public static int nPlayers = 0;
	public static NetworkPlayer[] players = new NetworkPlayer[8];
	public static int myId = -1;
	public static string serverAddress;
	//public const int NUM_PLAYERS = 2;
	private int tick = 0;
	private int resyncInterval = 100;
	
	[RPC]
	public void SyncPlayers(NetworkPlayer p1, NetworkPlayer p2, NetworkPlayer p3, NetworkPlayer p4, int nPlayers) {
		players[0] = p1;
		players[1] = p2;
		players[2] = p3;
		players[3] = p4;
		Networking.nPlayers = nPlayers;
	}
	
	[RPC]
	public void MakeLoad() {
		if (Application.loadedLevel != 2) {
			Application.LoadLevel("Main");	
		}
	}
	
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (!GameMode.gameStarted && players[0] == Network.player) {
			tick = (tick + 1) % resyncInterval;
			if (tick == 0) {
				networkView.RPC ("SyncPlayers", RPCMode.Others, players[0], players[1], players[2], players[3], nPlayers);	
			}
			
		}
	}
}
