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
	float playerTimeDeadline;
	bool touchedEnemy;

	GUI GameGUI;

	public override void OnStart() {
		GameGUI = GameObject.Find("Canvas")?.GetComponent<GUI>();

		Vector3 Pos = transform.position;
		Respawn();
		transform.position = Pos;

		InvokeRepeating(nameof(PlayWalkSound), 0, 0.3f);
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

	void PlayWalkSound() {
		if (horizontalMoveInput != 0 && isGrounded)
			AudioManager.PlaySfx(AudioEffects.PlayerWalk);
	}

	IEnumerator PlayLandSfxIfAlive() {
		yield return new WaitForSeconds(0.025f);

		if (health > 0)
			AudioManager.PlaySfx(AudioEffects.PlayerLand);
	}

	void ResetPlayerTimer(float Amount = 40) {
		playerTimeDeadline = Time.time + Amount;
	}

	int GetPlayerTimeLeft() {
		return (int)(playerTimeDeadline - Time.time);
	}

	public void OnPortalEnter() {
		ResetPlayerTimer();
		GameGUI?.AddScore(GetPlayerTimeLeft());
	}

	public override void OnUpdate() {
		if (GameGUI?.IsPaused() ?? false)
			return;

		horizontalMoveInput = Input.GetAxisRaw("Horizontal");
		anim.SetFloat("Speed", Mathf.Abs(horizontalMoveInput));
		anim.SetBool("Shooting", false);

		bool isNowGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

		if (isNowGrounded && !isGrounded)
			CoroutineMgr.Start(PlayLandSfxIfAlive());

		isGrounded = isNowGrounded;

		if (horizontalMoveInput > 0) {
			rnd.flipX = true;
			lookDir = new Vector2(1, 0);
		} else if (horizontalMoveInput < 0) {
			rnd.flipX = false;
			lookDir = new Vector2(-1, 0);
		}


		if (isGrounded == true && Input.GetButtonDown("Jump")) {
			isJumping = true;
			AudioManager.PlaySfx(AudioEffects.PlayerJump);
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

		// TODO: Update the score and time here

		int TimeLeft = GetPlayerTimeLeft();

		GameGUI?.SetRickHealth(health);
		GameGUI?.SetTime(TimeLeft);

		if (TimeLeft <= 0 && health > 0)
			OnReceiveDamage(9999);
	}

	public override void OnReceiveDamage(int Amt) {
		base.OnReceiveDamage(Amt);

		if (health > 0)
			AudioManager.PlaySfx(AudioEffects.PlayerReceiveDamage);
	}

	public override void OnDie() {
		AudioManager.PlaySfx(AudioEffects.PlayerDie);

		SpawnParticles(deathParticle);
		Gib.SpawnRandomGibs(transform.position, 5);

		GameGUI?.SetRickHealth(0);
		GameGUI?.AddScore(-10);

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
		if (gameObject == null)
			return;

		gameObject.SetActive(true);
		ResetPlayerTimer();

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

	// TODO: Move bullet speed to a variable
	void FireGun(Vector2 Dir, float Speed = 16, int Damage = 10) {
		AudioManager.PlaySfx(AudioEffects.PlayerShoot);

		anim.SetBool("Shooting", true);
		GameObject Bullet = ObjectPool.Alloc(bulletPrefab);

		Vector2 localGunPos = gunPos.localPosition;
		if (!rnd.flipX)
			localGunPos *= new Vector2(-1, 1);

		Bullet.transform.position = transform.position + (Vector3)localGunPos;

		BulletController BulletCnt = Bullet.GetComponent<BulletController>();
		BulletCnt.OnBulletCreated(Dir, Speed, Damage, Tags.BulletPlayer);
		BulletCnt.OnHitEnemy = () => GameGUI?.AddScore(1);
	}
}
