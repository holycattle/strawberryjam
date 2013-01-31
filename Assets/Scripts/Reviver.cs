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

public class Reviver : MonoBehaviour {
	public float remaining;
	
	void Start() {
		Debug.Log ("KILLING");
		transform.position = new Vector3(0, -100, 0);
		if (Networking.myId == 0) {
			for (int i = 0; i < Networking.nPlayers; ++i) {
				networkView.RPC ("BroadcastDeath", RPCMode.Others, i, GameMode.players[i].score.kills,
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
