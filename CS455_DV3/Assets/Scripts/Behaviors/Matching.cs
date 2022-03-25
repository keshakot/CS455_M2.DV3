using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Matching : SteeringBehavior
{
    public Kinematic character;

    public Kinematic[] targets;

    public override SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();
        result.linear = new Vector3(0, 0, 0);
        result.angular = 0.0f;

        // Sum all targets' velocities and orientations
        foreach (Kinematic target in targets)
        {
            result.linear += target.linearVelocity;
            result.angular += target.angularVelocity;
        }

        // Average all targets' velocities and orientations
        result.linear /= targets.Length;
        result.angular /= targets.Length;

        return result;
    }
}
