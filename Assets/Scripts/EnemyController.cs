using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {
	Rigidbody2D body2d;
	SpriteRenderer rnd;

	void Start() {
		body2d = GetComponent<Rigidbody2D>();
		rnd = GetComponent<SpriteRenderer>();
	}

	public void DealDamage(float Amt) {
		rnd.color = Utils.RandomColor();
	}
}
