using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : UnitController {

	//stats
	public int numThorns = 8;
	//variables
	float  xVel = 0; // input of X
	float yVel =0; // input of Y
	float  yChange = 0;// yVel+ current velocity
	bool doubleJump = false;
	// references
	public GameObject thornPrefab;
	public GameObject weapon;



	//update function
	void FixedUpdate(){
		//WASD keys
		//A+D
		xVel = Input.GetAxisRaw ("Horizontal");

		//W
		if (Input.GetKey (KeyCode.Space) && grounded) {
			if (rb2d.velocity.y <= 0) {
				yVel = jumpVel;
				doubleJump = true;
			}
		}else
		if (Input.GetKeyDown (KeyCode.Space) && !grounded && doubleJump) {
			rb2d.gravityScale = 1f;
			//rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
			//yVel = jumpVel;
			if (rb2d.velocity.y < 0) {
				rb2d.velocity = new Vector2 (rb2d.velocity.x, jumpVel);
			} else {
				rb2d.velocity = new Vector2 (rb2d.velocity.x, rb2d.velocity.y+(jumpVel*0.6f));
			}
			doubleJump = false;
		}
		//S
		if(Input.GetKey(KeyCode.S) && !grounded){
			yVel = -1;

		}
		//

		//Attack buttons
			//melee attacks
		if (Input.GetMouseButtonDown(0)) {
			//StartCoroutine ("CallAttack","SliceDown" );
			anim.SetTrigger("SliceDown");
		}

		if (Input.GetMouseButtonDown(1)) {
			anim.SetTrigger("UpperCutPullBack");
		}

		if (Input.GetMouseButtonUp (1)) {
			anim.SetTrigger ("UpperCut");
		}
			//specials attacks
		if (Input.GetKeyDown (KeyCode.E)) {
			Thorns ();
		}




		//calculating movement and executing
		yChange = yVel + rb2d.velocity.y;
		yVel = 0;
		MoveUnit( new Vector2 (xVel*walkSpeed,yChange));
		//


	}

	IEnumerator CallAttack(string attack){
		if (Camera.main.ScreenToWorldPoint (Input.mousePosition).x > transform.position.x) {
			rotateSprite (false);
			anim.SetTrigger (attack);
			yield return new WaitForSecondsRealtime (0.5f);
			rotateSprite (true);
		} else {
			rotateSprite (true);
			anim.SetTrigger (attack);
			yield return new WaitForSecondsRealtime (0.5f);
			rotateSprite (false);
		}
	}


	//Thorns
	void Thorns()
	{
		int increment = -270;
		Transform lobsterPos =this.transform;
		GameObject[] thornsArr;
			thornsArr = new GameObject[numThorns];
			for (int i = 0; i < numThorns; i++)
			{
				GameObject thornObject = Instantiate(thornPrefab, transform.position, Quaternion.Euler(0,0,0));
				thornsArr[i] = thornObject;                
				increment = increment - (360/numThorns);
				thornObject.GetComponent<Thorn>().init(increment);
		}
	}

	override public void HitGround(){
		base.HitGround ();
		doubleJump = true;
	}

}
