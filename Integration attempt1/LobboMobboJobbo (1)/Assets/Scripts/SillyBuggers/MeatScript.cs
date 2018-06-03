using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatScript : MonoBehaviour {
	public Vector2 explosionVector;
	private Rigidbody2D rb2d;
	void Start(){
		rb2d = GetComponent<Rigidbody2D>();
		rb2d.velocity = explosionVector;
		Invoke ("TimeOut", 15f);
	}

	//should we add some squelching sounds when it hits the ground for the first time?
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player") {
			//add +1 to game controller or w/e

			//play some kind of sound

			// & die
			GameObject.Destroy (this.gameObject);
		}
	}
	void TimeOut(){
	
		//play some kind of sound

		// & die
		GameObject.Destroy (this.gameObject);
	
	}
}
