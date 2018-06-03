using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

	public Animator animator;
	public float speed = 10f;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetAxis("Horizontal") != 0) {
			animator.SetBool("Walking", true);
			if(Input.GetAxis("Horizontal") < 0) {
				transform.rotation = Quaternion.Euler(0,180, 0);
				transform.Translate(Vector3.right * Time.deltaTime * speed);
			}
			if(Input.GetAxis("Horizontal") > 0) {
				transform.rotation = Quaternion.Euler(0,0, 0);
				transform.Translate(Vector3.right * Time.deltaTime * speed);
			}
			else {
				animator.SetBool("Walking", false);
			}
		}
	}
}
