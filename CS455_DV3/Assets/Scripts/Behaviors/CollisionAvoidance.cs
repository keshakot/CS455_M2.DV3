using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoidance : Seek
{
    //public Kinematic character;
    public Kinematic[] targets;
    //public GameObject target;

    float maxAcceleration = 10f;

    // the radius for collision w/ target
    float radius = 3.0f;

    public override SteeringOutput getSteering()
    {
    	base.character = character;
    	base.target = target;
        SteeringOutput result = base.getSteering();

        // 1. Find target closest to collision
        // Store time until 1st collision
        float shortestTime = (float)(double.PositiveInfinity);

        // Colliding target info
        Kinematic firstTarget = null;
        float firstMinSeparation = 0.0f;
        float firstDistance = 0.0f;
        Vector3 firstRelativePos = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 relativePos = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 relativeVel = new Vector3(0.0f, 0.0f, 0.0f);
        Vector3 firstRelativeVel = new Vector3(0.0f, 0.0f, 0.0f);


        foreach (Kinematic target in targets){
            relativePos = target.transform.position - character.transform.position;
            relativeVel = ( character.GetComponent<Rigidbody>().velocity - target.GetComponent<Rigidbody>().velocity );
            float relativeSpeed = relativeVel.magnitude;
            float timeToCollision = Vector3.Dot(relativePos, relativeVel) / (relativeSpeed * relativeSpeed);
            float distance = relativePos.magnitude;
            float minSeparation = distance - relativeSpeed * timeToCollision;
            
            if( minSeparation > 2 * radius )
                continue;
                
            if( timeToCollision > 0 && timeToCollision < shortestTime ){
            	//Debug.Log("TIMETOCOL > 0");
                shortestTime = timeToCollision;
                firstTarget = target;
                firstMinSeparation = minSeparation;
                firstDistance = distance;
                firstRelativePos = relativePos;
                firstRelativeVel = relativeVel;
            }
            
            //Debug.Log( relativePos + " [" + distance + "]| " + relativeVel + " [" + relativeSpeed + "]| " + timeToCollision );
        }
        
        // 2. Calculate the steering
        if( !firstTarget )
            return result;
            
        if( firstMinSeparation <= 0 || firstDistance < 2 * radius )
            relativePos = firstRelativePos + firstRelativeVel * shortestTime;
        
        // Avoid the target
        relativePos.Normalize();
        result.linear = -relativePos * maxAcceleration;
        result.angular = 0;

        return result;
    }
}
