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
}
