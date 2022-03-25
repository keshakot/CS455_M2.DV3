using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleAvoider : Kinematic
{

    public GameObject oa_target_prefab;

    ObstacleAvoidance myMoveType;
    Face myRotateType;

    // Start is called before the first frame update
    void Start()
    {
        myMoveType = new ObstacleAvoidance( oa_target_prefab );
        myMoveType.character = this;
        myMoveType.target = myTarget;
        myMoveType.dummyTargetPF = oa_target_prefab;

        myRotateType = new Face();
        myRotateType.character = this;
        myRotateType.target = myTarget;
    }

    // Update is called once per frame
    protected override void Update()
    {
        steeringUpdate = new SteeringOutput();
        steeringUpdate.linear = myMoveType.getSteering().linear;
        steeringUpdate.angular = myRotateType.getSteering().angular;
        base.Update();
    }
}
