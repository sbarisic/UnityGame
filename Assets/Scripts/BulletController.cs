using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class BulletController : MonoBehaviour {
	Vector2 DirNormal;
	float Speed;
	float DestroyTime;
	int Damage;

	Rigidbody2D body2d;

	public Action OnHitEnemy;

	public void OnBulletCreated(Vector3 DirNormal, float Speed, int Damage, string Tag) {
		this.DirNormal = DirNormal;
		this.Speed = Speed;
		this.Damage = Damage;
		OnHitEnemy = null;

		// TODO: Bullet alive for max 2 seconds, move to variable
		DestroyTime = Time.time + 1.0f;
		tag = Tag;
	}

	void Start() {
		body2d = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate() {
		body2d.velocity = DirNormal * Speed;
		transform.rotation = Quaternion.Euler(0, 0, Utils.Angle(Vector2.zero, DirNormal));
	}

	void Update() {
		if (Time.time >= DestroyTime)
			ObjectPool.Free(this.gameObject);
	}

	void OnTriggerEnter2D(Collider2D Other) {
		// Bullet hits ground
		if (Other.tag == Tags.Ground) {
			ObjectPool.Free(gameObject);
			return;
		}

		// When bullet fired by player hits enemy
		if (tag == Tags.BulletPlayer && Other.tag == Tags.Enemy) {
			EnemyController ECtrl = Other.gameObject.GetComponent<EnemyController>();
			ECtrl.OnReceiveDamage(Damage);
			//ECtrl.OnReceiveDamage(Damage);

			OnHitEnemy?.Invoke();

			ObjectPool.Free(gameObject);
			return;
		}
	}
}
