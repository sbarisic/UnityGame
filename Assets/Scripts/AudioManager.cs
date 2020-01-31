using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

enum AudioEffects {
	PlayerWalk,
	PlayerLand,
	PlayerJump,
	PlayerShoot,
	PlayerReceiveDamage,
	PlayerDie,

	Gibs,

	UIButton,
	UIConfirm,
	UICancel,

	// Music folders go here
	MainMenu,
	LevelOne,
	LevelTwo,
	LevelThree
}

class AudioClipSet {
	List<AudioClip> Clips = new List<AudioClip>();

	public void Add(AudioClip Clip) {
		Clips.Add(Clip);
	}

	public AudioClip GetRandom() {
		return Clips[Utils.RandomInt(0, Clips.Count)];
	}
}

class AudioManager : MonoBehaviour {
	public static float VolumeSfx {
		get {
			return ValueSerializer.GetValue<float>(nameof(VolumeSfx));
		}

		set {
			ValueSerializer.SetValue(nameof(VolumeSfx), value);
		}
	}

	public static float VolumeMusic {
		get {
			return ValueSerializer.GetValue<float>(nameof(VolumeMusic));
		}

		set {
			ValueSerializer.SetValue(nameof(VolumeMusic), value);
		}
	}

	static GameObject AudioManagerObj;
	static AudioSource SfxSrc;
	static AudioSource MusicSrc;

	static Dictionary<AudioEffects, AudioClipSet> AudioClips = new Dictionary<AudioEffects, AudioClipSet>();

	static AudioManager() {
		AudioEffects[] AllAudioEffects = (AudioEffects[])Enum.GetValues(typeof(AudioEffects));

		foreach (AudioEffects Eff in AllAudioEffects) {
			string EffectName = Enum.GetName(typeof(AudioEffects), Eff);
			AudioClip[] Clips = Resources.LoadAll<AudioClip>("Sounds/" + EffectName);

			foreach (var C in Clips)
				RegisterAudioClip(Eff, C);
		}
	}

	static void RegisterAudioClip(AudioEffects Effect, AudioClip Clip) {
		if (!AudioClips.ContainsKey(Effect))
			AudioClips.Add(Effect, new AudioClipSet());

		AudioClips[Effect].Add(Clip);
	}

	static void CreateInstance() {
		if (AudioManagerObj != null)
			return;

		AudioManagerObj = new GameObject("Audio Manager");
		DontDestroyOnLoad(AudioManagerObj);

		GameObject SfxObj = new GameObject("Sfx Manager");
		SfxObj.transform.parent = AudioManagerObj.transform;
		SfxSrc = SfxObj.AddComponent<AudioSource>();

		GameObject MusicObj = new GameObject("Music Manager");
		MusicObj.transform.parent = AudioManagerObj.transform;
		MusicSrc = MusicObj.AddComponent<AudioSource>();
		MusicSrc.loop = true;
	}

	public static void StopMusic() {
		CreateInstance();

		if (MusicSrc.isPlaying)
			MusicSrc.Stop();
	}

	public static void PlayMusic(AudioClip Clip) {
		StopMusic();
		MusicSrc.volume = VolumeMusic;
		MusicSrc.clip = Clip;
		MusicSrc.Play();
	}

	public static void PlayMusic(AudioEffects Effect) {
		if (!AudioClips.ContainsKey(Effect)) {
			Debug.LogWarning("Music not found " + Effect);
			return;
		}

		PlayMusic(AudioClips[Effect].GetRandom());
	}

	public static void PlaySfx(AudioClip Clip) {
		CreateInstance();
		SfxSrc.volume = VolumeSfx;
		SfxSrc.PlayOneShot(Clip);
	}

	public static void PlaySfx(AudioEffects Effect) {
		if (!AudioClips.ContainsKey(Effect)) {
			Debug.LogWarning("Sfx not found " + Effect);
			return;
		}

		PlaySfx(AudioClips[Effect].GetRandom());
	}
}
