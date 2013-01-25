using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour {
	
	public float rotationSpeed = 180;
	public float moveSpeed = 1f;
	public float moveSpeedMultiplier = 1f;
	
	private Transform trans;
	private Rigidbody rigid;
	
	void Start () {
		trans = transform;
		rigid = rigidbody;
	}
	
	void Update () {
		float turn = Input.GetAxis("Horizontal");
		float move = Input.GetAxis("Vertical");
		
		trans.Rotate(new Vector3(0, turn * rotationSpeed * Time.deltaTime));
		trans.Translate(Vector3.forward * move * moveSpeed * Time.deltaTime);
	}
}
