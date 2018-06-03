using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabController : MonoBehaviour {
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
	int xMin;
	int yMin;
	//references
	List<GameObject> crabs;
	//crab prefabs
	public GameObject regularCrab; //1
	public GameObject stackCrab; //2
	public GameObject largeCrab; //3
	// Use this for initialization
	void Start () {
		yMin = Screen.height / 2;
		xMin = Screen.width / 2; 
		toSpawn = SplitStrength ();
	}
	

	void FixedUpdate () {

		if (crabStrength < crabMaxStrength) {
		//Do something about it
			if(maxCrabs > crabs.Count){
				//Do it again
			}
		}


	}

	void SpawnCrab(CrabType type){
		//make a kelper bool and do that
		EnemyControl temp;
		switch (type) 
		{

		case CrabType.regular:

			crabs.Add (GameObject.Instantiate (regularCrab, GetVector (), Quaternion.Euler (0, 0, 0)));
			crabMaxStrength += 1;
			temp = crabs [crabs.Count - 1].GetComponent<EnemyControl> ();
			temp.state = UnitController.State.spawning;
			temp.rb2d.velocity = -temp.transform.position; 


			break;

		case CrabType.large:

			crabs.Add (GameObject.Instantiate (largeCrab, GetVector (),Quaternion.Euler(0, 0, 0)));
			crabMaxStrength += 3;
			temp = crabs [crabs.Count - 1].GetComponent<EnemyControl> ();
			temp.state = UnitController.State.spawning;
			temp.rb2d.velocity = -temp.transform.position; 


			break;

		case CrabType.stack:
			//could do this better
			crabs.Add (GameObject.Instantiate( stackCrab, GetVector (),Quaternion.Euler(0, 0, 0)));
			crabMaxStrength += 2;
			temp = crabs [crabs.Count - 1].GetComponentInChildren<EnemyControl> ();
			temp.state = UnitController.State.spawning;
			temp.rb2d.velocity = -temp.transform.position; 


			break;
		}
			
	
	
	}

	Vector2 GetVector(){
		float vecX = Random.Range (xMin, xMin * 2);
		float vecY = Random.Range (yMin, yMin * 2);

		return new Vector2(vecX,vecY);

	}

	int[] SplitStrength(){
		float workingRoom = crabMaxStrength - crabMaxStrength;
		List<int> toReturn = new List<int>();
		//add a big one
		if (workingRoom - 3 > 0) {
			toReturn.Add (3);
			workingRoom -= 3;
		} else if (workingRoom - 2 > 0) {
			toReturn.Add (2);
			workingRoom -= 2;
		}
		while (workingRoom > 0) {
			toReturn.Add (1);
			workingRoom -= 1;
		}

		return toReturn.ToArray ();
	}

	public void IAmDead(GameObject crab){
		crabStrength = crabStrength - crab.GetComponent<EnemyControl> ().strength;
		crabs.Remove (crab);
		toSpawn = SplitStrength ();
	}
		
}
