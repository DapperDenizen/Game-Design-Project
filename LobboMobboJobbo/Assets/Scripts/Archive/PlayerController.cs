using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {


    public float jumpPower;
    public float speed = 7;
    public float acceleration = 30;

    public float maxHealth = 100;
    public float currentHealth;
    public int numThorns = 8;

    public GameObject weapon;
	public Animator animator;
    public GameObject thornPrefab;

    public SpriteRenderer lobster;
    private Vector3 center;

    private bool isHit = false;
    private float recoilTimer = 0.3f;

	private Rigidbody2D rb2d;
	private bool grounded = false;
	private bool inAnimation = false;
	private float oldMoveX;
	public bool canDoubleJump = false;
	public int crabMeat = 0;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    // Use this for initialization
    void Start () {
		animator = GetComponent<Animator>();
		lobster = GetComponentInChildren<SpriteRenderer>();
        rb2d = GetComponent<Rigidbody2D>();
		oldMoveX = Input.GetAxis("Horizontal");
	}

    private bool isWalking(){
		return (Input.GetAxis("Horizontal") != 0);
	}

	void Update()
    {
    	Recoiled();
    	Melee();
        Thorns();
        CheckFlip();
        if(!isHit){
			Movement();
        }
    }

	public void OnCollisionEnter2D (Collision2D col) {
 		grounded = true;
		canDoubleJump = true;
		if(col.gameObject.tag == "Walls"){
			Rebound();
		}
		if(col.gameObject.tag == "Collectable"){
			crabMeat++;
		}
    }

	public void OnCollisionExit2D (Collision2D col) {
 		grounded = false;
    }


    private void Rebound(){
    	rb2d.AddForce(new Vector2(800*getDirection(),50));
    }

    public void Hit(GameObject enemy){
    	isHit = true;
		Vector3 thrust = new Vector3 (transform.position.x < enemy.transform.position.x ? -5 : 5, 3f, 0.0f);
		currentHealth -= enemy.GetComponent<EnemyController>().damage;
		CheckHealth();
		rb2d.velocity = thrust;
    }

    private void CheckHealth(){
		if(currentHealth<=0){
    		Destroy(this.gameObject);
    	}
    }

	void Recoiled(){
		if(isHit){
			recoilTimer -= Time.deltaTime;
			if(recoilTimer < 0){
				recoilTimer = 0.3f;
	            isHit = false;
	         }
		}
	}

	private void Movement(){
		//Set Animation
		animator.SetBool("Walking", isWalking());

		//Obtain horizontal input
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");

		//------- targetVelocity = move * maxSpeed; -------// 
		//------- rb2d.velocity = move * maxSpeed; -------//

		if((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D)) && grounded){
			rb2d.velocity *= 0;
		} 

		//Apply force to player as they move
		if(move.x!=0){
			rb2d.AddRelativeForce(move* acceleration);
		} else {
			rb2d.velocity *= 0.99f;
		}

		if (move.x > oldMoveX && rb2d.velocity.magnitude<1){
			rb2d.velocity = rb2d.velocity.normalized * 3;
		}

		oldMoveX = Input.GetAxis("Horizontal");

		//-------------------------------------------------------

        //Apply force to player as they fall
		if(!grounded && rb2d.velocity.y < 0) {
			rb2d.AddForce(new Vector2(0,-50));
			rb2d.sharedMaterial.friction = 0.001f;
        } 

		if(grounded && rb2d.sharedMaterial.friction < 0.4){
			rb2d.sharedMaterial.friction *= 1.1f;
		}

        //Apply force as player jumps
        if (Input.GetKeyDown(KeyCode.Space) && grounded) {
            rb2d.AddForce(new Vector2(0,jumpPower*100));
        }
        
        else if (Input.GetKeyDown(KeyCode.Space) && !grounded && canDoubleJump) {
			rb2d.velocity = new Vector2(rb2d.velocity.x, 0);
			rb2d.AddForce(new Vector2(0,jumpPower*100));
            canDoubleJump = false;
        }

        //Apply force to fall fast
        if (Input.GetKey(KeyCode.S) && !grounded) {
			//rb2d.velocity.y -= 0.4f;
		}

	}

    private void CheckFlip()
    {
		if(Input.GetAxis("Horizontal") < 0) {
			lobster.transform.rotation = Quaternion.Euler(0,180, 0);
		}
		if(Input.GetAxis("Horizontal") > 0) {
			lobster.transform.rotation = Quaternion.Euler(0,0, 0);
		}
    }

	void AttackPrimary(Vector3 lookingAt){
		weapon.GetComponent<WeaponController> ().Attack (lookingAt);
	}

	void Melee(){
		if (Input.GetMouseButtonDown(0) && !inAnimation) {
			animator.SetTrigger("SliceDown");
		}

		else if (Input.GetMouseButtonDown(1)) {
			animator.SetTrigger("UpperCutPullBack");
			inAnimation = true;
		}

		else if (Input.GetMouseButtonUp(1)) {
			animator.SetTrigger("UpperCut");
			inAnimation = false;
		}

	}

    void Thorns()
    {
       
        int increment = -270;
        Transform lobsterPos =this.transform;

        GameObject[] thornsArr;
        

        if (Input.GetKeyUp(KeyCode.E))
        {
            thornsArr = new GameObject[numThorns];
            for (int i = 0; i < numThorns; i++)
            {

                GameObject thornObject = Instantiate(thornPrefab, transform.position, Quaternion.Euler(0,0,0));

                thornsArr[i] = thornObject;                
                increment = increment - (360/numThorns);
				thornObject.GetComponent<Thorn>().init(increment);
            }
            
            //Destroy(cube);
        }
    }

    private int getDirection(){
		return Input.GetAxis("Horizontal") >0 ? -1 : 1;
    }

}