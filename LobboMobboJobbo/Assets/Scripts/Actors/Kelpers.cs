using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kelpers : EnemyControl {

	override public void OnPathFound(Pathfinding.PathWay[] newPath, bool pathSuccess){
	
		pathRequested = false;
		if (pathSuccess && this != null) {
			pathFailScore = 0;
			if (newPath.Length > 0) {

				if (newPath [newPath.Length - 1].worldPosition != targetPlace) {
					path = KelpPath( newPath);
					StopCoroutine ("FollowPath");
					pathInProgress = false;
					StartCoroutine ("FollowPath");
				}
			}
		} else if(!pathSuccess){

			if (pathFailScore > maxPathAttempt) {
				float panicMoveX =	transform.position.x < player.transform.position.x ? walkSpeed : -walkSpeed;
				MoveUnit (new Vector2 (panicMoveX, rb2d.velocity.y));
			} else {
				pathFailScore++;
			}
		}
	
	}

	public Pathfinding.PathWay[] KelpPath(Pathfinding.PathWay[] newPath){
		int numberOfTheDay = Random.Range (3, 5);
		List<Pathfinding.PathWay> kelpedPath = new List<Pathfinding.PathWay> ();
		int nowAt = 0;
		for (int i = 0; i < newPath.Length; i++) {
			print ("at " + nowAt + " / " + numberOfTheDay);
			if (nowAt == numberOfTheDay) {
				kelpedPath.Add (newPath [i - 1]);
				kelpedPath.Add (newPath [i - 2]);
				kelpedPath.Add (newPath [i - 3]);
				nowAt = 0;
			} else {
				nowAt++;
			}

			kelpedPath.Add (newPath [i]);
		}
		return kelpedPath.ToArray ();
	}
}
