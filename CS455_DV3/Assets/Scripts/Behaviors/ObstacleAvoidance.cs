using System.Collections;
using System.Collections.Generic;
//using UnityEditor;
using UnityEngine;

public class ObstacleAvoidance : Seek
{
    public Kinematic character;
    public GameObject target;
    public GameObject dummyTargetPF;
    
    GameObject dummyTarget;
    
    bool avoidFlag = false;
    float avoidThresh = 1.0f;

    // The min. distance to an obstacle
    float avoidDistance = 3.0f;
    
    // The distance to look ahead (and to the side))
    float lookahead = 5.0f;
    float lookaside = 3.0f;
    
    // The angle of the two side raycasts
    float sideangle = 40.0f; 

    public ObstacleAvoidance( GameObject dtpf ){
        dummyTarget = UnityEngine.Object.Instantiate(dtpf, new Vector3(0, 0, 0), Quaternion.identity) as GameObject;
    	base.character = character;
    	base.flee = false;
    }

    public override SteeringOutput getSteering()
    {   
    	base.character = character;
        SteeringOutput result = null;

        // 1. Calculate collision ray vector
	Vector3 ray = character.linearVelocity;
	ray.Normalize();
	ray.y = 0;
	Vector3 rayR = ray;
	Vector3 rayL = ray;
	rayR = Quaternion.AngleAxis(sideangle, Vector3.up) * rayR;
	rayL = Quaternion.AngleAxis(-sideangle, Vector3.up) * rayL;
	
	// Look for and handle collision
	RaycastHit hit, hitR, hitL;
	Debug.DrawRay(character.transform.position, ray * lookahead, Color.blue, 0.1f, false);
	Debug.DrawRay(character.transform.position, rayR * lookaside, Color.red, 0.1f, false);
	Debug.DrawRay(character.transform.position, rayL * lookaside, Color.red, 0.1f, false);
	
	if( avoidFlag ){
	    result = base.getSteering();
	    Vector3 dist = dummyTarget.transform.position - character.transform.position;
	    if( dist.magnitude < avoidThresh )
	        avoidFlag = false;
	        
	    return result;
	}
	
	bool hitcenter = Physics.Raycast(character.transform.position, ray, out hit, lookahead) && hit.transform.name != target.transform.name;
	bool hitright = Physics.Raycast(character.transform.position, rayR, out hitR, lookaside) && hitR.transform.name != target.transform.name;
	bool hitleft = Physics.Raycast(character.transform.position, rayL, out hitL, lookaside) && hitL.transform.name != target.transform.name;
	
	if ( hitcenter ){
	    dummyTarget.GetComponent<Renderer>().enabled = true;
	    dummyTarget.transform.position = hit.point + hit.normal * avoidDistance;
	    base.target = dummyTarget;
	    result = base.getSteering();
	    avoidFlag = true;
	}
	else if ( hitright ) {
	    dummyTarget.GetComponent<Renderer>().enabled = true;
	    dummyTarget.transform.position = character.transform.position + rayL;
	    base.target = dummyTarget;
	    result = base.getSteering();
	    avoidFlag = true;
	}
	else if ( hitleft ) {
	    dummyTarget.GetComponent<Renderer>().enabled = true;
	    dummyTarget.transform.position = character.transform.position + rayR;
	    base.target = dummyTarget;
	    result = base.getSteering();
	    avoidFlag = true;
	}
	else{
	    dummyTarget.GetComponent<Renderer>().enabled = false;
    	    base.target = target;
            result = base.getSteering();
	}
	
	return result;
    }
}
