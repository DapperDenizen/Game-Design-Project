using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackTrigger : MonoBehaviour {

    public float power = 5f;
    public float force = 5f;
	private float powerTimer = 1f;
	public float[] attack = new float[3];

	public ParticleSystem attackGlow; 

	void Start(){
		attackGlow = GetComponent<ParticleSystem>();

		attack[0] = 0;
		attack[1] = power;
		attack[2] = force;
	}

    void Update(){
    	swordSlice();
    }

    private void swordSlice(){
		if (Input.GetMouseButton(1)) {

			ParticleEffects();
			attack[0] = 1;

			if(force<20){
				force += force*0.02f;
				attack[2] = force;
			} 
			if(power<10){
				power += power*0.02f;
				attack[1] = 0;
			}	
		} 
		else if (Input.GetMouseButton(0)) {
			attack[0] = 0;
			attack[1] = 5;
			attack[2] = 20;

		} else {
			resetPower();
			resetParticleEffects();
		}
    }

    private void resetParticleEffects(){
		var main = attackGlow.main;
		var shape = attackGlow.shape;
		var noise = attackGlow.noise;
		var emission = attackGlow.emission;

		main.startColor = new Color(1f, 1f, 1f, 0.5f);
		emission.rateOverTime = 40;
		shape.angle=6.25f;
		noise.enabled = false;
    }

	private void ParticleEffects() {
		var main = attackGlow.main;
		var shape = attackGlow.shape;
		var noise = attackGlow.noise;
		var emission = attackGlow.emission;

		if(force < 20f) {
			main.startColor = new Color(240f/255f, 251f/255f, 48f/255f, 0.5f);
		} else {
			emission.rateOverTime = 80;
			shape.radius = 0.01f;
			noise.enabled = true;
			main.startColor = new Color(169f/255f,33f/255f,30f/255f, 0.5f);
		}

		noise.enabled = true;
	}

    private void resetPower(){
		powerTimer -= Time.deltaTime;
		if(powerTimer < 0){
			powerTimer = 1f;
			power = 5;
			force = 5;
			attack[1] = power;
			attack[2] = force;
	    }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
		if (other.gameObject.tag == "HitBox")
        {
			other.SendMessageUpwards("Hit", attack);
        }
    }

}
