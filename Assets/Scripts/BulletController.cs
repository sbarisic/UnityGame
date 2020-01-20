﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {
	Vector2 DirNormal;
	float Speed;
	float DestroyTime;
	float Damage;

	Rigidbody2D body2d;

	public void OnBulletCreated(Vector3 DirNormal, float Speed, float Damage, float CreationTime, string Tag) {
		this.DirNormal = DirNormal;
		this.Speed = Speed;
		this.Damage = Damage;

		// TODO: Bullet alive for max 2 seconds, move to variable
		DestroyTime = CreationTime + 2.0f;
		tag = Tag;
	}

	void Start() {
		body2d = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate() {
		body2d.velocity = DirNormal * Speed;
	}

	void Update() {
		if (Time.time >= DestroyTime)
			ObjectPool.Free(this.gameObject);
	}

	void OnTriggerEnter2D(Collider2D Other) {
		// When bullet fired by player hits enemy
		if (tag == Tags.BulletPlayer && Other.tag == Tags.Enemy) {
			EnemyController ECtrl = Other.gameObject.GetComponent<EnemyController>();
			ECtrl.DealDamage(Damage);

			ObjectPool.Free(gameObject);
			return;
		}
	}
}