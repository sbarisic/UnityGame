using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[ExecuteInEditMode]
public class EnemyController : MonoBehaviour {
	Rigidbody2D body2d;
	SpriteRenderer rnd;

	public Waypoint FirstWaypoint;
	public bool LinkToClosest;

	// Teleport to first waypoint
	[Space(10)]
	public bool StartAtWaypoint = true;
	public bool IsFlying = true;

	// Flip texture X when changing horizontal movement direction
	public bool FlipTowardsMoveDir = true;

	// Rotate towards move direction
	// Smooth rotation and recovery rate towards movement direction
	[Space(10)]
	public bool RotTowardsMoveDir = false;
	public bool SmoothRotation = false;
	public float SmoothRate = 360;

	Waypoint NextWaypoint;
	bool HasSpawned = false;
	float MoveSpeed = 4;

	void Start() {
		if (Application.isEditor && !Application.isPlaying)
			return;

		body2d = GetComponent<Rigidbody2D>();
		rnd = GetComponent<SpriteRenderer>();

		if (IsFlying)
			body2d.gravityScale = 0;
	}

	public virtual void DealDamage(float Amt) {
		rnd.color = Utils.RandomColor();
	}

	/// <summary>
	/// Gets called when enemy reaches a waypoint
	/// </summary>
	/// <param name="Wp"></param>
	protected virtual void OnWaypointReached(Waypoint Wp) {
		if (Wp.WaypointName == "fast")
			MoveSpeed = 8;
		else if (Wp.WaypointName == "slow")
			MoveSpeed = 2;
	}

	void FixedUpdate() {
		if (Application.isEditor && !Application.isPlaying)
			return;

		if (DistanceToWaypoint() < 1) {
			if (NextWaypoint != null)
				OnWaypointReached(NextWaypoint);

			FetchNextWaypoint();
		}

		Vector2 MoveVelocity = (GetNextWaypointPos() - (Vector2)transform.position).normalized * MoveSpeed;

		if (IsFlying)
			body2d.velocity = MoveVelocity;
		else
			body2d.velocity = new Vector2(MoveVelocity.x, body2d.velocity.y);
	}

	void Update() {
		if (Application.isEditor && !Application.isPlaying) {
			UpdateInEditor();
			return;
		}

		Vector2 MoveDir = body2d.velocity.normalized;
		float DirAngle = Utils.Angle(Vector2.zero, MoveDir);
		float CurDirAngle = transform.rotation.eulerAngles.z;

		if (FlipTowardsMoveDir) {
			if (MoveDir.x > 0) {
				rnd.flipX = false;
			} else if (MoveDir.x < 0) {
				rnd.flipX = true;

				if (RotTowardsMoveDir)
					DirAngle += 180;
			}
		}

		if (RotTowardsMoveDir) {
			if (SmoothRotation) {
				transform.rotation = Quaternion.Euler(0, 0, Mathf.MoveTowardsAngle(CurDirAngle, DirAngle, SmoothRate * Time.deltaTime));
			} else
				transform.rotation = Quaternion.Euler(0, 0, DirAngle);
		}
	}

	void UpdateInEditor() {
		if (LinkToClosest) {
			LinkToClosest = false;
			FirstWaypoint = Utils.GetClosestWaypoint(transform.position);
			EditorUtility.SetDirty(this);
		}
	}

	Vector2 GetNextWaypointPos() {
		if (NextWaypoint == null)
			return transform.position;

		return NextWaypoint.transform.position;
	}

	float DistanceToWaypoint() {
		if (NextWaypoint == null)
			return 0;

		return Vector2.Distance(transform.position, NextWaypoint.transform.position);
	}

	void FetchNextWaypoint() {
		if (NextWaypoint != null)
			NextWaypoint = NextWaypoint.Next;

		if (NextWaypoint == null)
			NextWaypoint = FirstWaypoint;

		if (StartAtWaypoint && !HasSpawned && FirstWaypoint != null) {
			HasSpawned = true;
			transform.position = FirstWaypoint.transform.position;
		}
	}

	void OnDrawGizmos() {
		if (FirstWaypoint != null) {
			Gizmos.color = Selection.Contains(gameObject) ? Color.green : Color.white;
			Utils.DrawArrow(transform.position, FirstWaypoint.transform.position);
		}
	}
}
