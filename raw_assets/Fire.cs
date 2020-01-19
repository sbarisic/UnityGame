using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Scripts;

public class Fire : MonoBehaviour {
	const float DealDamageInterval = 0.5f;
	const int DamageAmt = 5;

	Animator Anim;
	float NextDealDamageTime;

	List<IDamagable> ObjectsInFire = new List<IDamagable>();

	void Start() {
		Anim = GetComponent<Animator>();

		if (Anim == null) {
			Debug.LogError("Animator component not found");
			return;
		}

		Anim.Play(0, -1, Random.Range(0.0f, 1.0f));
		NextDealDamageTime = Time.time;
	}

	void Update() {
		if (NextDealDamageTime < Time.time) {
			NextDealDamageTime = Time.time + DealDamageInterval;
			DealDamage();
		}
	}

	void DealDamage() {
		foreach (var O in ObjectsInFire)
			O.DealDamage(DamageAmt, DamageType.Fire);
	}

	void OnTriggerEnter(Collider C) {
		if (C.GetScript() is IDamagable D && !ObjectsInFire.Contains(D))
			ObjectsInFire.Add(D);
	}

	void OnTriggerExit(Collider C) {
		if (C.GetScript() is IDamagable D && ObjectsInFire.Contains(D))
			ObjectsInFire.Add(D);
	}
}
