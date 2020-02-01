using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class Gib : MonoBehaviour {
	public static bool Enabled {
		get {
			return ValueSerializer.GetValue("GibEnabled", false);
		}

		set {
			ValueSerializer.SetValue("GibEnabled", value);
		}
	}

	static GameObject[] GibPrefabs = null;

	public static void SpawnRandomGib(Vector2 Pos) {
		if (!Enabled)
			return;

		if (GibPrefabs == null)
			GibPrefabs = Resources.LoadAll<GameObject>("Prefabs/Gibs");

		GameObject gibInstance = ObjectPool.Alloc(Utils.Random(GibPrefabs));
		gibInstance.transform.position = Pos;

		Rigidbody2D body2d = gibInstance.GetComponent<Rigidbody2D>();
		body2d.velocity = Utils.NormalFromAngle(135 - Utils.RandomFloat() * 90) * 5;

		CoroutineMgr.Start(gibInstance.GetComponent<Gib>().WaitAndFree());
	}

	public static void SpawnRandomGibs(Vector2 Pos, int Count) {
		if (!Enabled)
			return;

		AudioManager.PlaySfx(AudioEffects.Gibs);

		for (int i = 0; i < Count; i++)
			SpawnRandomGib(Pos);
	}

	public GameObject Particles;

	void FreeObject() {
		if (!ObjectPool.Free(gameObject))
			return;

		if (Particles != null) {
			GameObject ParticlesInstance = ObjectPool.Alloc(Particles);
			ParticlesInstance.transform.position = transform.position;
		}
	}

	IEnumerator WaitAndFree() {
		yield return new WaitForSeconds(5);
		FreeObject();
	}

	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.tag == Tags.Death)
			FreeObject();
	}
}
