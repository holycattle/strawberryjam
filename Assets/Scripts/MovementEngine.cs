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

