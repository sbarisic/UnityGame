using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;


public class PlayerController : Character {

	public Animator anim;

	public CinemachineVirtualCamera vcam;
	public GameObject currCheckpoint;

	public float movementSpeed;
	public float jumpForce;

	public Transform feetPos;
	public float checkRadius;
	public float jumpTime;
	public LayerMask whatIsGround;

	float horizontalMoveInput;
	float jumpTimeCounter;
	bool isGrounded;
	bool isJumping;

	public GameObject bulletPrefab;

	Vector2 lookDir;
	float lastHitAngle = 90;

	public override void OnStart() {
		lookDir = new Vector2(1, 0);
		health = 20;
	}

	void FixedUpdate() {
		horizontalMoveInput = Input.GetAxisRaw("Horizontal");

		//Debug.Log(string.Format("MoveInput = {0}, Angle = {1}", horizontalMoveInput, lastHitAngle));

		if (lastHitAngle > 135 && horizontalMoveInput > 0)
			horizontalMoveInput = 0;
		else if (lastHitAngle < 45 && horizontalMoveInput < 0)
			horizontalMoveInput = 0;

		/*if (lastHitAngle < 135 && lastHitAngle > 45) {

		}*/

		body2d.velocity = new Vector2(horizontalMoveInput * movementSpeed, body2d.velocity.y);
	}

	void Update() {
		if (Application.isEditor && !Application.isPlaying)
			return;

		anim.SetFloat("Speed", Mathf.Abs(horizontalMoveInput));
		anim.SetBool("Shooting", false);

		isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
		// body2d.sharedMaterial.friction = isGrounded ? 0.4f : 0.0f;


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

		if (Input.GetButtonDown("Fire1")) {

			/*if (Input.GetMouseButtonDown(0)) {
				Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				FireGun((MousePos - (Vector2)transform.position).normalized);
			} else*/

			FireGun(lookDir);
		}
	}

	public override void OnDie() {
		gameObject.SetActive(false);
		vcam.enabled = false;

		// TODO: Coroutines
		Respawn();
		//StartCoroutine(DelayAndRespawn());
	}

	IEnumerator DelayAndRespawn() {
		yield return new WaitForSeconds(2);
		Respawn();
	}

	public override void Respawn() {
		health = 20;
		gameObject.SetActive(true);
		Vector2 spawnPoint = currCheckpoint.transform.position;
		transform.position = new Vector3(spawnPoint.x, spawnPoint.y, transform.position.z);
		vcam.enabled = true;

		// TODO: Fix color
		rnd.color = Color.white;
	}

	ContactPoint2D[] Contacts = new ContactPoint2D[16];

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.tag == "Enemy") {

			//TODO: handle damage amount
			OnReceiveDamage(1);
			Debug.Log("Collision with: " + collision.gameObject.tag);
		}

		int NumContacts = collision.GetContacts(Contacts);

		for (int i = 0; i < NumContacts; i++) {
			ContactPoint2D CP = Contacts[i];
			float NormalAngle = Utils.Angle(Vector2.zero, CP.normal);

			lastHitAngle = NormalAngle;
			/*if (NormalAngle < 135 && NormalAngle > 45) {
				//Debug.Log("Hit floor");
			} else {
				// Debug.Log("AYYYYYYYYYYY LMAO");
			}*/
		}
	}


	private void OnTriggerEnter2D(Collider2D collision) {
		if (collision.gameObject.tag == "Portal") {
			Debug.Log("Next level");
		}
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

		Bullet.transform.position = transform.position;
		Bullet.GetComponent<BulletController>().OnBulletCreated(Dir, Speed, Damage, Time.time, Tags.BulletPlayer);
	}
}
