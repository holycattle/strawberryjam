using UnityEngine;

public class Utils
{
	public Utils ()
	{
	}
	static Vector3 ScreenSize = new Vector3(1280, 800, 0);
	public static Vector3 mousePosition(){
		Ray ray = Camera.mainCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit = new RaycastHit();
		Physics.Raycast(ray, out hit, float.MaxValue);
		return hit.point;
	}
}
