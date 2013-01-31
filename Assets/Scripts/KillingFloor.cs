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

public class KillingFloor : MonoBehaviour {
	
	private float respawnTimer;
	
	void Start() {
		respawnTimer = 0.50f;
	}
	void Update(){
		respawnTimer -= Time.deltaTime;
		if(respawnTimer <= 0){
			respawnTimer = 0.50f;
			GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
			foreach(GameObject go in players){
				Vector3 position = go.rigidbody.position;
				if(position.y < -20 && go.GetComponent<Reviver>() == null ){
					Player p = go.GetComponent<Player>();
					if (p.lastTouch != null) {
						p.lastTouch.score.kills++;
					}
					p.score.deaths++;
					p.transform.position = new Vector3(0, -1000, 0);
					Reviver r = go.AddComponent<Reviver>();
					r.remaining = 2f;
				}
			}
		}
	}
	
	void OnCollisionEnter(Collision c) {
		Player p = c.gameObject.GetComponent<Player>();
		if (p.lastTouch != null) {
			p.lastTouch.score.kills++;
		}
		p.score.deaths++;
		p.transform.position = new Vector3(0, -1000, 0);
//		Debug.Log("kills: " + p.lastTouch.score.kills.ToString());
		Debug.Log ("Attaching reviver");
		Reviver r = c.gameObject.AddComponent<Reviver>();
		r.remaining = 2f;
	}
}