using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFirer : Kinematic
{

    public int firePeriod = 1;
    int fireCnt = 0;

    public GameObject oa_target_prefab;

    ProjectileFire pf;
    Face myRotateType;

    // Start is called before the first frame update
    void Start()
    {
        pf = new ProjectileFire( oa_target_prefab );
        pf.character = this;
        pf.target = myTarget;

        myRotateType = new Face();
        myRotateType.character = this;
        myRotateType.target = myTarget;
    }

    // Update is called once per frame
    protected override void Update()
    {
        steeringUpdate = new SteeringOutput();
        steeringUpdate.angular = myRotateType.getSteering().angular;
        base.Update();

        if( fireCnt++ < firePeriod )
            return;
        else
            fireCnt = 0;

        /* Fire a projectile at the target */
        pf.fireProjectile();

        return;
    }
}
