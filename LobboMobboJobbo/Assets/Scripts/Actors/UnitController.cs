using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour {
	//
	public  enum State  {stunned,fine,spawning}; // states a unit can be in, these are typically to restrict movements
	//stats
	public float maxHealth; 
	public float stunTime;
	public float health; //dont touch this in the inspector use maxHealth to assign health
	public float jumpVel;
	public float walkSpeed;
	//references
	public Rigidbody2D rb2d;
	public LayerMask groundLayer;
	public LayerMask wallLayer;
	public Animator anim;
	public SpriteRenderer sprite;
	//variables
	public State state = State.fine; 
	public bool grounded = false;
	private float recoilTimer = 0.3f;
	int xDirection; // x direction is the current direction we are facing
	//

	//initialiser
	virtual public void Awake(){
		rb2d = GetComponent<Rigidbody2D>();
		groundLayer = LayerMask.NameToLayer ("Ground");	
		wallLayer = LayerMask.NameToLayer ("Wall");	
		anim = GetComponentInChildren<Animator> ();
		sprite =  GetComponentInChildren<SpriteRenderer>();
		health = maxHealth;
		xDirection = - Mathf.RoundToInt(sprite.transform.localScale.x);
	}


	//these are to deal with grounded functions, please ensure the walls are not on the ground layer
	void OnCollisionEnter2D (Collision2D collision)
	{
		if (collision.gameObject.layer == groundLayer) {
			HitGround ();
		}

		if(collision.gameObject.layer == wallLayer && state == State.stunned) {
			Vector3 info = new Vector3 (transform.position.x < collision.transform.position.x ? 20 : -20, 1f, 0);
			Hit (info);
		}

	}
	void OnCollisionExit2D (Collision2D collision)
	{
		if (collision.gameObject.layer == groundLayer) {
			LeaveGround ();
		}
	}

	//called when hit by weapon, this function is also used for general knockback (eg when a crab hits a player)
	//the vector 3 is a vector 2 of the knockback velocity/direction (x,y) and the damage of the weapon (z)
	virtual public void Hit(Vector3 info){
		//remove health
		health = health - info.z;
		if (health <= 0) {
			print ("Im dead");
			OnDeath ();
		}
		KnockBack (new Vector2(info.x,info.y));
		if (state == State.fine) {
			Recoiled ();
		}
	}

	//this is the stunn function, it is called when hit
	virtual public void Recoiled(){
		
		StartCoroutine (WaitForRecoil ());

	}

	//this is the knockback a unit will recieve
	virtual public void KnockBack(Vector2 vector){
		rb2d.AddRelativeForce (vector,ForceMode2D.Impulse);

	}

	IEnumerator WaitForRecoil(){
		state = State.stunned;
		yield return null;

		yield return new WaitForSecondsRealtime (stunTime);
		state = State.fine;

	}

	virtual public void OnDeath(){
	//dead
		Destroy(this.gameObject);
	}

	//this is the unit movement functions, works with the player but is kinda janky with the crabs, it deals with moving the player and the associated movement animations
	public void MoveUnit(Vector2 direction){

		if (direction.x > 0) {
			anim.SetBool ("Walking", true);
			if (xDirection > 0) {
				rotateSprite (false);
				xDirection = -1;
			}		
		} else if (direction.x < 0) {
			anim.SetBool ("Walking", true);
			if (xDirection < 0) {
				rotateSprite (true);
				xDirection = 1;
			}
		
		} else {
			anim.SetBool("Walking", false);
		}

		//Y vaules

		if(!grounded && rb2d.velocity.y < 0) {
			rb2d.AddForce(new Vector2(0,-50));
		} 



		if (state == State.fine) {
			rb2d.velocity = direction;
		}
	}

	//jumping functions
	 virtual public void HitGround(){
		grounded = true;
	}

	//this is the sprite rotation functions, i found it less of a headache to just reverse the scale of a sprite than rotate it!
	public void rotateSprite(bool side){
		if (side) {
			//xDirection = 1;
			sprite.transform.localScale = new Vector3 (-1, 1, 1);
		} else {
			//xDirection = -1;
			sprite.transform.localScale = new Vector3 (1, 1, 1);
		}
	}

	public void LeaveGround(){
		grounded = false;
	}
		
}
