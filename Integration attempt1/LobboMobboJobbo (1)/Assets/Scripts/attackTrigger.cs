﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackTrigger : MonoBehaviour {

    public float power = 5f;
	private float powerTimer = 0.5f;

    void Update(){
    	swordSlice();
    }

    private void swordSlice(){
		if (Input.GetMouseButton(0)) {
			if(power<20){
				power += power*0.02f;
			}	
		} else {
			resetPower();
		}
    }

    private void resetPower(){
		powerTimer -= Time.deltaTime;
		if(powerTimer < 0){
			powerTimer = 1f;
			power = 5;
	    }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
		if (other.gameObject.tag == "HitBox")
        {
            //Destroy(other.gameObject);   
            //other.SendMessageUpwards("Damage", damage);
			other.SendMessageUpwards("Hit", power);
        }
    }
}
