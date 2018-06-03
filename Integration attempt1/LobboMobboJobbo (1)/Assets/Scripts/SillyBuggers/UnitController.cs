using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitController : MonoBehaviour {

	//stats
	public float maxHealth;
	private float health;
	public float jumpVel;
	public float walkSpeed;
	//references
	public Rigidbody2D rb2d;
	public LayerMask groundLayer;
	public Collider2D col;
	public Animator anim;
	public SpriteRenderer sprite;
	//variables
	public bool grounded = false;
	private float recoilTimer = 0.3f;
	int xDirection; // x direction is the current direction we are facing
	//

	//initialiser
	virtual public void Start(){
		rb2d = GetComponent<Rigidbody2D>();
		col = GetComponent<Collider2D>();
		groundLayer = LayerMask.NameToLayer ("Ground");	
		anim = GetComponentInChildren<Animator> ();
		sprite =  GetComponentInChildren<SpriteRenderer>();
		health = maxHealth;
		xDirection = Mathf.RoundToInt(sprite.transform.localScale.x);
	}


	void OnCollisionEnter2D (Collision2D collision)
	{
		
		if (collision.gameObject.layer == groundLayer) {
			HitGround ();
		}
	}

	void OnCollisionExit2D (Collision2D collision)
	{
		if (collision.gameObject.layer == groundLayer) {
			LeaveGround ();
		}
		
	}
	//called when hit by weapon
	virtual public void Hit(float damage){
		//knockback (inplement later) -> either get the position of the trigger or pass through the knockback

		//remove health
		health = health - damage;
		if (health <= 0) {
			print ("Im dead");
			OnDeath ();
		}
		Recoiled ();
	}

	public void Recoiled(){
		
		StartCoroutine (WaitForRecoil ());

	}

	IEnumerator WaitForRecoil(){

		yield return null;

		yield return new WaitForSecondsRealtime (0.2f);

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

		if (!grounded && direction.y < 0) {
			rb2d.gravityScale = 3f;
		}
		rb2d.velocity = direction;
	}

	//jumping functions
	 virtual public void HitGround(){
		grounded = true;
		rb2d.gravityScale = 1f;
	}

	public void rotateSprite(bool side){
		if (side) {
			//xDirection = 1;
			sprite.transform.localScale = new Vector3 (1, 1, 1);
		} else {
			//xDirection = -1;
			sprite.transform.localScale = new Vector3 (-1, 1, 1);
		}
	}

	public void LeaveGround(){
		grounded = false;
	}
		
}
