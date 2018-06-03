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
	public float maxAngle; // maximum angle from 90 degrees ( 90 absolute max, 0 absolute min
	public float minAngle; // minimum angle from 90 degrees

	//RECCOMENDED SETTINGS!
	/*
	 * waypoint spacing = 1.5
	 * hover = 0.4
	 * max jump = 6
	 * max angle = 75
	 * min angle = 15
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



	//checks is two points should connect or not
	void Connectable(Waypoint a, Waypoint b){

		//check if they are platform connected
		if(a.worldPosition.y == b.worldPosition.y && Vector2.Distance(a.worldPosition,b.worldPosition) < (waypointSpacing*1.01f)){
			a.NextNeighbour (b, Waypoint.ConnectType.Walk);
		}

		//check if they are jump connected 
		if (a.onEdge || b.onEdge) {
			//both are eligable to have a jump connnection
			if (Vector2.Distance (a.worldPosition, b.worldPosition) <= maxJumpDistance) {
				//they are close enough
				RaycastHit2D hitA = Physics2D.Raycast (a.worldPosition, (b.worldPosition - a.worldPosition), Mathf.Infinity, platformMask);
				RaycastHit2D hitB = Physics2D.Raycast (b.worldPosition, (a.worldPosition - b.worldPosition), Mathf.Infinity, platformMask);
				if (hitA.collider != null) {
					if (Vector2.Distance (hitA.point, hitB.point) < hitA.collider.bounds.size.y * 0.9) {
						//check if theres already a jump connection to this platform
						if (!platformCompare (a, b)) {
							a.NextNeighbour (b, Waypoint.ConnectType.Jump);	
						}

					}
				}

			}
			//check if they are vertically connected (both are close on the Y axis B is above A
		} else if(Mathf.Abs(a.worldPosition.x - b.worldPosition.x) < 1f && b.worldPosition.y > a.worldPosition.y && a.throughConnection == null){
			a.NextNeighbour (b, Waypoint.ConnectType.Through); 

		}
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
	bool platformCompare(Waypoint a, Waypoint b){
		if (a.jumpConnections.Count == 0) {
			return false;
		}
		RaycastHit2D hitPoint; 
		RaycastHit2D hitB = Physics2D.Raycast(b.worldPosition,Vector2.down,Mathf.Infinity,platformMask);
		foreach (Waypoint point in a.jumpConnections) {
			hitPoint = Physics2D.Raycast(point.worldPosition,Vector2.down,Mathf.Infinity,platformMask);
			if (hitB.collider == hitPoint.collider) {
				return true;
			}
		}
		return false;
	}
	//utility


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




	void OnDrawGizmos() {
		for(int i = 0; i < waypoints.Length-1; i++){
			Gizmos.color = Color.red;
			Gizmos.DrawSphere (waypoints [i].worldPosition, 0.3f);
			Gizmos.color = Color.green;
			foreach (Waypoint point in waypoints[i].getNeighbours()) {
				Gizmos.DrawLine (waypoints [i].worldPosition, point.worldPosition);
			}
			
		}
	} //*/

}
