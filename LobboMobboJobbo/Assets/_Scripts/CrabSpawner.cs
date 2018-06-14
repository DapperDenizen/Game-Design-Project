using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabSpawner : MonoBehaviour {
	//THIS SCRIPT WAS BUILT AND DESIGNED TO MAKE CRABS THAT WILL KILL YOU, GET OUT OF MY FACE
	public enum CrabType {regular,large,kelp}
	//variables
	//spawn variables
	public int maxCrabs;
	public int minCrabs;
	public float timeBetweenCrabs;
	float crabStrength;
	float timeInbetweenKills;
	int[] toSpawn;
	//spawn vector variables feel free to add new ones!
	Vector2 targetOne = new Vector2(0,5);
	Vector2 targetTwo = new Vector2(5,10);
	Vector2 targetThree = new Vector2(-5,10);
		//lines we want to spawn within
	float boxXmin;
	float boxXmax;// dont be within box X min or max
	float boxYmin;
	float boxYmax;
	int boxOffSet = 2;
		//vector generation
	public float vecXOffset;
	public float vecYOffset;
	//references
	int crabNumbers = 0;
	GameObject background;
	GameObject player;
	//crab prefabs
	public bool regCrabEnabled;
	public bool KelpCrabEnabled;
	public bool largeCrabEnabled;
	public GameObject regularCrab; 
	public GameObject kelperCrab; 
	public GameObject largeCrab; 
	//spawnPoints
	List<Vector2> spawnPoints = new List<Vector2>();

	//initialization
	void Start () { 
		spawnPoints.Add (targetOne);
		spawnPoints.Add (targetTwo);
		spawnPoints.Add (targetThree);
		background = GameObject.FindGameObjectWithTag ("Bg");
		player = GameObject.FindGameObjectWithTag ("Player"); //this is the guy we hate
		//set scene
		boxXmin = background.GetComponent<SpriteRenderer>().bounds.min.x;
		boxXmax = background.GetComponent<SpriteRenderer>().bounds.max.x;
		boxYmin = background.GetComponent<SpriteRenderer>().bounds.min.y;
		boxYmax = background.GetComponent<SpriteRenderer>().bounds.max.y;
		//
		//load crabs
		regularCrab = Resources.Load("Prefab/Enemys/Regular_Crab") as GameObject;
		//kelperCrab = Resources.Load("Prefab/Enemys/Regular_Crab") as GameObject;
		//largeCrab = Resources.Load("Prefab/Enemys/Regular_Crab") as GameObject;
		//
		//SpawnCrab (CrabType.regular);
		Invoke("spawnReg",1.9f);
		Invoke("spawnReg",2.2f);
		Invoke("pickCrab",2f);
	}


	void Update () {

	}

	void pickCrab(){
		float invokeTime = timeBetweenCrabs + Random.Range (0, 2);
		if (crabNumbers < (maxCrabs / 2)) {
			Invoke ("spawnReg", invokeTime + 0.1f);
		}
		if (maxCrabs - crabNumbers > 2 && largeCrabEnabled) {
			Invoke ("spawnLarge", invokeTime);
		}
		if (maxCrabs - crabNumbers > 3 && KelpCrabEnabled) {
			Invoke ("spawnKelp", invokeTime);
			Invoke ("spawnKelp", invokeTime +0.3f);
		}
		if (maxCrabs - crabNumbers > 1) {
			Invoke ("spawnReg", invokeTime);
		}
	
	}

	//these are used because you cant invoke and send a
	void spawnReg(){
		SpawnCrab (CrabType.regular);
	}

	void spawnKelp(){
		SpawnCrab (CrabType.regular);
	}

	void spawnLarge(){
		SpawnCrab (CrabType.regular);
	}

	//this spawns the crab
	void SpawnCrab(CrabType type){
		//make a kelper bool and do that
		int max = spawnPoints.Count-1;
		Vector2 lauchTarget = spawnPoints[Random.Range(0,max)];
		EnemyControl crabsController;
		GameObject spawnedCrab;
		Vector2 forceVec;
		Vector2 direction = GetVector ();
		Vector2 spawn = GetPoint (lauchTarget, direction);
		switch (type) 
		{

		case CrabType.regular:

			spawnedCrab = GameObject.Instantiate (regularCrab, spawn, Quaternion.Euler (0, 0, 0));
			crabsController = spawnedCrab.GetComponentInChildren<EnemyControl> ();
			crabsController.state = UnitController.State.spawning;
			forceVec = lauchTarget - spawn;
			forceVec.x = forceVec.x * 5; 
			forceVec.y = forceVec.y * 25; 
			crabsController.rb2d.AddRelativeForce(forceVec,ForceMode2D.Impulse);
			crabNumbers++;


			break;

		case CrabType.kelp:

			spawnedCrab = GameObject.Instantiate (kelperCrab, spawn, Quaternion.Euler (0, 0, 0));
			crabsController = spawnedCrab.GetComponentInChildren<EnemyControl> ();
			crabsController.state = UnitController.State.spawning;
			forceVec = lauchTarget - spawn;
			forceVec.x = forceVec.x * 5; 
			forceVec.y = forceVec.y * 20; 
			crabsController.rb2d.AddRelativeForce(forceVec,ForceMode2D.Impulse);
			crabNumbers++;


			break;

		case CrabType.large:

			spawnedCrab = GameObject.Instantiate (largeCrab, spawn, Quaternion.Euler (0, 0, 0));
			crabsController = spawnedCrab.GetComponentInChildren<EnemyControl> ();
			crabsController.state = UnitController.State.spawning;
			forceVec = lauchTarget - spawn;
			forceVec.x = forceVec.x * 5; 
			forceVec.y = forceVec.y * 20; 
			crabsController.rb2d.AddRelativeForce(forceVec,ForceMode2D.Impulse);
			crabNumbers++;


			break;

		}

	}

	//this returns a vector we will shoot the crab on
	Vector2 GetVector(){
		float vecX = 0;
		int randomSpawn = Random.Range (0, 1);
		if (randomSpawn == 0) {
			vecX = Random.Range (boxXmin-boxOffSet - vecXOffset , boxXmin -boxOffSet);
		} else {
			vecX = Random.Range (boxXmax + boxOffSet, boxXmax + boxOffSet + vecXOffset);
		}
		float vecY = Random.Range (boxYmin - boxOffSet -vecYOffset, boxYmax + boxOffSet);
		//float vecY = Random.Range (vecMinY, vecMaxY);
		return new Vector2(vecX,vecY);
	}

	//this returns the starting point for our crab to spawn
	Vector2 GetPoint(Vector2 startPoint, Vector2 vector){
		Vector2 returnVec = startPoint + vector;

		return returnVec;
	}

	//this is called when a crab goes to a better place
	public void IAmDead(){
		crabNumbers--;
		Invoke("pickCrab",1f);
	}


	/*void OnDrawGizmos() {
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