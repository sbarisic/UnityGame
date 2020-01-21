using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEditor;
using UnityEngine;

[ExecuteInEditMode]
public class Waypoint : MonoBehaviour {
	public Waypoint Next;

	public string NextName;
	public bool AssignNext;
	public bool LinkToClosest;
	public string WaypointName;

	void Start() {
		tag = Tags.Waypoint;

		if (Application.isEditor && !Application.isPlaying) {
			StartCoroutine(StartInEditor());
			return;
		}
	}

	IEnumerator StartInEditor() {
		yield return new WaitForSeconds(0.1f);

		/*if (Next == null)
			LinkToClosestWaypoint();*/
	}

	void LinkToClosestWaypoint(float MinDist = 64) {
		Waypoint Closest = Utils.GetClosestWaypoint(transform.position, MinDist, this);
		if (Closest != null)
			Next = Closest;
	}

	void Update() {
		if (Application.isEditor && !Application.isPlaying) {
			UpdateInEditor();
			return;
		}
	}

	void UpdateInEditor() {
		bool Dirty = false;

		if (Next == this) {
			Next = null;
			Dirty = true;
		}

		if (Next != null && !Next.isActiveAndEnabled) {
			Next = null;
			Dirty = true;
		}

		// If this item is being selected and dragged
		// link automatically to a waypoint closer than 1 unit
		if (Next == null && Selection.Contains(gameObject)) {
			LinkToClosestWaypoint(1);
			Dirty = true;
		}

		// Assign next by name
		if (AssignNext) {
			if (!string.IsNullOrWhiteSpace(NextName)) {
				GameObject NextWaypoint = GameObject.Find(NextName);

				if (NextWaypoint == null)
					Debug.LogWarning(string.Format("Can not find waypoint by name '{0}'", NextName));
				else {
					Waypoint NextWaypointComponent = NextWaypoint.GetComponent<Waypoint>();

					if (NextWaypointComponent == null)
						Debug.LogWarning(string.Format("Selected object does not have waypoint component '{0}'", NextName));
					else
						Next = NextWaypointComponent;
				}
			}

			NextName = "";
			AssignNext = false;
			Dirty = true;
		}

		// Link to first closest
		if (LinkToClosest) {
			LinkToClosestWaypoint(256);
			LinkToClosest = false;
			Dirty = transform;
		}

		if (Dirty)
			EditorUtility.SetDirty(this);
	}

	void OnDrawGizmos() {
		const float SphereSize = 0.25f;

		bool IsSelected = Selection.Contains(gameObject);

		if (IsSelected) {
			if (string.IsNullOrWhiteSpace(WaypointName))
				Gizmos.color = Color.green;
			else
				Gizmos.color = Color.yellow;
		} else {
			if (string.IsNullOrWhiteSpace(WaypointName))
				Gizmos.color = Color.cyan;
			else
				Gizmos.color = Color.magenta;
		}

		Gizmos.DrawWireSphere(transform.position, SphereSize);
		Handles.Label((Vector2)transform.position + new Vector2(0, SphereSize * 2.5f), name);

		if (Next != null) {
			Gizmos.color = IsSelected ? Color.green : Color.white;
			Utils.DrawArrow(transform.position, Next.transform.position);
		}
	}
}
