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

public class SpriteAnimator1 : MonoBehaviour {
	public int numFrames;
	public int numDirections;
	public float interval = 0.25f;

	// Frame Count
	private float offsetAmountX;
	private float offsetAmountY;
	private float timeSince;
	private int frameNum;
	
	private Player p;

	void Start() {
		offsetAmountY = 1f / numFrames;
		
		p = transform.root.GetComponent<Player>();
	}

	void Update() {
		timeSince += Time.deltaTime;
		if (timeSince >= interval) {
			// Next Frame
			frameNum = (frameNum + 1) % numFrames;
			SetFrame(frameNum);

			// Reset Timer
			timeSince -= interval;
		}
	}

	public void SetFrame(int i) {
		renderer.material.mainTextureOffset = new Vector2(0, offsetAmountY * i);
	}
}
