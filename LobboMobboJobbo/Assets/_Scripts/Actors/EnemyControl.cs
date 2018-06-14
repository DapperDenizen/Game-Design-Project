﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : UnitController {

	Vector2 test = Vector2.zero;
	//DEBUG
	Vector2 gimzoDebug1;
	Vector2 gimzoDebug2;
	//
	//variables
	int xIntention = 0; //intended movement on the x axis,can be 1, 0 or -1
	float yIntention = 0; //intended movement on the y axis,can be 1, 0 or -1
	float yOffset =0;
	public float knock = 5f; // this is the knock back strength
	public float dam;
	//Pathfinding
	public Pathfinding.PathWay[] path; // this is the path we are following
	public Pathfinding.PathWay currentTarget;
	public Vector2 targetPlace;
	int currentIndex;
	public bool pathRequested = false;
	public bool pathInProgress = false;
	public int pathFailScore =0;
	public int maxPathAttempt =5;
	//dumb things
	public int strength; // used for spawning AI
	public bool stacking;
	public GameObject buddy; //this is the crab you're sitting on
	float stackOffset;
	//
	//references
	public GameObject player;
	public GameObject crabGore;
	public BoxCollider2D col;
	public CrabSpawner Spawner;
	LayerMask platformMask;
	//Death stuff
	public GameObject crabMeat;
	public GameObject[] bodyParts = new GameObject[5];
	private GameObject blood;
	//
	//override means i am using this Start() not the parent(unit)'s start, but calling base.start() means i call it as well
	override public void Awake(){
		base.Awake ();
		platformMask = LayerMask.GetMask("Ground");
		Spawner = GameObject.FindGameObjectWithTag ("GameController").GetComponent<CrabSpawner>();
		player = GameObject.FindGameObjectWithTag ("Player"); //this is the guy we hate
		yOffset = GetComponent<BoxCollider2D>().bounds.extents.y;
		if (player != null) {
			targetPlace = player.transform.position;
		}
		stunTime = 1f;
		//death stuff
		crabMeat = Resources.Load("Prefab/CrabMeat")as GameObject;
		blood = Resources.Load("Prefab/Blood")as GameObject;
		for(int x = 0; x<bodyParts.Length; x++){
			bodyParts[x] = Resources.Load("Prefab/CrabPart" + (x+1))as GameObject;
		}
		//
		if (stacking) {
			stackOffset = buddy.GetComponent<BoxCollider2D> ().bounds.extents.y+this.GetComponent<BoxCollider2D>().bounds.extents.y;
			this.GetComponent<BoxCollider2D> ().enabled = false;
			rb2d.gravityScale = 0;
		}

	}

	//these are the updates to do with getting a path, this could be more efficent
	void FixedUpdate(){
		if (player != null) {
			if (!pathRequested && !stacking) {
				if (Vector2.Distance (transform.position, player.transform.position) < 2.1f) {
					//attack animation plays
					CloseEnough();
				} else if (!pathInProgress) {
					StartPath (player.transform.position);
					pathRequested = true;
				} else if (Vector2.Distance (targetPlace, player.transform.position) > 4.5f) {

					StartPath (player.transform.position);
					pathRequested = true;
				}
				if (!grounded && !pathInProgress) {
					MoveUnit(new Vector2(0, rb2d.velocity.y+-1f));
				}
			} else if (stacking) {
				//check buddys not dead
				if (buddy != null) {
					transform.position = new Vector2 (buddy.transform.position.x, buddy.transform.position.y + stackOffset); 
					sprite.transform.localScale = new Vector3 (buddy.GetComponent<EnemyControl> ().sprite.transform.localScale.x*-1,1,1);

				} else {
					this.GetComponent<BoxCollider2D> ().enabled = true;
					stacking = false;
					rb2d.gravityScale = 1;
				}
			}
		}
	}

	//these updates are for the animations that arent handled in the unitcontroller script
	void Update(){
		if (player != null) {
			if (Vector2.Distance (transform.position, player.transform.position) < 2f) {
				anim.SetBool ("Attacking", true);
			} else {
				anim.SetBool ("Attacking", false);
			}
		}
	}


	//this is to reach the lobster in the case the player is JUST out of a waypoints reach
	virtual public void CloseEnough(){
		float xChange;
		xChange = transform.position.x < player.transform.position.x ? walkSpeed : -walkSpeed;
		MoveUnit(new Vector2(xChange, rb2d.velocity.y));
	} //Extend this for the boss!


	//this asks for a path from the path request manager
	void StartPath(Vector2 targetGoal){
		if (!pathRequested) {
			targetPlace = targetGoal;
			pathRequested = true;
			PathRequestManager.RequestPath (transform.position, targetGoal, OnPathFound);
			//debug_PATHFINDING_CALL++;
		}
	}

	//this is what the path request manager tells the path too
	virtual public void OnPathFound(Pathfinding.PathWay[] newPath, bool pathSuccess){
		pathRequested = false;
		if (pathSuccess && this != null) {
			pathFailScore = 0;
			if (newPath.Length > 0) {

				if (newPath [newPath.Length - 1].worldPosition != targetPlace) {
					path = newPath;
					StopCoroutine ("FollowPath");
					//print ("stopping because " + temp);
					pathInProgress = false;
					StartCoroutine ("FollowPath");
				}
			}
		} else if(!pathSuccess){
			
			if (pathFailScore > maxPathAttempt && this != null) {
				float panicMoveX =	transform.position.x < player.transform.position.x ? walkSpeed : -walkSpeed;
				MoveUnit (new Vector2 (panicMoveX, rb2d.velocity.y));
			} else {
				pathFailScore++;
			}
		}

	}
	override public void Recoiled(){
		if (!stacking) {
			base.Recoiled ();
		}
	}
		
	//this is the crabs brain, there is a lot of code here but my crabs are still dumb. i do love them tho
	IEnumerator FollowPath(){
		//initialise
		pathInProgress = true;
		int currentIndex = 0;
		bool unFinishedJump = false;
		//loop
		while(true){
			if (Mathf.Abs (transform.position.x - path[currentIndex].worldPosition.x) < 1f) {
				//check if jumping
				if(path[currentIndex].isJumping){
					//connection is jump type

						
						if (grounded) {
							yIntention = jumpVel;
						} else {
							unFinishedJump = true;
						}
					
				}
				if (!unFinishedJump) {
					currentIndex++;
				}
			}
			if (currentIndex > path.Length -1) {pathInProgress = false;break; }
			//x direction
			if (Mathf.Abs (transform.position.x - path [currentIndex].worldPosition.x) > 1f ) {
				if (transform.position.x < path [currentIndex].worldPosition.x) {
					xIntention = 1;
				} else if (transform.position.x > path [currentIndex].worldPosition.x) {
					xIntention = -1;
				}
			}
			if (unFinishedJump && grounded) {
				yIntention = jumpVel;
				unFinishedJump = false;
			}
			if (rb2d.velocity.y > 0 && yIntention > 0) {
				yIntention = 0;
			}
			float yChange = rb2d.velocity.y + yIntention;
			float xChange = xIntention * walkSpeed;
			yIntention = 0;
			xIntention = 0;

			if (!unFinishedJump) {
				MoveUnit (new Vector2 (xChange, yChange));
			} else {
				MoveUnit(new Vector2(0, rb2d.velocity.y+-1f));
			}
			yield return null;
		}
		//set movement to 0
		MoveUnit(new Vector2(0, rb2d.velocity.y));
	}

	//this is override is to see if our crab should explode painfully
	override public void Hit(Vector3 info){
		//remove health
		health = health - info.z;
		if (health <= 0) {
			OnDeath (Random.Range(1,3),false);
		}
		KnockBack (new Vector2(info.x,info.y));
		StartCoroutine ("CheckForce");
		if (state == State.fine) {
			Recoiled ();
		}
	}

	//the crabs literally can be killed by jumping into the player so maybe this should be tinkered with
	IEnumerator CheckForce() {
		if(rb2d.velocity.magnitude>45){
			//do something else?
			yield return new WaitForSecondsRealtime (0.2f);
			OnDeath(Random.Range(3,5),true);
		}
	}
		

	//this is where we tell the enemy spawner and explode
	public void OnDeath(int meat, bool hardDeath){
	//spew meat
		Explode(meat,hardDeath);
	//tell somone about it
		Spawner.IAmDead();
	// & die
		base.OnDeath();
	}

	//this is the explosion function, i rolled several of Rob's original functions into this one
	void Explode(int meat, bool hardDeath){
		//meat
		meatSpawner (meat);
		//blood
		GameObject bloodObject = Instantiate(blood, transform.position, Quaternion.Euler(0,0,0));
		Destroy(bloodObject,5);
		//bits of myself
		int increment = -270;
		if (hardDeath) {
			for (int i = 0; i < bodyParts.Length; i++) {
				GameObject bodyPart = Instantiate (bodyParts [i], transform.position, Quaternion.Euler (0, 0, 0));              
				increment = increment - (360 / bodyParts.Length);
				bodyPart.GetComponent<Explode> ().init (increment);
			}
		}
	}

	//i made the meat spawner spawn a random amount of meat (more on har death)
	private void meatSpawner(int amount){
		for(int x=0; x<amount; x++){
			GameObject crabMeatObject = Instantiate(crabMeat, transform.position, Quaternion.Euler(0,0,0));
			crabMeatObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,5), ForceMode2D.Impulse);
		}
	}

	//this is so our crabs dont start moving around while in flight
	override public void HitGround(){
		base.HitGround ();
		if (state == State.spawning) {
			state = State.fine;
		}
		//rb2d.gravityScale = gravityBase;
	}

	//Damage Dealers

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (state == State.fine) {
			if (other.gameObject.tag == "Player") {
				//x,y = pushback z = damage
				Vector3 info = new Vector3 (transform.position.x < player.transform.position.x ? knock : -knock, 10f, dam);

				other.SendMessageUpwards ("Hit", info);
				Vector3 temp = new Vector3 (transform.position.x > player.transform.position.x ? 25 : -25, 20f, 0);
				Hit (temp);
			}
		}
	}


/*void OnDrawGizmos() {

		if(path != null){
		for (int i = 0; i < path.Length - 1; i++) {
				Gizmos.color = Color.cyan;
			Gizmos.DrawSphere (path [i].worldPosition, 0.2f);
			Gizmos.color = Color.green;
		}
	}
	} //*/

}
