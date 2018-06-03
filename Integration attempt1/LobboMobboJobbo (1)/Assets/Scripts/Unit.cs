using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Unit : MonoBehaviour {
	
	//stats
	public int health;
	public float jumpVel;
	public float walkSpeed;
	//references
	public GameObject weapon;
	public Rigidbody2D rb2d;
	public LayerMask groundLayer;
	//bools
	public bool grounded = false;
	//


	//initialiser
	virtual public void Start(){
		rb2d = GetComponent<Rigidbody2D>();
		groundLayer = LayerMask.NameToLayer ("Ground");	
	}

	//called when hit by weapon
	virtual public void OnHurt(int damage){
		//remove health
		health = health - damage;
		if (health <= 0) {
			print ("Im dead");
		}
	}
		
	//primary attack
	public void AttackPrimary(Vector2 target){
		//call weapon attack
		weapon.GetComponent<WeaponController> ().Attack (target);
	}

	public void MoveUnit(Vector2 direction){
		if (!grounded && direction.y < 0) {
			rb2d.gravityScale = 2.5f;
		}
		rb2d.velocity = direction;
	}

	//jumping functions
	public void HitGround(){
		grounded = true;
		rb2d.gravityScale = 1f;
	}

	public void LeaveGround(){
		grounded = false;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.layer == groundLayer) {
			HitGround ();
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.layer == groundLayer) {
			LeaveGround ();
		}
	}
}
