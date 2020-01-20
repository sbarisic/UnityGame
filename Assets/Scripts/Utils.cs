using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Random = System.Random;

static class Utils {
	static Random Rnd = new Random();

	public static float RandomFloat() {
		return (float)Rnd.NextDouble();
	}

	public static Color RandomColor() {
		return new Color(RandomFloat(), RandomFloat(), RandomFloat(), 1.0f);
	}
}

