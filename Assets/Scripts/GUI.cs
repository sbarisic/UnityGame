using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

class GUIElementState {
	public bool Shown;

	public GameObject Obj;
	public Vector2 HiddenPos;
	public Vector2 ShownPos;

	public GUIElementState(GameObject Obj, bool Shown, Vector2 HiddenPos, Vector2 ShownPos) {
		this.Obj = Obj;
		this.Shown = Shown;
		this.HiddenPos = HiddenPos;
		this.ShownPos = ShownPos;
	}

	public Vector2 GetTargetPos() {
		if (Shown)
			return ShownPos;

		return HiddenPos;
	}
}

public class GUI : MonoBehaviour {
	public GameObject EventSystem;


	GUIElementState BtnContinue;
	GUIElementState BtnNewGame;
	GUIElementState BtnSettings;
	GUIElementState BtnQuit;
	GUIElementState PnlSettings;

	List<GUIElementState> GUIStates = new List<GUIElementState>();

	GUIElementState AddGUIState(string ElementName, Vector2 ShownPos) {
		GameObject Obj = GameObject.Find(ElementName);

		GUIElementState State = new GUIElementState(Obj, false, Obj.GetComponent<RectTransform>().anchoredPosition, ShownPos);
		GUIStates.Add(State);
		return State;
	}

	void HideAllElements() {
		foreach (var S in GUIStates)
			S.Shown = false;
	}

	public void ShowMainMenu(bool IsInGame = false) {
		BtnContinue.Shown = IsInGame;
		BtnNewGame.Shown = true;
		BtnSettings.Shown = true;
		BtnQuit.Shown = true;
	}

	void Start() {
		DontDestroyOnLoad(gameObject);
		DontDestroyOnLoad(EventSystem);

		BtnContinue = AddGUIState("BtnContinue", new Vector2(90, 20));
		BtnNewGame = AddGUIState("BtnNewGame", new Vector2(90, -15));
		BtnSettings = AddGUIState("BtnSettings", new Vector2(90, -50));
		BtnQuit = AddGUIState("BtnQuit", new Vector2(90, -85));

		PnlSettings = AddGUIState("PnlSettings", new Vector2(130, 0));
		ShowMainMenu();
	}

	/*void ResetCamera() {

	}*/

	void Animate(GameObject Obj, Vector2 TargetPos) {
		RectTransform Trans = Obj.GetComponent<RectTransform>();
		Trans.anchoredPosition = Vector2.MoveTowards(Trans.anchoredPosition, TargetPos, 400 * Time.deltaTime);
	}

	void Update() {
		foreach (var States in GUIStates)
			Animate(States.Obj, States.GetTargetPos());
	}

	// Main menu buttons

	public void OnContinue() {
		HideAllElements();
	}

	public void OnNewGame() {
		BtnContinue.Shown = false;
		BtnNewGame.Shown = false;
		BtnSettings.Shown = false;
		BtnQuit.Shown = false;
		SceneManager.LoadScene("LevelOne");
	}

	public void OnSettings() {
		GameObject.Find("TglGibsEnabled").GetComponent<Toggle>().isOn = Gib.Enabled;
		PnlSettings.Shown = true;
	}

	public void OnQuit() {
		Application.Quit();
	}

	// Settings buttons

	public void OnSettingsSave() {
		PnlSettings.Shown = false;
		Gib.Enabled = GameObject.Find("TglGibsEnabled").GetComponent<Toggle>().isOn;
	}

	public void OnSettingsCancel() {
		PnlSettings.Shown = false;
	}
}
