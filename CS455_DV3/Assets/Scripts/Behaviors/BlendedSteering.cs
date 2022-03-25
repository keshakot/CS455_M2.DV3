using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BehaviorAndWeight{
	public SteeringBehavior behavior;
	public float weight;

	public BehaviorAndWeight(SteeringBehavior b, float w){
		behavior = b;
		weight = w;
	}
}

public class BlendedSteering: SteeringBehavior
{
    public BehaviorAndWeight[] behaviors;

   	float maxAcceleration = 100f;
   	float maxRotation = 100f;

    public override SteeringOutput getSteering()
    {   
    	SteeringOutput result = new SteeringOutput();
    	result.linear = new Vector3(0, 0, 0);
    	result.angular = 0.0f;

    	// Accumulate all accelerations
        foreach (BehaviorAndWeight b in behaviors){
        	SteeringOutput gs = b.behavior.getSteering();
        	result.linear += b.weight * gs.linear;
        	result.angular += b.weight * gs.angular;
        }

        result.linear.Normalize();
        result.linear *= maxAcceleration;

        result.angular = Mathf.Max(result.angular, maxRotation);

        result.angular = 0.0f;
        return result; 
    }
}
