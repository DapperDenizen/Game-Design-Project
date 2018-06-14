using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    private bool attacking = false;

    private float attackTimer = 0;
    private float attackCd = 1f;
    private bool onCoolDown = false;

    public Collider2D attackTrigger;

    private void Awake()
    {
        attackTrigger.enabled = false;
    }

    public void enableAttackTrigger(){
		attackTrigger.enabled = true;
		attackTimer = attackCd;
		onCoolDown = true;
    }

	public void disableAttackTrigger(){
		onCoolDown = true;
    }

    private void Update() {
		if(onCoolDown){
			if(attackTimer>0){
				attackTimer-=Time.deltaTime;
			} else {
				onCoolDown = false;
				attackTimer = attackCd;
				attackTrigger.enabled = false;
			}
		}
    }

    /*


    if (Input.GetMouseButtonUp(0) || Input.GetMouseButtonUp(1) && !attacking)
        {
            if(!attacking){
			attacking = true;
            attackTimer = attackCd;
            attackTrigger.enabled = true;
    	}
		else {
            if(attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
                            
            }
            else
            {
                attacking = false;
                attackTrigger.enabled = false;  
            }
        }
        }

        if (attacking)
        {
            if(attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
                            
            }
            else
            {
                attacking = false;
                attackTrigger.enabled = false;  
            }
        }


    */

}
