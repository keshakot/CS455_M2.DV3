using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// based on Millington pp. 62-63
public class Wander : SteeringBehavior
{
    public Kinematic character;
    public GameObject target;

    float maxAcceleration = 100f;
    float maxSpeed = 10f;

    // the time over which to achieve target speed
    float timeToTarget = 0.1f;

    //
    float tRadius = 5.0f;
    float tAngle = 0.0f;
    float tAngleJitter = 10.0f;

    public override SteeringOutput getSteering()
    {
        SteeringOutput result = new SteeringOutput();

        // Find new 'target' angle
        tAngle = Random.Range( -tAngleJitter + tAngle, tAngle + tAngleJitter );

        // Calculate 'target' position
        Vector3 tPosition = new Vector3();
        tPosition.x = character.transform.position.x + tRadius * Mathf.Sin(tAngle * Mathf.Deg2Rad);
        tPosition.y = 1;
        tPosition.z = character.transform.position.z + tRadius * Mathf.Cos(tAngle * Mathf.Deg2Rad);;

        // get the direction to the 'target'
        Vector3 direction = tPosition - character.transform.position;
        float distance = direction.magnitude;

        float targetSpeed = maxSpeed;

        // the target velocity combines speed and direction
        Vector3 targetVelocity = direction;
        targetVelocity.Normalize();
        targetVelocity *= targetSpeed;

        // acceleration tries to get to the target velocity
        result.linear = targetVelocity - character.linearVelocity;
        result.linear /= timeToTarget;

        // check if the acceleration is too fast
        if (result.linear.magnitude > maxAcceleration)
        {
            result.linear.Normalize();
            result.linear *= maxAcceleration;
        }

        return result;
    }
}
