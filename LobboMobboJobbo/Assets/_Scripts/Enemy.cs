﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Base enemy class, this will be used as a basis of other enemy scripts
public class Enemy : Unit {

	//variables
	int xIntention = 0; //intended movement on the x axis,can be 1, 0 or -1
	float yIntention = 0; //intended movement on the y axis,can be 1, 0 or -1
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
		currentIndex = 0;
		Vector2 currentWaypoint = new Vector2 (0,0);
		if (path [0].worldPosition != null) {
			currentWaypoint = path [0].worldPosition;
		}
		//check if done
		while(currentIndex < path.Length -1 ){
			if (Mathf.Abs(currentWaypoint.x - transform.position.x) < 1f) {
				currentIndex++;
			}	
				currentWaypoint = path [currentIndex].worldPosition;

			//get x movements
			if (currentWaypoint.x > transform.position.x) {
				xIntention = 1;
			} else if (currentWaypoint.x < transform.position.x) {
				xIntention = -1;
			} else {
				xIntention = 0;
			}
			//check if jumping
			if(path[currentIndex].isJumping){
				if (grounded) {
					if (rb2d.velocity.y <= 0) {
						yIntention = jumpVel;
					}
				} 
			}
			float yChange = yIntention + rb2d.velocity.y;
			yIntention = 0;
			MoveUnit (new Vector2 (xIntention*walkSpeed,yChange));

			yield return null;
		}
	}
	/*void OnDrawGizmos() {
		if (path != null) {
			for (int i = 0; i < path.Length; i++) {
				Gizmos.color = Color.green;
				if (path [i].isJumping) {
					Gizmos.color = Color.white;
				}
				Gizmos.DrawSphere (path [i].worldPosition, 0.3f);
				if (i > 0) {
					Gizmos.color = Color.red;
					if (path [i].isJumping) {
						Gizmos.color = Color.yellow;
					}
					Gizmos.DrawLine (path [i].worldPosition, path [i - 1].worldPosition);
				}
			}

		}
	}*/
}
