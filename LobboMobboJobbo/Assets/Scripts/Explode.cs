using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explode : MonoBehaviour
{

    Rigidbody2D rb2d;
    public float projectileSpeed = 0.5f;
    private float startTime;
    private Rigidbody2D rigidbody;

    public void init(int increment)
    {
        transform.localRotation = Quaternion.Euler(0, 0, increment);
		
    }
    // Use this for initialization
    void Start()
    {
        startTime = Time.time;
		rigidbody = transform.GetChild(0).GetComponent<Rigidbody2D>();
		rigidbody.AddRelativeForce(new Vector2(10,10), ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - startTime >= 4)
        {
            DestroyObject();
        }
    }

    void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}