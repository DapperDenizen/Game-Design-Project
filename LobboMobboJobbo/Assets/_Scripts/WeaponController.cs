using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

	public enum WeaponType {Stab, Slash, Ranged};

	//need to be set up in editor
	public float weaponReach; // how far the weapons hitbox will go 
	public float hangTime;	//how long the hitbox is out
	public float weaponDamage; // how much damage it does
	public WeaponType type; // the type of weapon being used
	public Collider2D coll; //weapons hitbox
	//

	//TEST POINTS -> to check the stab raycast
	bool checking = false;
	Vector2 start;
	Vector2 mid;
	Vector2 end;
	//

	//what we're looking for when attacking
	private LayerMask enemyLayer;


	void Start () {
		coll = GetComponent<Collider2D> ();
		coll.enabled = false;
		enemyLayer = LayerMask.NameToLayer ("Enemy");	
	}
		
	public void Attack(Vector3 mouseAt){
		//move to position
		if(type == WeaponType.Stab){
			float angle = AngleBetweenTwoPoints(transform.position, mouseAt);
		transform.rotation = Quaternion.Euler(new Vector3(0f,0f,angle));
		}
		//get enemys hit
		getHit(mouseAt);

	}

	public void getHit(Vector3 mouseAt){
			coll.enabled = true;
		//print ("active at " + Time.time);
		Invoke ("ResetHitbox",hangTime);
		
	//stab
		if (type == WeaponType.Stab) {
			//LEGACY CODE MAY BE USED LATER
			//raycast towards lookat
			//math prep
			//float dist = Vector2.Distance (mouseAt, transform.position);
			//float distRatio = weaponReach / dist;
			//float endPointX = (1 - distRatio) * transform.position.x + (distRatio * mouseAt.x);
			//float endPointY = (1 - distRatio) * transform.position.y + (distRatio * mouseAt.y);
			//Vector2 endPoint = new Vector2 (endPointX, endPointY);

			//-----stuff for testing
			//checking = true;
			//start = transform.position;
			//mid = endPoint;
			//end = mouseAt;
			//-----



		}
	//slash
		//spawn hitbox and check
		else if (type == WeaponType.Slash) {
			print ("SHHHWWIING!");
		}
	//ranged
		//spawn projectile???? i dunno this is a dummy node until we know how often we'll be using this
		else if(type == WeaponType.Ranged){
			print ("PEW PEW PEW PEW");

		}


	}

	//Utility functions

	float AngleBetweenTwoPoints(Vector3 a, Vector3 b){

		return Mathf.Atan2 (a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;

	}

	void ResetHitbox(){
		coll.enabled = false;
		//print ("inActive at " + Time.time);
	}

	void OnDrawGizmos(){
		if (checking) {
			Gizmos.color = Color.cyan;
			Gizmos.DrawLine (start, mid);
			Gizmos.color = Color.green;
			Gizmos.DrawLine (mid, end);
			//Gizmos.color = Color.white;
			//Gizmos.DrawLine (start, end);
		}
	}

	void OnTriggerEnter2D(Collider2D touched){
		print ("hit");

	}
}