using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door: Kinematic
{
    /* 0 = closed; 1 = locked; 2 = open */
    public int doorState;

    public void doorStateClosed(){ doorState = 0; }
    public void doorStateLocked(){ doorState = 1; }
    public void doorStateOpen(){ doorState = 2; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    protected override void Update()
    {
        if( doorState == 0 || doorState == 1 ){
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                Quaternion.Euler(0, 90, 0),
                Time.deltaTime * 5.0f); 
            this.transform.position = new Vector3(0,1,0);
        }
        else{
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation,
                Quaternion.Euler(0, 0, 0),
                Time.deltaTime * 5.0f); 
            this.transform.position = new Vector3(-2,1,2);
        }

    }
}
