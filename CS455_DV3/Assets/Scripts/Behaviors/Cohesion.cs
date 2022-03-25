using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cohesion : SteeringBehavior
{
    public Kinematic character;

    public Kinematic[] targets;

    public override SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();
        result.linear = new Vector3(0, 0, 0);
        result.angular = 0.0f;

        Vector3 targetPos = new Vector3(0, 0, 0);

        // Sum all targets' positions
        foreach (Kinematic target in targets)
            targetPos += target.transform.position;

        // Average all targets' positions
        targetPos /= targets.Length;

        result.linear = character.transform.position - targetPos;

        return result;
    }
}
