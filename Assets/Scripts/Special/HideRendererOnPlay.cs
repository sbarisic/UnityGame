using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class HideRendererOnPlay : MonoBehaviour {
	//public GameObject MenuPanel;

	void Start() {
		Renderer Rnd = GetComponent<Renderer>();

		if (Rnd == null) {
			Debug.LogWarning("Renderer component not found");
			return;
		}

		Rnd.enabled = false;
	}
}
