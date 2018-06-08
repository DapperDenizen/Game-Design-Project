using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

	// self destruct in 7 ish seconds
	void Awake() {
		Invoke ("SelfDestruct", 7f);
	}

	//adds crab meat to playar but also a very small amount of health
	void OnCollisionEnter2D(Collision2D col){
		if(col.gameObject.tag == "Player"){
			col.gameObject.GetComponent<PlayerControl> ().crabMeat++;
			col.gameObject.GetComponent<PlayerControl> ().health++; //crabs stronk
			SelfDestruct ();
		}
	}

	void SelfDestruct(){
	Destroy(this.gameObject);
	}

}
