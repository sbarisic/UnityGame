//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class KillPlayer : MonoBehaviour {
//	public LevelManager lvlMng;
//	// Start is called before the first frame update
//	void Start() {
//		lvlMng = FindObjectOfType<LevelManager>();

//	}

//	// Update is called once per frame
//	void Update() {

//	}

//	private void OnTriggerEnter2D(Collider2D collision) {
//		PlayerController chr = collision.gameObject.GetComponent<PlayerController>();

//		if (chr != null)
//			chr.OnReceiveDamage(999);

//		/*if (collision.gameObject.tag == "Player") {
//			lvlMng.RespawnPlayer();
//		}*/
//	}
//}
