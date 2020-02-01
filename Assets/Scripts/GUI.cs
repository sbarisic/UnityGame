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
	static bool AlreadyLoaded = false;
	static float GUIAnimationTime = 600;

	GUIElementState BtnHighscore;
	GUIElementState BtnContinue;
	GUIElementState BtnNewGame;
	GUIElementState BtnSettings;
	GUIElementState BtnQuit;

	GUIElementState PnlSettings;
	GUIElementState PnlYesNo;
	GUIElementState PnlHighscore;
	GUIElementState PnlHUD;
	GUIElementState PnlEnterHighscore;

	List<GUIElementState> GUIStates = new List<GUIElementState>();

	bool IsInPauseMenu;
	Text HighscoreText;
	Toggle TglGibsEnabled;
	Slider SldMusic;
	Slider SldSfx;

	Text RickHealth;
	Text LevelTime;
	Text Score;

	InputField InputNameField;

	int PlayerScore;

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

	void Start() {
		if (AlreadyLoaded) {
			Destroy(gameObject);
			return;
		}

		AlreadyLoaded = true;

		DontDestroyOnLoad(gameObject);
		DontDestroyOnLoad(GameObject.Find("EventSystem"));

		RickHealth = GameObject.Find("TxtRickHealth")?.GetComponent<Text>();
		LevelTime = GameObject.Find("TxtLevelTime")?.GetComponent<Text>();
		Score = GameObject.Find("TxtScore")?.GetComponent<Text>();
		InputNameField = GameObject.Find("InputNameField").GetComponent<InputField>();

		HighscoreText = GameObject.Find("HighscoreText")?.GetComponent<Text>();
		TglGibsEnabled = GameObject.Find("TglGibsEnabled").GetComponent<Toggle>();
		SldMusic = GameObject.Find("SldMusic").GetComponent<Slider>();
		SldSfx = GameObject.Find("SldSfx").GetComponent<Slider>();

		BtnHighscore = AddGUIState("BtnHighscore", new Vector2(90, 80));
		BtnContinue = AddGUIState("BtnContinue", new Vector2(90, 20));
		BtnNewGame = AddGUIState("BtnNewGame", new Vector2(90, -15));
		BtnSettings = AddGUIState("BtnSettings", new Vector2(90, -50));
		BtnQuit = AddGUIState("BtnQuit", new Vector2(90, -85));

		PnlSettings = AddGUIState("PnlSettings", new Vector2(250, 0));
		PnlYesNo = AddGUIState("PnlYesNo", new Vector2(250, 0));
		PnlHighscore = AddGUIState("PnlHighscore", new Vector2(250, 0));
		PnlHUD = AddGUIState("PnlHUD", Vector2.zero);
		PnlEnterHighscore = AddGUIState("PnlEnterHighscore", new Vector2(250, 0));

		ShowMainMenu();

		SceneManager.sceneLoaded += (Scene, Mode) => {
			if (Enum.TryParse(Scene.name, out AudioEffects AudioEffect))
				AudioManager.PlayMusic(AudioEffect);
		};
	}

	public void ShowMainMenu(bool IsInGame = false) {
		BtnHighscore.Shown = true;

		BtnContinue.Shown = (IsInPauseMenu = IsInGame);
		BtnNewGame.Shown = true;
		BtnSettings.Shown = true;
		BtnQuit.Shown = true;
		PnlHUD.Shown = IsInGame;
	}

	public void ShowSaveHighscore() {
		HideAllElements();
		InputNameField.text = "";
		PnlEnterHighscore.Shown = true;
	}

	void Animate(GameObject Obj, Vector2 TargetPos) {
		RectTransform Trans = Obj.GetComponent<RectTransform>();
		Trans.anchoredPosition = Vector2.MoveTowards(Trans.anchoredPosition, TargetPos, GUIAnimationTime * Time.deltaTime);
	}

	void Update() {
		foreach (var States in GUIStates)
			Animate(States.Obj, States.GetTargetPos());


	}

	public void SetRickHealth(int Amt) {
		RickHealth.text = string.Format("Rick\n{0}%", Amt);
	}

	public void SetScore(int Amt) {
		PlayerScore = Amt;
		Score.text = string.Format("Score\n{0}", Amt);
	}

	public void SetTime(int Amt) {
		LevelTime.text = string.Format("Time\n{0}", Amt);
	}

	public void AddScore(int Amt) {
		SetScore(PlayerScore + Amt);
	}

	// Main menu buttons

	public void OnHighscore() {
		AudioManager.PlaySfx(AudioEffects.UIButton);

		HighscoreText.text = Highscore.GetInstance().ToString();
		PnlHighscore.Shown = true;
	}

	public void OnCloseHighscore() {
		AudioManager.PlaySfx(AudioEffects.UIButton);

		PnlHighscore.Shown = false;
	}

	public void OnContinue() {
		AudioManager.PlaySfx(AudioEffects.UIButton);

		HideAllElements();
		IsInPauseMenu = false;
		PnlHUD.Shown = true;
	}

	public void OnNewGame() {
		AudioManager.PlaySfx(AudioEffects.UIButton);

		if (IsPaused()) {
			AskConfirmation("Start new game?", StartNewGame);
			return;
		}

		StartNewGame();
	}

	void StartNewGame() {
		AudioManager.StopMusic();

		SetScore(0);

		HideAllElements();
		IsInPauseMenu = false;
		PnlHUD.Shown = true;
		SceneManager.LoadScene("LevelOne");
	}

	public void OnSettings() {
		AudioManager.PlaySfx(AudioEffects.UIButton);

		TglGibsEnabled.isOn = Gib.Enabled;
		SldMusic.value = AudioManager.VolumeMusic;
		SldSfx.value = AudioManager.VolumeSfx;

		PnlSettings.Shown = true;
	}

	void ExitToMainMenu() {
		SceneManager.LoadScene("MainMenu");
		ShowMainMenu();
	}

	public void OnQuit() {
		AudioManager.PlaySfx(AudioEffects.UIButton);

		if (IsInPauseMenu)
			AskConfirmation("Exit to main menu?", ExitToMainMenu);
		else
			AskConfirmation("Exit to desktop and discard progress?", Application.Quit);
	}

	// Settings buttons

	public void OnSettingsSave() {
		AudioManager.PlaySfx(AudioEffects.UIButton);

		PnlSettings.Shown = false;

		Gib.Enabled = TglGibsEnabled.isOn;
		AudioManager.VolumeMusic = SldMusic.value;
		AudioManager.VolumeSfx = SldSfx.value;
	}

	public void OnSettingsCancel() {
		AudioManager.PlaySfx(AudioEffects.UIButton);

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
		AudioManager.PlaySfx(AudioEffects.UIConfirm);

		PnlYesNo.Shown = false;
		OnYesAction?.Invoke();
		OnYesAction = null;
	}

	public void OnNo() {
		AudioManager.PlaySfx(AudioEffects.UICancel);

		PnlYesNo.Shown = false;
		OnNoAction?.Invoke();
		OnNoAction = null;
	}

	// Save highscore logic

	public void OnSaveHighscore() {
		string Name = InputNameField.text.Trim();
		InputNameField.text = "";

		if (string.IsNullOrWhiteSpace(Name))
			return;

		Highscore.GetInstance().Add(Name, PlayerScore);
		PnlEnterHighscore.Shown = false;

		ExitToMainMenu();
		OnHighscore();
	}
}
