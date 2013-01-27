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
