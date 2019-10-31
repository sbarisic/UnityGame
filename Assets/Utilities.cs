using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Game.Scripts {
	static class Utilities {
		public static MonoBehaviour GetScript(this Component C) {
			Component[] Components = C.gameObject.GetComponents<MonoBehaviour>();

			foreach (var Comp in Components)
				if (Comp is IGameObject && Comp is MonoBehaviour B)
					return B;

			return null;
		}

		public static Vector2 MoveTowards(Vector2 Cur, Vector2 Tgt, Vector2 Delta) {
			return new Vector2(Mathf.MoveTowards(Cur.x, Tgt.x, Delta.x), Mathf.MoveTowards(Cur.y, Tgt.y, Delta.y));
		}

		public static Vector2 MoveTowards(Vector2 Cur, Vector2 Tgt, float Delta) {
			return MoveTowards(Cur, Tgt, new Vector2(Delta, Delta));
		}

		public static float DistanceX(Vector2 A, Vector2 B, out float SignedDist) {
			return Mathf.Abs(SignedDist = (A.x - B.x));
		}

		public static float DistanceY(Vector2 A, Vector2 B) {
			return Mathf.Abs(A.y - B.y);
		}
	}
}
