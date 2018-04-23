using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

	public enum WeaponType {Stab, Slash, Ranged};

	//need to be set up in editor
	public float weaponReach;
	public float weaponDamage;
	public WeaponType type;
	public Collider2D coll;
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
	//stab
		if (type == WeaponType.Stab) {
			//raycast towards lookat
			//math prep
			float dist = Vector2.Distance (mouseAt, transform.position);
			float distRatio = weaponReach / dist;
			float endPointX = (1 - distRatio) * transform.position.x + (distRatio * mouseAt.x);
			float endPointY = (1 - distRatio) * transform.position.y + (distRatio * mouseAt.y);
			Vector2 endPoint = new Vector2 (endPointX, endPointY);

			//-----stuff for testing
			//checking = true;
			//start = transform.position;
			//mid = endPoint;
			//end = mouseAt;
			//-----

			//linecast (raycast but flatter)
			RaycastHit2D[] enemysHit = Physics2D.LinecastAll (transform.position, endPoint, enemyLayer);
			//go through the array and call the hurt method
			for(int i = 0; i < enemysHit.Length; i++){
				enemysHit [i].collider.gameObject.SendMessage ("ApplyDamage",weaponDamage);

			}
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
}