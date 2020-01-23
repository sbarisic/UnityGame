using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;


public class EnemyController : Character {
	
	public Waypoint FirstWaypoint;
	public bool LinkToClosest;

	// Teleport to first waypoint
	[Space(10)]
	public bool StartAtWaypoint = true;
	public float ReachDistance = 1.0f;
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

	bool IsReversing;
	Stack<Waypoint> VisitedWaypoints = new Stack<Waypoint>();

	

	public override void OnStart() {

		if (IsFlying)
			body2d.gravityScale = 0;
	}

	/// <summary>
	/// Gets called when enemy reaches a waypoint
	/// </summary>
	/// <param name="Wp"></param>
	protected virtual void OnWaypointReached(Waypoint Wp) {
		if (Wp.Reverse)
			IsReversing = true;

		if (!IsReversing)
			VisitedWaypoints.Push(Wp);
	}

	void FixedUpdate() {
		if (Application.isEditor && !Application.isPlaying)
			return;

		if (DistanceToWaypoint() < ReachDistance) {
			if (NextWaypoint != null)
				OnWaypointReached(NextWaypoint);

			FetchNextWaypoint();
		}

		Vector2 MoveVelocity = (GetNextWaypointPos() - (Vector2)transform.position).normalized * MoveSpeed;

		if (IsFlying)
			body2d.velocity = MoveVelocity;
		else
			body2d.velocity = new Vector2(MoveVelocity.x, body2d.velocity.y);

		OnFixedUpdate();
	}

	public virtual void OnFixedUpdate() {
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

		OnUpdate();
	}

	public virtual void OnUpdate() {
	}

	void UpdateInEditor() {
		if (LinkToClosest) {
			LinkToClosest = false;
			FirstWaypoint = Utils.GetClosestWaypoint(transform.position);
			EditorUtils.SetDirty(this);
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
		if (IsReversing) {
			if (VisitedWaypoints.Count > 0) {
				NextWaypoint = VisitedWaypoints.Pop();
				return;
			}

			VisitedWaypoints.Push(FirstWaypoint);
			IsReversing = false;
		}

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
			Gizmos.color = EditorUtils.SelectionContains(gameObject) ? Color.green : Color.white;
			Utils.DrawArrow(transform.position, FirstWaypoint.transform.position);

			Gizmos.color = Color.yellow;
			Gizmos.DrawWireSphere(transform.position, ReachDistance);
		}
	}
}
