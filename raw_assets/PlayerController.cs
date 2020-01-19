using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Game.Scripts;

class PlayerController : PawnController {
	bool Grounded = false;

	public override void OnStart() {
	}

	public override void OnUpdate() {

	}

	void UpdateCamPos() {
		/*Vector2 Position = new Vector2(transform.position.x, transform.position.y);
		Camera Cur = Camera.current;
		if (Cur == null)
			return;
		Cur.transform.position = new Vector3(Position.x, Position.y, Cur.transform.position.z);*/
	}

	public override void OnFixedUpdate() {
		float Dt = Time.fixedDeltaTime;
		float GravityY = Physics2D.gravity.y;

		const float MaxHorizontalSpeed = 6;
		const float MaxVerticalSpeed = 16;
		const float JumpHeight = 3;

		const float AccelScale = 20;
		const float AirAccelScale = 15;
		const float AirDeccelScale = 5;

		Vector2 Velocity = Body.velocity;

		// Jump movement while on ground
		if (Grounded) {
			Velocity.y = 0;

			if (Input.GetKey(KeyCode.Space))
				Velocity.y = Mathf.Sqrt(2 * JumpHeight * Mathf.Abs(GravityY));
		} else {
			// TODO: Wall jumping goes here
		}

		float MoveAmt = 0;

		// Left and right movement
		if (Input.GetKey(KeyCode.A))
			MoveAmt = -AccelScale;
		if (Input.GetKey(KeyCode.D))
			MoveAmt = AccelScale;


		if (MoveAmt != 0)
			Velocity.x = Grounded ? MoveAmt : Mathf.MoveTowards(Velocity.x, MaxHorizontalSpeed * MoveAmt, AirAccelScale * Time.deltaTime);
		else
			Velocity.x = Grounded ? 0 : Mathf.MoveTowards(Velocity.x, 0, AirDeccelScale * Time.deltaTime);


		Velocity.y += GravityY * Dt;
		Velocity.x = Mathf.Clamp(Velocity.x, -MaxHorizontalSpeed, MaxHorizontalSpeed);
		Velocity.y = Mathf.Clamp(Velocity.y, -MaxVerticalSpeed, MaxVerticalSpeed);
		Body.velocity = Velocity;

		Grounded = false;

		Collider2D[] Hits = Physics2D.OverlapBoxAll(transform.position - new Vector3(0, 1.0f / 16.0f, 0), Collider.size, 0);
		foreach (Collider2D Hit in Hits) {
			if (Hit == Collider)
				continue;

			if (Hit.isTrigger)
				continue;

			ColliderDistance2D colliderDistance = Hit.Distance(Collider);

			if (colliderDistance.isOverlapped)
				if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && Velocity.y < 0)
					Grounded = true;
		}

		UpdateCamPos();
	}
}

