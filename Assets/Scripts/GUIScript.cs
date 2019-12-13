using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class GUIScript : MonoBehaviour {
	public Button BtnContinue;
	public Image BackgroundImage;

	// YesNo animated panel
	public GameObject PanelYesNo;

	// YesNo animated panel text
	public Text YesNoPrompt;

	// Internal
	Animator YesNoAnim;
	Action OnYes;
	Action OnNo;

	void Start() {
		BtnContinue?.gameObject.SetActive(false);
		BackgroundImage?.gameObject.SetActive(false);

		YesNoAnim = PanelYesNo.GetComponent<Animator>();
		YesNoAnim.SetBool("open", false);
			}

	public void OnButtonClick(string Action) {
		if (Action == "yes") {
			OnYes?.Invoke();
			YesNoAnim.SetBool("open", false);
			return;
		} else if (Action == "no") {
			OnNo?.Invoke();
			YesNoAnim.SetBool("open", false);
			return;
		}

		if (Action == "exit")
			OpenYesNo("Are you sure you want to exit?", OnExitConfirm);

		if (Action == "newgame")
			OpenYesNo("Start new game?", OnStartNewGameConfirm);

		Debug.Log("You clicked " + Action);
	}

	void OpenYesNo(string Text, Action OnYes, Action OnNo = null) {
		if (!YesNoAnim.GetBool("open")) {
			this.OnYes = OnYes;
			this.OnNo = OnNo;

			YesNoPrompt.text = Text;
			YesNoAnim.SetBool("open", true);
		}
	}

	void OnExitConfirm() {
		Debug.Log("Exiting!");
		Application.Quit();
	}

	void OnStartNewGameConfirm() {
		Debug.Log("Starting new game!");
	}

	void Update() {

	}
}
