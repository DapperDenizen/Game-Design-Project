using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Physics {

    public bool canDoubleJump = false;

    float xVel = 0; // input of X
    float yVel = 0; // input of Y
    float yChange = 0;// yVel+ current velocity

    public float jumpPower;
    public float maxSpeed = 7;

    public float maxHealth = 100;
    public float currentHealth;
    public int numThorns = 8;

    public GameObject weapon;
	public Animator animator;
    public GameObject thornPrefab;
    //public Rigidbody2D rb2d;


    public SpriteRenderer lobster;
    private Vector3 center;
    private bool isHit = false;
    private float recoilTimer = 0.3f;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    // Use this for initialization
    void Start () {
		animator = GetComponent<Animator>();
		lobster = GetComponentInChildren<SpriteRenderer>();
        //rb2d = GetComponent<Rigidbody2D>();
        

	}

    private bool isWalking(){
		return (Input.GetAxis("Horizontal") != 0);
	}

	protected override void ComputeVelocity()
    {
    	Recoiled();
    	Melee();
        Thorns();
        CheckFlip();
        if(!isHit){
			Movement();
        }
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
		animator.SetBool("Walking", isWalking());
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");
        //targetVelocity = move * maxSpeed;
        rb2d.velocity = move * maxSpeed;


        /*
		if(Input.GetAxis("Horizontal")!=0){
			if(rb2d.velocity.x < 5 && rb2d.velocity.x > -5){
				if(rb2d.velocity.x <1){
        			rb2d.velocity = move * 7;
        		}
			rb2d.AddForce(move * 20);
 			}
        } else if (rb2d.velocity.x != 0){
        	rb2d.AddForce(rb2d.velocity * -2);
        }
 		*/



        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            velocity.y = jumpPower;
            canDoubleJump = true;
        }
        
        if (Input.GetKeyDown(KeyCode.Space) && !grounded && canDoubleJump)
        {
            velocity.y = 0;
            velocity.y = jumpPower;
            canDoubleJump = false;
        }

        //fast fall
        if (Input.GetKey(KeyCode.S) && !grounded)
        {
            velocity.y -= 0.4f;

        }

        yChange = yVel + rb2d.velocity.y;
        yVel = 0;

        /*if (Input.GetKey(KeyCode.Space) && grounded)
        {
			animator.SetTrigger("Crouch");
			if(jumpTakeOffSpeed < 20){
				jumpTakeOffSpeed = jumpTakeOffSpeed + (jumpTakeOffSpeed * 0.02f);
			}
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
			velocity.y = jumpTakeOffSpeed;
			jumpTakeOffSpeed = 3;
			//animator.SetTrigger("Jump");
			/*
            if (velocity.y > 0)
                velocity.y = velocity.y * .5f;
            
        }*/
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

		if (Input.GetMouseButtonDown(0)) {
			animator.SetTrigger("SliceDown");
		}

		if (Input.GetMouseButtonDown(1)) {
			animator.SetTrigger("UpperCutPullBack");
		}

		if (Input.GetMouseButtonUp(1)) {
			animator.SetTrigger("UpperCut");
		}

		/*
		if (Input.GetMouseButtonDown(0)) {
			animator.SetTrigger("SlicePullBack");
		}
		if (Input.GetMouseButtonUp(0)) {
			animator.SetTrigger("SliceDown");
		}
        */   
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

    void Die()
    {

    }

}