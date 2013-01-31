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

public class ItemSpawner : MonoBehaviour {
	const float MAP_RAD = 6;
	const float FOOD_INTERVAL = 1;
	const int FOOD_LIMIT = 20;
	public static int numFood = 0;
	
	public GameObject food;
	public static readonly string[] s = {"Bacon", "Burger", "Cake", "Drink", "Hotdog", "Pizza"};
	
	private float _timePassed;
	public int foodCounter = 0;
	
	void Start() {
		foodCounter = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameMode.gameStarted && Network.player == Networking.players[0]) {
			_timePassed += Time.deltaTime;
			if (_timePassed >= FOOD_INTERVAL) {
				_timePassed -= FOOD_INTERVAL;
				if (numFood <= FOOD_LIMIT) {
					float r = MAP_RAD*Mathf.Sqrt(Random.Range (0f, 1f));
					float theta = Random.Range (0f, 1f)*2*Mathf.PI;
					Vector3 v = new Vector3(r*Mathf.Cos(theta), 1, r*Mathf.Sin (theta));
					 	
					
					int foodType = Random.Range (0, s.Length -1);
					networkView.RPC ("SpawnFood", RPCMode.All, v, foodType, ""+foodCounter++);
				}
			}
		}
	}
	
	[RPC]
	void SpawnFood(Vector3 v, int foodType, string foodId) {
		GameObject obj = Instantiate(food, v, Quaternion.identity) as GameObject;
		Material m = Resources.Load("img/Food/" + s[foodType], typeof(Material)) as Material;
		obj.GetComponentInChildren<Renderer>().material = m;
		obj.name = foodId;
		++numFood;
	}
}
