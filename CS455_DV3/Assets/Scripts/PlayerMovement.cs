using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : Kinematic
{
    public Rigidbody rb;
    public Camera cam;
    public NavMeshAgent agent;
    
    public float forwardForce = 100f;
    public float sidewaysForce = 100f;
    
    // Start is called before the first frame update
    void Start()
    {
        //rb.AddForce(0, 200, 500);
        //rb.useGravity = false;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        //rb.AddForce(0, 0, forwardForce * Time.deltaTime);
        
        if ( Input.GetMouseButtonDown(0) ){
            RaycastHit hit;
            Ray ray = cam.ScreenPointToRay( Input.mousePosition );

            if( Physics.Raycast(ray, out hit) ){
                agent.SetDestination( hit.point );
            }

        }

        /*if ( Input.GetKey("w") || Input.GetKey("up") ) {
            rb.AddForce(0, 0, forwardForce * Time.deltaTime, ForceMode.VelocityChange);
        }
        
        if ( Input.GetKey("a") || Input.GetKey("left") ) {
            rb.AddForce(-sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
        
        if ( Input.GetKey("s") || Input.GetKey("down") ) {
            rb.AddForce(0, 0, -forwardForce * Time.deltaTime, ForceMode.VelocityChange);
        }
        
        if ( Input.GetKey("d") || Input.GetKey("right") ) {
            rb.AddForce(sidewaysForce * Time.deltaTime, 0, 0, ForceMode.VelocityChange);
        }
        
        if ( rb.position.y < -1f ) {
    	    FindObjectOfType<GameManager>().EndGame();
        }*/
    }
}
