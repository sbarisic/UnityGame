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
	}
}
