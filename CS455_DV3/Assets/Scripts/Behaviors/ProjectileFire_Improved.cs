using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFire_Improved
{
    public Kinematic character;
    public GameObject target;
    public GameObject projectilePrefab;

    float muzzleV = 26.0f;
    Vector3 offset = new Vector3(0, 2, 0);
    public Vector3 lead = new Vector3(0, 0, 0);
    Vector3 gravity;
    

    public ProjectileFire_Improved( GameObject dtpf ){
        projectilePrefab = dtpf;
        character = character;
    	Physics.gravity = new Vector3(0, -9.8f, 0);
    	gravity = Physics.gravity;
    }

    public bool fireProjectile()
    {   
    	float ttt = 0.0f;

    	// Calculate delta from character to target
        Vector3 targetPos = target.transform.position + lead;
    	Vector3 delta = targetPos - character.transform.position - offset;

    	// Calculate equation coefficients
    	float a = Mathf.Pow(gravity.magnitude, 2);
    	float b = -4 * ( Vector3.Dot(gravity, delta) + muzzleV * muzzleV );
    	float c = 4 * Mathf.Pow(delta.magnitude, 2);

    	// Check for no real solutions
    	float b2m4ac = b*b-4*a*c;
    	if( b2m4ac < 0 )
    		return false;

    	// Find the candidate times 
    	float time0 = Mathf.Sqrt( (-b + Mathf.Sqrt(b2m4ac)) / (2*a) );
    	float time1 = Mathf.Sqrt( (-b - Mathf.Sqrt(b2m4ac)) / (2*a) );

    	// Select time to target
    	if( time0 < 0 ){
    		if( time1 < 0 )
    			return false;
    		else
    			ttt = time1;
    	}
    	else{
    		if( time1 < 0 )
    			ttt = time0;
    		else
    			ttt = Mathf.Min(time0, time1);
    	}

    	/* Calculate the firing vector */
    	Vector3 fireVector = (delta * 2 - gravity * (ttt*ttt)) / (2 * ttt);

    	/* Fire the projectile */
		GameObject projectile = UnityEngine.Object.Instantiate(projectilePrefab, new Vector3(0, 0, 0), Quaternion.identity) as GameObject; 
		projectile.transform.position = character.transform.position + offset;
		projectile.GetComponent<Rigidbody>().velocity = fireVector;
        projectile.GetComponent<Projectile_Tracker>().target = target;
        projectile.GetComponent<Projectile_Tracker>().targetPos = targetPos;
        projectile.GetComponent<Projectile_Tracker>().firer = this;

    	return true;
    }
}
