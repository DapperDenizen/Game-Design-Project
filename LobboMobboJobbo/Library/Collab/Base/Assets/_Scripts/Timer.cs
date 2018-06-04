using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class Timer : MonoBehaviour {

    private float timer = 90;
    public TextMeshProUGUI myTimer;

    private int levelToLoad;

    // Use this for initialization
    void Start () {
        myTimer = GetComponent<TextMeshProUGUI>();
        levelToLoad = SceneManager.GetActiveScene().buildIndex;
    }
	
	// Update is called once per frame
	void Update () {
        timer -= Time.deltaTime;
        myTimer.text = timer.ToString("f0");
        print(myTimer.text);

        LoadScene();
	}

    void LoadScene()
    {
        if (timer <= 0)
        {
            SceneManager.LoadScene(levelToLoad + 1);
        }
    }
}
