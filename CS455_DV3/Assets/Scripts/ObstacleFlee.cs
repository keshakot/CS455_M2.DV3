using UnityEngine;
using System.Collections;

public class ObstacleFlee : MonoBehaviour
{
    public Rigidbody rb; /* The body of the Obstacle */
    public Transform tf; /* Own transform */
    public Transform player; /* The Player to seek */
    
    public float maxSpeed = 100f;
    
    float smooth = 5.0f;
    
    // Update is called once per frame
    void FixedUpdate()
    {
    	/* Get and normalize velocity */
    	Vector3 velocity = tf.position - player.position;
    	velocity.Normalize();
    	velocity *= maxSpeed;
    
    	/* Adjust velocity */
        rb.AddForce(velocity.x * Time.deltaTime, velocity.y * Time.deltaTime, velocity.z * Time.deltaTime, ForceMode.VelocityChange);
        
    	/* Adjust rotation */
    	if( velocity.magnitude > 0 ){
    	    float angle = Mathf.Atan2(velocity.x, velocity.z) * Mathf.Rad2Deg;
    	    
            Quaternion target = Quaternion.Euler(0, angle, 0);
            tf.rotation = Quaternion.Slerp(tf.rotation, target, Time.deltaTime * smooth);
    	}
    }
}
