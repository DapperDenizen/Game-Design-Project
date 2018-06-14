using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	private Transform target;
	public float speed;
	public float acceleration;

	private Animator anim;
	private SpriteRenderer crab;
	private Rigidbody2D rigidbody;
	private bool isHit = true;
	private float recoilTimer = 2f;
	public GameObject crabMeat;
	public GameObject[] bodyParts = new GameObject[5];
	private GameObject blood;

	public bool grounded = false;
	public float health = 50;
	public float damage = 15;
	// Use this for initialization
	void Start () {
		target = GameObject.Find("LobsterRig").transform;
		crabMeat = Resources.Load("Prefab/CrabMeat")as GameObject;
		blood = Resources.Load("Prefab/Blood")as GameObject;
		for(int x = 0; x<bodyParts.Length; x++){
			bodyParts[x] = Resources.Load("Prefab/CrabPart" + (x+1))as GameObject;
		}
		anim = GetComponent<Animator>();
		crab = GetComponentInChildren<SpriteRenderer>();
		rigidbody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		recoiled();
		Walk();
		Attack();
		CheckFlip();
		CheckForce();
	}

	private void CheckForce() {
		if(rigidbody.velocity.magnitude>35){
			StartCoroutine(selfDestruct());
		}
	}

	public void OnCollisionEnter2D (Collision2D col) {
		if(col.gameObject.tag != "Player" || col.gameObject.tag != "Weapon"){
			grounded = true;
		}
		Rebound(col.gameObject.tag);
    }

	public void OnCollisionExit2D (Collision2D col) {
		if(col.gameObject.tag == "Ground" || col.gameObject.tag == "Walls"){
			grounded = false;
		}
    }

	private void Rebound(string tag){
		switch(tag) {
			case "Walls":
				rigidbody.AddForce(new Vector2(800*-getDirection(),200));
				break;
			default:
				break;
		}
    	
    }

	public void Hit(float[] attack){
		isHit = true;
		recoilTimer = 2f;

		//Vector2 thrust = new Vector2 (transform.position.x < target.position.x ? -power : power, 4f);
		if(attack[0] == 0){
			rigidbody.AddRelativeForce(new Vector2(20*getDirection(),10), ForceMode2D.Impulse);
		} 
		if (attack[0] == 1){
			rigidbody.AddRelativeForce(new Vector2(0,attack[2]), ForceMode2D.Impulse);
		}
		health -=attack[1];
		checkHealth();
	}

	private void checkHealth(){
		if(health<=0){
			Die(1);
		}
	}

	private void Die(int meatAmount){
		meatSpawner(meatAmount);
		GameObject bloodObject = Instantiate(blood, transform.position, Quaternion.Euler(0,0,0));
		Destroy(bloodObject,5);
		Destroy(this.gameObject);
	}

	IEnumerator selfDestruct(){
	     //play your sound
	     yield return new WaitForSeconds(0.5f); //waits 3 seconds
		 Explode(); //this will work after 3 seconds.
 	}

 	private void meatSpawner(int amount){
		for(int x=0; x<amount; x++){
			GameObject crabMeatObject = Instantiate(crabMeat, transform.position, Quaternion.Euler(0,0,0));
			crabMeatObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0,5), ForceMode2D.Impulse);
		}
 	}

	private void OnTriggerEnter2D(Collider2D other)
    {
		if (other.gameObject.tag == "HitBox")
        {
            //Destroy(other.gameObject);   
            //other.SendMessageUpwards("Damage", damage);
			other.SendMessageUpwards("Hit", gameObject);
        }
    }

	void recoiled(){
		if(isHit){
			recoilTimer -= Time.deltaTime;
			if(recoilTimer < 0){
				recoilTimer = 2f;
	            isHit = false;
	         }
		}
	}

	void Walk(){
		//float step = speed * Time.deltaTime;
		anim.SetBool("Walking", shouldFollow());

		if(shouldFollow() && !isHit && grounded){
			Vector3 movement = new Vector3 (transform.position.x < target.position.x ? 2 : -2, 0.0f, 0.0f);

			rigidbody.AddForce(movement * acceleration);
			if (rigidbody.velocity.magnitude>speed){
				rigidbody.velocity = rigidbody.velocity.normalized * speed;
			}

		} 
		//Apply force to player as they fall
		if(!grounded && rigidbody.velocity.y < 0) {
			rigidbody.AddForce(new Vector2(0,-25));
        }
	}

	void Attack(){
		anim.SetBool("Attacking", (getDistance()<=4f));
	}

	private float getDistance(){
		return Vector3.Distance(transform.position, target.position);
	}

	private bool shouldFollow(){
		float distance = (transform.position.x - target.position.x);
		distance = distance < 0 ? distance*-1: distance*1;  
		return distance>=0.5f;
	} 

	private void CheckFlip()
    {
		crab.transform.rotation = Quaternion.Euler(0, transform.position.x < target.position.x ? 180 : 0, 0);
    }

	private int getDirection(){
		return transform.position.x < target.position.x ? -1 : 1;
    }

    private void Explode(){
        int increment = -270;
        for (int i = 0; i < bodyParts.Length; i++) {
            GameObject bodyPart = Instantiate(bodyParts[i], transform.position, Quaternion.Euler(0,0,0));              
            increment = increment - (360/bodyParts.Length);
			bodyPart.GetComponent<Explode>().init(increment);
        }
        Die(3);
    }


}
