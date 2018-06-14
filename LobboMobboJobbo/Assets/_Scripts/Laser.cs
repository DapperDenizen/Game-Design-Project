using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    private LineRenderer lineRenderer;
   

	// Use this for initialization
	void Start ()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.enabled = false;
        lineRenderer.useWorldSpace = false;
	}
	
	// Update is called once per frame
	void Update ()
    {
        /*lineRenderer.SetPosition(0, transform.position);
        RaycastHit2D hit;
        if (Physics2D.Raycast(transform.position, transform.right, out hit))
        {
            if (hit.collider)
            {
                lineRenderer.SetPosition(1, hit.point);
            }
        }
        else lineRenderer.SetPosition(1, transform.right*100);*/
     /*   RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right);
        Debug.DrawLine(transform.position, hit.point);
        LaserHit.position = hit.point;
        lineRenderer.SetPosition(0, transform.position); //start of laser at origin point
        //lineRenderer.SetPosition(1, LaserHit.position); //end of laser
        if (Input.GetKey(KeyCode.LeftShift))
        {
            lineRenderer.enabled = true;
        }
        else
        {
            lineRenderer.enabled = false;
        }*/
	}
}
