using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GUI : MonoBehaviour {
	// Start is called before the first frame update
	void Start() {

	}

	// Update is called once per frame
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
