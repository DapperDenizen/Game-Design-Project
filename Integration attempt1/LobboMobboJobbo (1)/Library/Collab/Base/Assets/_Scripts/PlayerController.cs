using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Physics {

	public float jumpTakeOffSpeed = 3;
    public float maxSpeed = 7;
    public int numThorns = 8;

    public GameObject weapon;
	public Animator animator;
    public GameObject thornPrefab;


    public SpriteRenderer lobster;
    private Vector3 center;
    

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator>();
		lobster = GetComponentInChildren<SpriteRenderer>();
        

	}

    private bool isWalking(){
		return (Input.GetAxis("Horizontal") != 0);
	}

	protected override void ComputeVelocity()
    {
    	Melee();
        Thorns();
        CheckFlip();
		animator.SetBool("Walking", isWalking());
        Vector2 move = Vector2.zero;
        move.x = Input.GetAxis("Horizontal");

        if (Input.GetKey(KeyCode.Space) && grounded)
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
            */
        }
        targetVelocity = move * maxSpeed;
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
			animator.SetTrigger("PullBack");
		}
		if (Input.GetMouseButtonUp(0)) {
			animator.SetTrigger("Stab");
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

}