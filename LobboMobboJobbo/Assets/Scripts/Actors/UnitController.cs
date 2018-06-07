using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour {
	//
	public  enum State  {stunned,fine,spawning};
	//stats
	public float maxHealth;
	public float stunTime;
	public float health;
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
	public float fallMulti = 3;
	public float gravityBase;
	int xDirection; // x direction is the current direction we are facing
	//

	//initialiser
	virtual public void Start(){
		rb2d = GetComponent<Rigidbody2D>();
		groundLayer = LayerMask.NameToLayer ("Ground");	
		wallLayer = LayerMask.NameToLayer ("Wall");	
		anim = GetComponentInChildren<Animator> ();
		sprite =  GetComponentInChildren<SpriteRenderer>();
		health = maxHealth;
		xDirection = - Mathf.RoundToInt(sprite.transform.localScale.x);
		//gravityBase = rb2d.gravityScale;
	}



	void OnCollisionExit2D (Collision2D collision)
	{
		if (collision.gameObject.layer == groundLayer) {
			LeaveGround ();
		}

		if(collision.gameObject.layer == wallLayer && state == State.stunned) {
			Vector3 info = new Vector3 (transform.position.x < collision.transform.position.x ? 20 : -20, 1f, 0);
			collision.gameObject.SendMessageUpwards ("Hit", info);
		}
		
	}

	//called when hit by weapon
	//the vector 3 is a vector 2 of the knockback velocity/direction (x,y) and the damage of the weapon (z)
	virtual public void Hit(Vector3 info){
		print ("im hit");
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

	virtual public void Recoiled(){
		
		StartCoroutine (WaitForRecoil ());

	}


	virtual public void KnockBack(Vector2 vector){
		rb2d.AddRelativeForce (vector,ForceMode2D.Impulse);

		//rb2d.velocity = vector;

	}

	IEnumerator WaitForRecoil(){
		state = State.stunned;
		yield return null;

		yield return new WaitForSecondsRealtime (stunTime);
		state = State.fine;
		//print ("end");

	}

	virtual public void OnDeath(){
	//dead
		Destroy(this.gameObject);
	}

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
		rb2d.gravityScale = gravityBase;
	}

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
