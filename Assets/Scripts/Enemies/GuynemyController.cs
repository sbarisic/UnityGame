using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuynemyController : EnemyController {
	GameObject player;

	Animator anim;

	bool playerInRange;
	bool playerInHitRng;

	GameObject testPoint;

	public float followRadius = 8.55f;
	float hitRange = 1.5f;
	bool isHitting;
	bool isCooldown;
	Vector2 lastMoveDir;

	public override void OnStart() {
		player = GameObject.FindGameObjectWithTag(Tags.Player);
		anim = GetComponent<Animator>();
		IsFlying = false;

		base.OnStart();

		testPoint = transform.Find("TestPoint").gameObject;
		anim.SetBool("canHit", false);
	}

	public override void OnUpdate() {
		base.OnUpdate();

		float distance = float.PositiveInfinity;

		if (player?.activeInHierarchy ?? false)
			distance = Vector2.Distance(transform.position, player.transform.position);

		playerInRange = distance < followRadius;
		playerInHitRng = distance < hitRange;

		anim.SetFloat("Speed", Mathf.Abs(body2d.velocity.x));

		if (playerInHitRng && !isHitting) {
			anim.SetBool("canHit", true);
			isHitting = true;
			CoroutineMgr.Start(WaitAndStopHitting());
		}
	}

	IEnumerator WaitAndStopHitting() {
		yield return new WaitForSeconds(0.2f);

		if (Vector2.Distance(transform.position, player.transform.position) < hitRange)
			player?.GetComponent<PlayerController>()?.OnReceiveDamage(10);

		anim.SetBool("canHit", false);
		yield return new WaitForSeconds(1);
		isHitting = false;
	}

	IEnumerator StartCooldown(float Time) {
		isCooldown = transform;
		yield return new WaitForSeconds(Time);
		isCooldown = false;
	}

	public override void DoFlipTowardsMoveDir(Vector2 MoveDir) {
		base.DoFlipTowardsMoveDir(MoveDir * new Vector2(-1, 1));
	}

	public override Vector2 GetNextWaypointPos() {
		if (isHitting || isCooldown)
			return transform.position;

		if (playerInRange) {
			Vector2 moveDir = (Vector2)player.transform.position - (Vector2)transform.position;
			moveDir.y = 0;
			moveDir = moveDir.normalized;

			// If enemy isn't moving in the same direction, wait
			if (!Utils.SameSign(moveDir.x, lastMoveDir.x))
				CoroutineMgr.Start(StartCooldown(1.0f));

			lastMoveDir = moveDir;
			Vector2 testLoc = (Vector2)testPoint.transform.position + moveDir;

			Collider2D Col = Physics2D.OverlapPoint(testLoc);
			if (Col == null || Col.isTrigger)
				return transform.position;

			return player.transform.position;
		}

		return base.GetNextWaypointPos();
	}

	private void OnDrawGizmos() {
		Gizmos.color = Color.white;
		Gizmos.DrawWireSphere(transform.position, followRadius);
	}

}
