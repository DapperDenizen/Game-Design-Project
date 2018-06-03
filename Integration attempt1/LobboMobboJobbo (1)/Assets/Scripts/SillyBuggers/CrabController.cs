using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabController : MonoBehaviour {
	//THIS SCRIPT WAS BUILT AND DESIGNED TO MAKE CRABS THAT WILL KILL YOU, GET OUT OF MY FACE
	public enum CrabType {regular,large,stack}
	//variables
	public int maxCrabs;
	public int minCrabs;
	public float crabMaxStrength;
	public float timeBetweenCrabs;
	float crabStrength;
	float timeInbetweenKills;
	//references
	List<GameObject> crabs;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void SpawnCrab(CrabType type){
	
	
	
	
	}

	public void IAmDead(GameObject crab){
		crabStrength = crabStrength - crab.GetComponent<EnemyControl> ().strength;
		crabs.Remove (crab);
		
	}
		
}
