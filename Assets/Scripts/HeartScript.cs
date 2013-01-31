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
		tex = (Texture2D) Resources.Load("img/hearts", typeof(Texture));
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
//		GUI.Box(new Rect(0, 0, 100, 100), "Delay: " + extraDelay);
		GUI.DrawTextureWithTexCoords(new Rect(0, 0, 64, 64), tex, new Rect((float) frame/numFrames ,0f,1f/numFrames,1f), true);
	}
}
