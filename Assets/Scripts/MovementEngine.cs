using UnityEngine;
using System.Collections.Generic;
using System;

public class MovementEngine : MonoBehaviour
{
	/*
	List<SimpleCollider> Colliders = new List<SimpleCollider>();
	List<Player> Players = new List<Player>();
	public MovementEngine ()
	{
		Players = new List<Player>();
	}
	public void RegisterPlayer(Player p){
		Players.Add (p);
	}
	public void DestroyPlayer(Player p){
		Players.Remove (p);
	}
	public void RegisterCollider(SimpleCollider c){
		Colliders.Add (c);
	}
	public void DestroyCollider(SimpleCollider c){
		Colliders.Remove (c);
	}
	public void FixedUpdate()
	{
		foreach(Player p in Players){
			
			p.transform.position += 0.02f*Vector3.down;
			p.GetComponent<SimpleCollider>().position += 0.02f*Vector3.down;
			if(p.distance > 0){
				p.transform.position += 0.02f*p.velocity;
				p.GetComponent<SimpleCollider>().position += 0.02f*p.velocity;
				
				p.distance -= 0.02f*p.velocity.magnitude;
			}
		}
		
		foreach(SimpleCollider c in Colliders){
			foreach(SimpleCollider d in Colliders){
				if(c == d) continue;
				if( (c.position - d.position).magnitude <= (c.radius+d.radius) ){
					if(!c.collided.Contains (d)){
						c.collided.Add (d);
						c.onCollisionBegin(d);
					}
				}else if(c.collided.Contains (d)){
					c.collided.Remove (d);
				}
			}
		}
	}
	*/
}

