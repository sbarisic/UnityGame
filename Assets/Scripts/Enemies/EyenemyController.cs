using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EyenemyController : EnemyController {

	public override void OnStart() {
		base.OnStart();
		health = 20;
	}

	public override void OnReceiveDamage(int Amt) {
		base.OnReceiveDamage(Amt);
	}

	public override int GetPlayerDamage() {
		return 7;
	}
}
