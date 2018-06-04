using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    public int maxHealth = 100;
    public int currentHealth;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "projectile")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
            
        }        
    }

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void Damage(int damage)
    {
        currentHealth -= damage;             
    }
}
