using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
	Rigidbody2D body2d;

	public float movementSpeed;
	public float jumpForce;
	private float horizontalMoveInput;

	private bool isGrounded;
	public Transform feetPos;
	public float checkRadius;
	public LayerMask whatIsGround;

	private float jumpTimeCounter;
	public float jumpTime;
	private bool isJumping;

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
		} else if (horizontalMoveInput < 0) {
			transform.eulerAngles = new Vector3(0, 0, 0);
		}

		if (isGrounded == true && Input.GetKeyDown(KeyCode.Space)) {
			isJumping = true;
			jumpTimeCounter = jumpTime;
			body2d.velocity = Vector2.up * jumpForce;
		}

		if (Input.GetKey(KeyCode.Space) && isJumping == true) {
			if (jumpTimeCounter > 0) {
				body2d.velocity = Vector2.up * jumpForce;
				jumpTimeCounter -= Time.deltaTime;
			} else {
				isJumping = false;
			}
		}

		if (Input.GetKeyUp(KeyCode.Space)) {
			isJumping = false;
		}
	}
}
