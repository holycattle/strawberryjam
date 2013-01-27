using UnityEngine;
using System.Collections;

public class HeartScript : MonoBehaviour {
	public Texture2D tex;
	
	public int numFrames;
	private int frame;
	
	private bool actual = true;
	
	public float animationDuration = 0.5f;
	private float interval;
	private float actInterval;
	
	public float extraDelay = 0.5f;
	private float actDelay;
	
	private Player p;
	
	void Start() {
		tex = (Texture2D) Resources.Load("img/heart", typeof(Texture));
		interval = animationDuration / numFrames;
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
				
				// Copy heartbeat interval
				if (p == null) {
					p = GameMode.mainPlayer;
				}
				extraDelay = p.heartbeatInterval;
			}
		}
//		Debug.Log("Extra Delay: " + extraDelay);
	}
	
	void OnGUI() {
		GUI.Box(new Rect(0, 0, 100, 100), "Delay: " + extraDelay);
		GUI.DrawTextureWithTexCoords(new Rect(0, 0, 64, 64), tex, new Rect((float) frame/numFrames ,0f,1f/numFrames,1f), true);
	}
}
