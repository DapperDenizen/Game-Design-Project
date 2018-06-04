using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackTrigger : MonoBehaviour {

    public float power = 5f;
    public float force = 5f;
	private float powerTimer = 1f;
	public Vector3 attack = new Vector3(); //using a vector cause its easier

	//
	float baseX = 20;//5 w/vel =
	float baseY = 10;//5 w/vel =
	//

	public ParticleSystem attackGlow; 

	void Start(){
		attackGlow = GetComponent<ParticleSystem>();

		attack.x = baseX;
		attack.y = baseY; 
		attack.z = power;
	}

    void Update(){
    	swordSlice();
    }

    private void swordSlice(){
		if (Input.GetMouseButton(1)) {

			ParticleEffects();
			attack.x = 5;

			if(force<40){
				force += force*0.03f;
				attack.y = force;
			} 
			if(power<10){
				power += power*0.02f;
				attack.z = power;
			}	
		} 
		else if (Input.GetMouseButton(0)) {
			attack.x = baseX;
			attack.y = baseY; 
			attack.z = power;

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

		if(force < 40f) {
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
			attack.z = power;
			attack.y =  force;
	    }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
		if (other.gameObject.tag == "HitBox")
        {
			int direction = transform.position.x < other.transform.position.x ? 1 : -1;
			Vector3 toSend = attack;
			toSend.x = toSend.x * direction;
			print ("this is it "+ toSend);
			other.SendMessageUpwards("Hit", toSend);
        }
    }

}
