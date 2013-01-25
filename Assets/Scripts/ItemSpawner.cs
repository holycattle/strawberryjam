using UnityEngine;
using System.Collections;

public class ItemSpawner : MonoBehaviour {
	const float MAP_SIZE = 10;
	const float FOOD_INTERVAL = 4;
	
	public GameObject food;
	
	private float _timePassed;
	
	// Update is called once per frame
	void Update () {
		_timePassed += Time.deltaTime;
		if (_timePassed >= FOOD_INTERVAL) {
			_timePassed -= FOOD_INTERVAL;
			
			Vector3 v = new Vector3(Random.Range(
				-MAP_SIZE / 2, MAP_SIZE/2), 1, Random.Range(-MAP_SIZE / 2, MAP_SIZE/2));
			
			GameObject g = Instantiate(food, v, Quaternion.identity) as GameObject;
		}
	}
}
