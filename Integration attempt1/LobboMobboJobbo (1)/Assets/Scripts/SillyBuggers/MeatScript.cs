using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeatScript : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "HitBox") {		//add +1 to game controller or w/e

			// & die
			GameObject.Destroy (this);
		}
	}
}
