using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabSpawner : MonoBehaviour {

	public float timer;
	private GameObject crab;
	private Transform target;

	// Use this for initialization
	void Start () {
		timer = Random.Range(5f,20f);
		crab = Resources.Load("Prefab/Enemy")as GameObject;
		target = GameObject.Find("LobsterRig").transform;
	}
	
	// Update is called once per frame
	void Update () {
		Timer();
	}

	private void Timer(){
		timer -= Time.deltaTime;
		if(timer < 0){
			timer = Random.Range(1f,10f);;
			Spawner();
	    }
	}

	private void Spawner(){
		GameObject crabObject = Instantiate(crab, transform.position, Quaternion.Euler(0,0,0));
		crabObject.transform.GetChild(0).GetComponent<Rigidbody2D>().AddForce(new Vector2(10*-getDirection(),Random.Range(5,15)), ForceMode2D.Impulse);
	}

	private int getDirection(){
		return transform.position.x < target.position.x ? -1 : 1;
    }
}
