using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class SimpleCollider : MonoBehaviour
{
	/*
	public float radius;
	public Vector3 position;
	public HashSet<SimpleCollider> collided;
	public void Start(){
		Transform t = gameObject.GetComponent<Transform>();
		this.position = t.position;
		this.radius = 0.50f;
		this.collided = new HashSet<SimpleCollider>();
		
		GameObject go = GameObject.FindGameObjectWithTag("GameController");
		MovementEngine engine = go.GetComponent<MovementEngine>();
		engine.RegisterCollider(this);		
	}
	public void onCollisionBegin(SimpleCollider other){
		this.gameObject.BroadcastMessage ("onCollide", other, SendMessageOptions.DontRequireReceiver);
	}
	public void OnDestroy(){
		GameObject go = GameObject.FindGameObjectWithTag ("GameController");
		MovementEngine engine = go.GetComponent<MovementEngine>();
		engine.DestroyCollider(this);
	}
	*/
}

