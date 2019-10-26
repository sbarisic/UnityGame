using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Scripts;

public class PawnController : MonoBehaviour, IAlive {
	public int Health = 100;

	protected CapsuleCollider2D Collider;
	protected Rigidbody2D Body;
	protected SpriteRenderer Rend;

	void Start() {
		Body = GetComponent<Rigidbody2D>();
		Rend = GetComponent<SpriteRenderer>();
		Collider = GetComponent<CapsuleCollider2D>();

		OnStart();
	}

	void Update() {
		OnUpdate();
	}

	void FixedUpdate() {
		OnFixedUpdate();
	}

	public virtual void OnStart() {
	}

	public virtual void OnUpdate() {
	}

	public virtual void OnFixedUpdate() {
	}

	public void DealDamage(int Damage, DamageType DType = DamageType.Default) {
		Debug.Log("Pawn taking damage " + Damage + ", " + DType);
	}

	public int GetHealth() {
		return Health;
	}

	public void Heal(int HP) {
		Health += HP;
	}
}
