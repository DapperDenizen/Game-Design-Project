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



	// Use this for initialization
	void Start () {
	//get everything
		platformMask = LayerMask.NameToLayer("Ground"); 
		platforms = GameObject.FindGameObjectsWithTag("Ground");
		//make waypoints
		Vector2 startPos;
		Vector2 endPos;
			// loop over platforms
		foreach (GameObject plat in platforms) {
			int platXSize = Mathf.RoundToInt (plat.GetComponent<BoxCollider2D>().bounds.size.x / waypointSpacing);
			//get left edge and start creating new waypoints until hit the other edge
			Vector2 centre = new Vector2 (plat.GetComponent<BoxCollider2D>().bounds.center.x, plat.GetComponent<BoxCollider2D>().bounds.center.y + plat.GetComponent<BoxCollider2D>().bounds.extents.y + hoverSpacing); 
			startPos = new Vector2 (centre.x - plat.GetComponent<BoxCollider2D>().bounds.extents.x, centre.y);
			endPos = new Vector2 (centre.x + plat.GetComponent<BoxCollider2D>().bounds.extents.x, centre.y);
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
			for (int z = i; z <  waypoints.Length; z++) {
				if (i == z) {continue;}
					if (Connectable (waypoints [i], waypoints [z])) {
						waypoints [i].NextNeighbour (waypoints [z]);
					}
				}
				//
		}
	}

	bool Connectable(Waypoint a, Waypoint b){

	//check if they are platform connected
		if(a.worldPosition.y == b.worldPosition.y && Vector2.Distance(a.worldPosition,b.worldPosition) < (waypointSpacing*1.01f)){
			return true;
		}
	//check if they are jump connected -------------------------------------------------------------if platforms float above other platforms change onEdge && to an ||
		if((b.jumpConnection == null && a.jumpConnection == null) && (a.onEdge && b.onEdge)){
			//both are eligable to have a jump connnection
			if(Vector2.Distance(a.worldPosition,b.worldPosition) < maxJumpDistance){
				//they are close enough
				RaycastHit2D hitA = Physics2D.Raycast(a.worldPosition,( b.worldPosition - a.worldPosition),Mathf.Infinity,~platformMask);
				RaycastHit2D hitB = Physics2D.Raycast(b.worldPosition,( a.worldPosition - b.worldPosition),Mathf.Infinity,~platformMask);
				if (hitA.collider != null) {
					if (Vector2.Distance (hitA.point,hitB.point) > hitA.collider.bounds.size.y*0.9) {
						return false;
					}
				}
				return true;
			}
		} 


		return false;
	}


	//THIS IS BAD MAKE IT MORE EFFICIENT, THE WAYPOINTS LIST IS SORTED BY THE X POSITION EXPLOIT THAT
	public Waypoint PointFromWorldPosition(Vector2 worldPosition){
		int wayIndex = 0;
		float smallestDistance = Mathf.Infinity;
		for(int i =0; i < waypoints.Length; i++) {
			if (Vector2.Distance (worldPosition, waypoints [i].worldPosition) < smallestDistance) {
			
				smallestDistance = Vector2.Distance (worldPosition, waypoints[i].worldPosition);
				wayIndex = i;
			}
		}
		return waypoints[wayIndex];
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

	//float TopLeftCorner(GameObject platform){
	//	return platform.GetComponent<BoxCollider2D> ().bounds.center.x - platform.GetComponent<BoxCollider2D> ().bounds.extents.x;
	//}



	/*void OnDrawGizmos() {
		for(int i = 0; i < waypoints.Length; i++){
			Gizmos.color = Color.red;

			Gizmos.DrawSphere (waypoints [i].worldPosition, 0.3f);
	
			
		}
	} */

}
