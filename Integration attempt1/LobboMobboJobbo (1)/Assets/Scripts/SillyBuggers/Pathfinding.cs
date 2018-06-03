using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
public class Pathfinding : MonoBehaviour {

	WaypointHandler handler;
	Vector2 gizmoME; 
	PathWay[] gizmoPath = new PathWay[10];
	PathRequestManager requestManager;

	// Use this for initialization
	void Awake () {
		handler = GetComponent<WaypointHandler> ();
		requestManager = GetComponent<PathRequestManager> ();
	}

	public void StartFindPath(Vector2 startPos, Vector2 targetPos){
		StartCoroutine (FindPath(startPos, targetPos));
	}

	IEnumerator FindPath(Vector2 start, Vector2 target){
		gizmoME = start;
		PathWay[] pathPoints = new PathWay[0];
		bool pathSuccess = false;
		Waypoint startingPoint = handler.PointFromWorldPosition (start);
		Waypoint targetPoint = handler.PointFromWorldPosition (target);

		Heap<Waypoint> openSet = new Heap<Waypoint> (handler.wayPointSize); //to be evaluated
		HashSet<Waypoint> closedSet = new HashSet<Waypoint> ();	//already evaluated
		openSet.Add (startingPoint);

		while (openSet.Count > 0) {

			Waypoint currentPoint = openSet.RemoveFirst ();
			closedSet.Add (currentPoint);
			//gizmoPath.Add (currentPoint);
			if (currentPoint == targetPoint) {
				pathSuccess = true;
				break;
			}
			foreach (Waypoint neighbour in currentPoint.getNeighbours()) {

				if(closedSet.Contains(neighbour)){continue;}
				int newMovementCostToNeighbour = currentPoint.gCost + GetDistance (currentPoint, neighbour);
				if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains (neighbour)) {

					neighbour.gCost = newMovementCostToNeighbour;
					neighbour.hCost = GetDistance (neighbour, targetPoint);
					neighbour.parent = currentPoint;

					if (!openSet.Contains (neighbour)) {
						openSet.Add (neighbour);
					} else {
						openSet.UpdateItem (neighbour);

					}

				}

			}


		}
		yield return null;
		if (pathSuccess) {
			pathPoints = RetracePath (startingPoint, targetPoint);
			if (pathPoints.Length <= 1) {
				//print ("UH OH SPEGGETTI Os");
			}
			//gizmoPath = pathPoints;
		}
		requestManager.FinishedProcessingPath (pathPoints, pathSuccess);

	}
	//

	PathWay[] RetracePath(Waypoint start, Waypoint end){
		List<PathWay> path = new List<PathWay> ();
		Waypoint currentPoint = end;
		bool jumpingHere = false;
		bool through = false;
		while (currentPoint != start) {

			path.Add (new PathWay (currentPoint.worldPosition, jumpingHere, through));
			jumpingHere = currentPoint.IsJump(currentPoint.parent);
			through = currentPoint.Isthrough(currentPoint.parent);
			currentPoint = currentPoint.parent;
		}
		PathWay[] returnPath;
		returnPath = path.ToArray();
		Array.Reverse(returnPath);
		return returnPath;

	}		

	int GetDistance(Waypoint a, Waypoint b)
	{
		float xChange = Mathf.Abs (a.worldPosition.x - b.worldPosition.x);
		float yChange = Mathf.Abs (a.worldPosition.y - b.worldPosition.y);
		int returnDist = Mathf.RoundToInt(10 + xChange + (yChange* 1.5f));
		return returnDist;
	}


	/*void OnDrawGizmos() {
		if (gizmoPath.Length != null) {
			for (int i = 0; i < gizmoPath.Length; i++) {
				Gizmos.color = Color.grey;
				//if (gizmoPath [i].jumpThrough) {
					Gizmos.DrawSphere (gizmoPath [i].worldPosition, 0.3f);	
				//}

			}
		}

	}
//*/



	public struct PathWay {
		public Vector2 worldPosition;
		public bool isJumping;
		public bool jumpThrough;

		public PathWay(Vector2 position, bool jump, bool through){
			worldPosition = position;
			jumpThrough = through;
			isJumping = jump;
			if(through){
				jump = true;
			}
		}


	}
}
