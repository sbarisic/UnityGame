using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UObject = UnityEngine.Object;
using System.Diagnostics;

#if UNITY_EDITOR
using UnityEditor;
#endif

static class EditorUtils {
	public static bool IsEditor() {
#if UNITY_EDITOR
		return true;
#else
		return false;
#endif
	}

	public static bool SelectionContains(UObject Obj) {
#if UNITY_EDITOR
		return Selection.Contains(Obj);
#else
		return false;
#endif
	}

	public static void SetDirty(UObject Obj) {
#if UNITY_EDITOR
		EditorUtility.SetDirty(Obj);
#endif
	}

	public static void DrawLabel(Vector2 Pos, string Txt) {
#if UNITY_EDITOR
		Handles.Label(Pos, Txt);
#endif
	}
}

