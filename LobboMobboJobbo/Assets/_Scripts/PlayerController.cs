using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	public float jumpVel;
	public float walkSpeed = 5.5f;
	public GameObject weapon;
	public Camera camera;
	float  xVel = 0; // input of X
	float yVel =0; // input of Y
	float  yChange = 0;// yVel+ current velocity
	bool grounded = false;
	bool jumping = false;
	Vector2 moveTarget;
	private Rigidbody2D rb2d;

	// Use this for initialization
	void Start () {
		rb2d = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate(){
		//WASD keys
		//A+D
		xVel = Input.GetAxisRaw ("Horizontal");
		if(Input.GetMouseButtonDown(0)){
			//print (camera.ScreenToWorldPoint(Input.mousePosition));
			AttackPrimary (camera.ScreenToWorldPoint(Input.mousePosition));
		}


		if (Input.GetKey (KeyCode.W) && grounded) {
			if (rb2d.velocity.y <= 0) {
				yVel = jumpVel;
			}
		}
		//fast fall
		if(Input.GetKey(KeyCode.S) && !grounded){
			yVel = -1;

		}

		yChange = yVel + rb2d.velocity.y;
		yVel = 0;
		if (jumping && rb2d.velocity.y < 0) {
		//increase gravity
			rb2d.gravityScale = 2.5f;
		}

		rb2d.velocity = new Vector2 (xVel*walkSpeed,yChange);


	}

	void Update(){
		//find the mouse position
		//mousePosition = camera.ScreenToWorldPoint(Input.mousePosition);
	}

	void OnTriggerEnter2D(Collider2D other){
		jumping = false;
		grounded = true;
		rb2d.gravityScale = 1;
	}

	void OnTriggerExit2D(Collider2D other){
		grounded = false;
	}


	void AttackPrimary(Vector3 lookingAt){
		weapon.GetComponent<WeaponController> ().Attack (lookingAt);
	}

	void OnDrawGizmos(){
		//Gizmos.color = Color.cyan;
		//Gizmos.DrawLine (transform.position, mousePosition);
	
	}
}