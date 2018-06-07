using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour {

	// Use this for initialization
	void Awake() {
		Invoke ("SelfDestruct", 7f);
	}


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
