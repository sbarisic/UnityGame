using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireAnimation : MonoBehaviour {
	Animator Anim;

	void Start() {
		Anim = GetComponent<Animator>();

		if (Anim == null) {
			Debug.LogError("Animator component not found");
			return;
		}

		Anim.Play(0, -1, Random.Range(0.0f, 1.0f));
	}

	void Update() {

	}
}
