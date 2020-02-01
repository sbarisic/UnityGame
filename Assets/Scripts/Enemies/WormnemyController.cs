using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WormnemyController : EnemyController {
	public override void OnStart() {
		IsFlying = false;
		base.OnStart();
		health = 30;
	}
	public virtual void DoFlipTowardsMoveDir(Vector2 MoveDir) {
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

	public override void OnDie() {
		
		//TODO: Spawn two more on one killed and ignore collision between enemies

		Instantiate(gameObject);

		



	}


	public override int GetPlayerDamage() {
		return 10;
	}
}
