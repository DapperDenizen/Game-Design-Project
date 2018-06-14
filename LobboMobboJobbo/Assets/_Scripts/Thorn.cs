using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thorn : MonoBehaviour
{

    Rigidbody2D rb2d;
    public float projectileSpeed = 0.5f;
    private float startTime;

    public void init(int increment)
    {
        transform.localRotation = Quaternion.Euler(0, 0, increment);
        transform.GetChild(0).localPosition = new Vector3(0, 1, 0);
    }
    // Use this for initialization
    void Start()
    {
        startTime = Time.time;
        print(startTime);
        //Vector3 dir = transform.GetChild(0).up;
        //transform.GetChild(0).GetComponent<Rigidbody2D>().AddForce(-dir * projectileSpeed);
    }

    // Update is called once per frame
    void Update()
    {
        transform.GetChild(0).localPosition += new Vector3(0, projectileSpeed * Time.deltaTime, 0);
        if (Time.time - startTime >= 0.5)
        {
            DestroyObject();
        }
    }

    void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}