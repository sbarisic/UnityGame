using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormnemyController : EnemyController {

	public override void OnStart() {
		IsFlying = false;
		base.OnStart();
		health = 30;
	}
	public override void DoFlipTowardsMoveDir(Vector2 MoveDir) {
		if (MoveDir.x > 0) {
			rnd.flipX = true;
		} else if (MoveDir.x < 0) {
			rnd.flipX = false;
		}
	}

	public override void OnUpdate() {
		Vector2 MoveDir = body2d.velocity.normalized;
		float DirAngle = Utils.Angle(Vector2.zero, MoveDir);

		if (FlipTowardsMoveDir) {
			DoFlipTowardsMoveDir(MoveDir);
		}

	}

	public override void OnReceiveDamage(int Amt) {
		base.OnReceiveDamage(Amt);
	}

	public override void OnDie() {
		base.OnDie();
	}


	void SpawnEnemy(float t) {
		for (int i = 0; i < 2; i++) {

			Instantiate(gameObject);
			Destroy(gameObject, t);
		}
	}

	public override int GetPlayerDamage() {
		return 10;
	}
}
