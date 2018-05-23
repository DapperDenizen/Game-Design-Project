using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Base enemy class, this will be used as a basis of other enemy scripts
public class Enemy : Unit {

	//variables
	int xIntention = 0; //intended movement on the x axis,can be 1, 0 or -1
	float yIntention = 0; //intended movement on the y axis,can be 1, 0 or -1
	//Pathfinding
	Pathfinding.PathWay[] path; // this is the path we are following
	Pathfinding.PathWay currentTarget;
	int currentIndex;
	//references
	GameObject player;

	//override means i am using this Start() not the parent(unit)'s start, but calling base.start() means i call it as well
	override public void Start(){
		base.Start ();
		player = GameObject.FindGameObjectWithTag ("Player"); //this is the guy we hate

	
	}

	void Update(){
		if (Input.GetKeyDown(KeyCode.J)) {
			StartPath (player.transform.position);
		}
	}



	void StartPath(Vector2 targetGoal){
	
		PathRequestManager.RequestPath (transform.position, targetGoal, OnPathFound);

	}

	public void OnPathFound(Pathfinding.PathWay[] newPath, bool pathSuccess){

		print ("Path found = " + pathSuccess);

		if (pathSuccess) {
			path = newPath;
			StopCoroutine ("FollowPath");
			StartCoroutine ("FollowPath");
		
		}

	}


	IEnumerator FollowPath(){
		currentIndex = 0;
		currentTarget = path [0];
		while(true){
		//get the X intention
			if (currentTarget.worldPosition.x > transform.position.x) {
				xIntention = 1;
			} else {
				xIntention = -1;
			}
			// check if you will need to jump
			if (currentTarget.isJumping) {
				yIntention = jumpVel+rb2d.velocity.y;

			} else {
				yIntention = 0;
			}
		//make the nessecary movements until x > waypoint.worlposition
			Vector2 intentionDir = new Vector2(xIntention*walkSpeed,yIntention);
			xIntention = 0;
			yIntention = 0;
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
