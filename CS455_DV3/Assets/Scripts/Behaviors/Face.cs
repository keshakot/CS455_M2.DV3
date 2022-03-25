using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : Align
{

    public override float getTargetAngle()
    {
        Vector3 dir = character.gameObject.transform.position - target.transform.position;
        float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
        
        return targetAngle;
    }
}
