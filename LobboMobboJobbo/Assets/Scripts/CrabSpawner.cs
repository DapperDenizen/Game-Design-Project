using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabSpawner : MonoBehaviour {
	//THIS SCRIPT WAS BUILT AND DESIGNED TO MAKE CRABS THAT WILL KILL YOU, GET OUT OF MY FACE
	public enum CrabType {regular,large,stack}
	//variables
	//spawn variables
	public int maxCrabs;
	public int minCrabs;
	public float crabMaxStrength;
	public float timeBetweenCrabs;
	float crabStrength;
	float timeInbetweenKills;
	int[] toSpawn;
	//spawn vector variables
	Vector2 lauchTarget = Vector2.zero;
		//lines we want to spawn within
	float boxXmin;
	float boxXmax;
	float boxYmin; // dont be within boxYmin <--> boxYmax
	float boxYmax;
	int boxOffSet = 2;
		//vector generation
	public float vecMaxX;
	public float vecMinX;
	public float vecYOffset;
	//references
	int crabNumbers = 0;
	GameObject background;
	GameObject player;
	//crab prefabs
	public GameObject regularCrab; //1
	public GameObject stackCrab; //2
	public GameObject largeCrab; //3
	//spawnPoints
	List<Vector2> spawnPoints;

	//initialization
	void Start () { 
		background = GameObject.FindGameObjectWithTag ("Bg");
		player = GameObject.FindGameObjectWithTag ("Player"); //this is the guy we hate
		//set scene
		boxXmin = background.GetComponent<SpriteRenderer>().bounds.min.x;
		boxXmax = background.GetComponent<SpriteRenderer>().bounds.max.x;
		boxYmax = background.GetComponent<SpriteRenderer>().bounds.max.y;
		boxYmin = background.GetComponent<SpriteRenderer>().bounds.min.y;
		//
		//load crabs
		regularCrab = Resources.Load("Prefab/Enemys/Regular_Crab") as GameObject;
		//
		toSpawn = SplitStrength ();
		//SpawnCrab (CrabType.regular);
		Invoke("debugtest",3f);
		Invoke("debugtest",3.2f);
		Invoke("debugtest",3.3f);
	}


	void Update () {

		if (crabStrength < crabMaxStrength) {
			//Do something about it

			if(maxCrabs > crabNumbers){
				//Do it again
			}
		}


	}
	void debugtest(){
		SpawnCrab (CrabType.regular);
	}

	void SpawnCrab(CrabType type){
		//make a kelper bool and do that
		EnemyControl crabsController;
		Vector2 direction = GetVector ();
		Vector2 spawn = GetPoint (lauchTarget, direction);
		print ("im starting at " + spawn + " going at a speed of " + direction); 
		switch (type) 
		{

		case CrabType.regular:

			GameObject spawnedCrab = GameObject.Instantiate (regularCrab, spawn, Quaternion.Euler (0, 0, 0));
			crabMaxStrength += 1;
			crabsController = spawnedCrab.GetComponentInChildren<EnemyControl> ();
			crabsController.state = UnitController.State.spawning;
			Vector2 forceVec = lauchTarget - spawn;
			forceVec.x = forceVec.x * 5; 
			forceVec.y = forceVec.y * 10; 
			crabsController.rb2d.AddRelativeForce(forceVec,ForceMode2D.Impulse);
			crabNumbers++;


			break;

		}



	}

	Vector2 GetVector(){
		float vecX = 0;
		int randomSpawn = Random.Range (0, 1);
		if (randomSpawn == 0) {
			vecX = Random.Range (boxXmin-boxOffSet - vecMaxX , boxXmin -boxOffSet);
		} else {
			vecX = Random.Range (boxXmax + boxOffSet, boxXmax + boxOffSet + vecMaxX);
		}
		float vecY = Random.Range (boxYmin - vecYOffset, boxYmax + boxOffSet);
		//float vecY = Random.Range (vecMinY, vecMaxY);
		return new Vector2(vecX,vecY);
	}

	Vector2 GetPoint(Vector2 startPoint, Vector2 vector){
		Vector2 returnVec = startPoint + vector;

		return returnVec;
	}

	//this makes a list of integers of what crabs we want to spawn
	int[] SplitStrength(){
		float workingRoom = crabMaxStrength - crabMaxStrength;
		List<int> toReturn = new List<int>();
		//split up a big number
		return toReturn.ToArray ();
	}

	public void IAmDead(){
		crabStrength = crabStrength - 1;
		//toSpawn = SplitStrength ();
		Invoke("debugtest",1f);
	}


	void OnDrawGizmos() {
		Gizmos.color = Color.blue;
		Gizmos.DrawSphere (new Vector2 (boxXmin-boxOffSet - vecMaxX , 0), 0.3f);
		Gizmos.color = Color.green;
		Gizmos.DrawSphere (new Vector2 ( boxXmin -boxOffSet, 0), 0.3f);
		Gizmos.color = Color.red;
		Gizmos.DrawSphere (new Vector2 (boxXmax + boxOffSet, 0), 0.3f);
		Gizmos.color = Color.yellow;
		Gizmos.DrawSphere (new Vector2 (boxXmax + boxOffSet + vecMaxX, 0), 0.3f);
		Gizmos.color = Color.grey;
		Gizmos.DrawSphere (new Vector2 (0 , 0), 0.3f);
		//Gizmos.DrawLine(new Vector2(-vecMaxX-boxXmin,0),new Vector2(boxXmin-boxOffSet,0));
		//Gizmos.DrawLine(new Vector2(boxXmax+boxOffSet,0),new Vector2(boxXmax+vecMaxX,0));
	} //*/

}