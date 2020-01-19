using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*
TODO: IT DOES NOT WORK AAAAAAAAAAAAAAAAAA
*/

public class Parallax : MonoBehaviour {
	public Color Tint = new Color(0.4f, 0.4f, 0.4f);
	public Vector2 Offset = new Vector2(0, 1.5f);
	public Sprite[] BackgroundSprites;

	public float SpriteScale = 1;

	SpriteRenderer[] Rnds;

	void Start() {
		if (BackgroundSprites == null)
			return;

		Rnds = new SpriteRenderer[BackgroundSprites.Length];

		for (int i = 0; i < BackgroundSprites.Length; i++)
			Rnds[i] = AddSpriteRenderer(-100 - i, BackgroundSprites[i], i >= BackgroundSprites.Length - 1 ? (Color?)null : Tint);
	}

	SpriteRenderer AddSpriteRenderer(int Order, Sprite S, Color? Tint = null) {
		GameObject RndObj = new GameObject();
		RndObj.transform.parent = gameObject.transform;
		RndObj.transform.localPosition = Vector3.zero;

		SpriteRenderer Rnd = RndObj.AddComponent<SpriteRenderer>();
		Rnd.transform.localScale = new Vector3(SpriteScale, SpriteScale, 1);
		Rnd.sortingOrder = Order;
		Rnd.sprite = S;

		if (Tint != null)
			Rnd.color = Tint.Value;

		return Rnd;
	}
}
