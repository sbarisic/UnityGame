//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using Cinemachine;

//public class LevelManager : MonoBehaviour {

//	public GameObject currCheckpoint;

//	private PlayerController player;

//	public CinemachineVirtualCamera vcam;

//	// Start is called before the first frame update
//	void Start() {
//		player = FindObjectOfType<PlayerController>();
//	}

//	// Update is called once per frame
//	void Update() {

//	}

//	public void RespawnPlayer() {
//		//Debug.Log("Player ought to be respawned");

//		StartCoroutine(RespawnDelay());
//	}

//	IEnumerator RespawnDelay() {
//		//Vector2 deathPoint = player.gameObject.transform.position;

//		player.gameObject.SetActive(false);
		
//		vcam.enabled = false;

//		yield return new WaitForSeconds(1);

//		player.gameObject.SetActive(true);
//		Vector2 spawnPoint = currCheckpoint.transform.position;
//		player.transform.position = new Vector3(spawnPoint.x, spawnPoint.y, player.transform.position.z);
//		vcam.enabled = true;
//	}
//}
