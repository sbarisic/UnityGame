using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CameraScript : MonoBehaviour {
	public Material PostProcessingMat;
	public bool UsePostProcessing = true;

	Camera Cam;

	void Start() {
		Cam = GetComponent<Camera>();
		//Cam.backgroundColor = new Color(0, 0, 0, 0);
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
