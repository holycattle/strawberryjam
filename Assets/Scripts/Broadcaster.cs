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
