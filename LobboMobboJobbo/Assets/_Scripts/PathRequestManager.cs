using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PathRequestManager : MonoBehaviour {

	Queue<PathRequest> pathRequestQueue = new Queue<PathRequest> ();
	PathRequest currentPathRequest;

	static PathRequestManager instance;
	Pathfinding pathfinding;

	bool isProcessingPath;

	void Awake(){
	
		instance = this;
		pathfinding = GetComponent<Pathfinding> ();

	}

	//request path
	public static void RequestPath(Vector2 pathStart, Vector2 pathEnd, Action<Pathfinding.PathWay[],bool> callback){

		PathRequest newRequest = new PathRequest (pathStart, pathEnd, callback);
		instance.pathRequestQueue.Enqueue (newRequest);
		instance.TryProcessNext ();

	}

	void TryProcessNext(){
		if (!isProcessingPath && pathRequestQueue.Count > 0) {
		
			currentPathRequest = pathRequestQueue.Dequeue ();
			isProcessingPath = true;
			pathfinding.StartFindPath(currentPathRequest.pathStart, currentPathRequest.pathEnd);
		
		}
	
	}

	public void FinishedProcessingPath(Pathfinding.PathWay[] path, bool success){

		currentPathRequest.callback (path, success);
		isProcessingPath = false;
		TryProcessNext ();

	}

	struct PathRequest {

		public Vector2 pathStart;
		public Vector2 pathEnd;
		public Action<Pathfinding.PathWay[],bool> callback;

		public PathRequest(Vector2 pathStart, Vector2 pathEnd, Action<Pathfinding.PathWay[],bool> callback){
			this.pathStart = pathStart;
			this.pathEnd = pathEnd;
			this.callback = callback;


		}

	}
}
