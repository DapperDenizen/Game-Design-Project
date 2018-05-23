using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Unit {

	//stats and things

	public Camera camera;
	float  xVel = 0; // input of X
	float yVel =0; // input of Y
	float  yChange = 0;// yVel+ current velocity

	// Use this for initialization
	void FixedUpdate(){
		//WASD keys
		//A+D
		xVel = Input.GetAxisRaw ("Horizontal");
		if(Input.GetMouseButtonDown(0)){
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

		MoveUnit( new Vector2 (xVel*walkSpeed,yChange));


	}

}