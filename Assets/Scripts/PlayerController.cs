using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System.Threading;

public class PlayerController : Character {

	public Animator anim;

	public GameObject deathParticle;

	public CinemachineVirtualCamera vcam;
	public GameObject currCheckpoint;

	public float movementSpeed;
	public float jumpForce;

	public Transform feetPos;
	public Transform gunPos;

	float checkRadius = 0.499f;
	float fireRatePause = 0.5f;
	public float jumpTime;
	public LayerMask whatIsGround;

	float horizontalMoveInput;
	float jumpTimeCounter;
	bool isGrounded;
	bool isJumping;

	public GameObject bulletPrefab;

	Vector2 lookDir;
	float lastHitAngle = 90;
	float nextFireTime;
	bool touchedEnemy;

	GUI GameGUI;

	public override void OnStart() {
		GameGUI = GameObject.Find("Canvas")?.GetComponent<GUI>();

		Vector3 Pos = transform.position;
		Respawn();
		transform.position = Pos;
	}

	void FixedUpdate() {
		if (!touchedEnemy) {
			if (lastHitAngle > 135 && horizontalMoveInput > 0)
				horizontalMoveInput = 0;
			else if (lastHitAngle < 45 && horizontalMoveInput < 0)
				horizontalMoveInput = 0;
		}

		body2d.velocity = new Vector2(horizontalMoveInput * movementSpeed, body2d.velocity.y);
	}

	public override void OnUpdate() {
		if (GameGUI?.IsPaused() ?? false)
			return;

		horizontalMoveInput = Input.GetAxisRaw("Horizontal");
		anim.SetFloat("Speed", Mathf.Abs(horizontalMoveInput));
		anim.SetBool("Shooting", false);
		isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

		if (horizontalMoveInput > 0) {
			rnd.flipX = true;
			lookDir = new Vector2(1, 0);
		} else if (horizontalMoveInput < 0) {
			rnd.flipX = false;
			lookDir = new Vector2(-1, 0);
		}


		if (isGrounded == true && Input.GetButtonDown("Jump")) {
			isJumping = true;
			anim.SetBool("isJumping", isJumping);
			jumpTimeCounter = jumpTime;
			body2d.velocity = Vector2.up * jumpForce;
		}

		if (Input.GetButton("Jump") && isJumping == true) {
			if (jumpTimeCounter > 0) {
				body2d.velocity = Vector2.up * jumpForce;
				jumpTimeCounter -= Time.deltaTime;
			} else {
				isJumping = false;
				anim.SetBool("isJumping", isJumping);
			}
		}

		if (Input.GetButtonUp("Jump")) {
			isJumping = false;
			anim.SetBool("isJumping", isJumping);
		}

		if (Input.GetButton("Fire1")) {
			if (nextFireTime <= Time.time) {
				nextFireTime = Time.time + fireRatePause;
				FireGun(lookDir);
			}
		}

		if (Input.GetKeyDown(KeyCode.Escape))
			GameGUI?.ShowMainMenu(true);
	}

	public override void OnDie() {
		SpawnParticles(deathParticle);
		Gib.SpawnRandomGibs(transform.position, 5);

		gameObject.SetActive(false);
		vcam.enabled = false;

		// TODO: Coroutines
		//Respawn();
		//StartCoroutine(DelayAndRespawn());

		CoroutineMgr.Start(DelayAndRespawn());
	}

	IEnumerator DelayAndRespawn() {
		yield return new WaitForSeconds(2);
		Respawn();
	}

	public override void Respawn() {
		gameObject.SetActive(true);

		health = 20;
		lookDir = new Vector2(1, 0);

		Vector2 spawnPoint = currCheckpoint.transform.position;
		transform.position = new Vector3(spawnPoint.x, spawnPoint.y, transform.position.z);
		vcam.enabled = true;

		// TODO: Fix color
		rnd.color = Color.white;
		SpawnParticles(deathParticle);
	}

	ContactPoint2D[] Contacts = new ContactPoint2D[16];

	void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag == "Enemy") {
			touchedEnemy = true;

			OnReceiveDamage(collision.gameObject.GetComponent<EnemyController>()?.GetPlayerDamage() ?? 1);
			Debug.Log("Collision with: " + collision.gameObject.tag);
		}

		int NumContacts = collision.GetContacts(Contacts);

		for (int i = 0; i < NumContacts; i++) {
			ContactPoint2D CP = Contacts[i];
			lastHitAngle = Utils.Angle(Vector2.zero, CP.normal);
		}
	}

	void OnCollisionExit2D(Collision2D collision) {
		lastHitAngle = 90;
	}

	private void OnTriggerEnter2D(Collider2D collision) {

	}

	/*void OnCollisionEnter2D(Collision2D Other) {
		int NumContacts = Other.GetContacts(Contacts);

		for (int i = 0; i < NumContacts; i++) {
			ContactPoint2D CP = Contacts[i];
			float NormalAngle = Utils.Angle(Vector2.zero, CP.normal);

			if (NormalAngle < 135 && NormalAngle > 45) {
				//Debug.Log("Hit floor");
			}



			Debug.DrawLine(CP.point, CP.point + CP.normal * 1, Color.white, 5);
		}

		//Debug.Log("Collision enter " + NumContacts);
	}*/


	// TODO: Move bullet speed to a variable
	void FireGun(Vector2 Dir, float Speed = 16, int Damage = 10) {
		anim.SetBool("Shooting", true);
		GameObject Bullet = ObjectPool.Alloc(bulletPrefab);

		Vector2 localGunPos = gunPos.localPosition;
		if (!rnd.flipX)
			localGunPos *= new Vector2(-1, 1);

		Bullet.transform.position = transform.position + (Vector3)localGunPos;
		Bullet.GetComponent<BulletController>().OnBulletCreated(Dir, Speed, Damage, Tags.BulletPlayer);
	}
}
