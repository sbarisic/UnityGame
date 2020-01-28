using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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

	public static bool SameSign(float A, float B) {
		if (A < 0 && B < 0)
			return true;

		if (A > 0 && B > 0)
			return true;

		return false;
	}


	public static void DrawArrow(Vector2 Start, Vector2 End, float ArrowHeadLength = 0.5f) {
		Vector2 Mid = (Start + End) / 2;
		Vector2 Dir = (Vector2)Vector3.Normalize(End - Start) * ArrowHeadLength;

		Gizmos.DrawLine(Start, End);
		Gizmos.DrawLine(Mid, Mid + (Vector2)(Quaternion.Euler(0, 0, 150) * Dir));
		Gizmos.DrawLine(Mid, Mid + (Vector2)(Quaternion.Euler(0, 0, -150) * Dir));
	}

	public static float Angle(Vector2 A, Vector2 B) {
		return (float)(Math.Atan2(B.y - A.y, B.x - A.x) * (180.0 / Math.PI));
	}

	public static Waypoint GetClosestWaypoint(Vector2 Position, float MinDist = 64, Waypoint IgnoreWaypoint = null) {
		Waypoint[] ClosestWaypoints = GameObject.FindGameObjectsWithTag(Tags.Waypoint).Select(GO => GO.GetComponent<Waypoint>()).Where(W => W != null).ToArray();

		if (IgnoreWaypoint != null)
			ClosestWaypoints = ClosestWaypoints.Where(W => W != IgnoreWaypoint).ToArray();

		Waypoint Closest = null;

		for (int i = 0; i < ClosestWaypoints.Length; i++) {
			float CurDist = Vector2.Distance(ClosestWaypoints[i].transform.position, Position);

			if (CurDist >= MinDist)
				continue;

			MinDist = CurDist;
			Closest = ClosestWaypoints[i];
		}

		if (Closest != null)
			return Closest;

		return null;
	}
}

