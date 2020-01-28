using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUI : MonoBehaviour {
	public GameObject EventSystem;

	void Start() {
		DontDestroyOnLoad(gameObject);
		DontDestroyOnLoad(EventSystem);
	}

	void ResetCamera() {

	}

	void Update() {

	}

	public void OnContinue() {

	}

	public void OnNewGame() {
		SceneManager.LoadScene("LevelOne");
	}

	public void OnSettings() {

	}

	public void OnQuit() {
		Application.Quit();
	}
}
