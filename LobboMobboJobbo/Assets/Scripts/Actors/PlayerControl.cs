using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : UnitController {

	//stats
	public int numThorns = 8;
	public int crabMeat = 0;
	//variables
	float  xVel = 0; // input of X
	float yVel =0; // input of Y
	float  yChange = 0;// yVel+ current velocity
	bool doubleJump = false;
	bool inAnimation = false;
	// references
	public GameObject thornPrefab;
	public GameObject weapon;
	PlayerAttack pAttack;
	override public void Awake(){
		base.Awake ();
		stunTime = 0.3f;
		pAttack = GetComponentInChildren<PlayerAttack> ();
	}

	//update function, this checks the inputs of the player
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
				rb2d.velocity = new Vector2 (rb2d.velocity.x, rb2d.velocity.y+(jumpVel));
			}
			doubleJump = false;
		}
		//S
		if(Input.GetKey(KeyCode.S) && !grounded){
			yVel = -1;

		}
		//


		//calculating movement and executing
		yChange = yVel + rb2d.velocity.y;
		yVel = 0;
		MoveUnit( new Vector2 (xVel*walkSpeed,yChange));
		//


	}
	//this is for the attack animations
	void Update(){
		//Attack buttons
		//melee attacks
		if (Input.GetMouseButtonDown(0)&& !inAnimation) {
			//StartCoroutine ("CallAttack","SliceDown" );
			anim.SetTrigger("SliceDown");
		}

		if (Input.GetMouseButtonDown(1)) {
			anim.SetTrigger("UpperCutPullBack");
			inAnimation = true;
		}

		if (Input.GetMouseButtonUp (1)) {
			anim.SetTrigger ("UpperCut");
			inAnimation = false;
		}
		//specials attacks
		if (Input.GetKeyDown (KeyCode.E)) {
			Thorns ();
		}
	}

	//this would have been nice to implement
/*	IEnumerator CallAttack(string attack){
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
	}//*/


	//Thorns doesnt work anymore
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
