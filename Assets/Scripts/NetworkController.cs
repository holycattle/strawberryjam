using UnityEngine;

public class NetworkController : MonoBehaviour
{
	Player character;	
	public void Start()
	{
		this.character = this.GetComponent<Player>();
	}
	
	
	
	public void FixedUpdate(){

	}
}

