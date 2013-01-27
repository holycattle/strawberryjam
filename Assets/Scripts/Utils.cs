using UnityEngine;

public class Utils
{
	public const bool DEBUG = true;
	
	public Utils ()
	{
	}
	
	public static Vector3 MousePosition(){
		Ray ray = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		Physics.Raycast(ray, out hit, float.MaxValue);
		return hit.point;
	}
	
	public static Vector3 startingPosition(int index) {
		float theta = 2*Mathf.PI/Networking.nPlayers * index;
		return (new Vector3(Mathf.Cos (theta), 1, Mathf.Sin (theta)));
	}
}
