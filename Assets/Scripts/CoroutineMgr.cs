using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineMgr : MonoBehaviour {
	static CoroutineMgr Mgr;

	 static CoroutineMgr GetInstance() {
		if (Mgr == null) {
			GameObject CoroutineMgrObject = new GameObject("Coroutine Manager");
			DontDestroyOnLoad(CoroutineMgrObject);

			Mgr = CoroutineMgrObject.AddComponent<CoroutineMgr>();
		}

		return Mgr;
	}

	public static void Start(IEnumerator Coroutine) {
		GetInstance().StartCoroutine(Coroutine);
	}
}
