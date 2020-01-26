using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour {
	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
	void Update() {

	}

	private void OnTriggerEnter2D(Collider2D collision) {
		PlayerController chr = collision.gameObject.GetComponent<PlayerController>();

		if (chr != null)
			chr.OnReceiveDamage(999);
	}
}
