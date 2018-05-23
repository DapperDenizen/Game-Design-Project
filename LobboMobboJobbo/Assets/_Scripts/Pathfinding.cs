using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System;
public class Pathfinding : MonoBehaviour {

	WaypointHandler handler;
	List<Waypoint> gizmoPath = new List<Waypoint>();
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
			gizmoPath.Add (currentPoint);
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
			//gizmoPath = pathPoints;
		}
		requestManager.FinishedProcessingPath (pathPoints, pathSuccess);

	}
	//

	PathWay[] RetracePath(Waypoint start, Waypoint end){
		List<PathWay> path = new List<PathWay> ();
		Waypoint currentPoint = end;
		Waypoint previousPoint = end;
		while (currentPoint != start) {

			path.Add (new PathWay (currentPoint.worldPosition, currentPoint.IsJump (currentPoint.parent)));
			previousPoint = currentPoint;
			currentPoint = currentPoint.parent;
		}
		PathWay[] returnPath;
		returnPath = path.ToArray();
		Array.Reverse(returnPath);
		return returnPath;

	}

	/*Waypoint[] SimplifyPath(List<Waypoint> path){
		List<Waypoint> simplerPoints = new List<Waypoint> ();

		Vector2 directionOld = Vector2.zero;
		for(int i = 1; i < path.Count; i++){
			Vector2 directionNew = new Vector2(path[i-1].worldPosition.x - path[i].worldPosition.x,path[i-1].worldPosition.y - path[i].worldPosition.y );
			if(directionNew != directionOld){
				simplerPoints.Add(path[i]);

			}
			directionOld = directionNew;
		}
		return simplerPoints.ToArray();
	}*/



	int GetDistance(Waypoint a, Waypoint b)
	{
		float xChange = Mathf.Abs (a.worldPosition.x - b.worldPosition.x);
		float yChange = Mathf.Abs (a.worldPosition.y - b.worldPosition.y);
		int returnDist = Mathf.RoundToInt( xChange + (yChange* 1.5f));
		return returnDist;
	}


/*	void OnDrawGizmos() {
		if (gizmoPath.Count != null) {
			for (int i = 0; i < gizmoPath.Count; i++) {
				Gizmos.color = Color.green;
				Gizmos.DrawSphere (gizmoPath [i].worldPosition, 0.3f);
			}
		}
	}
*/

	public struct PathWay {
		public Vector2 worldPosition;
		public bool isJumping;

		public PathWay(Vector2 position, bool jump){
			worldPosition = position;
			isJumping = jump;
		}


	}
}
