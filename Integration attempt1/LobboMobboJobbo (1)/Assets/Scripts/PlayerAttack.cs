using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour {

    private bool attacking = false;

    private float attackTimer = 0;
    private float attackCd = 0.6f;

    public Collider2D attackTrigger;

    private void Awake()
    {
        attackTrigger.enabled = false;
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0) && !attacking)
        {
            attacking = true;
            attackTimer = attackCd;
            attackTrigger.enabled = true;
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
    }
}
