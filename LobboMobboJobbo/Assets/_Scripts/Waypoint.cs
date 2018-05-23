using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : IHeapItem<Waypoint> {
	private List<Waypoint> neighbours = new List<Waypoint>(); // list of neighbours, 0 is the neighbour to the left 1+ is right
	public Waypoint jumpConnection;
	public Vector2 worldPosition;
	LayerMask platform = LayerMask.GetMask ("Ground");
	public bool onEdge;
	public int gCost; // Distance from starting position
	public int hCost; // distance from ending position
					  // F cost is G+H cost
	public Waypoint parent;
	int heapIndex;

	public Waypoint(Vector2 worldPosition, bool isEdge){
		this.worldPosition = worldPosition;
		onEdge = isEdge;

	}

	public void NextNeighbour(Waypoint next){
		bool jumpConnect = false;
		RaycastHit2D myHit = Physics2D.Raycast (worldPosition, Vector2.down,Mathf.Infinity,platform);
		RaycastHit2D nextHit = Physics2D.Raycast (next.worldPosition, Vector2.down,Mathf.Infinity,platform);
		if (myHit.collider != nextHit.collider) {
			jumpConnect = true;
		}
		if (jumpConnect) {
			jumpConnection = next;
			next.LastNeighbours (this, jumpConnect);
		} else {
			neighbours.Add (next);
			next.LastNeighbours (this, jumpConnect);
		}
	}



	public void LastNeighbours(Waypoint next, bool jumpConnect){
		if (jumpConnect) {
			jumpConnection = next;
		} else {
			neighbours.Add (next);
		}
	}

	public List<Waypoint> getNeighbours(){
		List<Waypoint> returnList = new List<Waypoint>(neighbours);
		if (jumpConnection != null) {
			returnList.Add (jumpConnection);
		}
		return returnList;
	}
		
	public bool IsJump(Waypoint neighbour){
		if (neighbour == jumpConnection) {
			return true;
		} else {
			return false;
		}
	}

	//Heap stuff
	public int fCost {
		get{ 
			return gCost + hCost;
		}

	}
	public int HeapIndex{

		get{ return heapIndex;}
		set{ heapIndex = value;}

	}
	public int CompareTo(Waypoint nodeToCompare){
		int compare = fCost.CompareTo (nodeToCompare.fCost);
		if (compare == 0) {
			compare = hCost.CompareTo (nodeToCompare.hCost);

		}
		return -compare;
	}
}
