using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	//Transform transform;
	Rigidbody2D rigidbody2D;
	public float movementSpeed;
	public float jumpForce;
	private float moveInput;

	private bool isGrounded;
	public Transform feetPos;
	public float checkRadius;
	public LayerMask whatIsGround;

	private float jumpTimeCounter;
	public float jumpTime;
	private bool isJumping;

	void Start() {
		rigidbody2D = GetComponent<Rigidbody2D>();
		//transform = GetComponent<Transform>();
	}

	void FixedUpdate() {
		moveInput = Input.GetAxisRaw("Horizontal");
		rigidbody2D.velocity = new Vector2(moveInput * movementSpeed, rigidbody2D.velocity.y);
	}

	void Update() {
		isGrounded = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
		//Collider2D isGroundCollider = Physics2D.OverlapCircle(feetPos.position, checkRadius, whatIsGround);
		//isGrounded = isGroundCollider;

		if (moveInput > 0) {
			transform.eulerAngles = new Vector3(0, 180, 0);
		} else if (moveInput < 0) {
			transform.eulerAngles = new Vector3(0, 0, 0);
		}
		if (isGrounded == true && Input.GetKeyDown(KeyCode.Space)) {
			isJumping = true;
			//isGrounded = false;
			jumpTimeCounter = jumpTime;
			rigidbody2D.velocity = Vector2.up * jumpForce;
		}

		if (Input.GetKey(KeyCode.Space) && isJumping == true) {
			if (jumpTimeCounter > 0) {
				rigidbody2D.velocity = Vector2.up * jumpForce;
				jumpTimeCounter -= Time.deltaTime;
			} else {
				isJumping = false;
			}
		}

		if (Input.GetKeyUp(KeyCode.Space)) {
			isJumping = false;
		}
	}

	/*float movementSpeed = 3f;
	float jumpSpeed = 500f;
	bool isGrounded = true;

	Rigidbody2D rigidbody2D;
	Transform transform;

	// Start is called before the first frame update
	void Start() {
		rigidbody2D = GetComponent<Rigidbody2D>();
		transform = GetComponent<Transform>();
	}

	void SetRotationY(float Y) {
		Quaternion QuatRot = transform.rotation;
		Vector3 EulerRotation = QuatRot.eulerAngles;

		EulerRotation.y = Y;

		QuatRot.eulerAngles = EulerRotation;
		transform.rotation = QuatRot;

	}

	// Update is called once per frame
	void Update() {
		Vector2 pos2d = new Vector2(transform.position.x, transform.position.y);

		//if (isGrounded) {
		rigidbody2D.gravityScale = 2.1f;
		//}

		if (Input.GetKey(KeyCode.LeftArrow)) {
			transform.position += Vector3.left * movementSpeed * Time.deltaTime;
			SetRotationY(0);
			// transform.rotation.y = 180; //This is for rotating sprite images
		}

		if (Input.GetKey(KeyCode.RightArrow)) {
			transform.position += Vector3.right * movementSpeed * Time.deltaTime;

			SetRotationY(180);
			//transform.rotation.y = 0; //This is for rotating sprite images
		}

		if (Input.GetKey(KeyCode.DownArrow)) {
			if (!isGrounded) {
				rigidbody2D.gravityScale += 2;
			}
		}



		if (Input.GetKey(KeyCode.Space)) {
			if (isGrounded) {
				rigidbody2D.AddForce(Vector3.up * jumpSpeed);
				isGrounded = false;
			} else {
				Vector3 v = rigidbody2D.velocity;

				if (v.y > 0) {
					v.y *= 1f;
					rigidbody2D.velocity = v;
				}
			}
		}



		RaycastHit2D Hit2d = Physics2D.Raycast(pos2d, new Vector2(0, -1), 1);
		if (Hit2d.collider != null) {
			isGrounded = true;
		}

		if (Hit2d.rigidbody != null && Hit2d.rigidbody.CompareTag("Ground"))
			isGrounded = true;
	}

	private void OnCollisionEnter2D(Collision2D collision) {
		if (collision.gameObject.CompareTag("Ground"))
			isGrounded = true;
	}*/
}
