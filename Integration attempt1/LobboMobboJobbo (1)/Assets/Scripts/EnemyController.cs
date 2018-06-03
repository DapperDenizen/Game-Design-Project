using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	private Transform target;
	public float speed;

	private Animator anim;
	private SpriteRenderer crab;
	private Rigidbody2D rigidbody;
	private bool isHit = false;
	private float recoilTimer = 2f;
	public GameObject crabMeat;

	public float health = 50;
	public float damage = 15;
	// Use this for initialization
	void Start () {
		target = GameObject.Find("LobsterRig").transform;
		crabMeat = Resources.Load("Prefab/CrabMeat")as GameObject;
		anim = GetComponent<Animator>();
		crab = GetComponentInChildren<SpriteRenderer>();
		rigidbody = GetComponent<Rigidbody2D>();	
	}
	
	// Update is called once per frame
	void Update () {
		Walk();
		Attack();
		CheckFlip();
		recoiled();
	}

	public void Hit(float power){
		isHit = true;
		print(power);
		Vector3 thrust = new Vector3 (transform.position.x < target.position.x ? -power : power, 10f, 0.0f);
		rigidbody.velocity = thrust;
		print("Power: " + power);
		health -=power;
		checkHealth();
	}

	private void checkHealth(){
		if(health<=0){
			GameObject crabMeatObject = Instantiate(crabMeat, transform.position, Quaternion.Euler(0,0,0));
			crabMeatObject.GetComponent<Rigidbody2D>().velocity = new Vector3(0,10,0);
			Destroy(this.gameObject);
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

		if(shouldFollow() && !isHit){
			Vector3 movement = new Vector3 (transform.position.x < target.position.x ? 2 : -2, 0.0f, 0.0f);
        	rigidbody.velocity = movement * speed;
		} 

		//rigidbody.velocity = Vector3.MoveTowards(transform.position, target.position, step);
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
}
