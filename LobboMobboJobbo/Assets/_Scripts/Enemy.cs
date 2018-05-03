using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Unit {

	//variables

	//references
	GameObject player;

	//override means i am using this Start() not the parent(unit)'s start, but calling base.start() means i call it as well
	override public void Start(){
		base.Start ();
		player = GameObject.FindGameObjectWithTag ("Player"); //this is the guy we hate
	}



}
