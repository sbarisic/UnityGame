using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour {

	public GameObject currCheckpoint;

	private PlayerController player;

	// Start is called before the first frame update
	void Start() {
		player = FindObjectOfType<PlayerController>();
	}

	// Update is called once per frame
	void Update() {

	}

	public void RespawnPlayer() {
		Debug.Log("Player ought to be respawned");
		player.transform.position = currCheckpoint.transform.position;
	}
}
