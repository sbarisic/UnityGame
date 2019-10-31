using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Game.Scripts;

public class CameraScript : MonoBehaviour {
	public Material PostProcessingMat;
	public bool UsePostProcessing = true;

	public GameObject FollowObject;

	Camera Cam;

	void Start() {
		Cam = GetComponent<Camera>();
		//Cam.backgroundColor = new Color(0, 0, 0, 0);
	}

	void LateUpdate() {
		if (FollowObject == null)
			return;

		// TODO: Horizontal smoothing and vertical offset
		Vector2 FollowPos = new Vector2(FollowObject.transform.position.x, FollowObject.transform.position.y);
		Vector2 CurPos = new Vector2(transform.position.x, transform.position.y);

		float DistX = 0;
		float DistY = 0;

		if (Utilities.DistanceX(FollowPos, CurPos, out DistX) > 2) {
			// TODO: Better way?
			if (DistX > 0)
				CurPos = CurPos + new Vector2(DistX - 2, 0);
			else
				CurPos = CurPos + new Vector2(DistX + 2, 0);
		}

		transform.position = new Vector3(CurPos.x, CurPos.y, transform.position.z);
	}

	void OnRenderImage(RenderTexture Src, RenderTexture Dst) {
		float W = Camera.current.pixelWidth;
		float H = Camera.current.pixelHeight;

		//float Ratio = W / H;
		//float RatioW = Cam.orthographicSize * Ratio;
		//Graphics.DrawTexture(new Rect(-(RatioW / 2), 0, RatioW, 1), Parallax1);

		if (UsePostProcessing) {
			PostProcessingMat.SetVector("Resolution", new Vector4(W, H, 0, 0));
			Graphics.Blit(Src, Dst, PostProcessingMat);
		} else
			Graphics.Blit(Src, Dst);
	}
}
