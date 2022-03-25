using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Tracker : MonoBehaviour
{
    public Vector3 targetPos;
    public GameObject target;
    public ProjectileFire_Improved firer;
    float collThresh = 0.5f;
    float lead_dampen = 1.3f;
    bool reached_flag = false;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //if( targetPos != null )
        //    Debug.Log("TARGETPOS: " + targetPos);
        if( !reached_flag && ((this.transform.position - targetPos).magnitude < collThresh) ){
            Debug.Log("Target reached");
            reached_flag = true;
            Vector3 miss = target.transform.position - this.transform.position;
            if( miss.magnitude > collThresh ){
                Debug.Log("Target missed by " + miss);
                firer.lead = miss / lead_dampen;
            }
            else
                firer.lead = miss;
        }
    }
}
