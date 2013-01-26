using UnityEngine;
using System.Collections.Generic;
using System;

public class MovementEngine : MonoBehaviour
{
	List<Player> Players = new List<Player>();
	public MovementEngine ()
	{
		Players = new List<Player>();
	}
	public void Register(Player p){
		Players.Add (p);
	}
	public void FixedUpdate()
	{
		foreach(Player p in Players){
			if(p.distance > 0){
				p.transform.position += 0.02f*p.velocity;
				p.distance -= 0.02f*p.velocity.magnitude;
			}
		}
	}
}

