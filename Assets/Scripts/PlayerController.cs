using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	Rigidbody2D body2d;

	public float movementSpeed;
	public float jumpForce;

	public Transform feetPos;
	public float checkRadius;
	public float jumpTime;
	public LayerMask whatIsGround;
	public GameObject bulletPrefab;

	Vector2 lookDir;
	float horizontalMoveInput;
	float jumpTimeCounter;
	bool isGrounded;
	bool isJumping;

	void Start() {
		body2d = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate() {
		horizontalMoveInput = Input.GetAxisRaw("Horizontal");
		body2d.velocity = new Vector2(horizontalMoveInput * movementSpeed, body2d.velocity.y);
	}

	void Update() {
		isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);

		if (horizontalMoveInput > 0) {
			transform.eulerAngles = new Vector3(0, 180, 0);
			lookDir = new Vector2(1, 0);
		} else if (horizontalMoveInput < 0) {
			transform.eulerAngles = new Vector3(0, 0, 0);
			lookDir = new Vector2(-1, 0);
		}

		if (isGrounded == true && Input.GetButtonDown("Jump")) {
			isJumping = true;
			jumpTimeCounter = jumpTime;
			body2d.velocity = Vector2.up * jumpForce;
		}

		if (Input.GetButton("Jump") && isJumping == true) {
			if (jumpTimeCounter > 0) {
				body2d.velocity = Vector2.up * jumpForce;
				jumpTimeCounter -= Time.deltaTime;
			} else {
				isJumping = false;
			}
		}

		if (Input.GetButtonUp("Jump")) {
			isJumping = false;
		}

		if (Input.GetButtonDown("Fire1"))
			FireGun(lookDir);
	}

	// TODO: Move bullet speed to a variable
	void FireGun(Vector2 Dir, float Speed = 16, float Damage = 10) {
		GameObject Bullet = ObjectPool.Alloc(bulletPrefab);

		Bullet.transform.position = transform.position;
		Bullet.GetComponent<BulletController>().OnBulletCreated(Dir, Speed, Damage, Time.time, Tags.BulletPlayer);
	}
}
