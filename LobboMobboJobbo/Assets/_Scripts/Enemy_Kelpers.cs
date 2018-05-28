using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Kelpers : Enemy {



	override public void OnPathFound(Pathfinding.PathWay[] newPath, bool pathSuccess){

		if (pathSuccess) {
			path = KelpPath(newPath);
			StopCoroutine ("FollowPath");
			StartCoroutine ("FollowPath");

		}

	}



	public Pathfinding.PathWay[] KelpPath(Pathfinding.PathWay[] newPath){
		int numberOfTheDay =  Random.Range (3, 5);
		List<Pathfinding.PathWay> kelpedPath = new List<Pathfinding.PathWay>();
		int nowAt = 0;
		for (int i = 0; i < newPath.Length; i++) {
			if (nowAt == numberOfTheDay) {
				kelpedPath.Add (newPath[i-1]);
				kelpedPath.Add (newPath[i-2]);
				kelpedPath.Add (newPath[i-3]);
				nowAt = 0;
			} else {
				nowAt++;
			}
		
			kelpedPath.Add (newPath [i]);
		}
		/*
		print ("Kelped Path");
		for (int z = 0; z < kelpedPath.Count; z++) {
			print (kelpedPath [z].worldPosition);
		}
		print ("Original Path");
		for (int z = 0; z < newPath.Length; z++) {
			print (newPath [z].worldPosition);
		}
		*/
		return kelpedPath.ToArray ();
	}

}
