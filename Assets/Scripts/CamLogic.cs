using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CamLogic : MonoBehaviour {
	public Texture[] Textures;

	Camera Cam;
	Vector2 LastCamPos;
	Vector2 DeltaSmooth;

	void Start() {
		Cam = GetComponent<Camera>();
		LastCamPos = Cam.transform.position;
	}

	void Update() {
	}

	void OnPreRender() {
		if (Textures == null || Cam == null)
			return;

		Vector2 PosDelta = LastCamPos - (Vector2)Cam.transform.position;
		LastCamPos = Cam.transform.position;
		DeltaSmooth = Vector2.MoveTowards(DeltaSmooth, PosDelta, 0.01f);
		float DeltaScale = 20;

		GL.PushMatrix();
		GL.LoadPixelMatrix();
		GL.Clear(true, true, Color.black);

		for (int i = 0; i < Textures.Length; i++)
			DrawTex(DeltaSmooth * DeltaScale * i, Textures[i]);

		GL.PopMatrix();
	}

	void DrawTex(Vector2 Pos, Texture Tex) {
		if (Tex == null)
			return;

		Rect r = Cam.pixelRect;
		r.x += Pos.x;
		r.y += Pos.y;
		Graphics.DrawTexture(r, Tex);
	}
}
