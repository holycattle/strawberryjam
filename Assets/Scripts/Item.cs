using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {
	public static readonly string[] s = {"Bacon", "Burger", "Cake", "Drink", "Hotdog", "Pizza"};
	
	public float fat = 0.1f;
	
	void Start() {
		Material m = Resources.Load("img/Food/" + s[Random.Range(0, s.Length - 1)], typeof(Material)) as Material;
		GetComponentInChildren<Renderer>().material = m;
	}
}
