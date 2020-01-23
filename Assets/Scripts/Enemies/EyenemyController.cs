using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class EyenemyController : EnemyController {
	public override void OnStart() {
		base.OnStart();
		Health = 20;
	}
}
