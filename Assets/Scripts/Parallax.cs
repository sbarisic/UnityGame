using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {
	public Color Tint = new Color(0.4f, 0.4f, 0.4f);
	public Vector2 Offset = new Vector2(0, 1.5f);
	public Sprite[] BackgroundSprites;

	SpriteRenderer[] Rnds;

	void Start() {
		if (BackgroundSprites == null)
			return;

		Rnds = new SpriteRenderer[BackgroundSprites.Length];

		for (int i = 0; i < BackgroundSprites.Length; i++)
			Rnds[i] = AddSpriteRenderer(-100 - i, BackgroundSprites[i], i >= BackgroundSprites.Length - 1 ? (Color?)null : Tint);

		transform.position = new Vector3(Offset.x, Offset.y, 0);
	}

	SpriteRenderer AddSpriteRenderer(int Order, Sprite S, Color? Tint = null) {
		GameObject RndObj = new GameObject();
		RndObj.transform.parent = gameObject.transform;
		RndObj.transform.localPosition = Vector3.zero;

		SpriteRenderer Rnd = RndObj.AddComponent<SpriteRenderer>();
		Rnd.sortingOrder = Order;
		Rnd.sprite = S;

		if (Tint != null)
			Rnd.color = Tint.Value;

		return Rnd;
	}

	void Update() {
		Camera Cur = Camera.current;

		if (Cur == null)
			return;

		Vector3 CamPos = Cur.transform.position;
		transform.position = new Vector3(CamPos.x + Offset.x, CamPos.y + Offset.y, 0);
	}
}
