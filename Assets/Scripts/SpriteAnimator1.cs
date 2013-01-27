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
