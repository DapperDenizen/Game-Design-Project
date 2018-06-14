using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointHandler : MonoBehaviour {
	GameObject[] platforms; // all platforms
	Waypoint[] waypoints;
	List<Waypoint> waypointsList = new List<Waypoint> (); //waypoints
	LayerMask platformMask;
	public int wayPointSize;
	public float waypointSpacing; // space between each waypoint
	public float hoverSpacing; //space above platform
	public float maxJumpDistance;
	//this code breaks very often so every new impelementation or level design change will require a rework of some descrption

	//
	Vector2 centreDubug = Vector2.zero;
	//

	//RECCOMENDED SETTINGS!
	/*
	 * waypoint spacing = 1.5 -> space between each waypoint( smaller = more waypoints)
	 * hover = 0.4 -> how far above a platform the points rest
	 * max jump = 6 -> max distance for jump connections
	 */
	//

	// Use this for initialization
	void Start () {
		//get everything
		platformMask = LayerMask.GetMask("Ground");
		platforms = GameObject.FindGameObjectsWithTag("Ground");
		//make waypoints
		Vector2 startPos;
		// loop over platforms
		foreach (GameObject plat in platforms) {
			int platXSize = Mathf.RoundToInt (plat.GetComponent<BoxCollider2D>().bounds.size.x / waypointSpacing);
			//get left edge and start creating new waypoints until hit the other edge
			Vector2 centre = new Vector2 (plat.GetComponent<BoxCollider2D>().bounds.center.x, plat.GetComponent<BoxCollider2D>().bounds.center.y + plat.GetComponent<BoxCollider2D>().bounds.extents.y + hoverSpacing); 
			startPos = new Vector2 (centre.x - plat.GetComponent<BoxCollider2D>().bounds.extents.x, centre.y);
			Vector2 currentWaypoint;
			int i = 0;
			bool isEdge = true;
			for ( i = 0; i < platXSize; i++) {
				if (i == 0 || i == platXSize-1) {
					isEdge = true;
				} else {
					isEdge = false;
				}
				currentWaypoint = startPos +  Vector2.right * (i* waypointSpacing + (waypointSpacing/3));
				waypointsList.Add (new Waypoint (currentWaypoint,isEdge));
			}

		}

		waypoints = waypointsList.ToArray ();
		QuickSort (0, waypoints.Length-1);
		wayPointSize = waypoints.Length;
		//--------------------------------------------------------------------------------------------------connect waypoints
		//loop over waypoints giving each of them the next in line and a null bool
		for(int i = 0; i < waypoints.Length-1; i ++){
			for (int z = 0; z <  waypoints.Length; z++) {
				if (i == z) {continue;}
				Connectable (waypoints [i], waypoints [z]);
			}
			//
		}
	}

	void Update(){
		if (Input.GetMouseButton (0)) {
			Vector3 dumbEngine =Camera.main.ScreenToWorldPoint( Input.mousePosition);
			Vector2 direction = centreDubug - new Vector2(dumbEngine.x,dumbEngine.y);
			print (Vector2.Angle (Vector2.up, direction));
		}
	}


	//checks is two points should connect or not
	void Connectable(Waypoint a, Waypoint b){
	//check if they are platform connected
		if (a.worldPosition.y == b.worldPosition.y && Vector2.Distance (a.worldPosition, b.worldPosition) < (waypointSpacing * 1.01f)) {
			//walk connected
			ConnectPoints(a,b,Waypoint.ConnectType.Walk);
		} else if ((a.onEdge && b.worldPosition.y < a.worldPosition.y) || (b.onEdge && a.worldPosition.y < b.worldPosition.y) || (a.onEdge && b.onEdge && a.worldPosition.y == b.worldPosition.y && a.platform != b.platform) ) {
			//check if they can jump connected
			if (WeightedDistance (a.worldPosition, b.worldPosition) <= maxJumpDistance) {
				RaycastHit2D hitA = Physics2D.Raycast (a.worldPosition, (b.worldPosition - a.worldPosition), Mathf.Infinity, platformMask);
				RaycastHit2D hitB = Physics2D.Raycast (b.worldPosition, (a.worldPosition - b.worldPosition), Mathf.Infinity, platformMask);
				bool valid = true;
				if (hitA.collider != null) {
					if (Vector2.Distance (hitA.point, hitB.point) > hitA.collider.bounds.size.y * 1f) {
						valid = false;
					}
				}
				Vector2 direction = a.worldPosition - b.worldPosition;
				if (Vector2.Angle (Vector2.up, direction) > 145) {
					valid = false;
				}
				if(valid){
				//check the platform isnt already in use!
				Waypoint compared = platformCompare (a, b);
				if (compared == a) {
						print (Vector2.Angle (Vector2.up, direction));
					ConnectPoints (a, b, Waypoint.ConnectType.Jump);

				} else if (BetterConnection (a, b, compared)) {
						print (Vector2.Angle (Vector2.up, direction));
						ConnectPoints (a, b, Waypoint.ConnectType.Jump);
				}
			}
			//
			}
		}
		if (Mathf.Abs (a.worldPosition.x - b.worldPosition.x) < 1f && a.worldPosition.y < b.worldPosition.y && a.throughConnection == null && Vector2.Distance(a.worldPosition,b.worldPosition) < maxJumpDistance*0.7f) {
		//check if they can through connect
			ConnectPoints(a,b,Waypoint.ConnectType.Through);
		}

	}

	void ConnectPoints(Waypoint a, Waypoint b, Waypoint.ConnectType connect){
		a.NextNeighbour (b, connect);
	}

	public float WeightedDistance(Vector2 a, Vector2 b){
		float yWDist = Mathf.Abs (a.y - b.y)*0.1f;
		float offsetDist = Vector2.Distance (a, b) + yWDist;
		//print ("unweightedDist = "+ Vector2.Distance (a, b) +" Weighted = "+ offsetDist  + yWDist);
		return offsetDist;
	}

	public Waypoint PointFromWorldPosition(Vector2 worldPosition){
		int wayIndex = 0;
		int iMax; //max being checked
		int iMin; //min being checked
		if (worldPosition.x > waypoints [Mathf.RoundToInt (waypoints.Length / 2)].worldPosition.x) {
			iMax = waypoints.Length;
			iMin = Mathf.RoundToInt (waypoints.Length / 2);
		} else {
			iMin = 0;
			iMax = Mathf.RoundToInt (waypoints.Length / 2);
		}//*/
		float smallestDistance = Mathf.Infinity;
		for(int i =iMin; i < iMax; i++) {
			if (Vector2.Distance (worldPosition, waypoints [i].worldPosition) < smallestDistance ) {
				//&& worldPosition.y == waypoints[i].worldPosition.y
				smallestDistance = Vector2.Distance (worldPosition, waypoints[i].worldPosition);
				wayIndex = i;
			}
		}
		return waypoints[wayIndex];
	}


	//returns true if there is a connection from a to platform on b
	Waypoint platformCompare(Waypoint a, Waypoint b){
		if (a.jumpConnections.Count == 0 || b.jumpConnections.Count == 0) {
			return a;
		}
		foreach (Waypoint point in a.jumpConnections) {
			if (b.platform == point.platform) {
				return point;
			}
		}
		return a;
	}

	//checks if there is a better connection
	bool BetterConnection(Waypoint a, Waypoint b, Waypoint c){
		float distAB = Vector2.Distance (a.worldPosition, b.worldPosition);
		float distAC = Vector2.Distance(c.worldPosition,a.worldPosition);
			if( distAC > distAB){
				//remove cd return true
				a.RemoveConnection(c);
				c.RemoveConnection(a);
				return true;
			}
		return false;
	}


	//utility functions
	void QuickSort(int low, int high){
		//sort platforms so the smaller X position is first in the list
		if(low< high){
			int p = Partition (low, high);
			QuickSort ( low, p - 1);
			QuickSort ( p + 1, high);
		}
	}
	int Partition( int low, int high){
		float pivot = waypoints [high].worldPosition.x;
		int i = low - 1;
		for (int j = low; j < high; j++) {
			if(waypoints[j].worldPosition.x < pivot){
				i++;
				SwapArray (i,j);
			}

		}
		SwapArray (i+1, high);
		return i;
	}
	void SwapArray(int a, int b){
		Waypoint temp = waypoints [b];
		waypoints [b] = waypoints[a];
		waypoints[a] = temp;
	}
		
	//use this to check the waypoint generation
	void OnDrawGizmos() {
		for(int i = 0; i < waypoints.Length-1; i++){
			int count = waypoints [i].getNeighbours ().Count;
			Gizmos.color = Color.red;
			Gizmos.DrawSphere (waypoints [i].worldPosition, 0.2f);
			foreach (Waypoint point in waypoints[i].getNeighbours()) {
				Gizmos.color = Color.green;
				Gizmos.DrawLine (waypoints [i].worldPosition, point.worldPosition);
			}
		}
	} //*/

}
