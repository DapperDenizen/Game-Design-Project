using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : IHeapItem<Waypoint> {
	public enum ConnectType {Jump, Walk, Through};
	private List<Waypoint> neighbours = new List<Waypoint>(); // list of neighbours, 0 is the neighbour to the left 1+ is right
	public List<Waypoint> jumpConnections = new List<Waypoint>();
	public Waypoint throughConnection; // you can go through a platform here
	public Waypoint fromConnection; //you arrive from a platform here
	public Vector2 worldPosition;
	public Collider2D platform;
	LayerMask platMask = LayerMask.GetMask ("Ground");
	public bool onEdge;
	public int gCost; // Distance from starting position
	public int hCost; // distance from ending position
	// F cost is G+H cost
	public Waypoint parent;
	int heapIndex;

	public Waypoint(Vector2 worldPosition, bool isEdge){
		this.worldPosition = worldPosition;
		onEdge = isEdge;
		RaycastHit2D hit = Physics2D.Raycast (worldPosition, Vector2.down, Mathf.Infinity, platMask);
		platform = hit.collider;
	}

	public void NextNeighbour(Waypoint next, ConnectType type){
		if (!alreadyContained (next)) {
			if (type == ConnectType.Jump) {
				jumpConnections.Add (next);
				next.LastNeighbours (this, type);
			} else if (type == ConnectType.Walk) {
				neighbours.Add (next);
				next.LastNeighbours (this, type);
			} else {
				throughConnection = next;
				next.fromConnection = this;
			}
		}
	}

	public void RemoveConnection(Waypoint remove){
		//this is a little bugged, occasionally wont remove things
		jumpConnections.Remove (remove);
	}

	bool alreadyContained(Waypoint newFriend){
		foreach (Waypoint point in getNeighbours()) {
			if (newFriend.Equals (point)) {
				return true;
			}
		}
		return false;
	}

	public void LastNeighbours(Waypoint next, ConnectType type){
		if (type == ConnectType.Jump) {
			jumpConnections.Add (next);
		} else if (type == ConnectType.Walk) {
			neighbours.Add (next);
		} 
	}

	public List<Waypoint> getNeighbours(){
		List<Waypoint> returnList = new List<Waypoint>(neighbours);
		if (jumpConnections.Count != 0) {
			returnList.AddRange(jumpConnections);
		}
		if (throughConnection != null) {
			returnList.Add (throughConnection);
		}
		return returnList;
	}

	public bool IsJump(Waypoint neighbour){
		if (fromConnection != null) {
			if (neighbour == fromConnection) {
				return true;
			} 
		}
		foreach (Waypoint point in jumpConnections) {
			if (neighbour == point) {
				return true;
			}
		}
		return false;
	}
	public bool Isthrough(Waypoint neighbour){
		if (fromConnection != null) {
			if ( neighbour == fromConnection) {
				return true;
			}
		}
		return false;
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
