using UnityEngine;
using System.Collections;

public class HeartScript : MonoBehaviour {
	public Texture2D tex;
	
	public int numFrames;
	private int frame;
	
	private bool actual = true;
	
	public float interval = 0.1f;
	private float actInterval;
	
	public float extraDelay = 0.5f;
	private float actDelay;
	
	void Start() {
		tex = (Texture2D) Resources.Load("img/heart", typeof(Texture));
		actInterval = interval;
		actDelay = extraDelay;
		frame = 0;
	}
	
	void Update () {
		if (actual) {
			actInterval -= Time.deltaTime;
			if (actInterval <= 0) {
				frame = (frame + 1) % numFrames;
				actInterval += interval;
				
				if (frame == 0) {
					actual = !actual;	
				}
			}
		} else {
			actDelay -= Time.deltaTime;
			if (actDelay <= 0) {
				actDelay += extraDelay;
				actual = !actual;
			}
		}
	}
	
	void OnGUI() {
		GUI.Box(new Rect(50, 50, 50, 50), "MAXTOR");
		GUI.DrawTextureWithTexCoords(new Rect(0, 0, 64, 64), tex, new Rect((float) frame/numFrames ,0f,1f/numFrames,1f), true);
	}
}
