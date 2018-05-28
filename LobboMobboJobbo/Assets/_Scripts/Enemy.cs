using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Base enemy class, this will be used as a basis of other enemy scripts
public class Enemy : Unit {

	Vector2 test = Vector2.zero;

	//variables
	int xIntention = 0; //intended movement on the x axis,can be 1, 0 or -1
	float yIntention = 0; //intended movement on the y axis,can be 1, 0 or -1
	float yOffset =0;
	//Pathfinding
	public Pathfinding.PathWay[] path; // this is the path we are following
	public Pathfinding.PathWay currentTarget;
	int currentIndex;
	//references
	GameObject player;
	//override means i am using this Start() not the parent(unit)'s start, but calling base.start() means i call it as well
	override public void Start(){
		base.Start ();
		player = GameObject.FindGameObjectWithTag ("Player"); //this is the guy we hate
		yOffset = GetComponent<BoxCollider2D>().bounds.extents.y;
	
	}
	void Update(){
		//check if close enough
		/*if (Vector2.Distance (transform.position, player.transform.position) > weapon.GetComponent<WeaponController> ().weaponReach) {
			// not close enough
			StartPath (player.transform.position);
		} else {
			//close enough

			//for testing purposes we dont have a weapon
			print("~ ~ oo im gunna get you! you better be scared! boogedy boogedy boo ~ ~");
			//AttackPrimary(player.transform.position);
		} */
		if (Input.GetKeyUp (KeyCode.J)) {
			StartPath (player.transform.position);
		}

	}

	void StartPath(Vector2 targetGoal){
	
		PathRequestManager.RequestPath (transform.position, targetGoal, OnPathFound);

	}

	virtual public void OnPathFound(Pathfinding.PathWay[] newPath, bool pathSuccess){

		if (pathSuccess) {
			path = newPath;
			StopCoroutine ("FollowPath");
			StartCoroutine ("FollowPath");
		
		}

	}


	IEnumerator FollowPath(){
		//initialise
		int currentIndex = 0;
		bool unFinishedJump = false;
		//loop
		while(true){
			if (Mathf.Abs (transform.position.x - path[currentIndex].worldPosition.x) < 1f) {
				//check if jumping
				if(path[currentIndex].isJumping){
					//connection is jump type
					Debug.Log(transform.position.y+" < "+path[currentIndex+1].worldPosition.y);
					if(transform.position.y < path[currentIndex+1].worldPosition.y || !grounded){
						//not dropping
						if (grounded) {
							yIntention = jumpVel;
						} else {
							print ("whoa");
							unFinishedJump = true;
						}
					}
				}
				if (!unFinishedJump) {
					currentIndex++;
				}
			}
			if (currentIndex > path.Length -1) { print ("done");break; }
			//x direction
			if (Mathf.Abs (transform.position.x - path [currentIndex].worldPosition.x) > 1f ) {
				if (transform.position.x < path [currentIndex].worldPosition.x) {
					xIntention = 1;
				} else if (transform.position.x > path [currentIndex].worldPosition.x) {
					xIntention = -1;
				}
			}
			if (unFinishedJump && grounded) {
				Debug.Log ("jumped");
				yIntention = jumpVel;
				unFinishedJump = false;
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
	/*void OnDrawGizmos() {
		Gizmos.color = Color.cyan;
		if (path != null) {
			foreach (Pathfinding.PathWay path in path) {
				if (path.isJumping) {
					Gizmos.DrawSphere (path.worldPosition, .5f);
				}
			}
		}
	}//*/
}
