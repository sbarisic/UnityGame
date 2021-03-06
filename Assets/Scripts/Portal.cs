﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour {
	public string NextLevel;
	public GameObject PortalParticles;

	public void LoadNextLevel() {
		if (NextLevel == "ENDGAME") {
			GUI GameGUI = GameObject.Find("Canvas")?.GetComponent<GUI>();

			if (GameGUI != null)
				GameGUI.ShowSaveHighscore();

			return;
		}

		if (string.IsNullOrWhiteSpace(NextLevel)) {
			Debug.LogWarning("Next level not defined");
			return;
		}

		SceneManager.LoadScene(NextLevel, LoadSceneMode.Single);
	}

	IEnumerator WaitAndLoadNextLevel() {
		yield return new WaitForSeconds(2);
		LoadNextLevel();
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.tag == Tags.Player) {
			collision.gameObject.GetComponent<PlayerController>()?.OnPortalEnter();

			Destroy(collision.gameObject);

			GameObject partObj = ObjectPool.Alloc(PortalParticles);
			partObj.transform.position = transform.position;
			partObj.transform.rotation = transform.rotation;

			CoroutineMgr.Start(WaitAndLoadNextLevel());
		}
	}
}
