using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : UnitController {

	Vector2 test = Vector2.zero;

	//variables
	int xIntention = 0; //intended movement on the x axis,can be 1, 0 or -1
	float yIntention = 0; //intended movement on the y axis,can be 1, 0 or -1
	float yOffset =0;
	public float knock = 5f; // this is the knock back strength
	public float dam;
	//Pathfinding
	public Pathfinding.PathWay[] path; // this is the path we are following
	public Pathfinding.PathWay currentTarget;
	Vector2 targetPlace;
	int currentIndex;
	bool pathRequested = false;
	bool pathInProgress = false;
	//dumb things
	public bool stacking;
	public GameObject buddy; //this is the crab you're sitting on
	float stackOffset;
	//
	//references
	GameObject player;
	public GameObject crabGore;
	//

	//override means i am using this Start() not the parent(unit)'s start, but calling base.start() means i call it as well
	override public void Start(){
		base.Start ();
		player = GameObject.FindGameObjectWithTag ("Player"); //this is the guy we hate
		yOffset = GetComponent<BoxCollider2D>().bounds.extents.y;
		targetPlace = player.transform.position;
		if (stacking) {
			stackOffset = buddy.GetComponent<BoxCollider2D> ().bounds.extents.y+this.GetComponent<BoxCollider2D>().bounds.extents.y;
			Physics2D.IgnoreCollision (this.GetComponent<BoxCollider2D>(),buddy.GetComponent<BoxCollider2D> ());
			rb2d.gravityScale = 0;
		}

	}

	void FixedUpdate(){
		if (player != null) {
			if (!pathRequested && !stacking) {
				if (Vector2.Distance (transform.position, player.transform.position) < 2.1f) {
					//attack animation plays


				} else if (!pathInProgress) {
					StartPath (player.transform.position);
					pathRequested = true;
				} else if (Vector2.Distance (targetPlace, player.transform.position) > 4.5f) {

					StartPath (player.transform.position);
					pathRequested = true;
				}

			} else if (stacking) {
				//check buddys not dead
				if (buddy.activeInHierarchy) {
					transform.position = new Vector2 (buddy.transform.position.x, buddy.transform.position.y + stackOffset); 
				} else {
					stacking = false;
					rb2d.gravityScale = 1;
				}
			}
		}
	}

	void StartPath(Vector2 targetGoal){
		if (!pathRequested) {
			targetPlace = targetGoal;
			pathRequested = true;
			PathRequestManager.RequestPath (transform.position, targetGoal, OnPathFound);
			//debug_PATHFINDING_CALL++;
		}
	}

	virtual public void OnPathFound(Pathfinding.PathWay[] newPath, bool pathSuccess){
		pathRequested = false;
		if (pathSuccess) {
			if (newPath.Length > 0 ){
				if (newPath [newPath.Length - 1].worldPosition != targetPlace) {
					path = newPath;
					StopCoroutine ("FollowPath");
					//print ("stopping because " + temp);
					pathInProgress = false;
					StartCoroutine ("FollowPath");
				}
			}
		}

	}


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
					if(transform.position.y < path[currentIndex+1].worldPosition.y || !grounded){
						//not dropping
						if (grounded) {
							yIntention = jumpVel;
						} else {
							unFinishedJump = true;
						}
					}
				}
				if (!unFinishedJump) {
					currentIndex++;
				}
			}
			if (currentIndex > path.Length -1) {print ("done");pathInProgress = false;break; }
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

	override public void OnDeath(){
	//spew meat
		GameObject crabBits = Instantiate(crabGore,transform.position,Quaternion.Euler(0,0,0));
	// & die
		base.OnDeath();
	}


	//Damage Dealers

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "HitBox")
		{
			anim.SetBool("Attacking",true);

			print ("hit him");
			//x,y = pushback z = damage
			Vector3 info = new Vector3 (transform.position.x < player.transform.position.x ? -knock : knock,1f,dam);

			other.SendMessageUpwards("Hit",info);
		}
	}
	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "HitBox")
		{
			anim.SetBool("Attacking",false);
		}
	}
		
	//*/
}
