using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreCounter : MonoBehaviour {
	public Text mytext; 
	int scoreInt;
	// Use this for initialization
	void Start () {
		scoreInt = PlayerPrefs.GetInt ("crabMeat");
		mytext.text = "Crab Meat: " + scoreInt;
	}

}
