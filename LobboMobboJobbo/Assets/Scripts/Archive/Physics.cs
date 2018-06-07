using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Physics : MonoBehaviour {

	public float gravityModifier = 1f; // allow scaling of gravity
    public float minGroundNormalY = .65f;

    protected Vector2 targetVelocity;
    protected bool grounded;
    protected Vector2 groundNormal;
    protected Vector2 velocity; //move object downward every frame
    protected Rigidbody2D rb2d;
    protected ContactFilter2D contactFilter;
    protected RaycastHit2D[] hitBuffer = new RaycastHit2D[16];
    protected List<RaycastHit2D> hitBufferList = new List<RaycastHit2D>(16);

    protected const float minMoveDistance = 0.001f;
    protected const float shellRadius = 0.01f;

	private void OnEnable()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Use this for initialization
    void Start ()
    {
        contactFilter.useTriggers = false;
        contactFilter.SetLayerMask(Physics2D.GetLayerCollisionMask(gameObject.layer)); //getting layer mask from project settings for physics2d - refer Unity Edit --> ProjectSettings --> Physics2D
        //use settings from physics2d settings to determine what layers we're checking collision against
        contactFilter.useLayerMask = true;

    }
	
	// Update is called once per frame
	void Update ()
    {
        targetVelocity = Vector2.zero;
        ComputeVelocity();
	}

    protected virtual void ComputeVelocity()
    {


    }

    private void FixedUpdate()
    {
        velocity = velocity += gravityModifier * Physics2D.gravity * Time.deltaTime;
        velocity.x = targetVelocity.x;

        grounded = false;

        Vector2 deltaPosition = velocity * Time.deltaTime;

        Vector2 moveAlongGround = new Vector2(groundNormal.y, -groundNormal.x); //accounts for slopes

        Vector2 move = moveAlongGround * deltaPosition.x;

        Movement(move, false);

        move = Vector2.up * deltaPosition.y; //vertical movement

        Movement(move, true); //move object based on values calculated - set position of object rigidbody2d
    }

    void Movement(Vector2 move, bool yMovement)
    {
        float distance = move.magnitude;
        
        if(distance > minMoveDistance)
        {
            int count = rb2d.Cast(move, contactFilter, hitBuffer, distance + shellRadius);
            hitBufferList.Clear();
            for(int i=0; i < count; i++)
            {

                BoxCollider2D platform = hitBuffer[i].collider.GetComponent<BoxCollider2D>();
                //PolygonCollider2D polyPlatform = hitBuffer[i].collider.GetComponent<PolygonCollider2D>();
                if (!platform || (hitBuffer[i].normal == Vector2.up && velocity.y < 0 && yMovement))
                {
                    hitBufferList.Add(hitBuffer[i]);
                }
                /*else if (!polyPlatform || (hitBuffer[i].normal == Vector2.up && velocity.y < 0 && yMovement))
                {
                    hitBufferList.Add(hitBuffer[i]);
                }*/
            }

            for(int i =0; i < hitBufferList.Count; i++)
            {
                Vector2 currentNormal = hitBufferList[i].normal;
                if(currentNormal.y > minGroundNormalY) //use to set players grounded state - check if angle of the object we collide with means it would be considered ground
                {
                    grounded = true;
                    if (yMovement)
                    {
                        groundNormal = currentNormal;
                        currentNormal.x = 0;
                    }
                }

                float projection = Vector2.Dot(velocity, currentNormal);
                if(projection < 0)
                {
                    velocity = velocity - projection * currentNormal;
                }

                float modifiedDistance = hitBufferList[i].distance - shellRadius;
                distance = modifiedDistance < distance ? modifiedDistance : distance;

            }
        }

        rb2d.position = rb2d.position + move.normalized * distance;
    }

    internal static bool Raycast(Ray ray, out RaycastHit hit, int v1, int v2)
    {
        throw new NotImplementedException();
    }
}
