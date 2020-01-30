using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

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
	static float GUIAnimationTime = 600;

	public GameObject EventSystem;

	GUIElementState BtnHighscore;
	GUIElementState BtnContinue;
	GUIElementState BtnNewGame;
	GUIElementState BtnSettings;
	GUIElementState BtnQuit;

	GUIElementState PnlSettings;
	GUIElementState PnlYesNo;
	GUIElementState PnlHighscore;

	List<GUIElementState> GUIStates = new List<GUIElementState>();

	bool IsInPauseMenu;
	Text HighscoreText;

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

	public bool IsPaused() {
		return IsInPauseMenu;
	}

	public void ShowMainMenu(bool IsInGame = false) {
		BtnHighscore.Shown = true;

		BtnContinue.Shown = (IsInPauseMenu = IsInGame);
		BtnNewGame.Shown = true;
		BtnSettings.Shown = true;
		BtnQuit.Shown = true;
	}

	void Start() {
		DontDestroyOnLoad(gameObject);
		DontDestroyOnLoad(EventSystem);

		HighscoreText = GameObject.Find("HighscoreText")?.GetComponent<Text>();

		BtnHighscore = AddGUIState("BtnHighscore", new Vector2(90, 80));
		BtnContinue = AddGUIState("BtnContinue", new Vector2(90, 20));
		BtnNewGame = AddGUIState("BtnNewGame", new Vector2(90, -15));
		BtnSettings = AddGUIState("BtnSettings", new Vector2(90, -50));
		BtnQuit = AddGUIState("BtnQuit", new Vector2(90, -85));

		PnlSettings = AddGUIState("PnlSettings", new Vector2(250, 0));
		PnlYesNo = AddGUIState("PnlYesNo", new Vector2(250, 0));
		PnlHighscore = AddGUIState("PnlHighscore", new Vector2(250, 0));
		ShowMainMenu();
	}

	void Animate(GameObject Obj, Vector2 TargetPos) {
		RectTransform Trans = Obj.GetComponent<RectTransform>();
		Trans.anchoredPosition = Vector2.MoveTowards(Trans.anchoredPosition, TargetPos, GUIAnimationTime * Time.deltaTime);
	}

	void Update() {
		foreach (var States in GUIStates)
			Animate(States.Obj, States.GetTargetPos());
	}

	// Main menu buttons

	public void OnHighscore() {
		HighscoreText.text = Highscore.GetInstance().ToString();
		PnlHighscore.Shown = true;
	}

	public void OnCloseHighscore() {
		PnlHighscore.Shown = false;
	}

	public void OnContinue() {
		HideAllElements();
		IsInPauseMenu = false;
	}

	public void OnNewGame() {
		if (IsPaused()) {
			AskConfirmation("Start new game?", StartNewGame);
			return;
		}

		StartNewGame();
	}

	void StartNewGame() {
		OnContinue();
		SceneManager.LoadScene("LevelOne");
	}

	public void OnSettings() {
		GameObject.Find("TglGibsEnabled").GetComponent<Toggle>().isOn = Gib.Enabled;
		PnlSettings.Shown = true;
	}

	public void OnQuit() {
		AskConfirmation("Exit to desktop and discard progress?", Application.Quit);
	}

	// Settings buttons

	public void OnSettingsSave() {
		PnlSettings.Shown = false;
		Gib.Enabled = GameObject.Find("TglGibsEnabled").GetComponent<Toggle>().isOn;
	}

	public void OnSettingsCancel() {
		PnlSettings.Shown = false;
	}

	// YesNo panel confirmation logic
	Action OnYesAction;
	Action OnNoAction;

	public void AskConfirmation(string Text, Action OnYes = null, Action OnNo = null) {
		Text TxtComp = GameObject.Find("YesNoText")?.GetComponent<Text>() ?? null;

		if (TxtComp != null)
			TxtComp.text = Text;

		OnYesAction = OnYes;
		OnNoAction = OnNo;
		PnlYesNo.Shown = true;
	}

	public void OnYes() {
		PnlYesNo.Shown = false;
		OnYesAction?.Invoke();
		OnYesAction = null;
	}

	public void OnNo() {
		PnlYesNo.Shown = false;
		OnNoAction?.Invoke();
		OnNoAction = null;
	}
}
