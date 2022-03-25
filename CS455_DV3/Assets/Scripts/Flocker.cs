using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flocker: Kinematic
{

    public GameObject[] flock;
    public GameObject target;

    BlendedSteering myMoveType;
    Face myRotateType;

    // Start is called before the first frame update
    void Start()
    {
        myMoveType = new BlendedSteering();
       
        /* Behavior 1: Separation */ 
        Separation b1 = new Separation();
        b1.character = this;
        b1.targets = new Kinematic[ flock.Length ];
        for( int i=0; i<flock.Length; i++ )
            b1.targets[i] = flock[i].GetComponent<Kinematic>();

        /* Behavior 2: Matching */
        Matching b2 = new Matching();
        b2.character = this;
        b2.targets = b1.targets;

        /* Behavior 3: Cohesion */
        Cohesion b3 = new Cohesion(); 
        b3.character = this;
        b3.targets = b1.targets;

        /* Behavior 4: Seek */
        Seek b4 = new Seek();
        b4.character = this;
        b4.target = target;
        b4.flee = false;


        myMoveType.behaviors = new BehaviorAndWeight[] { new BehaviorAndWeight(b1, 150),
                                                         new BehaviorAndWeight(b2, 1),
                                                         new BehaviorAndWeight(b3, 1),
                                                         new BehaviorAndWeight(b4, 1) };

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
