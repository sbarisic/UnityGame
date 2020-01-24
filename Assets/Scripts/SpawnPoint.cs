using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
	public LevelManager lvlMng;
	// Start is called before the first frame update
	void Start() {
		lvlMng = FindObjectOfType<LevelManager>();

	}


	// Update is called once per frame
	void Update()
    {
        
    }

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.tag == "Player") {
			lvlMng.currCheckpoint = gameObject;
		}
	}
}
