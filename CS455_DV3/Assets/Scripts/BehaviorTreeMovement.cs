using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract class Task{

    public abstract bool run();

}

class Selector: Task {

    public Task[] children; 

    public override bool run(){
        foreach( Task c in children )
            if( c.run() )
                return true;
        return false;
    }
}

class Sequence: Task {

    public Task[] children; 

    public override bool run(){
        foreach( Task c in children )
            if( !c.run() )
                return false;
        return true;
    }
}



public class BehaviorTreeMovement: Kinematic
{
    //Arrive myMoveType;
    //Align myRotateType;
    public Door door;
    //BehaviorTree t;
    Selector root;

    /* Implementation-specific tasks */

    class DoorOpen: Task {
        private BehaviorTreeMovement btm;
        public DoorOpen( BehaviorTreeMovement b ){ btm = b; }
        public override bool run(){ return (btm.door.doorState == 2); }
    }

    class DoorOpenL: Task {
        private BehaviorTreeMovement btm;
        public DoorOpenL( BehaviorTreeMovement b ){ btm = b; }
        public override bool run(){ return !(btm.door.doorState == 2); }
    }

    class DoorLocked: Task {
        private BehaviorTreeMovement btm;
        public DoorLocked( BehaviorTreeMovement b ){ btm = b; }
        public override bool run(){ return !(btm.door.doorState == 1); }
    }

    class OpenDoor: Task {
        private BehaviorTreeMovement btm;
        public OpenDoor( BehaviorTreeMovement b ){ btm = b; }
        public override bool run(){ btm.door.doorState = 2; return true; }
    }

    class BargeDoor: Task {
        private BehaviorTreeMovement btm;
        public BargeDoor( BehaviorTreeMovement b ){ btm = b; }
        public override bool run(){ 
            btm.transform.localScale = new Vector3(3,3,3);
            Vector3 trg = new Vector3(10,1,0);
            btm.door.GetComponent<Rigidbody>().velocity = new Vector3(100,100,100);
            while( Vector3.Distance(btm.transform.position, trg) > 0.5f )
                btm.transform.position = Vector3.MoveTowards(btm.transform.position, trg, 1*Time.deltaTime);
            return true;
        }
    }

    class MoveIntoRoom: Task{
        private BehaviorTreeMovement btm;
        public MoveIntoRoom( BehaviorTreeMovement b ){ btm = b; }
        public override bool run(){
            btm.StartCoroutine(needful());
            return true;
        }
        IEnumerator needful(){
            Vector3 trg = new Vector3(10,1,0);
            yield return new WaitForSeconds(2);
            while( Vector3.Distance(btm.transform.position, trg) > 0.5f )
                btm.transform.position = Vector3.MoveTowards(btm.transform.position, trg, 0.01f*Time.deltaTime);
        }
    }

    class MoveToDoor: Task{
        private BehaviorTreeMovement btm;
        public MoveToDoor( BehaviorTreeMovement b ){ btm = b; }
        public override bool run(){
            btm.StartCoroutine(needful());
            return true;
        }
        IEnumerator needful(){
            Vector3 trg = new Vector3(-3,1,0);
            yield return new WaitForSeconds(0.0f);
            while( Vector3.Distance(btm.transform.position, trg) > 0.5f )
                btm.transform.position = Vector3.MoveTowards(btm.transform.position, trg, 1*Time.deltaTime);
        }
    }
    /* ----------------------------- */

    // Start is called before the first frame update
    IEnumerator Start()
    {
        yield return new WaitForSecondsRealtime(5);

        // Create the behavior tree
        root = new Selector();
        Sequence l1_1 = new Sequence();
        Sequence l1_2 = new Sequence();
        root.children = new Task[]{ l1_1, l1_2 };

        l1_1.children = new Task[]{ new DoorOpen(this), new MoveIntoRoom(this) };

        Selector l2_2_2 = new Selector();
        Sequence l3_1 = new Sequence();
        Sequence l3_2 = new Sequence();
        l2_2_2.children = new Task[]{ l3_1, l3_2 };

        l3_1.children = new Task[]{ new DoorLocked(this), new OpenDoor(this) };
        l3_2.children = new Task[]{ new DoorOpenL(this), new BargeDoor(this) };

        l1_2.children = new Task[]{ new MoveToDoor(this), l2_2_2, new MoveIntoRoom(this) };

        Debug.Log("Running tree...");
        root.run();
        Debug.Log("Ran tree.");

        /*myMoveType = new Arrive();
        myMoveType.character = this;
        myMoveType.target = myTarget;

        myRotateType = new Align();
        myRotateType.character = this;
        myRotateType.target = myTarget;*/
    }

    // Update is called once per frame
    protected override void Update()
    {
        /*steeringUpdate = new SteeringOutput();
        steeringUpdate.linear = myMoveType.getSteering().linear;
        steeringUpdate.angular = myRotateType.getSteering().angular;
        base.Update();*/
    }

    IEnumerator delay( float s ){
        Debug.Log("sample text");
        yield return new WaitForSecondsRealtime( s );
        int i = 1; i++;
        Debug.Log("sample text");
    }
}
