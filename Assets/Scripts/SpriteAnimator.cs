using UnityEngine;
using System.Collections;

public class SpriteAnimator : MonoBehaviour {
	public int numFrames;
	public float interval = 0.25f;

	// Frame Count
	private float offsetAmount;
	private float timeSince;
	private int frameNum;

	void Start() {
		offsetAmount = 1f / numFrames;
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
		renderer.material.mainTextureOffset = new Vector2(offsetAmount * frameNum, 0f);
	}
}
