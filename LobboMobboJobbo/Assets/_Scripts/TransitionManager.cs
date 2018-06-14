using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class TransitionManager : MonoBehaviour {

	int nextLevel;
	// Use this for initialization
	void Start () {
		nextLevel = PlayerPrefs.GetInt ("level");
		if (nextLevel % 2 == 0) {
			Invoke ("LoadNext", 1f);
		}
	}
	
	public void LoadNext(){

		if (nextLevel == 7) {
		//load boss! 

		}else if (nextLevel == 8) {
			//load end

		}

		//this can either be loaded by the int but it requires combat levels to be 1-4/5 and the transition scene and sushi shop to be 6+7

		if (nextLevel % 2 == 0) {
			string levelTo = "Level " +1+ nextLevel/2;
			SceneManager.LoadScene(levelTo);
			//load combat
		}else{
			//load sushi shop
			SceneManager.LoadScene("SushiShop");
		}



	}
}
