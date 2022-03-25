using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionAvoider : Kinematic
{
    public GameObject[] targets;
    
    CollisionAvoidance myMoveType;
    Face myRotateType;

    // Start is called before the first frame update
    void Start()
    {
        int i = 0;
    
        myMoveType = new CollisionAvoidance();
        myMoveType.character = this;
        myMoveType.targets = new Kinematic[targets.Length];
        myMoveType.target = myTarget;
        
        foreach (GameObject target in targets)
            myMoveType.targets[i] = targets[i++].GetComponent<Kinematic>();
        

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
