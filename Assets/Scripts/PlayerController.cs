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
		lookDir = new Vector2(1, 0);
	}

	void FixedUpdate() {
		horizontalMoveInput = Input.GetAxisRaw("Horizontal");
		body2d.velocity = new Vector2(horizontalMoveInput * movementSpeed, body2d.velocity.y);
	}

	void Update() {
		isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
		// body2d.sharedMaterial.friction = isGrounded ? 0.4f : 0.0f;

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

		if (Input.GetButtonDown("Fire1")) {
			if (Input.GetMouseButtonDown(0)) {
				Vector2 MousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
				FireGun((MousePos - (Vector2)transform.position).normalized);
			} else
				FireGun(lookDir);
		}
	}

	ContactPoint2D[] Contacts = new ContactPoint2D[16];

	void OnCollisionEnter2D(Collision2D Other) {
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
	}

	// TODO: Move bullet speed to a variable
	void FireGun(Vector2 Dir, float Speed = 16, float Damage = 10) {
		GameObject Bullet = ObjectPool.Alloc(bulletPrefab);

		Bullet.transform.position = transform.position;
		Bullet.GetComponent<BulletController>().OnBulletCreated(Dir, Speed, Damage, Time.time, Tags.BulletPlayer);
	}
}
