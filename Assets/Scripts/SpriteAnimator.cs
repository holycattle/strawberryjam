using UnityEngine;
using System.Collections;

public class SpriteAnimator : MonoBehaviour {
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
		offsetAmountX = 1f / numFrames;
		offsetAmountY = 1f / numDirections;
		
		p = transform.root.GetComponent<Player>();
	}

	void Update() {
		timeSince += Time.deltaTime;
		if (timeSince >= interval) {
			// Next Frame
			frameNum = (frameNum + 1) % numFrames;
			SetFrame(frameNum, p.activeDirection);

			// Reset Timer
			timeSince -= interval;
		}
	}

	public void SetFrame(int i, int o) {
		renderer.material.mainTextureOffset = new Vector2(offsetAmountX * o, offsetAmountY * i);
	}
}
