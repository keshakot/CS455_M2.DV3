using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Separator : Kinematic
{
    Separation myMoveType;
    Face myRotateType;

    // Start is called before the first frame update
    void Start()
    {
    	Debug.Log( myTarget.name );
    
        Kinematic t = myTarget.GetComponent<Kinematic>();
    	Debug.Log( "ASD: " + t );
//    	Debug.Log( "asdasd: " + myMoveType.targets.Length );
    	if( t == null )
    	    Debug.Log(" NU LLLL LL!! ");
    
        myMoveType = new Separation();
        myMoveType.character = this;
        myMoveType.targets = new Kinematic[] { t } ;

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
