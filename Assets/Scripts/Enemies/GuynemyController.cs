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
	float hitRange = 3f;
	bool isHitting;

	public override void OnStart() {
		player = GameObject.FindGameObjectWithTag(Tags.Player);
		anim = GetComponent<Animator>();

		IsFlying = false;
		base.OnStart();
		testPoint = transform.Find("TestPoint").gameObject;
	}


	public override void OnUpdate() {
		base.OnUpdate();
		float distance = Vector2.Distance(transform.position, player.transform.position);

		playerInRange = distance < followRadius;
		playerInHitRng = distance < hitRange;

		anim.SetBool("canHit", playerInHitRng);
		anim.SetFloat("Speed", Mathf.Abs(body2d.velocity.x));

		if (playerInHitRng) {
			isHitting = true;
			CoroutineMgr.Start(WaitAndStopHitting());
		}
	}

	IEnumerator WaitAndStopHitting() {
		yield return new WaitForSeconds(0.3f);
		// TODO: Check if player in range again and apply damage if true

		yield return new WaitForSeconds(0.1f);
		isHitting = false;
	}

	public override void DoFlipTowardsMoveDir(Vector2 MoveDir) {
		base.DoFlipTowardsMoveDir(MoveDir * new Vector2(-1, 1));
	}

	public override Vector2 GetNextWaypointPos() {
		if (isHitting)
			return transform.position;

		if (playerInRange) {
			Vector2 MoveDir = (Vector2)player.transform.position - (Vector2)transform.position;
			MoveDir.y = 0;
			MoveDir = MoveDir.normalized;

			Vector2 testLoc = (Vector2)testPoint.transform.position + MoveDir;

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
