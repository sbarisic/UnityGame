using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UObject = UnityEngine.Object;

static class ObjectPool {
	static List<GameObject> Pooled = new List<GameObject>();

	/// <summary>
	/// Gets or creates new instance of a prefab
	/// </summary>
	/// <param name="Prefab"></param>
	/// <returns></returns>
	public static GameObject Alloc(GameObject Prefab) {
		GameObject New = null;
		int RemoveIndex = -1;

		if (!IsPrefab(Prefab))
			throw new Exception("Alloc should only work with prefabs");

		for (int i = 0; i < Pooled.Count; i++)
			if (IsPrefabInstance(Prefab, Pooled[i])) {
				// Debug.Log(string.Format("{0} - Creating instance from pool", Prefab.name));

				RemoveIndex = i;
				New = Pooled[i];
				break;
			}

		if (RemoveIndex >= 0 && New != null)
			Pooled.RemoveAt(RemoveIndex);

		if (New == null) {
			// Debug.Log(string.Format("{0} - Creating new instance", Prefab.name));

			New = UObject.Instantiate(Prefab);
		}

		New.SetActive(true);
		return New;
	}

	public static void Free(GameObject Obj) {
		// Debug.Log(string.Format("{0} - Destroying and pooling", Obj.name));

		Obj.SetActive(false);
		Pooled.Add(Obj);
	}

	static bool IsPrefabInstance(GameObject Prefab, GameObject Instance) {
		// TODO: Better way, waaaaaaaaaaaaaaaaaaaaaaaa
		return Instance.name == (Prefab.name + "(Clone)");
	}

	static bool IsPrefab(GameObject Obj) {
		// TODO: Better way, waaaaaaaaaaaaaaaaaaaaaaaa #2

		if (Obj.gameObject.scene.name == null)
			return true;

		return false;
	}
}
