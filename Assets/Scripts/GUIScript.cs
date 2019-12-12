using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GUIScript : MonoBehaviour {
	public Button BtnContinue;

	void Start() {
		BtnContinue?.gameObject.SetActive(false);
	}

	public void OnButtonClick(string Action) {
		Debug.Log("You clicked " + Action);
	}

	void Update() {

	}
}
